﻿using Microsoft.AspNetCore.Mvc;

namespace BusesControl.Controllers {
    public class FuncionarioController : Controller {

        public IActionResult index() {
            ViewData["Title"] = "Funcionários";
            return View();
        }

    }
}
