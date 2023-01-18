using BusesControl.Helper;
using BusesControl.Models;
using BusesControl.Repositorio;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BusesControl.Controllers {
    public class LogarController : Controller {
        private readonly IFuncionarioRepositorio _funcionarioRepositorio;
        private readonly ISection _section;
        private readonly IEmail _email;
        private readonly IFinanceiroRepositorio _financeiroRepositorio;
        public LogarController(IFuncionarioRepositorio funcionarioRepositorio, ISection section, IEmail email, IFinanceiroRepositorio financeiroRepositorio) {
            _funcionarioRepositorio = funcionarioRepositorio;
            _section = section;
            _email = email;
            _financeiroRepositorio = financeiroRepositorio;
        }
        public ActionResult Index() {
            ViewData["Title"] = "Autenticar";
            _financeiroRepositorio.TaskMonitorParcelas();
            _financeiroRepositorio.TaskMonitorParcelasLancamento();
            if (_section.buscarSectionUser() != null) {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Index(Login login) {
            ViewData["Title"] = "Autenticar";
            try {
                if (ModelState.IsValid) {
                    Funcionario usuario = _funcionarioRepositorio.ListarPorlogin(login.Cpf);
                    if (usuario != null) {
                        if (usuario.ValidarSenha(login.Senha)) {
                            _section.CriarSection(usuario);
                            TempData["MensagemDeSucesso"] = "Autenticado com sucesso!";
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    TempData["MensagemDeErro"] = "CPF ou senha inválida!";
                }
                return View(login);
            }
            catch(Exception erro) {
                TempData["MensagemDeErro"] = "Erro ao autenticar! Detalhe: " + erro.Message;
                return View(login);
            }
        }

        public IActionResult RedefinirSenha() {
            ViewData["Title"] = "Redefinir senha";
            return View();
        }

        [HttpPost]
        public IActionResult RedefinirSenha(RedefinirSenha redefinirSenha) {
            ViewData["Title"] = "Redefinir senha";
            try {
                if (ModelState.IsValid) {
                    Funcionario usuario = _funcionarioRepositorio.ListarPorloginAndEmail(redefinirSenha.Email, redefinirSenha.Cpf);
                    if (usuario != null) {
                        usuario.Senha = usuario.GerarSenha();
                        string senhaUser = usuario.Senha;
                        usuario.setPasswordHash();
                        string mensagem = $"Informamos que a senha do usuário {usuario.Name} foi redefinida para: <strong>{senhaUser}<strong/>";
                        bool emailEnviado = _email.Enviar(usuario.Email, "Buses Control - Redefinição de senhas", mensagem);
                        if (emailEnviado) {
                            _funcionarioRepositorio.NovaSenha(usuario);
                            TempData["MensagemDeSucesso"] = "Enviamos uma nova senha para seu e-mail!";
                            return RedirectToAction("Index", "Logar");
                        }
                        else {
                            TempData["MensagemDeErro"] = $"Não conseguimos enviar o e-mail com a senha.";
                            return View(redefinirSenha);
                        }
                    }
                    TempData["MensagemDeErro"] = "CPF ou e-mail inválido!";
                }
                return View(redefinirSenha);
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = "Erro ao redefinir! Detalhe: " + erro.Message;
                return View(redefinirSenha);
            }
        }
        public ActionResult Sair() {
            _section.EncerrarSection();
            return RedirectToAction("Index", "Logar");
        }
    }
}
