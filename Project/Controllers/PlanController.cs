using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Models;
using Project.Services.Native;
using Project.ViewModels;

namespace Project.Controllers {
	[Authorize(Roles = "Administrator")]
	public class PlanController : Controller {
		private readonly IPlanClient _service;
		private readonly IMapper _mapper;

		public PlanController(
			IPlanClient service,
			IMapper mapper
		) {
			this._service = service;
			this._mapper = mapper;
		}

		[HttpGet("/Admin/Plans")]
		public IActionResult Index(int Page = 1, int Show = 10) {
			PlansViewModel model = new PlansViewModel {
				Page = Page,
				Show = Show,
			};
			model.Plans = this._service.GetPlans(model)
				.Select(this._mapper.Map<PlanViewModel>)
				.ToList();
			return View(model);
		}

		[HttpGet("/Admin/Plan")]
		public async Task<IActionResult> Details(int Number) {
			PlanViewModel model = this._mapper.Map<PlanViewModel>(await this._service.Find(Number));
			if (model == null)
				return this.Redirect("/404");
			return View(model);
		}

		[HttpPatch("/Admin/Plan")]
		public async Task<IActionResult> Update(PlanEditInputModel model) {
			if (!ModelState.IsValid)
				return BadRequest();
			try {
				Plan plan = this._mapper.Map<Plan>(model);
				await this._service.UpdatePlan(plan);
			} catch {
				return NotFound();
			}
			return Ok();
		}
	}
}