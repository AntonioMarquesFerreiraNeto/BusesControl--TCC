using Microsoft.AspNetCore.Mvc;

namespace BusesControl.Controllers {
    public class FuncionarioController : Controller {

        public IActionResult Index() {
            ViewData["Title"] = "Funcionários";
            return View();
        }

    }
}
