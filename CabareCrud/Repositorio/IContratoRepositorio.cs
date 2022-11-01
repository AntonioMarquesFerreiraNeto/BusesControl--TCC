using BusesControl.Models;
using System.Collections.Generic;

namespace BusesControl.Repositorio {
    public interface IContratoRepositorio {
        public List<Contrato> ListContratoAtivo();
        public List<Contrato> ListContratoInativo();
        public List<Contrato> ListContratoEmAnalise();
        public List<Contrato> ListContratoNegados();
        public List<Contrato> ListContratoAprovados();
        public Contrato ListarPorId(int id);
        public Contrato ListarJoinPorId(int id);
        public Contrato Adicionar(Contrato contrato);
        public Contrato EditarContrato(Contrato contrato);
        public Contrato InativarContrato(Contrato contrato);
        public Contrato AtivarContrato(Contrato contrato);
        public Contrato AprovarContrato(Contrato contrato);
        public Contrato RevogarContrato(Contrato contrato);
        public decimal? ValorTotAprovados();
        public decimal? ValorTotEmAnalise();
        public decimal? ValorTotContratos();
    }
}
