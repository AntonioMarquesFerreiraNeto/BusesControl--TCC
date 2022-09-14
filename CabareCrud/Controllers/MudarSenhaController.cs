using BusesControl.Filter;
using BusesControl.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BusesControl.Controllers {
    [PagUserAutenticado]
    public class MudarSenhaController : Controller {
        public IActionResult Index() {
            ViewData["Title"] = "Alterar senha";
            TempData["MensagemDeInfo"] = "Descrição: As senhas deverão ser maiores que oito dígitos e menores que 14 dígitos.";
            return View();
        }

        [HttpPost]
        public IActionResult Index(MudarSenha mudarSenha) {
            try {
                /* if (usuario.ValidarDuplicata(mudarSenha.NovaSenha) == true) {
                    TempData["MensagemDeErro"] = "A nova senha não pode ser igual a atual!";
                    return View(mudarSenha);
                }*/
                if (mudarSenha.ValidarSenhaAtual() != true) {
                    TempData["MensagemDeErro"] = "Nova senha e confirmar senha não são iguais!";
                    return View(mudarSenha);
                }
                if(ModelState.IsValid) {
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