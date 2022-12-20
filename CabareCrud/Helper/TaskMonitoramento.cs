using BusesControl.Repositorio;

namespace BusesControl.Helper {
    public class TaskMonitoramento : ITaskMonitoramento {
        private readonly ITaskMonitoramento _taskMonitoramento;
        private readonly IFinanceiroRepositorio _financeiroRepositorio;

        public TaskMonitoramento(ITaskMonitoramento taskMonitoramento, IFinanceiroRepositorio financeiroRepositorio) {
            _taskMonitoramento = taskMonitoramento;
            _financeiroRepositorio = financeiroRepositorio;
        }

        //Apagar método amanhã, já que ele é apenas para ilustrar um método que inicia um método na camada de serviço do financeiro.
        public void TaskDateVencimentoContrato() {
            throw new System.NotImplementedException();
        }
    }
}
