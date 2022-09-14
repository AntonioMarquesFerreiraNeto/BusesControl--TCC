using BusesControl.Filter;
using BusesControl.Models;
using BusesControl.Models.Enums;
using BusesControl.Repositorio;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BusesControl.Controllers {
    [PagUserAutenticado]
    public class ClienteController : Controller {
        private readonly IClienteRepositorio _clienteRepositorio;
        public ClienteController(IClienteRepositorio iclienteRepositorio) {
            _clienteRepositorio = iclienteRepositorio;
        }

        public IActionResult Index() {
            ViewData["Title"] = "Clientes físicos habilitados";
            List<PessoaFisica> clientesHabilitados = _clienteRepositorio.BuscarTodosHabilitados();
            return View(clientesHabilitados);
        }
        public IActionResult IndexJuridico() {
            ViewData["Title"] = "Clientes jurídicos habilitados";
            List<PessoaJuridica> clientesHabilitados = _clienteRepositorio.BuscarTodosHabJuridico();
            return View(clientesHabilitados);
        }

        public IActionResult Desabilitados() {
            ViewData["Title"] = "Clientes físicos desabilitados";
            List<PessoaFisica> clientesDesabilitados = _clienteRepositorio.BuscarTodosDesabilitados();
            return View("Index", clientesDesabilitados);
        }
        public IActionResult DesabilitadosJuridico() {
            ViewData["Title"] = "Clientes jurídicos desabilitados";
            List<PessoaJuridica> clienteDesabilitados = _clienteRepositorio.BuscarTodosDesaJuridico();
            return View("IndexJuridico", clienteDesabilitados);
        }

        public IActionResult NovoCliente() {
            ViewData["Title"] = "Incluir";
            TempData["MensagemDeInfo"] = "O e-mail não é obrigatório para clientes.";
            return View();
        }
        public IActionResult NovoClienteJuridico() {
            ViewData["Title"] = "Incluir";
            TempData["MensagemDeInfo"] = "O e-mail não é obrigatório para clientes.";
            return View();
        }
        [HttpPost]
        public IActionResult NovoCliente(PessoaFisica cliente) {
            ViewData["Title"] = "Incluir";
            try {
                if (ValidarCampo(cliente)) {
                    TempData["MensagemDeErro"] = "Informe os campos obrigatórios!";
                    return View(cliente);
                }
                if (ModelState.IsValid) {
                    cliente.Status = StatuCliente.Habilitado;
                    _clienteRepositorio.Adicionar(cliente);
                    TempData["MensagemDeSucesso"] = "Registrado com sucesso!";
                    return RedirectToAction("Index");
                }
                return View(cliente);
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return View(cliente);
            }
        }
        [HttpPost]
        public IActionResult NovoClienteJuridico(PessoaJuridica cliente) {
            ViewData["Title"] = "Incluir";
            try {
                if (ValidarCampoJurico(cliente)) {
                    TempData["MensagemDeErro"] = "Informe os campos obrigatórios!";
                    return View(cliente);
                }
                if (ModelState.IsValid) {
                    cliente.Status = StatuCliente.Habilitado;
                    cliente = _clienteRepositorio.AdicionarJ(cliente);
                    TempData["MensagemDeSucesso"] = "Registrado com sucesso!";
                    return RedirectToAction("IndexJuridico");
                }
                return View(cliente);
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return View(cliente);
            }
        }
        public IActionResult EditarCliente(long id) {
            ViewData["Title"] = "Editar";
            PessoaFisica cliente = _clienteRepositorio.ListarPorId(id);
            return View(cliente);
        }
        public IActionResult EditarClienteJuridico(long id) {
            ViewData["Title"] = "Editar";
            PessoaJuridica cliente = _clienteRepositorio.ListarPorIdJuridico(id);
            return View(cliente);
        }
        [HttpPost]
        public IActionResult EditarCliente(PessoaFisica cliente) {
            ViewData["Title"] = "Editar";
            try {
                if (ValidarCampo(cliente)) {
                    TempData["MensagemDeErro"] = "Informe os campos obrigatórios!";
                    return View(cliente);
                }
                if (ModelState.IsValid) {
                    _clienteRepositorio.Editar(cliente);
                    TempData["MensagemDeSucesso"] = "Editado com sucesso!";
                    return RedirectToAction("Index");
                }
                return View(cliente);

            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return View(cliente);
            }
        }
        [HttpPost]
        public IActionResult EditarClienteJuridico(PessoaJuridica cliente) {
            ViewData["Title"] = "Editar";
            try {
                if (ValidarCampoJurico(cliente)) {
                    TempData["MensagemDeErro"] = "Informe os campos obrigatórios!";
                    return View(cliente);
                }
                if (ModelState.IsValid) {
                    _clienteRepositorio.EditarJurico(cliente);
                    TempData["MensagemDeSucesso"] = "Editado com sucesso!";
                    return RedirectToAction("IndexJuridico");
                }
                return View(cliente);
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return View(cliente);
            }
        }

        public IActionResult Desabilitar(long id) {
            ViewData["Title"] = "Desabilitar";
            PessoaFisica cliente = _clienteRepositorio.ListarPorId(id);
            return View(cliente);
        }
        public IActionResult DesabilitarJuridico(long id) {
            ViewData["Title"] = "Desabilitar";
            PessoaJuridica cliente = _clienteRepositorio.ListarPorIdJuridico(id);
            return View(cliente);
        }
        [HttpPost]
        public IActionResult Desabilitar(PessoaFisica cliente) {
            ViewData["Title"] = "Desabilitar";
            try {
                _clienteRepositorio.Desabilitar(cliente);
                TempData["MensagemDeSucesso"] = "Desabilitado com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return View(cliente);
            }
        }
        [HttpPost]
        public IActionResult DesabilitarJuridico(PessoaJuridica cliente) {
            ViewData["Title"] = "Desabilitar";
            try {
                _clienteRepositorio.DesabilitarJuridico(cliente);
                TempData["MensagemDeSucesso"] = "Desabilitado com sucesso!";
                return RedirectToAction("IndexJuridico");
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return View(cliente);
            }
        }

        public IActionResult Habilitar(long id) {
            ViewData["Title"] = "Habilitar";
            PessoaFisica cliente = _clienteRepositorio.ListarPorId(id);
            return View(cliente);
        }
        public IActionResult HabilitarJuridico(long id) {
            ViewData["Title"] = "Habilitar";
            PessoaJuridica cliente = _clienteRepositorio.ListarPorIdJuridico(id);
            return View(cliente);
        }
        [HttpPost]
        public IActionResult HabilitarJuridico(PessoaJuridica cliente) {
            ViewData["Title"] = "Habilitar";
            try {
                _clienteRepositorio.HabilitarJuridico(cliente);
                TempData["MensagemDeSucesso"] = "Habilitado com sucesso!";
                return RedirectToAction("IndexJuridico");
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return View(cliente);
            }
        }
        [HttpPost]
        public IActionResult Habilitar(PessoaFisica cliente) {
            ViewData["Title"] = "Habilitar";
            try {
                _clienteRepositorio.Habilitar(cliente);
                TempData["MensagemDeSucesso"] = "Habilitado com sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return View(cliente);
            }
        }

        //Métodos abaixo apenas para retornar mensagem de erro em geral, já que a ModelState não os deixa serem registrados no banco de dados. 
        public bool ValidarCampo(PessoaFisica cliente) {
            if (cliente.Name == null || cliente.Cpf == null || cliente.Rg == null || cliente.NameMae == null || cliente.Cep == null ||
                   cliente.NumeroResidencial == null || cliente.ComplementoResidencial == null ||
                   cliente.Logradouro == null || cliente.Bairro == null || cliente.Cidade == null || cliente.Estado == null || cliente.Telefone == null || cliente.DataNascimento == null) {
                return true;
            }
            else return false;
        }
        public bool ValidarCampoJurico(PessoaJuridica cliente) {
            if (cliente.NomeFantasia == null || cliente.Cnpj == null || cliente.InscricaoEstadual == null || cliente.InscricaoMunicipal == null || cliente.RazaoSocial == null || cliente.Cep == null ||
                   cliente.NumeroResidencial == null || cliente.ComplementoResidencial == null ||
                   cliente.Logradouro == null || cliente.Bairro == null || cliente.Cidade == null || cliente.Estado == null || cliente.Ddd == null || cliente.Telefone == null) {
                return true;
            }
            else {
                return false;
            }
        }
    }
}