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
            relatorio.ValTotAprovados = _contratoRepositorio.ValorTotAprovados();
            relatorio.ValTotEmAnalise = _contratoRepositorio.ValorTotEmAnalise();
            relatorio.ValTotContratos = _contratoRepositorio.ValorTotContratos();
            return View(relatorio);
        }
    }
}
