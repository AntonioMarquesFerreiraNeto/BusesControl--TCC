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

        public ContratoController(IOnibusRepositorio onibusRepositorio, IFuncionarioRepositorio funcionarioRepositorio,
                IClienteRepositorio clienteRepositorio) {

            _onibusRepositorio = onibusRepositorio;
            _funcionarioRepositorio = funcionarioRepositorio;
            _clienteRepositorio = clienteRepositorio;
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

            List<Onibus> onibusList = _onibusRepositorio.ListarTodosHab();
            List<Funcionario> motoristaList = _funcionarioRepositorio.ListarTodosMotoristasHab();
            List<PessoaFisica> pessoaFisicaList = _clienteRepositorio.BuscarTodosHabilitados();
            List<PessoaJuridica> pessoaJuridicaList = _clienteRepositorio.BuscarTodosHabJuridico();

            modelsContrato.OnibusList = onibusList;
            modelsContrato.MotoristaList = motoristaList;
            modelsContrato.ClienteFisicoList = pessoaFisicaList;
            modelsContrato.ClienteJuridicoList = pessoaJuridicaList;
            return View(modelsContrato);
        }
    }
}
