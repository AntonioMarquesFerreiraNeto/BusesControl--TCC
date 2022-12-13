using BusesControl.Filter;
using BusesControl.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BusesControl.Controllers {
    [PagUserAutenticado]
    public class HomeController : Controller {
        public IActionResult Index() {
            ViewData["Title"] = "Página principal";
            return View();
        }
    }
}
