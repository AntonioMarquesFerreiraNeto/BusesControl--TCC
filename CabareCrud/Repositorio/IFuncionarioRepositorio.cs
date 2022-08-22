using BusesControl.Models;
using System;
using System.Collections.Generic;

namespace BusesControl.Repositorio {
    public interface IFuncionarioRepositorio {
        public Funcionario Adicionar(Funcionario funcionario);
        public Funcionario ListarPorId(long id);
        public Funcionario ListarPorlogin(string cpf);
        public Funcionario EditarFuncionario(Funcionario funcionario);
        public List<Funcionario> ListarTodosHab();
        public List<Funcionario> ListarTodosDesa();
        public Funcionario Desabilitar(Funcionario funcionario);
        public Funcionario Habilitar(Funcionario funcionario);
        public Funcionario HabilitarUsuario(Funcionario funcionario);
        public Exception TratarErro(Funcionario funcionario, Exception erro);
    }
}
