using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Data;
using Project.Models;
using Project.Services.Native;
using Project.ViewModels;

namespace Project.Controllers {
	[Authorize(Roles = "Administrator")]
	public class AnnouncementController : Controller {
		private readonly ApplicationDbContext _context;
		private readonly IAnnouncementClient _service;
		private readonly IMapper _mapper;

		public AnnouncementController(
			ApplicationDbContext context,
			IAnnouncementClient service,
			IMapper mapper
		) {
			this._context = context;
			this._service = service;
			this._mapper = mapper;
		}

		[AllowAnonymous]
		[HttpGet("/Announcements")]
		public IActionResult Announcements() => Json(this._service.GetAnnouncements());

		[HttpGet("/Admin/Announcements")]
		public IActionResult Index(int Page = 1, int Show = 5) {
			List<Announcement> announcements = this._service.GetAnnouncements();
			int Total = announcements.Count;
			int Pages = (Total / Show) + (Total % Show != 0 ? 1 : 0);
			AnnouncementsViewModel model = new AnnouncementsViewModel {
				Announcements = announcements
					.Skip(Show * (Page - 1))
					.Take(Show)
					.ToList(),
				Total = Total,
				Show = Show,
				Page = Page,
				Pages = Pages,
			};
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
			} catch (Exception) {
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
			} catch (Exception) {
				return NotFound();
			}
			return Ok();
		}
	}
}