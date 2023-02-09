using BusesControl.Models;
using BusesControl.Models.ViewModels;
using System.Collections.Generic;

namespace BusesControl.Repositorio {
    public interface IContratoRepositorio {
        public ClientesContrato ListarClientesContratoId(int id);
        public List<Contrato> ListContratoAtivo();
        public List<Contrato> ListContratoInativo();
        public List<Contrato> ListContratoEmAnalise();
        public List<Contrato> ListContratoNegados();
        public List<Contrato> ListContratoAprovados();
        public Contrato ListarPorId(int id);
        public Contrato ListarJoinPorId(int id);
        public Contrato ListarJoinPorIdAprovado(int? id);
        public ModelsContrato Adicionar(ModelsContrato modelsContrato);
        public ModelsContrato EditarContrato(ModelsContrato modelsContrato);
        public Contrato InativarContrato(Contrato contrato);
        public Contrato AtivarContrato(Contrato contrato);
        public Contrato AprovarContrato(Contrato contrato);
        public Contrato RevogarContrato(Contrato contrato);
    }
}
