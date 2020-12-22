using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
	}
}