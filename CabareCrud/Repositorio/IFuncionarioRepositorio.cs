﻿using BusesControl.Models;
using System;
using System.Collections.Generic;

namespace BusesControl.Repositorio {
    public interface IFuncionarioRepositorio {
        public Funcionario Adicionar(Funcionario funcionario);
        public Funcionario ListarPorId(long id);
        public Funcionario ListarPorlogin(string cpf);
        public Funcionario ListarPorloginAndEmail(string email, string login);
        public Funcionario EditarFuncionario(Funcionario funcionario);
        public List<Funcionario> ListarTodosHab();
        public List<Funcionario> ListarTodosDesa();
        public Funcionario Desabilitar(Funcionario funcionario);
        public Funcionario DesabilitarUsuario(Funcionario funcionario);
        public Funcionario Habilitar(Funcionario funcionario);
        public Funcionario HabilitarUsuario(Funcionario funcionario);
        public Funcionario AlterarSenha(MudarSenha mudarSenha);
        public Funcionario NovaSenha(Funcionario usuario);
        public Funcionario RegistroApelido(Funcionario usuario);
        public Exception TratarErro(Funcionario funcionario, Exception erro);
    }
}
