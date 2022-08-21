
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

        public List<PessoaFisica> BuscarTodosHabilitados() {
            var buscar = _bancocontext.PessoaFisica.ToList();
            return buscar.Where(x => x.Status == StatuCliente.Habilitado).ToList();
        }
        public List<PessoaJuridica> BuscarTodosHabJuridico() {
            var buscar = _bancocontext.PessoaJuridica.ToList();
            return buscar.Where(x => x.Status == StatuCliente.Habilitado).ToList();
        }

        public List<PessoaFisica> BuscarTodosDesabilitados() {
            var buscar = _bancocontext.PessoaFisica.ToList();
            return buscar.Where(x => x.Status == StatuCliente.Desabilitado).ToList();
        }
        public List<PessoaJuridica> BuscarTodosDesaJuridico() {
            var buscar = _bancocontext.PessoaJuridica.ToList();
            return buscar.Where(x => x.Status == StatuCliente.Desabilitado).ToList();
        }

        public PessoaFisica Adicionar(PessoaFisica cliente) {
            try {
                _bancocontext.PessoaFisica.Add(cliente);
                _bancocontext.SaveChanges();
                return cliente;
            }
            catch (Exception erro) {
                TratarErro(cliente, erro);
                return null;
            }
        }
        public PessoaJuridica AdicionarJ(PessoaJuridica cliente) {
            try {
                _bancocontext.PessoaJuridica.Add(cliente);
                _bancocontext.SaveChanges();
                return cliente;
            }
            catch (Exception erro) {
                TratarErroJ(cliente, erro);
                return null;
            }
        }

        public PessoaFisica ListarPorId(long id) {
            return _bancocontext.PessoaFisica.FirstOrDefault(x => x.Id == id);
        }
        public PessoaJuridica ListarPorIdJuridico(long id) {
            return _bancocontext.PessoaJuridica.FirstOrDefault(x => x.Id == id);
        }
        public PessoaFisica Editar(PessoaFisica cliente) {
            try {
                PessoaFisica clienteBD = ListarPorId(cliente.Id);
                if (clienteBD == null) throw new System.Exception("Desculpe, houve alguma falha na aplicação.");
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
                clienteBD.Ddd = cliente.Ddd;
                clienteBD.Bairro = cliente.Bairro;
                clienteBD.Cidade = cliente.Cidade;
                clienteBD.Estado = cliente.Estado;
                _bancocontext.Update(clienteBD);
                _bancocontext.SaveChanges();

                return cliente;
            }
            catch (Exception erro) {
                TratarErro(cliente, erro);
                return null;
            }
        }
        public PessoaJuridica EditarJurico(PessoaJuridica cliente) {
            try {
                PessoaJuridica clienteUpdate = ListarPorIdJuridico(cliente.Id);
                if (clienteUpdate == null) throw new System.Exception("Desculpe, houve alguma falha na aplicação.");
                clienteUpdate.NomeFantasia = cliente.NomeFantasia;
                clienteUpdate.RazaoSocial = cliente.RazaoSocial;
                clienteUpdate.Cnpj = cliente.Cnpj;
                clienteUpdate.InscricaoEstadual = cliente.InscricaoEstadual;
                clienteUpdate.InscricaoMunicipal = cliente.InscricaoMunicipal;
                clienteUpdate.Email = cliente.Email;
                clienteUpdate.Telefone = cliente.Telefone;
                clienteUpdate.Cep = cliente.Cep;
                clienteUpdate.Logradouro = cliente.Logradouro;
                clienteUpdate.NumeroResidencial = cliente.NumeroResidencial;
                clienteUpdate.ComplementoResidencial = cliente.ComplementoResidencial;
                clienteUpdate.Ddd = cliente.Ddd;
                clienteUpdate.Bairro = cliente.Bairro;
                clienteUpdate.Cidade = cliente.Cidade;
                clienteUpdate.Estado = cliente.Estado;
                _bancocontext.Update(clienteUpdate);
                _bancocontext.SaveChanges();
                return cliente;
            }
            catch (Exception erro) {
                TratarErroJ(cliente, erro);
                return null;
            }
            
        }
        public PessoaFisica Desabilitar(PessoaFisica cliente) {
            PessoaFisica clienteDesabilitado = ListarPorId(cliente.Id);
            if (clienteDesabilitado == null) throw new System.Exception("Desculpe, houve um erro ao desabilitar.");
            clienteDesabilitado.Status = StatuCliente.Desabilitado;
            _bancocontext.Update(clienteDesabilitado);
            _bancocontext.SaveChanges();
            return cliente;
        }
        public PessoaJuridica DesabilitarJuridico(PessoaJuridica cliente) {
            PessoaJuridica clienteDesabilitado = ListarPorIdJuridico(cliente.Id);
            if (clienteDesabilitado == null) throw new Exception("Desculpe, houve um erro ao desabilitar");  
            clienteDesabilitado.Status = StatuCliente.Desabilitado;
            _bancocontext.Update(clienteDesabilitado);
            _bancocontext.SaveChanges();
            return clienteDesabilitado;
        }

        public PessoaFisica Habilitar(PessoaFisica cliente) {
            PessoaFisica clienteHabilitado = ListarPorId(cliente.Id);
            if (clienteHabilitado == null) throw new System.Exception("Desculpe, houve um erro ao habilitar.");
            clienteHabilitado.Status = StatuCliente.Habilitado;
            _bancocontext.Update(clienteHabilitado);
            _bancocontext.SaveChanges();
            return cliente;
        }
        public PessoaJuridica HabilitarJuridico(PessoaJuridica cliente) {
            PessoaJuridica clienteHabilitado = ListarPorIdJuridico(cliente.Id);
            if (clienteHabilitado == null) throw new System.Exception("Desculpe, houve um erro ao habilitar.");
            clienteHabilitado.Status = StatuCliente.Habilitado;
            _bancocontext.Update(clienteHabilitado);
            _bancocontext.SaveChanges();
            return clienteHabilitado;
        }
       
        public Exception TratarErro(PessoaFisica cliente, Exception erro) {
            if (erro.InnerException.Message.Contains(cliente.Cpf)) {
                throw new System.Exception("Cliente já se encontra cadastrado!");
            }
            if (erro.InnerException.Message.Contains(cliente.Rg)) {
                throw new System.Exception("Cliente já se encontra cadastrado!");
            }
            if (erro.InnerException.Message.Contains(cliente.Email)) {
                throw new System.Exception("Cliente já se encontra cadastrado!");
            }
            if (erro.InnerException.Message.Contains(cliente.Telefone)){
                throw new System.Exception("Cliente já se encontra cadastrado!");
            }
            throw new System.Exception("Desculpe, houve alguma falha na aplicação.");
        }
        public Exception TratarErroJ(PessoaJuridica cliente, Exception erro) {
            if (erro.InnerException.Message.Contains(cliente.Cnpj)) {
                throw new System.Exception("Cliente já se encontra cadastrado!");
            }
            if (erro.InnerException.Message.Contains(cliente.NomeFantasia)) {
                throw new System.Exception("Cliente já se encontra cadastrado!");
            }
            if (erro.InnerException.Message.Contains(cliente.InscricaoEstadual)) {
                throw new System.Exception("Cliente já se encontra cadastrado!");
            }
            if (erro.InnerException.Message.Contains(cliente.RazaoSocial)) {
                throw new System.Exception("Cliente já se encontra cadastrado!");
            }
            if (erro.InnerException.Message.Contains(cliente.Telefone)) {
                throw new System.Exception("Cliente já se encontra cadastrado!");
            }
            if (erro.InnerException.Message.Contains(cliente.Email)) {
                throw new System.Exception("Cliente já se encontra cadastrado!");
            }
            throw new System.Exception("Desculpe, houve alguma falha na aplicação.");
        }
    }
}