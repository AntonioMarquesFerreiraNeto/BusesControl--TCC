using BusesControl.Models;
using BusesControl.Repositorio;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BusesControl.Controllers {
    public class ApelidoController : Controller {

        private readonly IFuncionarioRepositorio _funcionarioRepositorio;

        public ApelidoController(IFuncionarioRepositorio funcionarioRepositorio) {
            _funcionarioRepositorio = funcionarioRepositorio;
        }

        [HttpPost]
        public IActionResult InserirApelido(Funcionario funcionario) {
            try {
                if (!string.IsNullOrEmpty(funcionario.Apelido)) {
                    funcionario.Apelido = funcionario.Apelido.Trim();
                    _funcionarioRepositorio.RegistroApelido(funcionario);
                    TempData["MensagemDeSucesso"] = "Registrado com sucesso!";
                    return RedirectToAction("Index", "Home");
                }
                else {
                    TempData["MensagemDeErro"] = "O campo não pode ser vazio. Tente novamente!";
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
