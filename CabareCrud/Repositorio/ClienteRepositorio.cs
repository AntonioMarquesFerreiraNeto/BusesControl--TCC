using BusesControl.Data;
using BusesControl.Models;
using System.Collections.Generic;
using System.Linq;

namespace BusesControl.Repositorio {
    public class ClienteRepositorio : IClienteRepositorio {
        private readonly BancoContext _bancocontext; 
        public ClienteRepositorio(BancoContext bancoContext) {
            _bancocontext = bancoContext;
        }
        public List<Cliente> BuscarTodos() {
            return _bancocontext.Cliente.ToList();
        }
        public Cliente Adicionar(Cliente cliente) {
            //Adiciona o cliente no banco de dados. 
            _bancocontext.Cliente.Add(cliente);
            _bancocontext.SaveChanges();
            return cliente;
        }
    }
}
