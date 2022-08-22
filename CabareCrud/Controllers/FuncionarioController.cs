﻿using BusesControl.Models;
using BusesControl.Repositorio;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace BusesControl.Controllers {
    public class FuncionarioController : Controller {

        private readonly IFuncionarioRepositorio _funcionarioRepositorio;
        public FuncionarioController(IFuncionarioRepositorio funcionarioRepositorio) {
            _funcionarioRepositorio = funcionarioRepositorio;
        }
        public IActionResult Index() {
            ViewData["Title"] = "Funcionários";
            List<Funcionario> funcionariosHabilitados = _funcionarioRepositorio.ListarTodosHab();
            return View(funcionariosHabilitados);
        }
        public IActionResult Desabilitados() {
            ViewData["Title"] = "Funcionários";
            List<Funcionario> funcionariosDesabilitados = _funcionarioRepositorio.ListarTodosDesa();
            return View("Index", funcionariosDesabilitados);
        }
        public IActionResult NovoFuncionario() {
            ViewData["Title"] = "Incluir";
            return View();
        }
        [HttpPost]
        public IActionResult NovoFuncionario(Funcionario funcionario) {
            try {
                if (ValidarCampo(funcionario)) {
                    TempData["MensagemDeErro"] = "Informe os campos obrigatórios!";
                    return View(funcionario);
                }
                else if (ModelState.IsValid) {
                    funcionario.Status = StatuFuncionario.Habilitado;
                    if (ValidarCargo(funcionario)) {
                        funcionario.StatusUsuario = UsuarioStatus.Ativado;
                    }
                    _funcionarioRepositorio.Adicionar(funcionario);
                    TempData["MensagemDeSucesso"] = "Registrado com sucesso!";
                    return RedirectToAction("Index");
                }
                return View(funcionario);
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return View(funcionario);
            }
        }

        public IActionResult EditarFuncionario(long id) {
            ViewData["Title"] = "Editar";
            Funcionario funcionario = _funcionarioRepositorio.ListarPorId(id);
            return View(funcionario);
        }
        [HttpPost]
        public IActionResult EditarFuncionario(Funcionario funcionario) {
            try {
                if (ValidarCampo(funcionario)) {
                    TempData["MensagemDeErro"] = "Informe os campos obrigatórios!";
                    return View(funcionario);
                }
                if (ModelState.IsValid) {
                    _funcionarioRepositorio.EditarFuncionario(funcionario);
                    TempData["MensagemDeSucesso"] = "Editado com sucesso!";
                    return RedirectToAction("Index");
                }
                return View(funcionario);
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return View(funcionario);
            }
        }
        public IActionResult Visualizar(long id) {
            Funcionario funcionario = _funcionarioRepositorio.ListarPorId(id);
            return View(funcionario);
        }

        public IActionResult Desabilitar(long id) {
            Funcionario funcionario = _funcionarioRepositorio.ListarPorId(id);
            return View(funcionario);
        }
        [HttpPost]
        public IActionResult Desabilitar(Funcionario funcionario) {
            try {
                _funcionarioRepositorio.Desabilitar(funcionario);
                TempData["MensagemDeSucesso"] = "Desabilitado com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return View(funcionario);
            }
        }

        public IActionResult Habilitar(long id) {
            Funcionario funcionario = _funcionarioRepositorio.ListarPorId(id);
            return View(funcionario);
        }
        [HttpPost]
        public IActionResult Habilitar(Funcionario funcionario) {
            try {
                _funcionarioRepositorio.Habilitar(funcionario);
                TempData["MensagemDeSucesso"] = "Habilitado com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return View(funcionario);
            }
        }

        public IActionResult HabilitarUsuario(long id) {
            Funcionario funcionario = _funcionarioRepositorio.ListarPorId(id);
            return View(funcionario);
        }
        [HttpPost]
        public IActionResult HabilitarUsuario(Funcionario funcionario) {
            try {
                _funcionarioRepositorio.HabilitarUsuario(funcionario);
                TempData["MensagemDeSucesso"] = "Habilitado com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return View(funcionario);
            }
        }

        //Métodos abaixo apenas para retornar mensagem de erro em geral, já que a ModelState não os deixa serem registrados no banco de dados
        public bool ValidarCampo(Funcionario value) {
            if (value.Name == null || value.DataNascimento == null || value.Cpf == null ||
                value.Email == null || value.Telefone == null || value.Cep == null ||
                value.Logradouro == null || value.NumeroResidencial == null || value.ComplementoResidencial == null ||
                value.Ddd == null || value.Bairro == null || value.Cidade == null || value.Estado == null) {
                return true;
            }
            else {
                return false;
            }
        }

        public bool ValidarCargo(Funcionario funcionario) {
            if (funcionario.Cargos == CargoFuncionario.Administrador || funcionario.Cargos == CargoFuncionario.Assistente) {
                return true;
            }
            else return false;
        }
    }
}
