using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Project.Services.Native;
using Project.ViewModels;

namespace Project.Controllers {
	[Authorize(Roles = "Administrator")]
	public class AnnouncementController : Controller {
		private readonly IAnnouncementClient _service;
		private readonly IMapper _mapper;

		public AnnouncementController(
			IAnnouncementClient service,
			IMapper mapper
		) {
			this._service = service;
			this._mapper = mapper;
		}

		[AllowAnonymous]
		[HttpGet("/Announcements")]
		public IActionResult Announcements() => Json(this._service.GetAnnouncements());

		[HttpGet("/Admin/Announcements")]
		public IActionResult Index(int Page = 1, int Show = 5) {
			AnnouncementsViewModel model = new AnnouncementsViewModel {
				Page = Page,
				Show = Show,
			};
			model.Announcements = this._service.GetAnnouncements(model);
			return View(model);
		}

		[HttpGet("/Admin/Announcements/New")]
		public IActionResult New() => View();

		[HttpPost("/Admin/Announcements/New")]
		public async Task<IActionResult> New(AnnouncementInputModel model) {
			if (!ModelState.IsValid)
				return BadRequest();
			try {
				await this._service.CreateAnnouncement(this._mapper.Map<Announcement>(model));
			} catch {
				return BadRequest();
			}
			return Ok();
		}

		[HttpGet("/Admin/Announcements/Remove")]
		public IActionResult RemovePage(string Id) {
			Announcement target = this._service.GetAnnouncement(Id);
			if (target == null)
				return Redirect("/404");
			return View("Remove", Id);
		}

		[HttpDelete("/Admin/Announcements/Remove")]
		public async Task<IActionResult> Remove(string Id) {
			try {
				await this._service.RemoveAnnouncement(Id);
			} catch {
				return NotFound();
			}
			return Ok();
		}
	}
}