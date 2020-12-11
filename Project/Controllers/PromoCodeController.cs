using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.Data;
using Project.Models;
using Project.Services.Native;
using Project.ViewModels;
using Project.Enums;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;

namespace Project.Controllers
{
	// [Authorize(Roles = "Administrator")]
	public class PromoCodeController : Controller
	{
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
		public IActionResult Index(int Page = 1, int Show = 10)
		{
			List<PromoCode> codes = this._service.GetPromoCodes();
			codes.Reverse();
			int Total = codes.Count;
			int Pages = (Total / Show) + (Total % Show != 0 ? 1 : 0);
			PromoCodesViewModel model = new PromoCodesViewModel {
				Codes = codes
					.Skip(Show * (Page - 1))
					.Take(Show)
					.Select(this._mapper.Map<PromoCodeViewModel>)
					.ToList(),
				Total = Total,
				Show = Show,
				Page = Page,
				Pages = Pages,
				Active = codes.Count(code => code.IsValid),
				Inactive = codes.Count(code => !code.IsValid),
				FixedAmount = codes.Count(code => code.Type == PromoCodeType.FixedAmount),
				Percentage = codes.Count(code => code.Type == PromoCodeType.Percentage),
				PriceOverride = codes.Count(code => code.Type == PromoCodeType.PriceOverride),
				Free = codes.Count(code => code.Type == PromoCodeType.Free),
			};
			return View(model);
		}

		[HttpGet("/Admin/Codes/New")]
		public async Task<IActionResult> New() {
			try {
				PromoCode code = await this._service.CreateCode();
				return Redirect("/Admin/Codes/Details?Id=" + code.Id);
			} catch (Exception)
			{
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
	}
}
