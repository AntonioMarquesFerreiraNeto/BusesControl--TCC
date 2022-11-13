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
                    if (_clienteRepositorio.PessoaFisicaOrJuridica(modelsContrato.ClienteId.Value)) {
                        contrato.PessoaFisicaId = modelsContrato.ClienteId;
                    }
                    else {
                        contrato.PessoaJuridicaId = modelsContrato.ClienteId;
                    }
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
                return RedirectToAction("Index");
            }
            modelsContrato.ClienteId = IdPessoaFisicaOrJuridica(modelsContrato.Contrato);
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
                if (ModelState.IsValid) {
                    if (!contrato.ValidarValorMonetario()) {
                        TempData["MensagemDeErro"] = "Valor monetário menor que R$ 150.00!";
                        modelsContrato.Contrato = ModelsError(contrato);
                        return View(modelsContrato);
                    }
                    if (ValidationDateEmissaoAndVencimento(contrato)) {
                        TempData["MensagemDeErro"] = "Data de vencimento anterior à data de emissão!";
                        modelsContrato.Contrato = ModelsError(contrato);
                        return View(modelsContrato);
                    }
                    if (ValidationDateVencimento(contrato.DataVencimento.ToString())) {
                        TempData["MensagemDeErro"] = "O contrato não pode ser superior a dois anos!";
                        modelsContrato.Contrato = ModelsError(contrato);
                        return View(modelsContrato);
                    }
                    if (ValidationQtParcelas(contrato)) {
                        TempData["MensagemDeErro"] = "Quantidade de parcelas inválida!";
                        modelsContrato.Contrato = ModelsError(contrato);
                        return View(modelsContrato);
                    }
                    if (_clienteRepositorio.PessoaFisicaOrJuridica(modelsContrato.ClienteId.Value)) {
                        contrato.PessoaFisicaId = modelsContrato.ClienteId;
                        contrato.PessoaJuridicaId = null;
                    }
                    else {
                        contrato.PessoaJuridicaId = modelsContrato.ClienteId;
                        contrato.PessoaFisicaId = null;
                    }
                    _contratoRepositorio.EditarContrato(contrato);
                    TempData["MensagemDeSucesso"] = "Editado com sucesso!";
                    return RedirectToAction("Index");
                }
                modelsContrato.Contrato = ModelsError(contrato);
                return View(modelsContrato);
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                modelsContrato.Contrato = ModelsError(modelsContrato.Contrato);
                return View(modelsContrato);
            }
        }

        public IActionResult Inativar(int id) {
            ViewData["Title"] = "Inativar contrato";
            Contrato contrato = _contratoRepositorio.ListarJoinPorId(id);
            if (contrato == null) {
                TempData["MensagemDeErro"] = "Desculpe, ID não foi encontrado.";
                return View(contrato);
            }
            return View(contrato);
        }
        [HttpPost]
        public IActionResult Inativar(Contrato contrato) {
            ViewData["Title"] = "Inativar contrato";
            try {
                _contratoRepositorio.InativarContrato(contrato);
                TempData["MensagemDeSucesso"] = "Inativado com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return View();
            }
        }

        public IActionResult Ativar(int id) {
            ViewData["Title"] = "Ativar contrato";
            Contrato contrato = _contratoRepositorio.ListarJoinPorId(id);
            if (contrato == null) {
                TempData["MensagemDeErro"] = "Desculpe, ID não foi encontrado.";
                return View(contrato);
            }
            return View(contrato);
        }
        [HttpPost]
        public IActionResult Ativar(Contrato contrato) {
            ViewData["Title"] = "Ativar contrato";
            try {
                _contratoRepositorio.AtivarContrato(contrato);
                TempData["MensagemDeSucesso"] = "Ativado com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return View();
            }
        }

        public bool ValidarCampo(Contrato contrato) {

            if (contrato.MotoristaId == null || contrato.OnibusId == null
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

        public int? IdPessoaFisicaOrJuridica(Contrato contrato) {
            if (!string.IsNullOrEmpty(contrato.PessoaFisicaId.ToString())) {
                return contrato.PessoaFisicaId;
            }
            return contrato.PessoaJuridicaId;
        }

        public Contrato ModelsError(Contrato contrato) {
            //Para não ter problema de referências de na view em momentos de erros.
            contrato = _contratoRepositorio.ListarJoinPorId(contrato.Id);
            return contrato;
        }
    }
}