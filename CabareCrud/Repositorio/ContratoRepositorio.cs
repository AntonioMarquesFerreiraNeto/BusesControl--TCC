using BusesControl.Data;
using BusesControl.Models;
using System;

namespace BusesControl.Repositorio {
    public class ContratoRepositorio : IContratoRepositorio {
        
        private readonly BancoContext _bancoContext;

        public ContratoRepositorio(BancoContext bancoContext) {
            _bancoContext = bancoContext;
        }

        public Contrato Adicionar(Contrato contrato) {
            try {
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
