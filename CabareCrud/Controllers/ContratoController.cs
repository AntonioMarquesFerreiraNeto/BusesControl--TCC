using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BusesControl.Controllers {
    public class ContratoController : Controller {
        public IActionResult Index() {
            ViewData["Title"] = "Contratos ativos";
            return View();
        }
        public async Task<IActionResult> NovoContrato() {
            ViewData["Title"] = "Novo contrato";
            return View();
        }

        public async Task<IActionResult> Desabilitados() {
            ViewData["Title"] = "Contratos inativos";
            return View("Index");
        }
    }
}
