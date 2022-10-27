using BusesControl.Filter;
using BusesControl.Models;
using BusesControl.Models.Enums;
using BusesControl.Models.ViewModels;
using BusesControl.Repositorio;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            List<Contrato> ListContrato = _contratoRepositorio.ListContratoAtivo();
            return View(ListContrato);
        }
        public IActionResult Inativos() {
            ViewData["Title"] = "Contrato inativos";
            List<Contrato> ListContrato = _contratoRepositorio.ListContratoInativo();
            return View("Index", ListContrato);
        }

        public IActionResult NovoContrato() {
            TempData["MensagemDeInfo"] = "Nº de parcelas não pode ultrapassar a quantidade de meses do contrato.";
            ViewData["Title"] = "Novo contrato";
            ModelsContrato modelsContrato = new ModelsContrato {
                OnibusList = _onibusRepositorio.ListarTodosHab(),
                MotoristaList = _funcionarioRepositorio.ListarTodosMotoristasHab(),
                ClienteFisicoList = _clienteRepositorio.ListClienteFisicoLegal(),
                ClienteJuridicoList = _clienteRepositorio.ListClienteJuridicoLegal()
            };
            Contrato contrato = new Contrato {
                DataEmissao = DateTime.Now
            };
            modelsContrato.Contrato = contrato;
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
                    if (!contrato.ValidarValorMonetario()) {
                        TempData["MensagemDeErro"] = "Valor monetário menor que R$ 150.00!";
                        return View(modelsContrato);
                    }
                    if (ValidationDateEmissaoAndVencimento(contrato)) {
                        TempData["MensagemDeErro"] = "Data de vencimento anterior à data de emissão!";
                        return View(modelsContrato);
                    }
                    if (ValidationDateVencimento(contrato.DataVencimento.ToString())) {
                        TempData["MensagemDeErro"] = "O contrato não pode ser superior a dois anos!";
                        return View(modelsContrato);
                    }
                    if (ValidationQtParcelas(contrato)) {
                        TempData["MensagemDeErro"] = "Quantidade de parcelas inválida!";
                        return View(modelsContrato);
                    }
                    contrato.StatusContrato = ContratoStatus.Ativo;
                    contrato.Aprovacao = StatusAprovacao.EmAnalise;
                    //Colocando a data atual novamente como medida de proteção em casos que o usuário desabilite a restrição do input pelo inspecionar. 
                    contrato.DataEmissao = DateTime.Now;
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

        public IActionResult EditarContrato(int id) {
            ViewData["Title"] = "Editar contrato";
            ModelsContrato modelsContrato = new ModelsContrato {
                ClienteFisicoList = _clienteRepositorio.ListClienteFisicoLegal(),
                ClienteJuridicoList = _clienteRepositorio.ListClienteJuridicoLegal(),
                MotoristaList = _funcionarioRepositorio.ListarTodosMotoristasHab(),
                OnibusList = _onibusRepositorio.ListarTodosHab(),
                Contrato = _contratoRepositorio.ListarPorId(id)
            };
            if (modelsContrato.Contrato == null) {
                TempData["MensagemDeErro"] = "Desculpe, ID não foi encontrado.";
                return View(modelsContrato);
            }
            return View(modelsContrato);
        }
        [HttpPost]
        public IActionResult EditarContrato(ModelsContrato modelsContrato) {
            ViewData["Title"] = "Editar contrato";
            try {
                modelsContrato.ClienteJuridicoList = _clienteRepositorio.ListClienteJuridicoLegal();
                modelsContrato.ClienteFisicoList = _clienteRepositorio.ListClienteFisicoLegal();
                modelsContrato.MotoristaList = _funcionarioRepositorio.ListarTodosMotoristasHab();
                modelsContrato.OnibusList = _onibusRepositorio.ListarTodosHab();

                Contrato contrato = modelsContrato.Contrato;
                modelsContrato.Contrato = contrato;
                if (ModelState.IsValid) {
                    if (!contrato.ValidarValorMonetario()) {
                        TempData["MensagemDeErro"] = "Valor monetário menor que R$ 150.00!";
                        return View(modelsContrato);
                    }
                    if (ValidationDateEmissaoAndVencimento(contrato)) {
                        TempData["MensagemDeErro"] = "Data de vencimento anterior à data de emissão!";
                        return View(modelsContrato);
                    }
                    if (ValidationDateVencimento(contrato.DataVencimento.ToString())) {
                        TempData["MensagemDeErro"] = "O contrato não pode ser superior a dois anos!";
                        return View(modelsContrato);
                    }
                    if (ValidationQtParcelas(contrato)) {
                        TempData["MensagemDeErro"] = "Quantidade de parcelas inválida!";
                        return View(modelsContrato);
                    }
                    _contratoRepositorio.EditarContrato(contrato);
                    TempData["MensagemDeSucesso"] = "Editado com sucesso!";
                    return RedirectToAction("Index");
                }
                return View(modelsContrato);
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return View(modelsContrato);
            }
        }

        public IActionResult Inativar(int id) {
            ViewData["Title"] = "Inativar contrato";
            //Objeto criado para receber os id´s necessários e passar para viewModel.
            Contrato contrato = _contratoRepositorio.ListarJoinPorId(id);
            ModelsContrato modelsContratoError = new ModelsContrato();
            if (contrato == null) {
                TempData["MensagemDeErro"] = "Desculpe, ID não foi encontrado.";
                return View(modelsContratoError);
            }
            ModelsContrato modelsContrato = new ModelsContrato {
                DetalhesClienteView = _clienteRepositorio.ReturnDetalhesCliente((int)contrato.ClienteId),
                Contrato = contrato
            };
            return View(modelsContrato);
        }
        [HttpPost]
        public IActionResult Inativar(ModelsContrato modelsContrato) {
            ViewData["Title"] = "Inativar contrato";
            try {
                Contrato contrato = modelsContrato.Contrato;
                _contratoRepositorio.InativarContrato(contrato);
                TempData["MensagemDeSucesso"] = "Inativado com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return View(modelsContrato);
            }
        }

        public IActionResult Ativar(int id) {
            ViewData["Title"] = "Ativar contrato";
            //Objeto criado para receber os Ids necessários e passar para viewModel.
            Contrato contrato = _contratoRepositorio.ListarJoinPorId(id);
            ModelsContrato modelsContratoError = new ModelsContrato();
            if (contrato == null) {
                TempData["MensagemDeErro"] = "Desculpe, ID não foi encontrado.";
                return View(modelsContratoError);
            }
            ModelsContrato modelsContrato = new ModelsContrato {
                DetalhesClienteView = _clienteRepositorio.ReturnDetalhesCliente((int)contrato.ClienteId),
                Contrato = contrato
            };
            return View(modelsContrato);
        }
        [HttpPost]
        public IActionResult Ativar(ModelsContrato modelsContrato) {
            ViewData["Title"] = "Ativar contrato";
            try {
                Contrato contrato = modelsContrato.Contrato;
                _contratoRepositorio.AtivarContrato(contrato);
                TempData["MensagemDeSucesso"] = "Ativado com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return View(modelsContrato);
            }
        }
        public bool ValidarCampo(Contrato contrato) {

            if (contrato.ClienteId == null || contrato.MotoristaId == null || contrato.OnibusId == null
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
        public bool ValidationQtParcelas(Contrato contrato) {

            DateTime dateVencimento = DateTime.Parse(contrato.DataVencimento.ToString());
            DateTime dataEmissao = DateTime.Parse(contrato.DataEmissao.ToString());

            float dias = (float)dateVencimento.Subtract(dataEmissao).TotalDays;
            float ano = dias / 365;
            bool resultado = (contrato.QtParcelas > ano * 12) ? true : false;
            return resultado;
        }
    }
}