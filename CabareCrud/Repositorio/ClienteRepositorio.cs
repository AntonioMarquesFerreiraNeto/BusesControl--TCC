﻿
using BusesControl.Data;
using BusesControl.Models;
using BusesControl.Models.Enums;
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
        //Adicionar as regras de negócio para clientes físicos que podem realizar contratos neste método.
        public List<PessoaFisica> ListClienteFisicoLegal() {
            var list = _bancocontext.PessoaFisica.ToList();
            return list.Where(x => x.Status == StatuCliente.Habilitado 
                && string.IsNullOrEmpty(x.IdVinculacaoContratual.ToString())).ToList();
        }

        //Adicionar as regras de negócio para clientes jurídicos que podem realizar contratos neste método.
        public List<PessoaJuridica> ListClienteJuridicoLegal() {
            var list = _bancocontext.PessoaJuridica.ToList();
            return list.Where(x => x.Status == StatuCliente.Habilitado).ToList();
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
                cliente = TrimPessoaFisica(cliente);
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
                cliente = TrimPessoaJuridica(cliente);
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
                clienteBD.Name = cliente.Name.Trim();
                clienteBD.DataNascimento = cliente.DataNascimento;
                clienteBD.Cpf = cliente.Cpf;
                clienteBD.Rg = cliente.Rg.Trim();
                clienteBD.Email = cliente.Email;
                clienteBD.Telefone = cliente.Telefone.Trim();
                clienteBD.NameMae = cliente.NameMae.Trim();
                clienteBD.Cep = cliente.Cep.Trim();
                clienteBD.ComplementoResidencial = cliente.ComplementoResidencial.Trim();
                clienteBD.Logradouro = cliente.Logradouro.Trim();
                clienteBD.NumeroResidencial = cliente.NumeroResidencial.Trim();
                clienteBD.Ddd = cliente.Ddd.Trim();
                clienteBD.Bairro = cliente.Bairro.Trim();
                clienteBD.Cidade = cliente.Cidade.Trim();
                clienteBD.Estado = cliente.Estado.Trim();
                clienteBD.IdVinculacaoContratual = cliente.IdVinculacaoContratual;

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
                clienteUpdate.NomeFantasia = cliente.NomeFantasia.Trim();
                clienteUpdate.RazaoSocial = cliente.RazaoSocial.Trim();
                clienteUpdate.Cnpj = cliente.Cnpj;
                clienteUpdate.InscricaoEstadual = cliente.InscricaoEstadual.Trim();
                clienteUpdate.InscricaoMunicipal = cliente.InscricaoMunicipal.Trim();
                clienteUpdate.Email = cliente.Email;
                clienteUpdate.Telefone = cliente.Telefone.Trim();
                clienteUpdate.Cep = cliente.Cep.Trim();
                clienteUpdate.Logradouro = cliente.Logradouro.Trim();
                clienteUpdate.NumeroResidencial = cliente.NumeroResidencial.Trim();
                clienteUpdate.ComplementoResidencial = cliente.ComplementoResidencial.Trim();
                clienteUpdate.Ddd = cliente.Ddd.Trim();
                clienteUpdate.Bairro = cliente.Bairro.Trim();
                clienteUpdate.Cidade = cliente.Cidade.Trim();
                clienteUpdate.Estado = cliente.Estado.Trim();
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
        public PessoaFisica TrimPessoaFisica(PessoaFisica value) {
            value.Name = value.Name.Trim();
            value.Rg = value.Rg.Trim();
            value.Telefone = value.Telefone.Trim();
            value.NameMae = value.NameMae.Trim();
            value.Cep = value.Cep.Trim();
            value.ComplementoResidencial = value.ComplementoResidencial.Trim();
            value.Logradouro = value.Logradouro.Trim();
            value.NumeroResidencial = value.NumeroResidencial.Trim();
            value.Ddd = value.Ddd.Trim();
            value.Bairro = value.Bairro.Trim();
            value.Cidade = value.Cidade.Trim();
            value.Estado = value.Estado.Trim();

            return value;
        }
        public PessoaJuridica TrimPessoaJuridica(PessoaJuridica value) {
            value.NomeFantasia = value.NomeFantasia.Trim();
            value.RazaoSocial = value.RazaoSocial.Trim();
            value.InscricaoEstadual = value.InscricaoEstadual.Trim();
            value.InscricaoMunicipal = value.InscricaoMunicipal.Trim();
            value.Telefone = value.Telefone.Trim();
            value.Cep = value.Cep.Trim();
            value.Logradouro = value.Logradouro.Trim();
            value.NumeroResidencial = value.NumeroResidencial.Trim();
            value.ComplementoResidencial = value.ComplementoResidencial.Trim();
            value.Ddd = value.Ddd.Trim();
            value.Bairro = value.Bairro.Trim();
            value.Cidade = value.Cidade.Trim();
            value.Estado = value.Estado.Trim();

            return value;
        }
        public Exception TratarErro(PessoaFisica cliente, Exception erro) {
            if (erro.InnerException.Message.Contains(cliente.Cpf)) {
                throw new System.Exception("Cliente já se encontra cadastrado!");
            }
            if (erro.InnerException.Message.Contains(cliente.Rg)) {
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

        public string ReturnDetalhesCliente(int id) {
            PessoaFisica pessoaFisica = ListarPorId(id);
            if(pessoaFisica == null) {
                PessoaJuridica pessoaJuridica = ListarPorIdJuridico(id);
                if (pessoaJuridica == null && pessoaFisica == null) throw new Exception("Desculpe, houve uma falha na aplicação.");
                return $"{pessoaJuridica.NomeFantasia} – CNPJ: {pessoaJuridica.Cnpj}";
            }
            return $"{pessoaFisica.Name} – CPF: {pessoaFisica.Cpf}";
        }
    }
}