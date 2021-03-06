using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Project.Models;
using Project.Services.Native;
using Project.ViewModels;

namespace Project.Controllers {
	public class HomeController : Controller {
		private readonly IPlanClient _planService;
		private readonly IMapper _mapper;

		public HomeController(
			IPlanClient service,
			ILogger<HomeController> logger,
			IMapper mapper
		) {
			this._planService = service;
			this._mapper = mapper;
		}

		public IActionResult Index() =>
			View(this._mapper.Map<List<PlanViewModel>>(this._planService.GetPlans()));

		[HttpGet("/Error")]
		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error() =>
			View(new ErrorViewModel { RequestId = System.Diagnostics.Activity.Current?.Id ?? HttpContext.TraceIdentifier });

		[HttpGet("/404")]
		public new IActionResult NotFound() =>
			View();
	}
}
