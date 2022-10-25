using BusesControl.Filter;
using BusesControl.Models;
using BusesControl.Models.Enums;
using BusesControl.Repositorio;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace BusesControl.Controllers {
    [PagUserAutenticado]
    public class FrotaController : Controller {
        private readonly IOnibusRepositorio _onibusRepositorio;
        public FrotaController(IOnibusRepositorio onibusRepositorio) {
            _onibusRepositorio = onibusRepositorio;
        }
        public IActionResult Index() {
            ViewData["Title"] = "Ônibus habilitados";
            List<Onibus> onibusHabilitados = _onibusRepositorio.ListarTodosHab();
            return View(onibusHabilitados);
        }
        public IActionResult Desabilitados() {
            ViewData["Title"] = "Ônibus desabilitados";
            List<Onibus> onibusDesabilitados = _onibusRepositorio.ListarTodosDesa();
            return View("Index", onibusDesabilitados);
        }

        public IActionResult NovoOnibus() {
            ViewData["Title"] = "Incluir";
            return View();
        }
        [HttpPost]
        public IActionResult NovoOnibus(Onibus onibus) {
            ViewData["Title"] = "Incluir";
            try {
                if (ValidarCampo(onibus)) {
                    TempData["MensagemDeErro"] = "Informe os campos obrigatórios!";
                    return View(onibus);
                }
                if (ModelState.IsValid) {
                    onibus.StatusOnibus = OnibusStatus.Habilitado;
                    _onibusRepositorio.AdicionarBus(onibus);
                    TempData["MensagemDeSucesso"] = "Registrado com sucesso";
                    return RedirectToAction("Index");
                }
                return View(onibus);
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return View(onibus);
            }
        }

        public IActionResult Editar(long id) {
            ViewData["Title"] = "Editar";
            Onibus onibus = _onibusRepositorio.ListarPorId(id);
            if (onibus == null) {
                TempData["MensagemDeErro"] = "Desculpe, ID não foi encontrado.";
                return View(onibus);
            }
            return View(onibus);
        }
        [HttpPost]
        public IActionResult Editar(Onibus onibus) {
            Onibus onibusError = _onibusRepositorio.ListarPorId(onibus.Id);
            ViewData["Title"] = "Editar";
            try {
                if (ValidarCampo(onibus)) {
                    TempData["MensagemDeErro"] = "Informe os campos obrigatórios!";
                    return View(onibus);
                }
                if (ModelState.IsValid) {
                    _onibusRepositorio.EditarOnibus(onibus);
                    TempData["MensagemDeSucesso"] = "Editado com sucesso!";
                    return RedirectToAction("Index");
                }
                return View(onibus);
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return View(onibusError);
            }
        }

        public IActionResult Desabilitar(long id) {
            ViewData["Title"] = "Desabilitar";
            Onibus onibus = _onibusRepositorio.ListarPorId(id);
            if (onibus == null) {
                TempData["MensagemDeErro"] = "Desculpe, ID não foi encontrado.";
                return View(onibus);
            }
            return View(onibus);
        }
        [HttpPost]
        public IActionResult Desabilitar(Onibus onibus) {
            Onibus onibusError = _onibusRepositorio.ListarPorId(onibus.Id);
            ViewData["Title"] = "Desabilitar";
            try {
                _onibusRepositorio.Desabilitar(onibus);
                TempData["MensagemDeSucesso"] = "Desabilitado com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return View(onibusError);
            }
        }
        
        public IActionResult Habilitar(long id) {
            ViewData["Title"] = "Habilitar";
            Onibus onibus = _onibusRepositorio.ListarPorId(id);
            if (onibus == null) {
                TempData["MensagemDeErro"] = "Desculpe, ID não foi encontrado.";
                return View(onibus);
            }
            return View(onibus);
        }
        [HttpPost]
        public IActionResult Habilitar(Onibus onibus) {
            Onibus onibusError = _onibusRepositorio.ListarPorId(onibus.Id);
            ViewData["Title"] = "Habilitar";
            try {
                _onibusRepositorio.Habilitar(onibus);
                TempData["MensagemDeSucesso"] = "Habilitado com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return View(onibusError);
            }
        }

        public bool ValidarCampo(Onibus onibus) {
            if (onibus.Marca == null || onibus.NameBus == null || onibus.DataFabricacao == null || onibus.Placa == null || onibus.Renavam == null
                || onibus.Chassi == null || onibus.Assentos == null) {
                return true;
            }
            else {
                return false;
            }
        }
    }
}
