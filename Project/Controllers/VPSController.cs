﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Project.Controllers
{
    public class VPSController : Controller
    {
        public IActionResult Index() => View();

        public IActionResult Manage(string id)
        {
            return View();
        }
    }
}
