using BusesControl.Filter;
using BusesControl.Models;
using BusesControl.Models.Enums;
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
        private readonly IFornecedorRepositorio _fornecedorRepositorio;
        private readonly IRelatorioRepositorio _relatorioRepositorio;

        public FinanceiroController(IContratoRepositorio contratoRepositorio, IFinanceiroRepositorio financeiroRepositorio, 
                IClienteRepositorio clienteRepositorio, IFornecedorRepositorio fornecedorRepositorio, IRelatorioRepositorio relatorioRepositorio) {
            _contratoRepositorio = contratoRepositorio;
            _financeiroRepositorio = financeiroRepositorio;
            _clienteRepositorio = clienteRepositorio;
            _fornecedorRepositorio = fornecedorRepositorio;
            _relatorioRepositorio = relatorioRepositorio;
        }

        public IActionResult Index() {
            ViewData["Title"] = "Financeiro";
            _financeiroRepositorio.TaskMonitorParcelas();
            List<Financeiro> ListFinanceiro = _financeiroRepositorio.ListFinanceiros();
            return View(ListFinanceiro);
        }

        public IActionResult ReturnDashFinanceiro() {
            Relatorio relatorio = new Relatorio();
            relatorio.ValTotReceitas = _relatorioRepositorio.ValorTotReceitas();
            relatorio.ValTotDespesas = _relatorioRepositorio.ValorTotDespesas();
            relatorio.ValTotEfetuadoDespesa = _relatorioRepositorio.ValorTotPagoDespesas();
            relatorio.ValTotEfetuadoReceita = _relatorioRepositorio.ValorTotPagoReceitas();
            return PartialView("_ReturnDashView", relatorio);
        }

        public IActionResult NovaDespesa() {
            ViewData["Title"] = "Nova despesa";
            ModelsFinanceiroRD modelsFinanceiroRD = new ModelsFinanceiroRD();
            modelsFinanceiroRD.CredorFisicoList = _fornecedorRepositorio.ListFornecedoreFisicos();
            modelsFinanceiroRD.CredorJuridicoList = _fornecedorRepositorio.ListFornecedoresJuridicos();
            Financeiro financeiro = new Financeiro {
                Pagament = ModelPagament.Avista,
                DataEmissao = DateTime.Now
            };
            modelsFinanceiroRD.Financeiro = financeiro;
            return View(modelsFinanceiroRD);
        }

        [HttpPost]
        public IActionResult NovaDespesa(ModelsFinanceiroRD modelsFinanceiroRD) {
            ViewData["Title"] = "Nova despesa";
            modelsFinanceiroRD.CredorFisicoList = _fornecedorRepositorio.ListFornecedoreFisicos();
            modelsFinanceiroRD.CredorJuridicoList = _fornecedorRepositorio.ListFornecedoresJuridicos();
            try {
                int op = int.Parse(Request.Form["format_pagament"]);
                modelsFinanceiroRD.Financeiro.Pagament = (op == 0) ? ModelPagament.Avista : ModelPagament.Parcelado;
                if (op == 0) {
                    modelsFinanceiroRD.Financeiro.QtParcelas = 1;
                }
                if (ModelState.IsValid) {
                    FornecedorFisico fornecedorFisico = _fornecedorRepositorio.ListPorIdFisico(modelsFinanceiroRD.CredorDevedorId.Value);
                    if (fornecedorFisico != null) {
                        modelsFinanceiroRD.Financeiro.FornecedorFisicoId = modelsFinanceiroRD.CredorDevedorId.Value;
                    }
                    else {
                        modelsFinanceiroRD.Financeiro.FornecedorJuridicoId = modelsFinanceiroRD.CredorDevedorId.Value;
                    }
                    if (ValidationDateEmissaoAndVencimento(modelsFinanceiroRD.Financeiro)) {
                        TempData["MensagemDeErro"] = "Data de vencimento anterior à data de emissão!";
                        return View(modelsFinanceiroRD);
                    }
                    if (ValidationQtParcelas(modelsFinanceiroRD.Financeiro)) {
                        TempData["MensagemDeErro"] = "Quantidade de parcelas inválida!";
                        return View(modelsFinanceiroRD);
                    }
                    if (ValidationDateVencimento(modelsFinanceiroRD.Financeiro.DataVencimento.ToString())) {
                        TempData["MensagemDeErro"] = "A receita/despesa não pode ser superior a dois anos!";
                        return View(modelsFinanceiroRD);
                    }
                    _financeiroRepositorio.AdicionarDespesa(modelsFinanceiroRD.Financeiro);
                    TempData["MensagemDeSucesso"] = "Registrado com sucesso!";
                    return RedirectToAction("Index");
                }
                return View(modelsFinanceiroRD);
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = $"{erro.Message}";
                return View(modelsFinanceiroRD);
            }
        }

        public IActionResult NovaReceita() {
            ViewData["Title"] = "Nova receita";
            ModelsFinanceiroRD modelsFinanceiroRD = new ModelsFinanceiroRD();
            modelsFinanceiroRD.PessoaFisicoList = _clienteRepositorio.ListClienteFisicoLegalContrato();
            modelsFinanceiroRD.PessoaJuridicaList = _clienteRepositorio.ListClienteJuridicoLegal();
            Financeiro financeiro = new Financeiro {
                Pagament = ModelPagament.Avista,
                DataEmissao = DateTime.Now
            };
            modelsFinanceiroRD.Financeiro = financeiro;
            return View(modelsFinanceiroRD);
        }
        [HttpPost]
        public IActionResult NovaReceita(ModelsFinanceiroRD modelsFinanceiroRD) {
            ViewData["Title"] = "Nova receita";
            modelsFinanceiroRD.PessoaFisicoList = _clienteRepositorio.ListClienteFisicoLegalContrato();
            modelsFinanceiroRD.PessoaJuridicaList = _clienteRepositorio.ListClienteJuridicoLegal();
            try {
                int op = int.Parse(Request.Form["format_pagament"]);
                modelsFinanceiroRD.Financeiro.Pagament = (op == 0) ? ModelPagament.Avista : ModelPagament.Parcelado;
                if (op == 0) {
                    modelsFinanceiroRD.Financeiro.QtParcelas = 1;
                }
                if (ModelState.IsValid) {
                    PessoaFisica pessoaFisica = _clienteRepositorio.ListarPorId(modelsFinanceiroRD.CredorDevedorId.Value);
                    if (pessoaFisica != null) {
                        modelsFinanceiroRD.Financeiro.PessoaFisicaId = modelsFinanceiroRD.CredorDevedorId.Value;
                    }
                    else {
                        modelsFinanceiroRD.Financeiro.PessoaJuridicaId = modelsFinanceiroRD.CredorDevedorId.Value;
                    }
                    if (ValidationDateEmissaoAndVencimento(modelsFinanceiroRD.Financeiro)) {
                        TempData["MensagemDeErro"] = "Data de vencimento anterior à data de emissão!";
                        return View(modelsFinanceiroRD);
                    }
                    if (ValidationQtParcelas(modelsFinanceiroRD.Financeiro)) {
                        TempData["MensagemDeErro"] = "Quantidade de parcelas inválida!";
                        return View(modelsFinanceiroRD);
                    }
                    if (ValidationDateVencimento(modelsFinanceiroRD.Financeiro.DataVencimento.ToString())) {
                        TempData["MensagemDeErro"] = "A receita/despesa não pode ser superior a dois anos!";
                        return View(modelsFinanceiroRD);
                    }
                    _financeiroRepositorio.AdicionarReceita(modelsFinanceiroRD.Financeiro);
                    TempData["MensagemDeSucesso"] = "Registrado com sucesso!";
                    return RedirectToAction("Index");
                }
                return View(modelsFinanceiroRD);
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = $"{erro.Message}";
                return View(modelsFinanceiroRD);
            }
        }


        public IActionResult InativarLancamento(int? id) {
            Financeiro financeiro = _financeiroRepositorio.listPorIdFinanceiro(id);
            return PartialView("_InativarLancamento", financeiro);
        }

        [HttpPost]
        public IActionResult InativarLancamento(Financeiro financeiro) {
            try {
                if (financeiro != null) {
                    _financeiroRepositorio.InativarReceitaOrDespesa(financeiro);
                    TempData["MensagemDeSucesso"] = "Inativado com sucesso!";
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return RedirectToAction("Index");
            }
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
            Financeiro financeiro = _financeiroRepositorio.listPorIdFinanceiro(id);
            return PartialView("_RescisaoContrato", financeiro);
        }

        [HttpPost]
        public IActionResult Rescendir(Financeiro financeiro) {
            try {

                if (financeiro != null) {
                    _financeiroRepositorio.RescisaoContrato(financeiro);
                    TempData["MensagemDeSucesso"] = "Rescisão realizado com sucesso!";
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return View("Index");
            }
        }

        public IActionResult Contabilizar(int? id) {
            Financeiro financeiro = _financeiroRepositorio.listPorIdFinanceiro(id);
            if (financeiro == null) {
                TempData["MensagemDeErro"] = "Desculpe, ID não foi encontrado!";
                return RedirectToAction("Index");
            }
            string name;
            if (!string.IsNullOrEmpty(financeiro.ContratoId.ToString())) {
                name = (!string.IsNullOrEmpty(financeiro.PessoaFisicaId.ToString())) ? financeiro.PessoaFisica.Name : financeiro.PessoaJuridica.RazaoSocial;
                ViewData["Title"] = $"Parcelas contrato Nº {financeiro.ContratoId} – {name}";
            }
            else {
                if (!string.IsNullOrEmpty(financeiro.PessoaFisicaId.ToString()) || !string.IsNullOrEmpty(financeiro.PessoaJuridicaId.ToString())) {
                    name = (!string.IsNullOrEmpty(financeiro.PessoaFisicaId.ToString())) ? financeiro.PessoaFisica.Name : financeiro.PessoaJuridica.RazaoSocial;
                    ViewData["Title"] = $"Parcelas – {name}";
                }
                else {
                    name = (!string.IsNullOrEmpty(financeiro.FornecedorFisicoId.ToString())) ? financeiro.FornecedorFisico.Name : financeiro.FornecedorJuridico.RazaoSocial;
                    ViewData["Title"] = $"Parcelas – {name}";
                }
            }
            financeiro.Parcelas = financeiro.Parcelas.OrderBy(x => x.DataVencimentoParcela.Value).ToList();
            return View(financeiro);
        }

        [HttpPost]
        public IActionResult Contabilizar(int parcelaId, int financeiroId) {
            try {
                _financeiroRepositorio.ContabilizarParcela(parcelaId);
                Financeiro financeiro = _financeiroRepositorio.listPorIdFinanceiro(financeiroId);
                if (financeiro != null) {
                    string name;
                    if (!string.IsNullOrEmpty(financeiro.ContratoId.ToString())) {
                        name = (!string.IsNullOrEmpty(financeiro.PessoaFisicaId.ToString())) ? financeiro.PessoaFisica.Name : financeiro.PessoaJuridica.RazaoSocial;
                        ViewData["Title"] = $"Parcelas contrato Nº {financeiro.ContratoId} – {name}";
                    }
                    else {
                        if (!string.IsNullOrEmpty(financeiro.PessoaFisicaId.ToString()) || !string.IsNullOrEmpty(financeiro.PessoaJuridicaId.ToString())) {
                            name = (!string.IsNullOrEmpty(financeiro.PessoaFisicaId.ToString())) ? financeiro.PessoaFisica.Name : financeiro.PessoaJuridica.RazaoSocial;
                            ViewData["Title"] = $"Parcelas – {name}";
                        }
                        else {
                            name = (!string.IsNullOrEmpty(financeiro.FornecedorFisicoId.ToString())) ? financeiro.FornecedorFisico.Name : financeiro.FornecedorJuridico.RazaoSocial;
                            ViewData["Title"] = $"Parcelas – {name}";
                        }
                    }
                    TempData["MensagemDeSucesso"] = "Contabilizado com sucesso!";
                    financeiro.Parcelas = financeiro.Parcelas.OrderBy(x => x.DataVencimentoParcela.Value).ToList();
                    return View(financeiro);
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

        public bool ValidationDateEmissaoAndVencimento(Financeiro financeiro) {
            if (financeiro.DataEmissao >= financeiro.DataVencimento) {
                return true;
            }
            return false;
        }
        public bool ValidationDateVencimento(string value) {
            DateTime dataVencimento = DateTime.Parse(value);
            DateTime dataAtual = DateTime.Now;

            long dias = (int)dataVencimento.Subtract(dataAtual).TotalDays;
            long tempoValidation = dias / 365;

            if (tempoValidation >= 2) {
                return true;
            }
            return false;
        }
        public bool ValidationQtParcelas(Financeiro financeiro) {

            DateTime dateVencimento = DateTime.Parse(financeiro.DataVencimento.ToString());
            DateTime dataEmissao = DateTime.Parse(financeiro.DataEmissao.ToString());

            float dias = (float)dateVencimento.Subtract(dataEmissao).TotalDays;
            float ano = dias / 365;
            if (financeiro.Pagament == ModelPagament.Parcelado) {
                bool resultado = (financeiro.QtParcelas > ano * 12 || financeiro.QtParcelas < 2 || string.IsNullOrEmpty(financeiro.QtParcelas.ToString())) ? true : false;
                return resultado;
            }
            else {
                bool resultado = (financeiro.QtParcelas < 1 || string.IsNullOrEmpty(financeiro.QtParcelas.ToString())) ? true : false;
                return resultado;
            }
        }
    }
}
