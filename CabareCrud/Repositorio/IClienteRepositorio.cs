using BusesControl.Models;
using System;
using System.Collections.Generic;

namespace BusesControl.Repositorio {
    public interface IClienteRepositorio  {
        List<Cliente> BuscarTodos();

        Cliente Adicionar(Cliente cliente);

        Cliente ListarPorId(long id);

        Cliente Editar(Cliente cliente);
        Exception TratarErro(Cliente cliente, Exception erro);
    }
}
