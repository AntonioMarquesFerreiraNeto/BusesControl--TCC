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
            List<Cliente> clientes = _clienteRepositorio.BuscarTodos();
            return View(clientes);
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
                    _clienteRepositorio.Adicionar(cliente);
                    TempData["MensagemDeSucesso"] = "Cliente salvado com sucesso!";
                    return RedirectToAction("Index");
                }
                return View(cliente);
            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return View(cliente);
            }
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
                    TempData["MensagemDeSucesso"] = "Cliente editado com sucesso!";
                    return RedirectToAction("Index");
                }
                return View(cliente);

            }
            catch (Exception erro) {
                TempData["MensagemDeErro"] = erro.Message;
                return View(cliente);
            }
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