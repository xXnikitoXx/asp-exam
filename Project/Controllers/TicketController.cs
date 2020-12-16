using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.Data;
using Project.Services.Native;
using Project.ViewModels;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Project.Models;
using Microsoft.AspNetCore.Identity;
using System;

namespace Project.Controllers {
	[Authorize]
	public class TicketController : Controller {

		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly ITicketClient _service;
		private readonly IMapper _mapper;

		public TicketController(
			ApplicationDbContext context,
			UserManager<ApplicationUser> userManager,
			ITicketClient service,
			IMapper mapper
		) {
			this._context = context;
			this._userManager = userManager;
			this._service = service;
			this._mapper = mapper;
		}

		[HttpGet("/Tickets")]
		public async Task<IActionResult> Index(int Page = 1, int Show = 10) {
			TicketsViewModel model = new TicketsViewModel {
				Page = Page,
				Show = Show,
			};
			model.Tickets = (await this._service.GetTickets(User, model))
				.Select(this._mapper.Map<TicketViewModel>)
				.ToList();
			return View(model);
		}

		[HttpGet("/Ticket/New")]
		public IActionResult New() => View();

		[HttpPost("/Ticket/New")]
		public async Task<IActionResult> New(TicketCreateInputModel model) {
			if (!ModelState.IsValid)
				return BadRequest();
			try {
				Ticket ticket = this._mapper.Map<Ticket>(model);
				ticket.User = await this._userManager.GetUserAsync(User);
				await this._service.RegisterTicket(ticket);
			} catch (Exception) {
				return BadRequest();
			}
			return Ok();
		}

		[HttpGet("/Admin/Tickets")]
		[Authorize(Roles = "Administrator")]
		public async Task<IActionResult> AdminList(int Page = 1, int Show = 10) {
			TicketsViewModel model = new TicketsViewModel {
				Page = Page,
				Show = Show,
			};
			model.Tickets = this._service.GetTickets(model)
				.Select(this._mapper.Map<TicketViewModel>)
				.ToList();

			foreach (TicketViewModel ticket in model.Tickets) {
				ApplicationUser user = await this._userManager.FindByIdAsync(ticket.UserId);
				ticket.Username = user == null ? "{{DELETED}}" : user.UserName;
			}
			return View(model);
		}
	}
}