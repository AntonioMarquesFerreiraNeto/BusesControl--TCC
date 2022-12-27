namespace BusesControl.Repositorio {
    public interface IRelatorioRepositorio {
        public decimal? ValorTotAprovados();
        public decimal? ValorTotEmAnalise();
        public decimal? ValorTotContratos();
        public int QtContratosAprovados();
        public int QtContratosEmAnalise();
        public int QtContratosNegados();
        public int QtContratosAdimplentes();
        public int QtContratosInadimplentes();
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
