using BusesControl.Models;
using System.Collections.Generic;

namespace BusesControl.Repositorio {
    public interface IContratoRepositorio {
        public List<Contrato> ListContratoAtivo();
        public List<Contrato> ListContratoInativo();
        public Contrato ListarPorId(int id);
        public Contrato ListarJoinPorId(int id);
        public Contrato Adicionar(Contrato contrato);
        public Contrato EditarContrato(Contrato contrato);
        public Contrato InativarContrato(Contrato contrato);
        public Contrato AtivarContrato(Contrato contrato);
        public decimal? ValorTotalContrato();
    }
}
