using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.Data;
using Project.Hubs;
using Project.Models;
using Project.Services.Native;
using Project.ViewModels;

namespace Project.Controllers {
	[Authorize]
	public class MessageController : Controller {
		private readonly IMessageClient _service;
		private readonly ITicketClient _ticketService;
		private readonly IRoleClient _roleService;
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IMapper _mapper;
		private readonly MessageHub _messageHub;

		public MessageController(
			IMessageClient service,
			ITicketClient ticketService,
			IRoleClient roleService,
			ApplicationDbContext context,
			UserManager<ApplicationUser> userManager,
			IMapper mapper,
			MessageHub messageHub
		) {
			this._service = service;
			this._ticketService = ticketService;
			this._roleService = roleService;
			this._context = context;
			this._userManager = userManager;
			this._mapper = mapper;
			this._messageHub = messageHub;
		}

		[HttpGet("/Notifications/Messages/Last")]
		public async Task<IActionResult> Last(int Count) =>
			Json((await this._service.GetMessages(User))
				.OrderByDescending(messages => messages.Time)
				.Take(Count)
				.Select(this._mapper.Map<NotificationMessageViewModel>)
				.ToList());

		[HttpGet("/Notifications/Messages/Raw")]
		public async Task<IActionResult> ListRaw(int Page = 1, int Show = 10) {
			MessagesViewModel model = new MessagesViewModel {
				Page = Page,
				Show = Show,
			};
			model.Messages = (await this._service.GetMessages(User, model))
				.Select(this._mapper.Map<MessageViewModel>)
				.ToList();
			return Json(model.Messages);
		}

		[HttpGet("/Notifications/Messages")]
		public async Task<IActionResult> List(int Page = 1, int Show = 10) {
			MessagesViewModel model = new MessagesViewModel {
				Page = Page,
				Show = Show,
			};
			model.Messages = (await this._service.GetMessages(User, model))
				.Select(this._mapper.Map<MessageViewModel>)
				.ToList();
			return View(model);
		}

		[HttpGet("/Admin/Message/Send")]
		[Authorize(Roles = "Administrator")]
		public IActionResult Send(string To = "{{ALL}}") {
			ViewData["To"] = To;
			return View();
		}

		[HttpPost("/Admin/Message/Send")]
		public async Task<IActionResult> Send(MessageCreateInputModel model, [FromQuery] string To = "{{ALL}}") {
			if (!ModelState.IsValid)
				return BadRequest();
			List<ApplicationUser> targets = new List<ApplicationUser>();
			if (To == "{{ALL}}")
				targets = this._roleService.GetUsers();
			else
				targets.Add(this._roleService.Find(To));
			foreach (ApplicationUser target in targets) {
				Message message = this._mapper.Map<Message>(model);
				ApplicationUser source = await this._userManager.GetUserAsync(User);
				if (target == null)
					return NotFound();
				message.Sender = source;
				message.User = target;
				await this._service.RegisterMessage(message, source, target);
				await this._messageHub.SendMessage(message);
			}
			return Ok();
		}
	}
}