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
        private readonly IClienteRepositorio _clienteRepositorio;

        public FinanceiroController(IContratoRepositorio contratoRepositorio, IFinanceiroRepositorio financeiroRepositorio, IClienteRepositorio clienteRepositorio) {
            _contratoRepositorio = contratoRepositorio;
            _financeiroRepositorio = financeiroRepositorio;
            _clienteRepositorio = clienteRepositorio;
        }

        public IActionResult Index() {
            ViewData["Title"] = "Financeiro";
            _financeiroRepositorio.TaskMonitorParcelasContrato();
            List<Contrato> ListContratos = _financeiroRepositorio.ContratosEmAndamento();
            return View(ListContratos);
        }
        public IActionResult ContratosEncerrados() {
            ViewData["Title"] = "Financeiro";
            List<Contrato> ListContratos = _financeiroRepositorio.ContratosEncerrados();
            return View("Index", ListContratos);
        }

        public IActionResult FinanceiroContrato(int id) {
            ViewData["Title"] = $"Parcelas – contrato Nº {id}";
            Contrato contrato = _financeiroRepositorio.ListarJoinPorId(id);
            if (contrato == null) {
                TempData["MensagemDeErro"] = "Desculpe, ID não foi encontrado!";
                return RedirectToAction("Index");
            }
            return View(contrato);
        }
        public IActionResult RescendirContrato(int? id) {
            ClientesContrato clientesContrato = _financeiroRepositorio.listPorIdClientesContrato(id);
            return PartialView("_RescisaoContrato", clientesContrato);
        }

        [HttpPost]
        public IActionResult Rescendir(ClientesContrato clientesContrato) {
            try {
                Contrato contrato = _financeiroRepositorio.ListarJoinPorId(clientesContrato.Contrato.Id);
                if (contrato == null) {
                    TempData["MensagemDeErro"] = "Desculpe, ID não foi encontrado!";
                    return RedirectToAction("Index");
                }
                if (clientesContrato != null) {
                    _financeiroRepositorio.RescisaoContrato(clientesContrato);
                    TempData["MensagemDeSucesso"] = "Rescisão realizado com sucesso!";
                    return RedirectToAction("FinanceiroContrato", contrato);
                }
                return View("FinanceiroContrato", contrato);
            }
            catch (Exception erro) {
                Contrato contrato = _financeiroRepositorio.ListarJoinPorId(clientesContrato.Contrato.Id);
                if (contrato == null) {
                    TempData["MensagemDeErro"] = "Desculpe, ID não foi encontrado!";
                    return RedirectToAction("Index");
                }
                else {
                    ViewData["Title"] = $"Parcelas - contrato N º {contrato.Id}";
                    TempData["MensagemDeErro"] = erro.Message;
                    return View("FinanceiroContrato", contrato);
                }
            }
        }

        public IActionResult Contabilizar(int? id) {
            ClientesContrato clientesContrato = _financeiroRepositorio.listPorIdClientesContrato(id);
            if (clientesContrato == null) {
                TempData["MensagemDeErro"] = "Desculpe, ID não foi encontrado!";
                return RedirectToAction("Index");
            }
            string name = (clientesContrato.PessoaFisica != null) ? clientesContrato.PessoaFisica.Name : clientesContrato.PessoaJuridica.RazaoSocial;
            ViewData["Title"] = $"Parcelas contrato Nº {clientesContrato.ContratoId} – {name}";
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
                    ViewData["Title"] = $"Financeiro contrato Nº {clientesContrato.ContratoId} – {name}";
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

        public IActionResult ReturnClienteResponsavel(int? id) {
            PessoaFisica pessoaFisica = _clienteRepositorio.ListarPorId(id.Value);
            if (pessoaFisica != null) {
                if (string.IsNullOrEmpty(pessoaFisica.Email)) {
                    pessoaFisica.Email = "Não foi informado.";
                }
                return PartialView("_ClienteResponsavelFisico", pessoaFisica);
            }
            PessoaJuridica pessoaJuridica = _clienteRepositorio.ListarPorIdJuridico(id.Value);
            if (pessoaJuridica != null) {
                if (string.IsNullOrEmpty(pessoaJuridica.Email)) {
                    pessoaJuridica.Email = "Não foi informado.";
                }
                return PartialView("_ClienteResponsavelJuridico", pessoaJuridica);
            }
            //returna um cliente fisico nulo para que a mensagem de id não encontrado seja captada na página.
            return PartialView("_ClienteResponsavelFisico", pessoaFisica);
        }
    }
}
