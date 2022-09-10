using BusesControl.Filter;
using Microsoft.AspNetCore.Mvc;

namespace BusesControl.Controllers {
    [PagUserAdmin]
    public class FinanceiroController : Controller {
        public ActionResult Index() {
            ViewData["Title"] = "Financeiro";
            return View();
        }
    }
}
