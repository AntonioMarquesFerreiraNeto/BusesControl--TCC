using BusesControl.Data;
using BusesControl.Models;
using BusesControl.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusesControl.Repositorio {
    public class FornecedorRepositorio : IFornecedorRepositorio {
        private readonly BancoContext _bancoContext;

        public FornecedorRepositorio(BancoContext bancoContext) {
            _bancoContext = bancoContext;
        }

        public FornecedorFisico AdicionarFornecedorFisico(FornecedorFisico fornecedorFisico) {
            try {
                if (Duplicata(fornecedorFisico)) {
                    throw new Exception("Fornecedor já se encontra cadastrado!");
                }
                fornecedorFisico.Status = StatuCliente.Habilitado;
                _bancoContext.FornecedorFisico.Add(fornecedorFisico);
                fornecedorFisico = TrimPessoaFisica(fornecedorFisico);
                _bancoContext.SaveChanges();
                return fornecedorFisico;
            }
            catch (Exception erro) {
                throw new Exception(erro.Message);
            }
        }

        public FornecedorJuridico AdicionarFornecedorJuridico(FornecedorJuridico fornecedorJuridico) {
            try {
                if (DuplicataJuridica(fornecedorJuridico)) {
                    throw new Exception("Fornecedor já se encontra cadastrado!");
                }
                fornecedorJuridico = TrimPessoaJuridica(fornecedorJuridico);
                fornecedorJuridico.Status = StatuCliente.Habilitado;
                _bancoContext.FornecedorJuridico.Add(fornecedorJuridico);
                _bancoContext.SaveChanges();
                return fornecedorJuridico;
            }
            catch (Exception erro) {
                throw new Exception(erro.Message);
            }
        }

        public FornecedorFisico AtivarFornecedorFisico(FornecedorFisico fornecedorFisico) {
            FornecedorFisico fornecedorFisicoDB = ListPorIdFisico(fornecedorFisico.Id);
            if (fornecedorFisicoDB == null) throw new Exception("Desculpe, ID não foi encontrado!");
            fornecedorFisicoDB.Status = StatuCliente.Habilitado;
            _bancoContext.FornecedorFisico.Update(fornecedorFisicoDB);
            _bancoContext.SaveChanges();
            return fornecedorFisicoDB;
        }

        public FornecedorJuridico AtivarFornecedorJuridico(FornecedorJuridico fornecedorJuridico) {
            FornecedorJuridico fornecedorJuridicoDB = ListPorIdJuridico(fornecedorJuridico.Id);
            if (fornecedorJuridicoDB == null) throw new Exception("Desculpe, ID não foi encontrado!");
            fornecedorJuridicoDB.Status = StatuCliente.Habilitado;
            _bancoContext.FornecedorJuridico.Update(fornecedorJuridicoDB);
            _bancoContext.SaveChanges();
            return fornecedorJuridicoDB;
        }

        public FornecedorFisico EditarFornecedorFisico(FornecedorFisico fornecedorFisico) {
            try {
                FornecedorFisico fornecedorFisicoDB = ListPorIdFisico(fornecedorFisico.Id);
                if (fornecedorFisicoDB == null) throw new Exception("Desculpe, ID não foi encontrado!");
                if (DuplicataEditar(fornecedorFisico, fornecedorFisicoDB)) {
                    throw new Exception("Fornecedor já se encontra cadastrado!");
                }
                fornecedorFisicoDB.Name = fornecedorFisico.Name.Trim();
                fornecedorFisicoDB.DataNascimento = fornecedorFisicoDB.DataNascimento;
                fornecedorFisicoDB.Cpf = fornecedorFisico.Cpf;
                fornecedorFisicoDB.Rg = fornecedorFisico.Rg.Trim();
                fornecedorFisicoDB.Telefone = fornecedorFisico.Telefone.Trim();
                fornecedorFisicoDB.Email = fornecedorFisico.Email;
                fornecedorFisicoDB.NameMae = fornecedorFisico.NameMae.Trim();
                fornecedorFisicoDB.Estado = fornecedorFisico.Estado.Trim();
                fornecedorFisicoDB.Cidade = fornecedorFisico.Cidade.Trim();
                fornecedorFisicoDB.Bairro = fornecedorFisico.Bairro.Trim();
                fornecedorFisicoDB.Cep = fornecedorFisico.Cep.Trim();
                fornecedorFisicoDB.ComplementoResidencial = fornecedorFisico.ComplementoResidencial.Trim();
                fornecedorFisicoDB.Logradouro = fornecedorFisico.Logradouro.Trim();
                fornecedorFisicoDB.NumeroResidencial = fornecedorFisico.NumeroResidencial.Trim();
                fornecedorFisicoDB.Ddd = fornecedorFisico.Ddd.Trim();
                _bancoContext.FornecedorFisico.Update(fornecedorFisicoDB);
                _bancoContext.SaveChanges();
                return fornecedorFisicoDB;
            }
            catch (Exception erro) {
                throw new Exception(erro.Message);
            }
        }

        public FornecedorJuridico EditarFornecedorJuridico(FornecedorJuridico fornecedorJuridico) {
            try {
                FornecedorJuridico fornecedorJuridicoDB = ListPorIdJuridico(fornecedorJuridico.Id);
                if (fornecedorJuridicoDB == null) throw new System.Exception("Desculpe, ID não foi encontrado.");
                if (DuplicataEditarJuridica(fornecedorJuridico, fornecedorJuridicoDB)) {
                    throw new Exception("Fornecedor já se encontra cadastrado!");
                }
                fornecedorJuridicoDB.NomeFantasia = fornecedorJuridico.NomeFantasia.Trim();
                fornecedorJuridicoDB.RazaoSocial = fornecedorJuridico.RazaoSocial.Trim();
                fornecedorJuridicoDB.Cnpj = fornecedorJuridico.Cnpj;
                fornecedorJuridicoDB.InscricaoEstadual = fornecedorJuridico.InscricaoEstadual.Trim();
                fornecedorJuridicoDB.InscricaoMunicipal = fornecedorJuridico.InscricaoMunicipal.Trim();
                fornecedorJuridicoDB.Email = fornecedorJuridico.Email;
                fornecedorJuridicoDB.Telefone = fornecedorJuridico.Telefone.Trim();
                fornecedorJuridicoDB.Cep = fornecedorJuridico.Cep.Trim();
                fornecedorJuridicoDB.Logradouro = fornecedorJuridico.Logradouro.Trim();
                fornecedorJuridicoDB.NumeroResidencial = fornecedorJuridico.NumeroResidencial.Trim();
                fornecedorJuridicoDB.ComplementoResidencial = fornecedorJuridico.ComplementoResidencial.Trim();
                fornecedorJuridicoDB.Ddd = fornecedorJuridico.Ddd.Trim();
                fornecedorJuridicoDB.Bairro = fornecedorJuridico.Bairro.Trim();
                fornecedorJuridicoDB.Cidade = fornecedorJuridico.Cidade.Trim();
                fornecedorJuridicoDB.Estado = fornecedorJuridico.Estado.Trim();
                _bancoContext.FornecedorJuridico.Update(fornecedorJuridicoDB);
                _bancoContext.SaveChanges();
                return fornecedorJuridicoDB;
            }
            catch (Exception erro) {
                throw new Exception(erro.Message);
            }
        }

        public FornecedorFisico InativarFornecedorFisico(FornecedorFisico fornecedorFisico) {
            FornecedorFisico fornecedorFisicoDB = ListPorIdFisico(fornecedorFisico.Id);
            if (fornecedorFisicoDB == null) throw new Exception("Desculpe, ID não foi encontrado!");
            if (fornecedorFisicoDB.Financeiros.Any(x => x.FinanceiroStatus == FinanceiroStatus.Ativo)) throw new Exception("Cliente/fornecedor possui financeiro em andamento!");
            fornecedorFisicoDB.Status = StatuCliente.Desabilitado;
            _bancoContext.FornecedorFisico.Update(fornecedorFisicoDB);
            _bancoContext.SaveChanges();
            return fornecedorFisicoDB;
        }

        public FornecedorJuridico InativarFornecedorJuridico(FornecedorJuridico fornecedorJuridico) {
            FornecedorJuridico fornecedorJuridicoDB = ListPorIdJuridico(fornecedorJuridico.Id);
            if (fornecedorJuridicoDB == null) throw new Exception("Desculpe, ID não foi encontrado!");
            if (fornecedorJuridicoDB.Financeiros.Any(x => x.FinanceiroStatus == FinanceiroStatus.Ativo)) throw new Exception("Cliente/fornecedor possui financeiro em andamento!");
            fornecedorJuridicoDB.Status = StatuCliente.Desabilitado;
            _bancoContext.FornecedorJuridico.Update(fornecedorJuridicoDB);
            _bancoContext.SaveChanges();
            return fornecedorJuridicoDB;
        }

        public List<FornecedorFisico> ListFornecedoreFisicoDesa() {
            return _bancoContext.FornecedorFisico.Where(x => x.Status == StatuCliente.Desabilitado).ToList();
        }

        public List<FornecedorFisico> ListFornecedoreFisicos() {
            return _bancoContext.FornecedorFisico.Where(x => x.Status == StatuCliente.Habilitado).ToList();
        }

        public List<FornecedorJuridico> ListFornecedoreJuriDesa() {
            return _bancoContext.FornecedorJuridico.Where(x => x.Status == StatuCliente.Desabilitado).ToList();
        }

        public List<FornecedorJuridico> ListFornecedoresJuridicos() {
            return _bancoContext.FornecedorJuridico.Where(x => x.Status == StatuCliente.Habilitado).ToList();
        }

        public FornecedorFisico ListPorIdFisico(int id) {
            return _bancoContext.FornecedorFisico.AsNoTracking().Include(x => x.Financeiros).FirstOrDefault(x => x.Id == id);
        }

        public FornecedorJuridico ListPorIdJuridico(int id) {
            return _bancoContext.FornecedorJuridico.AsNoTracking().Include(x => x.Financeiros).FirstOrDefault(x => x.Id == id);
        }

        public bool Duplicata(FornecedorFisico fornecedor) {
            if (_bancoContext.FornecedorFisico.Any(x => x.Cpf == fornecedor.Cpf || x.Telefone == fornecedor.Telefone || x.Email == fornecedor.Email)) {
                return true;
            }
            return false;
        }
        public bool DuplicataEditar(FornecedorFisico fornecedor, FornecedorFisico fornecedorDB) {
            if (_bancoContext.FornecedorFisico.Any(x => (x.Cpf == fornecedor.Cpf && fornecedor.Cpf != fornecedorDB.Cpf)
                || (x.Email == fornecedor.Email && fornecedor.Email != fornecedorDB.Email)
                || (x.Telefone == fornecedor.Telefone && fornecedor.Telefone != fornecedorDB.Telefone))) {
                return true;
            }
            return false;
        }

        public bool DuplicataJuridica(FornecedorJuridico fornecedor) {
            if (_bancoContext.FornecedorJuridico.Any(x => x.Cnpj == fornecedor.Cnpj || x.RazaoSocial == fornecedor.RazaoSocial
                || fornecedor.NomeFantasia == x.NomeFantasia || x.Telefone == fornecedor.Telefone || x.Email == fornecedor.Email
                || fornecedor.InscricaoEstadual == x.InscricaoEstadual)) {
                return true;
            }
            return false;
        }
        public bool DuplicataEditarJuridica(FornecedorJuridico fornecedor, FornecedorJuridico fornecedorDB) {
            if (_bancoContext.FornecedorJuridico.Any(x => (x.Cnpj == fornecedor.Cnpj && fornecedor.Cnpj != fornecedorDB.Cnpj)
                || (x.Email == fornecedor.Email && fornecedor.Email != fornecedorDB.Email)
                || (x.Telefone == fornecedor.Telefone && fornecedor.Telefone != fornecedorDB.Telefone)
                || (fornecedor.InscricaoEstadual == x.InscricaoEstadual && fornecedor.InscricaoEstadual != fornecedorDB.InscricaoEstadual)
                || (fornecedor.RazaoSocial == x.RazaoSocial && fornecedor.RazaoSocial != fornecedorDB.RazaoSocial)
                || (fornecedor.NomeFantasia == x.NomeFantasia && fornecedor.NomeFantasia != fornecedorDB.NomeFantasia))) {
                return true;
            }
            return false;
        }

        public FornecedorFisico TrimPessoaFisica(FornecedorFisico value) {
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
        public FornecedorJuridico TrimPessoaJuridica(FornecedorJuridico value) {
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
    }
}
