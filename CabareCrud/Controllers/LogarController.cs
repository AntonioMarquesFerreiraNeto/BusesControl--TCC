using BusesControl.Helper;
using BusesControl.Models;
using BusesControl.Repositorio;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BusesControl.Controllers {
    public class LogarController : Controller {
        private readonly IFuncionarioRepositorio _funcionarioRepositorio;
        private readonly ISection _section;
        public LogarController(IFuncionarioRepositorio funcionarioRepositorio, ISection section) {
            _funcionarioRepositorio = funcionarioRepositorio;
            _section = section;
        } 
        public ActionResult Index() {
            ViewData["Title"] = "Autenticar";
            if (_section.buscarSectionUser() != null) {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Index(Login login) {
            try {
                if (ModelState.IsValid) {
                    Funcionario usuario = _funcionarioRepositorio.ListarPorlogin(login.Cpf);
                    if (usuario != null) {
                        if (usuario.ValidarSenha(login.Cep)) {
                            _section.CriarSection(usuario);
                            TempData["MensagemDeSucesso"] = "Autenticado com sucesso!";
                            return RedirectToAction("Index", "Home");
                        }
                        else {
                            TempData["MensagemDeErro"] = "CPF ou senha inválida!";
                        }
                    }
                    else {
                        TempData["MensagemDeErro"] = "CPF ou senha inválida!";
                    }
                }
                ViewData["Title"] = "Autenticar";
                return View(login);
            }
            catch(Exception erro) {
                TempData["MensagemDeErro"] = "Erro ao autenticar! Detalhe: " + erro.Message;
                return View(login);
            }
        }

        public ActionResult Sair() {
            _section.EncerrarSection();
            return RedirectToAction("Index", "Logar");
        }
    }
}
