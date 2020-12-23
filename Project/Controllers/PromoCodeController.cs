using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.Data;
using Project.Models;
using Project.Services.Native;
using Project.ViewModels;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace Project.Controllers {
	[Authorize(Roles = "Administrator")]
	public class PromoCodeController : Controller {
		private readonly ApplicationDbContext _context;
		private readonly IPromoCodeClient _service;
		private readonly IMapper _mapper;

		public PromoCodeController(
			ApplicationDbContext context,
			IPromoCodeClient service,
			IMapper mapper
		) {
			this._context = context;
			this._service = service;
			this._mapper = mapper;
		}

		[HttpGet("/Admin/Codes")]
		public IActionResult Index(int Page = 1, int Show = 10) {
			PromoCodesViewModel model = new PromoCodesViewModel {
				Page = Page,
				Show = Show,
			};
			model.Codes = this._service.GetPromoCodes(model)
				.Select(this._mapper.Map<PromoCodeViewModel>)
				.Reverse()
				.ToList();
			foreach (PromoCodeViewModel code in model.Codes)
				code.Usage = this._context.UserPromoCodes.Count(upc => upc.PromoCodeId == code.Id);
			return View(model);
		}

		[HttpGet("/Admin/Codes/New")]
		public async Task<IActionResult> New() {
			try {
				PromoCode code = await this._service.CreateCode();
				return Redirect("/Admin/Codes/Details?Id=" + code.Id);
			} catch (Exception) {
				return StatusCode(500);
			}
		}

		[HttpGet("/Admin/Codes/Details")]
		public IActionResult Details(string Id) {
			PromoCode target = this._service.GetCode(Id);
			if (target == null)
				return Redirect("/404");
			return View(this._mapper.Map<PromoCodeViewModel>(target));
		}

		[HttpPost("/Admin/Codes/Switch")]
		public async Task<IActionResult> Switch(string Id) {
			bool state;
			try {
				state = await this._service.SwitchCode(Id);
			} catch (Exception) {
				return NotFound();
			}
			return Json(state);
		}

		[HttpPatch("/Admin/Codes/Details")]
		public async Task<IActionResult> Update(PromoCodeEditInputModel model) {
			if (!ModelState.IsValid)
				return BadRequest();
			PromoCode code = this._mapper.Map<PromoCode>(model);
			try {
				await this._service.UpdateCode(code);
			} catch(Exception) {
				return BadRequest();
			}
			return Ok();
		}

		[HttpGet("/Admin/Codes/Remove")]
		public IActionResult RemovePage(string Id) {
			PromoCode target = this._service.GetCode(Id);
			if (target == null)
				return Redirect("/404");
			return View("Remove", this._mapper.Map<PromoCodeViewModel>(target));
		}

		[HttpDelete("/Admin/Codes/Remove")]
		public async Task<IActionResult> Remove(string Id) {
			PromoCode target = this._service.GetCode(Id);
			if (target == null)
				return BadRequest();
			await this._service.RemoveCode(target);
			return Ok();
		}

		[AllowAnonymous]
		[HttpGet("/PromoCode/Discount")]
		public IActionResult Discount(double Price, List<string> Codes) {
			List<PromoCode> promoCodes = this._service.Codes
				.Where(code => Codes.Contains(code.Code))
				.ToList();
			return Json(this._service.GetDiscount(Price, promoCodes));
		}

		[AllowAnonymous]
		[HttpGet("/PromoCode/FinalPrice")]
		public IActionResult FinalPrice(double Price, List<string> Codes) {
			List<PromoCode> promoCodes = this._service.Codes
				.Where(code => Codes.Contains(code.Code))
				.ToList();
			return Json(this._service.GetFinalPrice(Price, promoCodes));
		}
	}
}
