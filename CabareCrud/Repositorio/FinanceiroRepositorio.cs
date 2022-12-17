using BusesControl.Data;
using BusesControl.Models;
using BusesControl.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace BusesControl.Repositorio {
    public class FinanceiroRepositorio : IFinanceiroRepositorio {

        private readonly BancoContext _bancoContext;

        public FinanceiroRepositorio(BancoContext bancoContext) {
            _bancoContext = bancoContext;
        }

        public ClientesContrato listPorIdClientesContrato(int? id) {
            return _bancoContext.ClientesContrato
                   .Include(x => x.PessoaFisica)
                   .Include(x => x.PessoaJuridica)
                   .Include(x => x.PessoaJuridica)
                   .Include(x => x.Contrato)
                   .Include(x => x.ParcelasContrato)
                   .FirstOrDefault(x => x.Id == id);
        }

        public Financeiro ContabilizarFinanceiro(int id) {
            try {
                Financeiro financeiroDB = ListarFinanceiroPorId(id);
                if (financeiroDB == null) throw new Exception("Desculpe, ID não foi encontrado!");
                financeiroDB.StatusPagamento = SituacaoPagamento.PagamentoContabilizado;
                _bancoContext.Financeiro.Update(financeiroDB);
                _bancoContext.SaveChanges();
                return financeiroDB;
            }
            catch (Exception erro) {
                throw new Exception(erro.Message);
            }
        }

        public Financeiro ListarFinanceiroPorId(int id) {
            return _bancoContext.Financeiro.FirstOrDefault(x => x.Id == id);
        }
    }
}
