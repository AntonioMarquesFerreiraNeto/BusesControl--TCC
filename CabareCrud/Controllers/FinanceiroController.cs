using Microsoft.AspNetCore.Mvc;

namespace BusesControl.Controllers {
    public class FinanceiroController : Controller {
        public ActionResult Index() {
            ViewData["Title"] = "Financeiro";
            return View();
        }
    }
}
