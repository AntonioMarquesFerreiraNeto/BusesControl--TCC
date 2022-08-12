using Microsoft.AspNetCore.Mvc;

namespace BusesControl.Controllers {
    public class FrotaController : Controller {
        public IActionResult index() {
            ViewData["Title"] = "Frota";
            return View();
        }

    }
}
