using BusesControl.Filter;
using BusesControl.Helper;
using BusesControl.Models;
using BusesControl.Models.Enums;
using BusesControl.Repositorio;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace BusesControl.Controllers {
    [PagUserAdmin]
    public class FuncionarioController : Controller {

        private readonly IFuncionarioRepositorio _funcionarioRepositorio;
        private readonly IEmail _email;
        public FuncionarioController(IFuncionarioRepositorio funcionarioRepositorio, IEmail email) {
            _funcionarioRepositorio = funcionarioRepositorio;
            _email = email;
        }
        public IActionResult Index() {
            ViewData["Title"] = "Funcionários habilitados";
            List<Funcionario> funcionariosHabilitados = _funcionarioRepositorio.ListarTodosHab();
            return View(funcionariosHabilitados);
        }
        public IActionResult Desabilitados() {
            ViewData["Title"] = "Funcionários desabilitados";
            List<Funcionario> funcionariosDesabilitados = _funcionarioRepositorio.ListarTodosDesa();
            return View("Index", funcionariosDesabilitados);
        }
        public IActionResult NovoFuncionario() {
            ViewData["Title"] = "Incluir";
            return View();
        }
        [HttpPost]
        public IActionResult NovoFuncionario(Funcionario funcionario) {
            ViewData["Title"] = "Incluir";
            try {
                if (ValidarCampo(funcionario)) {
                    TempData["MensagemDeErro"] = "Informe os campos obrigatórios!";
                    return View(funcionario);
                }
                else if (ModelState.IsValid) {
                    funcionario.Status = StatuFuncionario.Habilitado;
                    if (ValidarCargo(funcionario)) {
                        funcionario.Senha = funcionario.GerarSenha();
                        funcionario.StatusUsuario = UsuarioStatus.Ativado;
                        bool emailEnviado = EnviarSenha(funcionario.Name, funcionario.Senha, funcionario.Email);
                        if (!emailEnviado) {
                            TempData["MensagemDeErro"] = "Não conseguimos enviar o e-mail com a senha, " +
                                "valide se ele é existente.";
                            return View(funcionario);
                        }
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
            ViewData["Title"] = "Editar";
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

        public IActionResult Desabilitar(long id) {
            ViewData["Title"] = "Desabilitar";
            Funcionario funcionario = _funcionarioRepositorio.ListarPorId(id);
            return View(funcionario);
        }
        [HttpPost]
        public IActionResult Desabilitar(Funcionario funcionario) {
            ViewData["Title"] = "Desabilitar";
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
            ViewData["Title"] = "Habilitar";
            Funcionario funcionario = _funcionarioRepositorio.ListarPorId(id);
            return View(funcionario);
        }
        [HttpPost]
        public IActionResult Habilitar(Funcionario funcionario) {
            ViewData["Title"] = "Habilitar";
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

        public IActionResult ControlarUsuario(long id) {
            ViewData["Title"] = "Controlar usuário";
            Funcionario funcionario = _funcionarioRepositorio.ListarPorId(id);
            return View(funcionario);
        }
        [HttpPost]
        public IActionResult HabilitarUsuario(Funcionario funcionario) {
            ViewData["Title"] = "Controlar usuário";
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
        [HttpPost]
        public IActionResult DesabilitarUsuario(Funcionario funcionario) {
            ViewData["Title"] = "Controlar usuário";
            try {
                _funcionarioRepositorio.DesabilitarUsuario(funcionario);
                TempData["MensagemDeSucesso"] = "Desabilitado com sucesso!";
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
        
        public bool EnviarSenha(string name, string senha, string email) {
            string mensagem = $"Informamos que foi gerado uma senha para o usuário {name}. <br> A senha gerada para o usuário é: <strong>{senha}<strong/>";
            if (_email.Enviar(email, "Buses Control - Gerador de senhas", mensagem)) {
                return true;
            }
            else {
                return false;
            }
        }
    }
}
