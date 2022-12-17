using BusesControl.Filter;
using BusesControl.Models;
using BusesControl.Models.ViewModels;
using BusesControl.Repositorio;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusesControl.Controllers {
    [PagUserAdmin]
    public class FinanceiroController : Controller {
        private readonly IContratoRepositorio _contratoRepositorio;
        private readonly IFinanceiroRepositorio _financeiroRepositorio;

        public FinanceiroController(IContratoRepositorio contratoRepositorio, IFinanceiroRepositorio financeiroRepositorio) {
            _contratoRepositorio = contratoRepositorio;
            _financeiroRepositorio = financeiroRepositorio;
        }

        public IActionResult Index() {
            ViewData["Title"] = "Financeiro - adimplentes";
            List<Contrato> ListContratos = _contratoRepositorio.ListContratoAdimplentes();
            return View(ListContratos);
        }
        public IActionResult ListInadimplentes() {
            ViewData["Title"] = "Financeiro - inadimplentes";
            List<Contrato> ListContratos = _contratoRepositorio.ListContratoInadimplentes();
            return View("Index", ListContratos);
        }

        public IActionResult FinanceiroContrato(int id) {
            ViewData["Title"] = $"Financeiro - contrato Nº {id}";
            Contrato contrato = _contratoRepositorio.ListarJoinPorId(id);
            if (contrato == null) {
                TempData["MensagemDeErro"] = "Desculpe, ID não foi encontrado!";
                return RedirectToAction("Index");
            }
            return View(contrato);
        }

        public IActionResult Contabilizar(int? id) {
            ClientesContrato clientesContrato = _financeiroRepositorio.listPorIdClientesContrato(id);
            if (clientesContrato == null) {
                TempData["MensagemDeErro"] = "Desculpe, ID não foi encontrado!";
                return RedirectToAction("Index");
            }
            string name = (clientesContrato.PessoaFisica != null) ? clientesContrato.PessoaFisica.Name : clientesContrato.PessoaJuridica.RazaoSocial;
            ViewData["Title"] = $"Financeiro contrato Nº {clientesContrato.ContratoId} - {name}";
            clientesContrato.ParcelasContrato = clientesContrato.ParcelasContrato.OrderBy(x => x.DataVencimentoParcela.Value).ToList();
            return View(clientesContrato);
        }

        [HttpPost]
        public IActionResult Contabilizar(int id, int clientescontratoId) {
            try {
                _financeiroRepositorio.ContabilizarFinanceiro(id);
                ClientesContrato clientesContrato = _financeiroRepositorio.listPorIdClientesContrato(clientescontratoId);
                if (clientesContrato != null) {
                    string name = (clientesContrato.PessoaFisica != null) ? clientesContrato.PessoaFisica.Name : clientesContrato.PessoaJuridica.RazaoSocial;
                    TempData["MensagemDeSucesso"] = "Contabilizado com sucesso!";
                    ViewData["Title"] = $"Financeiro contrato Nº {clientesContrato.ContratoId} - {name}";
                    clientesContrato.ParcelasContrato = clientesContrato.ParcelasContrato.OrderBy(x => x.DataVencimentoParcela.Value).ToList();
                    return View(clientesContrato);
                }
                return RedirectToAction("Index");
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return RedirectToAction("Index");
            }
        }
    }
}
