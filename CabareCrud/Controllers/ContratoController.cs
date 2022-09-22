using BusesControl.Filter;
using BusesControl.Models;
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
            modelsContrato.ClienteFisicoList = _clienteRepositorio.BuscarTodosHabilitados();
            modelsContrato.ClienteJuridicoList = _clienteRepositorio.BuscarTodosHabJuridico();
            return View(modelsContrato);
        }

        [HttpPost]
        public IActionResult NovoContrato(ModelsContrato modelsContrato) {
            ViewData["Title"] = "Novo contrato";
            try {
                modelsContrato.OnibusList = _onibusRepositorio.ListarTodosHab();
                modelsContrato.MotoristaList = _funcionarioRepositorio.ListarTodosMotoristasHab();
                modelsContrato.ClienteFisicoList = _clienteRepositorio.BuscarTodosHabilitados();
                modelsContrato.ClienteJuridicoList = _clienteRepositorio.BuscarTodosHabJuridico();

                Contrato contrato = modelsContrato.Contrato;
                modelsContrato.Contrato = contrato;

                if (!ModelState.IsValid) {
                    TempData["MensagemDeErro"] = "Informe os campos obrigatórios!";
                    return View(modelsContrato);
                }
                _contratoRepositorio.Adicionar(contrato);
                TempData["MensagemDeSucesso"] = "Registrado com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return View(modelsContrato);
            }
        }
    }
}
