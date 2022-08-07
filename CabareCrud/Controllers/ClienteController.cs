using BusesControl.Models;
using BusesControl.Repositorio;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BusesControl.Controllers {
    public class ClienteController : Controller {
        private readonly IClienteRepositorio _clienteRepositorio;
        public ClienteController(IClienteRepositorio iclienteRepositorio) {
            _clienteRepositorio = iclienteRepositorio;
        }
        public IActionResult Index() {
            List<Cliente> clientes = _clienteRepositorio.BuscarTodos();
            return View();

            //Passar o clientes dentro da view quando resolver o conflito do model na mesma view.
        }

        [HttpPost]
        public IActionResult Criar(Cliente cliente) {
            _clienteRepositorio.Adicionar(cliente);
            return RedirectToAction("Index");
        }
    }
}
