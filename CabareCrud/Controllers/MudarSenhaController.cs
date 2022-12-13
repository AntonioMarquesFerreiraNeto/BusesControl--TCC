using BusesControl.Filter;
using BusesControl.Helper;
using BusesControl.Models;
using BusesControl.Repositorio;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BusesControl.Controllers {
    [PagUserAutenticado]
    public class MudarSenhaController : Controller {

        private readonly IFuncionarioRepositorio _funcionarioRepositorio;
        private readonly ISection _section;

        public MudarSenhaController(IFuncionarioRepositorio funcionarioRepositorio, ISection section) {
            _funcionarioRepositorio = funcionarioRepositorio;
            _section = section;
        }

        public IActionResult Index() {
            ViewData["Title"] = "Alterar senha";
            TempData["MensagemDeInfo"] = "Descrição: As senhas deverão ser maiores que oito dígitos, conter números, letras e caracteres especiais.";
            return View();
        }

        [HttpPost]
        public IActionResult Index(MudarSenha mudarSenha) {
            ViewData["Title"] = "Alterar senha";
            try {
                Funcionario usuarioAutenticado = _section.buscarSectionUser();
                mudarSenha.Id = usuarioAutenticado.Id;
                if (ModelState.IsValid) {
                    if (mudarSenha.ValNovaSenhaConfirmSenha() != true) {
                        TempData["MensagemDeErro"] = "Nova senha e confirmar senha não são iguais!";
                        return View(mudarSenha);
                    }
                    mudarSenha.NovaSenha = mudarSenha.NovaSenha.Trim();
                    mudarSenha.NovaSenha = mudarSenha.NovaSenha.GerarHash();
                    _funcionarioRepositorio.AlterarSenha(mudarSenha);
                    TempData["MensagemDeSucesso"] = "Senha alterada com sucesso!";
                    return RedirectToAction("Index", "Home");
                }
                return View(mudarSenha);
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return View(mudarSenha);
            }
        }
    }
}
