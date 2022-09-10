using BusesControl.Filter;
using Microsoft.AspNetCore.Mvc;

namespace BusesControl.Controllers {
    [PagUserAdmin]
    public class RelatorioController : Controller {
        public IActionResult Index() {
            ViewData["Title"] = "Relatórios";
            return View();
        }
    }
}
