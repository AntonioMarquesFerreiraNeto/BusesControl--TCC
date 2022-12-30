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
            if (!string.IsNullOrEmpty(financeiroDB.ValorJuros.ToString())) {
                if (!string.IsNullOrEmpty(clientesContrato.ValorTotTaxaJurosPaga.ToString())) {
                    clientesContrato.ValorTotTaxaJurosPaga += financeiroDB.ValorJuros;
                }
                else {
                    clientesContrato.ValorTotTaxaJurosPaga = financeiroDB.ValorJuros;
                }
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
                    if (!string.IsNullOrEmpty(pessoaFisica.IdVinculacaoContratual.ToString())) {
                        pessoaFisica.Adimplente = Adimplente.Adimplente;
                        _bancoContext.PessoaFisica.Update(pessoaFisica);
                        //Chama o método que valida e seta com adimplente se passar na validação.
                        ValidarAndSetAdimplenteClienteResponsavel(pessoaFisica.IdVinculacaoContratual);
                    }
                    //Se o cliente for maior de idade, este método que é executado e realiza a validação se o cliente possui clientes vinculados em inadimplência.
                    else {
                        int clientesVinculadosInadimplentes = _bancoContext.PessoaFisica.Where(x => x.IdVinculacaoContratual == pessoaFisica.Id && x.Adimplente == Adimplente.Inadimplente).ToList().Count;
                        if (clientesVinculadosInadimplentes == 0) {
                            pessoaFisica.Adimplente = Adimplente.Adimplente;
                            _bancoContext.PessoaFisica.Update(pessoaFisica);
                        }
                    }
                }
            }
            else if (pessoaJuridica != null) {
                int result = ReturnQtParcelasAtrasadaCliente(pessoaJuridica.ClientesContratos);
                if (result == 0) {
                    int clientesVinculadosInadimplentes = _bancoContext.PessoaFisica.Where(x => x.IdVinculacaoContratual == pessoaJuridica.Id && x.Adimplente == Adimplente.Inadimplente).ToList().Count;
                    if (clientesVinculadosInadimplentes == 0) {
                        pessoaJuridica.Adimplente = Adimplente.Adimplente;
                        _bancoContext.PessoaJuridica.Update(pessoaJuridica);
                    }
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
        //Método que valida se o cliente tem parcelas atrasadas e outros clientes menores de idade com parcelas atrasadas,
        //caso não tenha, o mesmo é colocado em adimplência por não ter nenhuma infração das regras do contrato na aplicação. 
        public void ValidarAndSetAdimplenteClienteResponsavel(int? id) {
            PessoaFisica pessoaFisicaResponsavel = _bancoContext.PessoaFisica.Include(x => x.ClientesContratos).ThenInclude(x => x.ParcelasContrato).FirstOrDefault(x => x.Id == id);
            if (pessoaFisicaResponsavel != null) {
                int resultParcelasAtrasadas = ReturnQtParcelasAtrasadaCliente(pessoaFisicaResponsavel.ClientesContratos);
                int clientesVinculadosInadimplentes = _bancoContext.PessoaFisica.Where(x => x.IdVinculacaoContratual == id && x.Adimplente == Adimplente.Inadimplente).ToList().Count;
                if (resultParcelasAtrasadas == 0 && clientesVinculadosInadimplentes == 1) {
                    pessoaFisicaResponsavel.Adimplente = Adimplente.Adimplente;
                    _bancoContext.Update(pessoaFisicaResponsavel);
                }
            }
            else {
                PessoaJuridica pessoaJuridicaResponsavel = _bancoContext.PessoaJuridica.Include(x => x.ClientesContratos).ThenInclude(x => x.ParcelasContrato).FirstOrDefault(x => x.Id == id);
                if (pessoaJuridicaResponsavel != null) {
                    int resultParcelasAtrasadas = ReturnQtParcelasAtrasadaCliente(pessoaJuridicaResponsavel.ClientesContratos);
                    int clientesVinculadosInadimplentes = _bancoContext.PessoaFisica.Where(x => x.IdVinculacaoContratual == id && x.Adimplente == Adimplente.Inadimplente).ToList().Count;
                    if (resultParcelasAtrasadas == 0 && clientesVinculadosInadimplentes == 1) {
                        pessoaJuridicaResponsavel.Adimplente = Adimplente.Adimplente;
                        _bancoContext.Update(pessoaJuridicaResponsavel);
                    }
                }
            }
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
                                if (!string.IsNullOrEmpty(pessoaFisicaDB.IdVinculacaoContratual.ToString())) {
                                    SetInadimplenciaClienteResponsavel(pessoaFisicaDB.IdVinculacaoContratual.Value);
                                }
                            }
                            else {
                                pessoaJuridicaDB.Adimplente = Adimplente.Inadimplente;
                                _bancoContext.PessoaJuridica.Update(pessoaJuridicaDB);
                            }
                        }
                    }
                }
            }
            _bancoContext.SaveChanges();
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
        public void SetInadimplenciaClienteResponsavel(int id) {
            PessoaFisica pessoaFisicaResponsavel = _bancoContext.PessoaFisica.FirstOrDefault(x => x.Id == id);
            if (pessoaFisicaResponsavel != null) {
                pessoaFisicaResponsavel.Adimplente = Adimplente.Inadimplente;
                _bancoContext.PessoaFisica.Update(pessoaFisicaResponsavel);
            }
            else {
                PessoaJuridica pessoaJuridicaResponsavel = _bancoContext.PessoaJuridica.FirstOrDefault(x => x.Id == id);
                if (pessoaJuridicaResponsavel != null) {
                    pessoaJuridicaResponsavel.Adimplente = Adimplente.Inadimplente;
                    _bancoContext.PessoaJuridica.Update(pessoaJuridicaResponsavel);
                }
            }
        }
        public int ReturnQtmMeses(DateTime date) {
            return date.Year * 12 + date.Month;
        }
    }
}
