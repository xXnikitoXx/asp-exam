using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Project.Controllers
{
	public class AccountController : Controller
	{
		[HttpGet("/account")]
		[Authorize]
		public IActionResult Index() => this.View();

		[HttpGet("/profile")]
		[Authorize]
		public IActionResult Profile() => this.Redirect("/account");

		public IActionResult Settings() => this.Redirect("/Identity/Account");

		[HttpGet("/login")]
		public IActionResult Login() => this.Redirect("/Identity/Account/Login");

		[HttpGet("/register")]
		public IActionResult Register() => this.Redirect("/Identity/Account/Register");
	}
}
