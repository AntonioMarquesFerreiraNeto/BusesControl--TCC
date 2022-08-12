
using BusesControl.Data;
using BusesControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

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
            try {
                //Adiciona o cliente no banco de dados. 
                _bancocontext.Cliente.Add(cliente);
                _bancocontext.SaveChanges();
                return cliente;
            }
            catch (Exception erro) {
                TratarErro(cliente, erro);
                return null;
            }
        }

        public Cliente ListarPorId(long id) {
            return _bancocontext.Cliente.FirstOrDefault(x => x.Id == id);
        }

        public Cliente Editar(Cliente cliente) {
            try {
                Cliente clienteBD = ListarPorId(cliente.Id);
                if (clienteBD == null) throw new System.Exception("Ops, houve um erro ao editar o contato.");
                clienteBD.Name = cliente.Name;
                clienteBD.DataNascimento = cliente.DataNascimento;
                clienteBD.Cpf = cliente.Cpf;
                clienteBD.Rg = cliente.Rg;
                clienteBD.Email = cliente.Email;
                clienteBD.Telefone = cliente.Telefone;
                clienteBD.NameMae = cliente.NameMae;
                clienteBD.Cep = cliente.Cep;
                clienteBD.ComplementoResidencial = cliente.ComplementoResidencial;
                clienteBD.Logradouro = cliente.Logradouro;
                clienteBD.NumeroResidencial = cliente.NumeroResidencial;
                clienteBD.Bairro = cliente.Bairro;
                clienteBD.Cidade = cliente.Cidade;
                clienteBD.Estado = cliente.Estado;

                _bancocontext.Update(clienteBD);
                _bancocontext.SaveChanges();

                return (cliente);
            }
            catch (Exception erro) {
                TratarErro(cliente, erro);
                return null;
            }
        }

        public Exception TratarErro(Cliente cliente, Exception erro) {
            if (erro.InnerException.Message.Contains(cliente.Cpf)) {
                throw new System.Exception("CPF já se encontra cadastrado!");
            }
            if (erro.InnerException.Message.Contains(cliente.Rg)) {
                throw new System.Exception("RG já se encontrada cadastrado!");
            }
            if (erro.InnerException.Message.Contains(cliente.Email)) {
                throw new System.Exception("E-mail já se encontrada cadastrado!");
            }
            throw new System.Exception("Houve alguma falha na aplicação.");
        }
    }
}