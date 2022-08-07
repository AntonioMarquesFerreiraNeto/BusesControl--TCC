using BusesControl.Models;
using System.Collections.Generic;

namespace BusesControl.Repositorio {
    public interface IClienteRepositorio  {
        List<Cliente> BuscarTodos();
        Cliente Adicionar(Cliente cliente);
    }
}
