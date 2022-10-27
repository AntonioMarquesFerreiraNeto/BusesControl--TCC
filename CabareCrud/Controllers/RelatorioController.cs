using BusesControl.Filter;
using BusesControl.Models;
using BusesControl.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace BusesControl.Controllers {
    [PagUserAdmin]
    public class RelatorioController : Controller {
        private readonly IContratoRepositorio _contratoRepositorio;
        public RelatorioController(IContratoRepositorio contratoRepositorio) { 
            _contratoRepositorio = contratoRepositorio;
        }
        public IActionResult Index() {
            ViewData["Title"] = "Relatórios";
            Relatorio relatorio = new Relatorio();
            relatorio.ValorMonetarioTotal = _contratoRepositorio.ValorTotalContrato();
            return View(relatorio);
        }
    }
}
