using BusesControl.Filter;
using BusesControl.Models;
using BusesControl.Models.ViewModels;
using BusesControl.Repositorio;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace BusesControl.Controllers {
    [PagUserAdmin]
    public class AprovarContratoController : Controller {
        private readonly IContratoRepositorio _contratoRepositorio;

        public AprovarContratoController(IContratoRepositorio contratoRepositorio) {
            _contratoRepositorio = contratoRepositorio;
        }

        public IActionResult Index() {
            List<Contrato> ContratosList = _contratoRepositorio.ListContratoEmAnalise();
            ViewData["Title"] = "Contratos em análise";
            return View(ContratosList);
        }

        public IActionResult Negados() {
            List<Contrato> ContratoList = _contratoRepositorio.ListContratoNegados();
            ViewData["Title"] = "Contratos negados";
            return View("Index", ContratoList);
        }

        public IActionResult Aprovados() {
            List<Contrato> ContratoList = _contratoRepositorio.ListContratoAprovados();
            ViewData["Title"] = "Contratos aprovados";
            return View("Index", ContratoList);
        }

        public IActionResult AprovarContrato(int id) {
            ViewData["Title"] = "Aprovar contrato";
            Contrato contrato = _contratoRepositorio.ListarJoinPorId(id);
            if (contrato == null) {
                TempData["MensagemDeErro"] = "Desculpe, ID não foi encontrado.";
                return View(contrato);
            }
            return View(contrato);
        }
        [HttpPost]
        public IActionResult AprovarContrato(Contrato contrato) {
            try {
                _contratoRepositorio.AprovarContrato(contrato);
                TempData["MensagemDeSucesso"] = "Aprovado com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return View();
            }
        }

        public IActionResult RevogarContrato(int id) {
            ViewData["Title"] = "Negar contrato";
            Contrato contrato = _contratoRepositorio.ListarJoinPorId(id);
            if (contrato == null) {
                TempData["MensagemDeErro"] = "Desculpe, ID não foi encontrado.";
                return View(contrato);
            }
            return View(contrato);
        }
        [HttpPost]
        public IActionResult RevogarContrato(Contrato contrato) {
            try {
                _contratoRepositorio.RevogarContrato(contrato);
                TempData["MensagemDeSucesso"] = "Negado com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return View();
            }
        }
    }
}
