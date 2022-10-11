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
                _bancoContext.Contrato.Add(contrato);
                _bancoContext.SaveChanges();
                return contrato;
            }
            catch (Exception erro) {
                throw new Exception(erro.Message);
            }
        }
    }
}
