using Microsoft.AspNetCore.Mvc;

namespace BusesControl.Controllers {
    public class AprovarContratoController : Controller {
        public ActionResult Index() {
            ViewData["Title"] = "Aprovar Contratos";
            return View();
        }
    }
}
