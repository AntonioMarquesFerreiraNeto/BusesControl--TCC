using BusesControl.Data;
using BusesControl.Models;
using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using BusesControl.Models.Enums;

namespace BusesControl.Repositorio {
    public class ContratoRepositorio : IContratoRepositorio {
        
        private readonly BancoContext _bancoContext;

        public ContratoRepositorio(BancoContext bancoContext) {
            _bancoContext = bancoContext;
        }
        public Contrato ListarPorId(int id) {
            return _bancoContext.Contrato.FirstOrDefault(x => x.Id == id);  
        }
        public List<Contrato> ListContratoAtivo() {
            var list = _bancoContext.Contrato.ToList();
            return list.Where(x => x.StatusContrato == ContratoStatus.Ativo).ToList();
        }
        public List<Contrato> ListContratoInativo() {
            var list = _bancoContext.Contrato.ToList();
            return list.Where(x => x.StatusContrato == ContratoStatus.Inativo).ToList();
        }
        public Contrato Adicionar(Contrato contrato) {
            try {
                //adicionar apenas dois números após a vírgula neste local, para ficar dentro da normalidade.
                contrato = ContratoTrim(contrato);
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
                contratoDB.ClienteId = contrato.ClienteId;
                contratoDB.MotoristaId = contrato.MotoristaId;
                contratoDB.OnibusId = contrato.OnibusId;
                contratoDB.ValorMonetario = contrato.ValorMonetario;
                contratoDB.QtParcelas = contrato.QtParcelas;
                contratoDB.DataVencimento = contrato.DataVencimento;
                contratoDB.Detalhamento = contrato.Detalhamento.Trim();
                _bancoContext.Update(contratoDB);
                _bancoContext.SaveChanges();
                return contratoDB;
            }
            catch (Exception erro) {
                throw new Exception(erro.Message);
            }
        }
        public Contrato InativarContrato(Contrato contrato) {
            try {
                Contrato contratoDB = ListarPorId(contrato.Id);
                if (contratoDB == null) throw new Exception("Desculpe, ID não foi encontrado.");
                contratoDB.StatusContrato = ContratoStatus.Inativo;
                _bancoContext.Contrato.Update(contratoDB);
                _bancoContext.SaveChanges();
                return contratoDB;
            }
            catch (Exception erro) {
                throw new Exception(erro.Message);
            }
        }
        public Contrato AtivarContrato(Contrato contrato) {
            try {
                Contrato contratoDB = ListarPorId(contrato.Id);
                if (contratoDB == null) throw new Exception("Desculpe, ID não foi encontrado.");
                contratoDB.StatusContrato = ContratoStatus.Ativo;
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
    }
}
