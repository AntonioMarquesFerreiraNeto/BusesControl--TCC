using BusesControl.Models;
using System.Collections.Generic;

namespace BusesControl.Repositorio {
    public interface IFinanceiroRepositorio {
        public List<Financeiro> ListFinanceiros();
        public Financeiro ReturnPorId(int id);
        public List<Financeiro> ListFinanceirosFiltros(Filtros filtros);
        public Financeiro listPorIdFinanceiro(int? id);
        public Parcelas ListarFinanceiroPorId(int id);
        public Parcelas ContabilizarParcela(int id);
        public Contrato ListarJoinPorId(int id);
        public Financeiro RescisaoContrato(Financeiro financeiro);
        public Financeiro AdicionarDespesa(Financeiro financeiro);
        public Financeiro AdicionarReceita(Financeiro financeiro);
        public Financeiro EditarLancamento(Financeiro financeiro);
        public Financeiro InativarReceitaOrDespesa(Financeiro financeiro);
        public void TaskMonitorParcelas();
        public void TaskMonitorParcelasLancamento();
        public void TaskMonitorPdfRescisao();
        public ClientesContrato ConfirmarImpressaoPdf(ClientesContrato clientesContrato);
        public Financeiro ListFinanceiroPorContratoAndClientesContrato(int? id);
    }
}
