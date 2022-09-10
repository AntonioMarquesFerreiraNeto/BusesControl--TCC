using BusesControl.Filter;
using Microsoft.AspNetCore.Mvc;

namespace BusesControl.Controllers {
    public class AprovarContratoController : Controller {
        [PagUserAdmin]
        public ActionResult Index() {
            ViewData["Title"] = "Aprovar Contratos";
            return View();
        }
    }
}
