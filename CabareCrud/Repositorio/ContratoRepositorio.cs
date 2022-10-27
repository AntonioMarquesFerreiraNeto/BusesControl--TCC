using BusesControl.Data;
using BusesControl.Models;
using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using BusesControl.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Repositorio {
    public class ContratoRepositorio : IContratoRepositorio {
        
        private readonly BancoContext _bancoContext;

        public ContratoRepositorio(BancoContext bancoContext) {
            _bancoContext = bancoContext;
        }

        public Contrato ListarPorId(int id) {
            return _bancoContext.Contrato.FirstOrDefault(x => x.Id == id);  
        }
        public Contrato ListarJoinPorId(int id) {
            return _bancoContext.Contrato
                .AsNoTracking().Include("Motorista")
                .AsNoTracking().Include("Onibus")
                .AsNoTracking().Include("PessoaFisica")
                .AsNoTracking().Include("PessoaJuridica")
                .FirstOrDefault(x => x.Id == id);
        }

        public List<Contrato> ListContratoAtivo() {
            return _bancoContext.Contrato.Where(x => x.StatusContrato == ContratoStatus.Ativo)
                .AsNoTracking().Include("Motorista")
                .AsNoTracking().Include("Onibus")
                .AsNoTracking().Include("PessoaFisica")
                .AsNoTracking().Include("PessoaJuridica")
                .ToList();
        }
        public List<Contrato> ListContratoInativo() {
            return _bancoContext.Contrato.Where(x => x.StatusContrato == ContratoStatus.Inativo)
               .AsNoTracking().Include("Motorista")
                .AsNoTracking().Include("Onibus")
                .AsNoTracking().Include("PessoaFisica")
                .AsNoTracking().Include("PessoaJuridica")
               .ToList();
        }
        public List<Contrato> ListContratoEmAnalise() {
            return _bancoContext.Contrato.Where(x => x.Aprovacao == StatusAprovacao.EmAnalise)
                .AsNoTracking().Include("Motorista")
                .AsNoTracking().Include("Onibus")
                .AsNoTracking().Include("PessoaFisica")
                .AsNoTracking().Include("PessoaJuridica")
                .ToList();
        }
        public List<Contrato> ListContratoNegados() {
            return _bancoContext.Contrato.Where(x => x.Aprovacao == StatusAprovacao.Negado)
                .AsNoTracking().Include("Motorista")
                .AsNoTracking().Include("Onibus")
                .AsNoTracking().Include("PessoaFisica")
                .AsNoTracking().Include("PessoaJuridica")
                .ToList();
        }
        public List<Contrato> ListContratoAprovados() {
            return _bancoContext.Contrato.Where(x => x.Aprovacao == StatusAprovacao.Aprovado)
                .AsNoTracking().Include("Motorista")
                .AsNoTracking().Include("Onibus")
                .AsNoTracking().Include("PessoaFisica")
                .AsNoTracking().Include("PessoaJuridica")
                .ToList();
        }

        public Contrato Adicionar(Contrato contrato) {
            try {
                //adicionar apenas dois números após a vírgula neste local, para ficar dentro da normalidade.
                contrato = ContratoTrim(contrato);
                contrato.ValorParcelaContrato = contrato.ReturnValorParcela();
                _bancoContext.Contrato.Add(contrato);
                _bancoContext.SaveChanges();
                return contrato;
            }
            catch (Exception erro) {
                throw new Exception(erro.Message);
            }
        }
        public Contrato EditarContrato(Contrato contrato) {
            try {
                Contrato contratoDB = ListarPorId(contrato.Id);
                if (contratoDB == null) throw new Exception("Desculpe, ID não foi encontrado.");
                if (!string.IsNullOrEmpty(contrato.PessoaFisicaId.ToString())) {
                    contratoDB.PessoaFisicaId = contrato.PessoaFisicaId;
                    contratoDB.PessoaJuridicaId = null;
                }
                else {
                    contratoDB.PessoaJuridicaId = contrato.PessoaJuridicaId;
                    contratoDB.PessoaFisicaId = null;
                }
                contratoDB.MotoristaId = contrato.MotoristaId;
                contratoDB.OnibusId = contrato.OnibusId;
                contratoDB.ValorMonetario = contrato.ValorMonetario;
                contratoDB.QtParcelas = contrato.QtParcelas;
                contratoDB.ValorParcelaContrato = contratoDB.ReturnValorParcela();
                contratoDB.DataVencimento = contrato.DataVencimento;
                contratoDB.Detalhamento = contrato.Detalhamento.Trim();
                _bancoContext.Contrato.Update(contratoDB);
                _bancoContext.SaveChanges();
                return contratoDB;
            }
            catch (Exception erro) {
                throw new Exception(erro.Message);
            }
        }
        public Contrato InativarContrato(Contrato contrato) {
            try {
                Contrato contratoDB = ListarJoinPorId(contrato.Id);
                if (contratoDB == null) throw new Exception("Desculpe, ID não foi encontrado.");
                if (contratoDB.Aprovacao == StatusAprovacao.Aprovado) {
                    throw new Exception("Não é possível inativar contratos em andamento.");
                }
                contratoDB.StatusContrato = ContratoStatus.Inativo;
                _bancoContext.Contrato.Update(contratoDB);
                _bancoContext.SaveChanges();
                return contrato;
            }
            catch (Exception erro) {
                throw new Exception(erro.Message);
            }
        }
        public Contrato AtivarContrato(Contrato contrato) {
            try {
                Contrato contratoDB = ListarJoinPorId(contrato.Id);
                if (contratoDB == null) throw new Exception("Desculpe, ID não foi encontrado.");
                contratoDB.StatusContrato = ContratoStatus.Ativo;
                _bancoContext.Update(contratoDB);
                _bancoContext.SaveChanges();
                return contrato;
            }
            catch (Exception erro) {
                throw new Exception(erro.Message);
            }
        }
        public Contrato AprovarContrato(Contrato contrato) {
            try {
                Contrato contratoDB = ListarJoinPorId(contrato.Id);
                if (contratoDB == null) {
                    throw new Exception("Desculpe, ID não foi encontrado.");
                }
                contratoDB.Aprovacao = StatusAprovacao.Aprovado;
                _bancoContext.Update(contratoDB);
                _bancoContext.SaveChanges();
                return contratoDB;
            }
            catch (Exception erro) {
                throw new Exception(erro.Message);
            }
        }
        public Contrato RevogarContrato(Contrato contrato) {
            try {
                Contrato contratoDB = ListarJoinPorId(contrato.Id);
                if (contratoDB == null) {
                    throw new Exception("Desculpe, ID não foi encontrado.");
                }
                contratoDB.Aprovacao = StatusAprovacao.Negado;
                _bancoContext.Update(contratoDB);
                _bancoContext.SaveChanges();
                return contratoDB;
            }
            catch (Exception erro) {
                throw new Exception(erro.Message);
            }
        }

        public Contrato ContratoTrim(Contrato contrato) {
            contrato.Detalhamento = contrato.Detalhamento.Trim();
            return contrato;
        }
        public decimal? ValorTotalContrato() {
            List<Contrato> ListContrato = ListContratoAprovados();
            decimal? valorTotalContrato = 0;
            foreach (Contrato contrato in ListContrato) {
                valorTotalContrato += contrato.ValorMonetario;
            }
            return valorTotalContrato;
        }
    }
}
