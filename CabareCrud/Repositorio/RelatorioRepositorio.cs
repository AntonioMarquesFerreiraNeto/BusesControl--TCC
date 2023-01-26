using BusesControl.Data;
using BusesControl.Models;
using BusesControl.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusesControl.Repositorio {
    public class RelatorioRepositorio : IRelatorioRepositorio {

        private readonly BancoContext _bancoContext;
        private readonly IContratoRepositorio _contratoRepositorio;
        private readonly IFinanceiroRepositorio _financeiroRepositorio;

        public RelatorioRepositorio(BancoContext bancoContext, IContratoRepositorio contratoRepositorio, IFinanceiroRepositorio financeiroRepositorio) {
            _bancoContext = bancoContext;
            _contratoRepositorio = contratoRepositorio; 
            _financeiroRepositorio = financeiroRepositorio;
        }

        public decimal? ValorTotAprovados() {
            List<Contrato> ListContrato = _contratoRepositorio.ListContratoAprovados();
            decimal? valorTotalContrato = 0;
            foreach (Contrato contrato in ListContrato) {
                valorTotalContrato += contrato.ValorMonetario;
            }
            return valorTotalContrato;
        }
        public decimal? ValorTotEmAnalise() {
            List<Contrato> ListContrato = _contratoRepositorio.ListContratoEmAnalise();
            decimal? valorTotalContrato = 0;
            foreach (Contrato contrato in ListContrato) {
                valorTotalContrato += contrato.ValorMonetario;
            }
            return valorTotalContrato;
        }
        public decimal? ValorTotContratos() {
            List<Contrato> ListContrato = _contratoRepositorio.ListContratoAprovados();
            ListContrato.AddRange(_contratoRepositorio.ListContratoEmAnalise());
            decimal? valorTot = 0;
            foreach (Contrato contrato in ListContrato) {
                valorTot += contrato.ValorMonetario;
            }
            return valorTot;
        }
        public decimal? ValorTotPagoContrato() {
            List<Contrato> contratos = _contratoRepositorio.ListContratoAprovados();
            decimal? valorPago = 0;
            foreach (var item in contratos) {
                if (!string.IsNullOrEmpty(item.ValorTotalPagoContrato.ToString())) {
                    valorPago += item.ValorTotalPagoContrato;
                }
                foreach (var rescisao in item.Rescisoes) {
                    if (!string.IsNullOrEmpty(rescisao.Multa.ToString())) {
                        valorPago += rescisao.Multa;
                    }
                }
            }
            return valorPago;
        }
        public decimal? ValorTotPendenteContrato() {
            List<Contrato> contratos = _contratoRepositorio.ListContratoAprovados();
            decimal? valorPago = 0;
            decimal? valorTotal = 0;
            foreach (var item in contratos) {
                if (!string.IsNullOrEmpty(item.ValorTotalPagoContrato.ToString())) {
                    valorPago += item.ValorTotalPagoContrato;
                }
                valorTotal += item.ValorMonetario;
            }
            decimal? valorPedente = valorTotal - valorPago;
            return valorPedente;
        }
        public decimal? ValorTotPagoReceitas() {
            List<Financeiro> financeiros = _financeiroRepositorio.ListFinanceiros();
            decimal? valorPago = 0;
            foreach (var financeiro in financeiros) {
                if (financeiro.FinanceiroStatus == FinanceiroStatus.Ativo && financeiro.DespesaReceita == DespesaReceita.Receita) {
                    if (!string.IsNullOrEmpty(financeiro.ValorTotalPagoCliente.ToString())) {
                        valorPago += financeiro.ValorTotalPagoCliente;
                    }
                }
            }
            List<Rescisao> rescisoes = _bancoContext.Rescisao.Where(x => !string.IsNullOrEmpty(x.Multa.ToString())).ToList();
            foreach (var rescisao in rescisoes) {
                valorPago += rescisao.Multa;
            }
            return valorPago;
        }

        public decimal? ValorTotPagoDespesas() {
            List<Financeiro> financeiros = _financeiroRepositorio.ListFinanceiros();
            decimal? valorPago = 0;
            foreach (var financeiro in financeiros) {
                if (financeiro.FinanceiroStatus == FinanceiroStatus.Ativo && financeiro.DespesaReceita == DespesaReceita.Despesa) {
                    if (!string.IsNullOrEmpty(financeiro.ValorTotalPagoCliente.ToString())) {
                        valorPago += financeiro.ValorTotalPagoCliente;
                    }
                }
            }
            return valorPago;
        }

        public decimal? ValorTotReceitas() {
            List<Financeiro> financeiros = _financeiroRepositorio.ListFinanceiros();
            decimal? valorPago = 0;
            foreach (var financeiro in financeiros) {
                if (financeiro.FinanceiroStatus == FinanceiroStatus.Ativo && financeiro.DespesaReceita == DespesaReceita.Receita) {
                    valorPago += financeiro.ValorTotDR;
                }
            }
            return valorPago;
        }

        public decimal? ValorTotDespesas() {
            List<Financeiro> financeiros = _financeiroRepositorio.ListFinanceiros();
            decimal? valorPago = 0;
            foreach (var financeiro in financeiros) {
                if (financeiro.FinanceiroStatus == FinanceiroStatus.Ativo && financeiro.DespesaReceita == DespesaReceita.Despesa) {
                    valorPago += financeiro.ValorTotDR;
                }
            }
            return valorPago;
        }
        public decimal? ValorTotJurosCliente(int? id) {
            List<Parcelas> financeiros = _bancoContext.Parcelas.Where(x => x.FinanceiroId == id).ToList();
            decimal? totValorJuros = 0;
            foreach (var item in financeiros) {
                if (!string.IsNullOrEmpty(item.ValorJuros.ToString())) {
                    totValorJuros += item.ValorJuros;
                }
            }
            return totValorJuros;
        }

        public int QtContratosAprovados() {
            int quantidade = _bancoContext.Contrato.Where(x => x.Aprovacao == StatusAprovacao.Aprovado && x.StatusContrato == ContratoStatus.Ativo).ToList().Count;
            return quantidade;
        }
        public int QtContratosEmAnalise() {
            int quantidade = _bancoContext.Contrato.Where(x => x.Aprovacao == StatusAprovacao.EmAnalise && x.StatusContrato == ContratoStatus.Ativo).ToList().Count;
            return quantidade;
        }
        public int QtContratosNegados() {
            int quantidade = _bancoContext.Contrato.Where(x => x.Aprovacao == StatusAprovacao.Negado && x.StatusContrato == ContratoStatus.Ativo).ToList().Count;
            return quantidade;
        }
        public int QtContratos() {
            int quantidade = _bancoContext.Contrato.Where(x => x.StatusContrato == ContratoStatus.Ativo).ToList().Count;
            return quantidade;
        }
        public int QtClientesAdimplentes() {
            int quantidade = _bancoContext.PessoaFisica.Where(x => x.Adimplente == Adimplencia.Adimplente && x.Status == StatuCliente.Habilitado).ToList().Count;
            quantidade += _bancoContext.PessoaJuridica.Where(x => x.Adimplente == Adimplencia.Adimplente && x.Status == StatuCliente.Habilitado).ToList().Count;
            return quantidade;
        }
        public int QtClientesInadimplentes() {
            int quantidade = _bancoContext.PessoaFisica.Where(x => x.Adimplente == Adimplencia.Inadimplente && x.Status == StatuCliente.Habilitado).ToList().Count;
            quantidade += _bancoContext.PessoaJuridica.Where(x => x.Adimplente == Adimplencia.Inadimplente && x.Status == StatuCliente.Habilitado).ToList().Count;
            return quantidade;
        }

        public int QtClientes() {
            int quantidade = _bancoContext.PessoaFisica.Where(x => x.Status == StatuCliente.Habilitado).ToList().Count;
            quantidade += _bancoContext.PessoaJuridica.Where(x => x.Status == StatuCliente.Habilitado).ToList().Count;
            return quantidade;
        }
        public int QtMotoristas() {
            int quantidade = _bancoContext.Funcionario
                  .Where(x => x.Status == StatuFuncionario.Habilitado && x.Cargos == CargoFuncionario.Motorista).ToList().Count;
            return quantidade;
        }
        public int QtMotoristasVinculados() {
            int quantidade = _bancoContext.Funcionario
                .Where(x => x.Status == StatuFuncionario.Habilitado && x.Cargos == CargoFuncionario.Motorista && x.Contratos.Any(x => x.StatusContrato == ContratoStatus.Ativo)).ToList().Count;
            return quantidade;
        }
        public int QtOnibus() {
            int quantidade = _bancoContext.Onibus
                .Where(x => x.StatusOnibus == OnibusStatus.Habilitado).ToList().Count;
            return quantidade;
        }
        public int QtOnibusVinculados() {
            int quantidade = _bancoContext.Onibus
                .Where(x => x.StatusOnibus == OnibusStatus.Habilitado && x.Contratos.Any(x => x.StatusContrato == ContratoStatus.Ativo)).ToList().Count;
            return quantidade;
        }
    }
}
