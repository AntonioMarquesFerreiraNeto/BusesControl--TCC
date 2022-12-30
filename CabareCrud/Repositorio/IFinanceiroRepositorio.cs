using BusesControl.Models;
using System.Collections.Generic;

namespace BusesControl.Repositorio {
    public interface IFinanceiroRepositorio {
        public Contrato ListarJoinPorId(int id);
        public ClientesContrato listPorIdClientesContrato(int? id);
        public Financeiro ListarFinanceiroPorId(int id);
        public Financeiro ContabilizarFinanceiro(int id);
        public void TaskMonitorParcelasContrato();
    }
}
