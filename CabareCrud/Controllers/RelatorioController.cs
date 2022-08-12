using Microsoft.AspNetCore.Mvc;

namespace BusesControl.Controllers {
    public class RelatorioController : Controller {
        public IActionResult Index() {
            ViewData["Title"] = "Relatórios";
            return View();
        }
    }
}
