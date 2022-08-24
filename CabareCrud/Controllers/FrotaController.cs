﻿using BusesControl.Models;
using BusesControl.Repositorio;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace BusesControl.Controllers {
    public class FrotaController : Controller {
        private readonly IOnibusRepositorio _onibusRepositorio;
        public FrotaController(IOnibusRepositorio onibusRepositorio) {
            _onibusRepositorio = onibusRepositorio;
        }
        public IActionResult Index() {
            ViewData["Title"] = "Frota";
            List<Onibus> onibusHabilitados = _onibusRepositorio.ListarTodosHab();
            return View(onibusHabilitados);
        }
        public IActionResult Desabilitados() {
            List<Onibus> onibusDesabilitados = _onibusRepositorio.ListarTodosDesa();
            return View("Index", onibusDesabilitados);
        }

        public IActionResult NovoOnibus() {
            ViewData["Title"] = "Incluir";
            return View();
        }
        [HttpPost]
        public IActionResult NovoOnibus(Onibus onibus) {
            try {
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

        public IActionResult Desabilitar(long id) {
            Onibus onibus = _onibusRepositorio.ListarPorId(id);
            return View(onibus);
        }
        [HttpPost]
        public IActionResult Desabilitar(Onibus onibus) {
            try {
                _onibusRepositorio.Desabilitar(onibus);
                TempData["MensagemDeSucesso"] = "Desabilitado com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return View(onibus);
            }
        }
        
        public IActionResult Habilitar(long id) {
            Onibus onibus = _onibusRepositorio.ListarPorId(id);
            return View(onibus);
        }
        [HttpPost]
        public IActionResult Habilitar(Onibus onibus) {
            try {
                _onibusRepositorio.Habilitar(onibus);
                TempData["MensagemDeSucesso"] = "Habilitado com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return View(onibus);
            }
        }
    }
}
