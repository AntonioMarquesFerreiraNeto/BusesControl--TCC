using BusesControl.Models;
using BusesControl.Repositorio;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BusesControl.Controllers {
    public class ClienteController : Controller {
        private readonly IClienteRepositorio _clienteRepositorio;
        public ClienteController(IClienteRepositorio iclienteRepositorio) {
            _clienteRepositorio = iclienteRepositorio;
        }

        public IActionResult Index() {
            ViewData["Title"] = "Clientes";
            List<Cliente> clientesHabilitados = _clienteRepositorio.BuscarTodosHabilitados();
            return View(clientesHabilitados);
        }
        public IActionResult Desabilitados() {
            ViewData["Title"] = "Clientes";
            List<Cliente> clientesDesabilitados = _clienteRepositorio.BuscarTodosDesabilitados();
            return View("Index", clientesDesabilitados);
        }

        public IActionResult NovoCliente() {
            ViewData["Title"] = "Incluir cliente";
            return View();
        }
        [HttpPost]
        public IActionResult NovoCliente(Cliente cliente) {
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

        public IActionResult NovoClienteJuridico() {
            ViewData["Title"] = "Incluir cliente"; 
            return View();
        }
        public IActionResult Visualisar(long id) {
            _clienteRepositorio.ListarPorId(id);
            Cliente cliente = _clienteRepositorio.ListarPorId(id);
            return View(cliente);
        }

        public IActionResult EditarCliente(long id) {
            TempData["Title"] = "Editar cliente";
            Cliente cliente = _clienteRepositorio.ListarPorId(id);
            return View(cliente);
        }
        [HttpPost]
        public IActionResult EditarCliente(Cliente cliente) {
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

        public IActionResult Desabilitar(long id) {
            Cliente cliente = _clienteRepositorio.ListarPorId(id);
            return View(cliente);
        }
        [HttpPost]
        public IActionResult Desabilitar(Cliente cliente) {
            _clienteRepositorio.Desabilitar(cliente);
            TempData["MensagemDeSucesso"] = "Desabilitado com sucesso!";
            return RedirectToAction("Index");
        }

        public IActionResult Habilitar(long id) {
            Cliente cliente = _clienteRepositorio.ListarPorId(id);
            return View(cliente);
        }
        [HttpPost]
        public IActionResult Habilitar(Cliente cliente) {
            _clienteRepositorio.Habilitar(cliente);
            TempData["MensagemDeSucesso"] = "Habilitado com sucesso!";
            return RedirectToAction("Index");
        }

        //Método apenas para retornar mensagem de erro em geral. 
        public bool ValidarCampo(Cliente cliente) {
            if (cliente.Name == null || cliente.Cpf == null || cliente.Rg == null || cliente.NameMae == null || cliente.Cep == null ||
                   cliente.NumeroResidencial == null || cliente.ComplementoResidencial == null ||
                   cliente.Logradouro == null || cliente.Bairro == null || cliente.Cidade == null || cliente.Estado == null || cliente.Telefone == null || cliente.DataNascimento == null) {
                return true;
            }
            else return false;
        }
    }
}