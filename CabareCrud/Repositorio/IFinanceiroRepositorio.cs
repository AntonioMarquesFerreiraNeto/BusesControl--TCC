using BusesControl.Models;
using System.Collections.Generic;

namespace BusesControl.Repositorio {
    public interface IFinanceiroRepositorio {
        public List<Contrato> ContratosEmAndamento();
        public List<Contrato> ContratosEncerrados();
        public Contrato ListarJoinPorId(int id);
        public ClientesContrato listPorIdClientesContrato(int? id);
        public ParcelasCliente ListarFinanceiroPorId(int id);
        public ParcelasCliente ContabilizarFinanceiro(int id);
        public ClientesContrato RescisaoContrato(ClientesContrato clientesContrato);
        public void TaskMonitorParcelasContrato();
    }
}
