using BusesControl.Filter;
using BusesControl.Models;
using BusesControl.Models.Enums;
using BusesControl.Models.ViewModels;
using BusesControl.Repositorio;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusesControl.Controllers {
    [PagUserAutenticado]
    public class ContratoController : Controller {
        
        private readonly IOnibusRepositorio _onibusRepositorio;
        private readonly IFuncionarioRepositorio _funcionarioRepositorio;
        private readonly IClienteRepositorio _clienteRepositorio;
        private readonly IContratoRepositorio _contratoRepositorio;

        public ContratoController(IOnibusRepositorio onibusRepositorio, IFuncionarioRepositorio funcionarioRepositorio,
                IClienteRepositorio clienteRepositorio, IContratoRepositorio contratoRepositorio) {

            _onibusRepositorio = onibusRepositorio;
            _funcionarioRepositorio = funcionarioRepositorio;
            _clienteRepositorio = clienteRepositorio;
            _contratoRepositorio = contratoRepositorio;
        }

        public IActionResult Index() {
            ViewData["Title"] = "Contratos ativos";
            return View();
        }

        public IActionResult Desabilitados() {
            ViewData["Title"] = "Contratos inativos";
            return View("Index");
        }

        public IActionResult NovoContrato() {
            ViewData["Title"] = "Novo contrato";
            ModelsContrato modelsContrato = new ModelsContrato();

            modelsContrato.OnibusList = _onibusRepositorio.ListarTodosHab();
            modelsContrato.MotoristaList = _funcionarioRepositorio.ListarTodosMotoristasHab();
            modelsContrato.ClienteFisicoList = _clienteRepositorio.ListClienteFisicoLegal();
            modelsContrato.ClienteJuridicoList = _clienteRepositorio.ListClienteJuridicoLegal();
            return View(modelsContrato);
        }

        [HttpPost]
        public IActionResult NovoContrato(ModelsContrato modelsContrato) {
            ViewData["Title"] = "Novo contrato";
            try {
                modelsContrato.OnibusList = _onibusRepositorio.ListarTodosHab();
                modelsContrato.MotoristaList = _funcionarioRepositorio.ListarTodosMotoristasHab();
                modelsContrato.ClienteFisicoList = _clienteRepositorio.ListClienteFisicoLegal();
                modelsContrato.ClienteJuridicoList = _clienteRepositorio.ListClienteJuridicoLegal();

                Contrato contrato = modelsContrato.Contrato;
                modelsContrato.Contrato = contrato;
                if (ValidarCampo(contrato) != true) {
                    TempData["MensagemDeErro"] = "Informe os campos obrigatórios!";
                    return View(modelsContrato);
                }
                if (ModelState.IsValid) {
                    if (ValidationDateEmissaoAndVencimento(contrato)) {
                        TempData["MensagemDeErro"] = "Data de vencimento anterior à data de emissão!";
                        return View(modelsContrato);
                    }
                    if (ValidationDateVencimento(contrato.DataVencimento.ToString())) {
                        TempData["MensagemDeErro"] = "O contrato não pode ser superior a dois anos!";
                        return View(modelsContrato);
                    }
                    contrato.StatusContrato = ContratoStatus.Ativo;
                    contrato.Aprovacao = StatusAprovacao.EmAnalise;
                    _contratoRepositorio.Adicionar(contrato);
                    TempData["MensagemDeSucesso"] = "Registrado com sucesso!";
                    return RedirectToAction("Index");
                }
                return View(modelsContrato);
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return View(modelsContrato);
            }
        }

        public bool ValidarCampo(Contrato contrato) {

            if (contrato.IdCliente == null || contrato.IdMotorista == null || contrato.IdOnibus == null 
                || contrato.DataEmissao == null || contrato.DataVencimento == null || contrato.Detalhamento == null 
                || contrato.ValorMonetario == null || contrato.QtParcelas == null) {
                return false;
            }
            return true;
        }

        public bool ValidationDateEmissaoAndVencimento(Contrato contrato) {
            if (contrato.DataEmissao >= contrato.DataVencimento) {
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
    }
}
