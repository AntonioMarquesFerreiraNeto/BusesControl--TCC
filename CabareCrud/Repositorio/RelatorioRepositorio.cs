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

        public RelatorioRepositorio(BancoContext bancoContext, IContratoRepositorio contratoRepositorio) {
            _bancoContext = bancoContext;
            _contratoRepositorio = contratoRepositorio; 
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
        public decimal? ValorTotPago() {
            List<Contrato> contratos = _contratoRepositorio.ListContratoAprovados();
            decimal? valorPago = 0;
            foreach (var item in contratos) {
                if (!string.IsNullOrEmpty(item.ValorTotalPagoContrato.ToString())) {
                    valorPago += item.ValorTotalPagoContrato;
                }
            }
            return valorPago;
        }
        public decimal? ValorTotPendente() {
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
        public decimal? ValorTotJurosCliente(int? id) {
            List<Financeiro> financeiros = _bancoContext.Financeiro.Where(x => x.ClientesContratoId == id).ToList();
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
        public int QtContratosAdimplentes() {
            int quantidade = _bancoContext.Contrato.Where(x => x.Situacao == Adimplente.Adimplente && x.StatusContrato == ContratoStatus.Ativo).ToList().Count;
            return quantidade;
        }
        public int QtContratosInadimplentes() {
            int quantidade = _bancoContext.Contrato.Where(x => x.Situacao == Adimplente.Inadimplente && x.StatusContrato == ContratoStatus.Ativo).ToList().Count;
            return quantidade;
        }
        public int QtContratos() {
            int quantidade = _bancoContext.Contrato.Where(x => x.StatusContrato == ContratoStatus.Ativo).ToList().Count;
            return quantidade;
        }
        public int QtClientesAdimplentes() {
            int quantidade = _bancoContext.PessoaFisica.Where(x => x.Adimplente == Adimplente.Adimplente && x.Status == StatuCliente.Habilitado).ToList().Count;
            quantidade += _bancoContext.PessoaJuridica.Where(x => x.Adimplente == Adimplente.Adimplente && x.Status == StatuCliente.Habilitado).ToList().Count;
            return quantidade;
        }
        public int QtClientesInadimplentes() {
            int quantidade = _bancoContext.PessoaFisica.Where(x => x.Adimplente == Adimplente.Inadimplente && x.Status == StatuCliente.Habilitado).ToList().Count;
            quantidade += _bancoContext.PessoaJuridica.Where(x => x.Adimplente == Adimplente.Inadimplente && x.Status == StatuCliente.Habilitado).ToList().Count;
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
