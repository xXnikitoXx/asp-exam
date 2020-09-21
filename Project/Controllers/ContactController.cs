using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Project.Controllers
{
	public class ContactController : Controller
	{
		[HttpGet("/contact")]
		public IActionResult Index() => this.View();

		public IActionResult Create() => this.View();

		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Create(IFormCollection collection)
		{
			try
			{
				return RedirectToAction(nameof(Index));
			}
			catch
			{
				return View();
			}
		}

		[HttpGet("/privacy")]
		public IActionResult Privacy() => this.View();

		[HttpGet("/terms")]
		public IActionResult Terms() => this.View();
	}
}
