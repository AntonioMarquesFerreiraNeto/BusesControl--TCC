using BusesControl.Models;
using System;
using System.Collections.Generic;

namespace BusesControl.Repositorio {
    public interface IClienteRepositorio  {
        List<Cliente> BuscarTodosHabilitados();
        List<Cliente> BuscarTodosDesabilitados();
        Cliente Adicionar(Cliente cliente);
        Cliente ListarPorId(long id);
        Cliente Editar(Cliente cliente);
        Cliente Desabilitar(Cliente cliente);
        Cliente Habilitar(Cliente cliente);
        Exception TratarErro(Cliente cliente, Exception erro);
    }
}
