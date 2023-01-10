using BusesControl.Models;
using System.Collections.Generic;

namespace BusesControl.Repositorio {
    public interface IFornecedorRepositorio {
        public FornecedorFisico ListPorIdFisico(int id);
        public FornecedorJuridico ListPorIdJuridico(int id);
        public List<FornecedorFisico> ListFornecedoreFisicos();
        public List<FornecedorFisico> ListFornecedoreFisicoDesa();
        public List<FornecedorJuridico> ListFornecedoreJuriDesa();
        public List<FornecedorJuridico> ListFornecedoresJuridicos();
        public FornecedorFisico AdicionarFornecedorFisico(FornecedorFisico fornecedorFisico);
        public FornecedorJuridico AdicionarFornecedorJuridico(FornecedorJuridico fornecedorJuridico);
        public FornecedorFisico EditarFornecedorFisico(FornecedorFisico fornecedorFisico);
        public FornecedorJuridico EditarFornecedorJuridico(FornecedorJuridico fornecedorJuridico);
        public FornecedorFisico InativarFornecedorFisico(FornecedorFisico fornecedorFisico);
        public FornecedorJuridico InativarFornecedorJuridico(FornecedorJuridico fornecedorJuridico);
        public FornecedorFisico AtivarFornecedorFisico(FornecedorFisico fornecedorFisico);
        public FornecedorJuridico AtivarFornecedorJuridico(FornecedorJuridico fornecedorJuridico);
    }
}
