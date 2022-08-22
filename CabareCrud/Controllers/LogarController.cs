using BusesControl.Models;
using BusesControl.Repositorio;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BusesControl.Controllers {
    public class LogarController : Controller {
        private readonly IFuncionarioRepositorio _funcionarioRepositorio;
        public LogarController(IFuncionarioRepositorio funcionarioRepositorio) {
            _funcionarioRepositorio = funcionarioRepositorio;
        } 
        public ActionResult Index() {
            ViewData["Title"] = "Autenticar";
            return View();
        }
        [HttpPost]
        public ActionResult Index(Login login) {
            try {
                if (ModelState.IsValid) {
                    Funcionario usuario = _funcionarioRepositorio.ListarPorlogin(login.Cpf);
                    if (usuario != null) {
                        if (usuario.ValidarSenha(login.Cep)) {
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
                return View(login);
            }
            catch(Exception erro) {
                TempData["MensagemDeErro"] = "CPF ou senha inválida!" + erro.Message;
                return View(login);
            }
        }
    }
}
