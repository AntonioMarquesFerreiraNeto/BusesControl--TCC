using BusesControl.Data;
using BusesControl.Models;
using BusesControl.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusesControl.Repositorio {
    public class FinanceiroRepositorio : IFinanceiroRepositorio {

        private readonly BancoContext _bancoContext;

        public FinanceiroRepositorio(BancoContext bancoContext) {
            _bancoContext = bancoContext;
        }
        public List<Contrato> ListContratoAdimplentes() {
            return _bancoContext.Contrato.Where(x => x.Aprovacao == StatusAprovacao.Aprovado && x.Situacao == Adimplente.Adimplente)
                .AsNoTracking().Include(x => x.ClientesContratos).ThenInclude(x => x.PessoaFisica)
                .AsNoTracking().Include(x => x.ClientesContratos).ThenInclude(x => x.PessoaJuridica)
                .AsNoTracking().Include(x => x.ClientesContratos).ThenInclude(x => x.ParcelasContrato)
                .AsNoTracking().Include("Motorista")
                .AsNoTracking().Include("Onibus")
                .ToList();
        }
        public List<Contrato> ListContratoInadimplentes() {
            return _bancoContext.Contrato.Where(x => x.Aprovacao == StatusAprovacao.Aprovado && x.Situacao == Adimplente.Inadimplente)
                .AsNoTracking().Include(x => x.ClientesContratos).ThenInclude(x => x.PessoaFisica)
                .AsNoTracking().Include(x => x.ClientesContratos).ThenInclude(x => x.PessoaJuridica)
                .AsNoTracking().Include(x => x.ClientesContratos).ThenInclude(x => x.ParcelasContrato)
                .AsNoTracking().Include("Motorista")
                .AsNoTracking().Include("Onibus")
                .ToList();
        }
        public Contrato ListarJoinPorId(int id) {
            var contrato = _bancoContext.Contrato
                .AsNoTracking().Include("Motorista")
                .AsNoTracking().Include("Onibus")
                .AsNoTracking().Include(x => x.ClientesContratos).ThenInclude(x => x.PessoaFisica)
                .AsNoTracking().Include(x => x.ClientesContratos).ThenInclude(x => x.PessoaJuridica)
                .AsNoTracking().Include(x => x.ClientesContratos).ThenInclude(x => x.ParcelasContrato)
                .FirstOrDefault(x => x.Id == id);
            return contrato;
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

        public Financeiro ListarFinanceiroPorId(int id) {
            return _bancoContext.Financeiro.Include(x => x.ClientesContrato).ThenInclude(x => x.Contrato).FirstOrDefault(x => x.Id == id);
        }

        public Financeiro ContabilizarFinanceiro(int id) {
            try {
                Financeiro financeiroDB = ListarFinanceiroPorId(id);
                if (financeiroDB == null) throw new Exception("Desculpe, ID não foi encontrado!");
                financeiroDB.StatusPagamento = SituacaoPagamento.PagamentoContabilizado;
                _bancoContext.Financeiro.Update(financeiroDB);
                ValidarInadimplenciaCliente(financeiroDB);
                SetValoresPagosContratoAndCliente(financeiroDB);
                _bancoContext.SaveChanges();
                return financeiroDB;
            }
            catch (Exception erro) {
                throw new Exception(erro.Message);
            }
        }
        public void SetValoresPagosContratoAndCliente(Financeiro financeiroDB) {
            ClientesContrato clientesContrato = financeiroDB.ClientesContrato;
            if (!string.IsNullOrEmpty(clientesContrato.ValorTotalPagoCliente.ToString())) {
                clientesContrato.ValorTotalPagoCliente += clientesContrato.Contrato.ValorParcelaContratoPorCliente;
            }
            else {
                clientesContrato.ValorTotalPagoCliente = clientesContrato.Contrato.ValorParcelaContratoPorCliente;
            }
            if (!string.IsNullOrEmpty(clientesContrato.Contrato.ValorTotalPagoContrato.ToString())) {
                clientesContrato.Contrato.ValorTotalPagoContrato += clientesContrato.Contrato.ValorParcelaContratoPorCliente;
            }
            else {
                clientesContrato.Contrato.ValorTotalPagoContrato = clientesContrato.Contrato.ValorParcelaContratoPorCliente;
            }
            _bancoContext.ClientesContrato.Update(clientesContrato);
            _bancoContext.Contrato.Update(clientesContrato.Contrato);
        }
        public void ValidarInadimplenciaCliente(Financeiro value) {
            var pessoaJuridica = _bancoContext.PessoaJuridica.Include(x => x.ClientesContratos).ThenInclude(x => x.ParcelasContrato).FirstOrDefault(pessoa =>
               pessoa.ClientesContratos.Any(clientesContrato => clientesContrato.ParcelasContrato.Any(financeiro =>
               financeiro.Id == value.Id)) && !string.IsNullOrEmpty(pessoa.Cnpj));

            var pessoaFisica = _bancoContext.PessoaFisica.Include(x => x.ClientesContratos).ThenInclude(x => x.ParcelasContrato).FirstOrDefault(pessoa =>
                pessoa.ClientesContratos.Any(clientesContrato => clientesContrato.ParcelasContrato.Any(financeiro =>
                financeiro.Id == value.Id) && !string.IsNullOrEmpty(pessoa.Cpf)));


            if (pessoaFisica != null) {
                int result = ReturnQtParcelasAtrasadaCliente(pessoaFisica.ClientesContratos);
                if (result == 0) {
                    pessoaFisica.Adimplente = Adimplente.Adimplente;
                    _bancoContext.PessoaFisica.Update(pessoaFisica);
                }
            }
            else if (pessoaJuridica != null) {
                int result = ReturnQtParcelasAtrasadaCliente(pessoaJuridica.ClientesContratos);
                if (result == 0) {
                    pessoaJuridica.Adimplente = Adimplente.Adimplente;
                    _bancoContext.PessoaJuridica.Update(pessoaJuridica);
                }
            }
        }
        public int ReturnQtParcelasAtrasadaCliente(List<ClientesContrato> clientesContrato) {
            int cont = 0;
            foreach (var item in clientesContrato) {
                foreach (var item2 in item.ParcelasContrato) {
                    if (item2.StatusPagamento == SituacaoPagamento.Atrasada) cont++;
                }
            }
            return cont;
        }

        //Método agendado que executa sem interação com o usuário. 
        public void TaskMonitorParcelasContrato() {
            var contratos = _bancoContext.Contrato.Where(x => x.Aprovacao == StatusAprovacao.Aprovado)
                .AsNoTracking().Include(x => x.ClientesContratos).ThenInclude(x => x.ParcelasContrato)
                .AsNoTracking().Include(x => x.ClientesContratos).ThenInclude(x => x.PessoaFisica)
                .AsNoTracking().Include(x => x.ClientesContratos).ThenInclude(x => x.PessoaJuridica)
                .ToList();

            //Verifica se a parcela está atrasada e realiza as devidas medidas. 
            DateTime dateAtual = DateTime.Now.Date;
            foreach (var contrato in contratos) {
                foreach (var clientesContrato in contrato.ClientesContratos) {
                    foreach (var financeiro in clientesContrato.ParcelasContrato) {
                        if (dateAtual > financeiro.DataVencimentoParcela && financeiro.StatusPagamento != SituacaoPagamento.PagamentoContabilizado) {
                            Financeiro financeiroDB = _bancoContext.Financeiro.FirstOrDefault(x => x.Id == financeiro.Id);
                            financeiroDB.StatusPagamento = SituacaoPagamento.Atrasada;
                            financeiroDB.ValorJuros = setJurosParcela(financeiroDB, contrato);
                            var pessoaFisicaDB = _bancoContext.PessoaFisica.FirstOrDefault(x => x.Id == clientesContrato.PessoaFisicaId);
                            var pessoaJuridicaDB = _bancoContext.PessoaJuridica.FirstOrDefault(x => x.Id == clientesContrato.PessoaJuridicaId);
                            _bancoContext.Financeiro.Update(financeiroDB);
                            if (pessoaFisicaDB != null) {
                                pessoaFisicaDB.Adimplente = Adimplente.Inadimplente;
                                _bancoContext.PessoaFisica.Update(pessoaFisicaDB);
                            }
                            else {
                                pessoaJuridicaDB.Adimplente = Adimplente.Inadimplente;
                                _bancoContext.PessoaJuridica.Update(pessoaJuridicaDB);
                            }
                            _bancoContext.SaveChanges();
                        }
                    }
                }
            }
        }
        public decimal? setJurosParcela(Financeiro financeiro, Contrato contrato) {
            DateTime dataAtual = DateTime.Now.Date;
            int qtMeses = ReturnQtmMeses(dataAtual) - ReturnQtmMeses(financeiro.DataVencimentoParcela.Value.Date);
            if (qtMeses == 0) {
                decimal? valorJuros = (contrato.ValorParcelaContratoPorCliente * 2) / 100;
                return valorJuros;
            }
            else {
                decimal? valorJuros = ((contrato.ValorParcelaContratoPorCliente * (2 * (qtMeses + 1))) / 100);
                return valorJuros;
            }
        }
        public int ReturnQtmMeses(DateTime date) {
            return date.Year * 12 + date.Month;
        }
    }
}
