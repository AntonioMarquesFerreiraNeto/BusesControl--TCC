using BusesControl.Filter;
using BusesControl.Models;
using BusesControl.Models.ViewModels;
using BusesControl.Repositorio;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BusesControl.Controllers {
    [PagUserAdmin]
    public class FinanceiroController : Controller {
        private readonly IContratoRepositorio _contratoRepositorio;

        public FinanceiroController(IContratoRepositorio contratoRepositorio) {
            _contratoRepositorio = contratoRepositorio;
        }

        public ActionResult Index() {
            ViewData["Title"] = "Financeiro - adimplentes";
            List<Contrato> ListContratos = _contratoRepositorio.ListContratoAdimplentes();
            return View(ListContratos);
        }
        public ActionResult ListInadimplentes() {
            ViewData["Title"] = "Financeiro - inadimplentes";
            List<Contrato> ListContratos = _contratoRepositorio.ListContratoInadimplentes();
            return View("Index", ListContratos);
        }

        public ActionResult FinanceiroContrato(int id) {
            ViewData["Title"] = $"Financeiro - contrato Nº {id}";
            Contrato contrato = _contratoRepositorio.ListarJoinPorId(id);
            if (contrato == null) {
                TempData["MensagemDeErro"] = "Desculpe, ID não foi encontrado!";
                return RedirectToAction("Index");
            }
            return View(contrato);
        }
    }
}
