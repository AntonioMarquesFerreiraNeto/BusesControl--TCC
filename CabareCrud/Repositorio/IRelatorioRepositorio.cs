namespace BusesControl.Repositorio {
    public interface IRelatorioRepositorio {
        public decimal? ValorTotAprovados();
        public decimal? ValorTotEmAnalise();
        public decimal? ValorTotContratos();
        public decimal? ValorTotPagoContrato();
        public decimal? ValorTotPendenteContrato();
        public decimal? ValorTotJurosCliente(int? id);
        public decimal? ValorTotPagoReceitas();
        public decimal? ValorTotPagoDespesas();
        public decimal? ValorTotReceitas();
        public decimal? ValorTotDespesas();
        public int QtContratosAprovados();
        public int QtContratosEmAnalise();
        public int QtContratosNegados();
        public int QtContratos();
        public int QtClientesAdimplentes();
        public int QtClientesInadimplentes();
        public int QtClientes();
        public int QtMotoristas();
        public int QtMotoristasVinculados();
        public int QtOnibus();
        public int QtOnibusVinculados();
    }
}
