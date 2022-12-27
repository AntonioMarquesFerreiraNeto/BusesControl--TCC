using BusesControl.Filter;
using BusesControl.Models;
using BusesControl.Models.ViewModels;
using BusesControl.Repositorio;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BusesControl.Controllers {
    [PagUserAdmin]
    public class RelatorioController : Controller {

        private readonly IRelatorioRepositorio _relatorioRepositorio;
        private readonly IContratoRepositorio _contratoRepositorio;

        public RelatorioController(IRelatorioRepositorio relatorioRepositorio, IContratoRepositorio contratoRepositorio) { 
            _relatorioRepositorio = relatorioRepositorio;
            _contratoRepositorio = contratoRepositorio;
        }

        public IActionResult Index() {
            ViewData["Title"] = "Relatórios do negócio";
            ModelsRelatorio modelsRelatorio = new ModelsRelatorio();
            modelsRelatorio.Relatorio = PopularRelatorio();
            modelsRelatorio.Contratos = _contratoRepositorio.ListContratoAprovados();
            return View(modelsRelatorio);
        }
        public Relatorio PopularRelatorio() {
            Relatorio relatorio = new Relatorio();
            relatorio.ValTotAprovados = _relatorioRepositorio.ValorTotAprovados();
            relatorio.ValTotEmAnalise = _relatorioRepositorio.ValorTotEmAnalise();
            relatorio.ValTotContratos = _relatorioRepositorio.ValorTotContratos();
            relatorio.QtContratos = _relatorioRepositorio.QtContratos();
            relatorio.QtContratosAprovados = _relatorioRepositorio.QtContratosAprovados();
            relatorio.QtContratosNegados = _relatorioRepositorio.QtContratosNegados();
            relatorio.QtContratosEmAnalise = _relatorioRepositorio.QtContratosEmAnalise();
            relatorio.QtContratosAdimplente = _relatorioRepositorio.QtContratosAdimplentes();
            relatorio.QtContratosInadimplentes = _relatorioRepositorio.QtContratosInadimplentes();
            relatorio.QtClientes = _relatorioRepositorio.QtClientes();
            relatorio.QtClientesAdimplente = _relatorioRepositorio.QtClientesAdimplentes();
            relatorio.QtClientesInadimplente = _relatorioRepositorio.QtClientesInadimplentes();
            relatorio.QtMotorista = _relatorioRepositorio.QtMotoristas();
            relatorio.QtMotoristaVinculado = _relatorioRepositorio.QtMotoristasVinculados();
            relatorio.QtOnibus = _relatorioRepositorio.QtOnibus();
            relatorio.QtOnibusVinculado = _relatorioRepositorio.QtOnibusVinculados();
            return relatorio;
        }

        public IActionResult ClientesContrato(int? id) {
            Contrato contrato = _contratoRepositorio.ListarJoinPorIdAprovado(id);
            List<Contrato> listContratos = _contratoRepositorio.ListContratoAprovados();
            if (contrato == null) {
                TempData["MensagemDeErro"] = "Desculpe, ID não foi encontrado.";
                return View("Index", listContratos);
            }
            return PartialView("_ClientesContrato", contrato);
        }
    }
}
