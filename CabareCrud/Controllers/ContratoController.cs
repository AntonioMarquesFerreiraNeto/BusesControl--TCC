using Microsoft.AspNetCore.Mvc;

namespace BusesControl.Controllers {
    public class ContratoController : Controller {
        public IActionResult Index() {
            ViewData["Title"] = "Contratos";
            return View();
        }
    }
}
