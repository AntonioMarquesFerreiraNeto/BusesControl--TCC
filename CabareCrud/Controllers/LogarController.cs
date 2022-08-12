using Microsoft.AspNetCore.Mvc;

namespace BusesControl.Controllers {
    public class LogarController : Controller {
        public ActionResult Index() {
            ViewData["Title"] = "Autenticar";
            return View();
        }
    }
}
