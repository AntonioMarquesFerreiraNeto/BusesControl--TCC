using BusesControl.Models;

namespace BusesControl.Repositorio {
    public interface IFinanceiroRepositorio {
        ClientesContrato listPorIdClientesContrato(int? id);
        Financeiro ListarFinanceiroPorId(int id);
        Financeiro ContabilizarFinanceiro(int id);
    }
}
