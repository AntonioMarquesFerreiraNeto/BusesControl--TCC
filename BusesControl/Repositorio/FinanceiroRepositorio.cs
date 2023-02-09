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

        public List<Financeiro> ListFinanceiros() {
            return _bancoContext.Financeiro
                .AsNoTracking().Include(x => x.PessoaFisica)
                .AsNoTracking().Include(x => x.PessoaJuridica)
                .AsNoTracking().Include(x => x.FornecedorFisico)
                .AsNoTracking().Include(x => x.FornecedorJuridico)
                .AsNoTracking().Include(x => x.Contrato)
                .AsNoTracking().Include(x => x.Parcelas)
                .ToList();
        }

        public Financeiro ReturnPorId(int id) {
            return _bancoContext.Financeiro.FirstOrDefault(x => x.Id == id);
        }

        public Contrato ListarJoinPorId(int id) {
            return _bancoContext.Contrato
                .AsNoTracking().Include("Motorista")
                .AsNoTracking().Include("Onibus")
                .AsNoTracking().Include(x => x.Financeiros).ThenInclude(x => x.Parcelas)
                .AsNoTracking().Include(x => x.Financeiros).ThenInclude(x => x.PessoaFisica)
                .AsNoTracking().Include(x => x.Financeiros).ThenInclude(x => x.PessoaJuridica)
                .FirstOrDefault(x => x.Id == id);
        }
        public Financeiro listPorIdFinanceiro(int? id) {
            return _bancoContext.Financeiro
                .AsNoTracking().Include(x => x.PessoaFisica)
                .AsNoTracking().Include(x => x.PessoaJuridica)
                .AsNoTracking().Include(x => x.Contrato)
                .AsNoTracking().Include(x => x.FornecedorFisico)
                .AsNoTracking().Include(x => x.FornecedorJuridico)
                .AsNoTracking().Include(x => x.Parcelas)
                .FirstOrDefault(x => x.Id == id);
        }
        public Parcelas ListarFinanceiroPorId(int id) {
            return _bancoContext.Parcelas.Include(x => x.Financeiro).ThenInclude(x => x.Contrato).FirstOrDefault(x => x.Id == id);
        }

        public Parcelas ContabilizarParcela(int id) {
            try {
                Parcelas parcelaDB = ListarFinanceiroPorId(id);
                if (parcelaDB == null) throw new Exception("Desculpe, ID não foi encontrado!");
                if (parcelaDB.Financeiro.FinanceiroStatus == FinanceiroStatus.Inativo) throw new Exception("Desculpe, ID não foi encontrado!");
                if (parcelaDB.StatusPagamento == SituacaoPagamento.PagamentoContabilizado) throw new Exception("Pagamento já realizado!");
                parcelaDB.StatusPagamento = SituacaoPagamento.PagamentoContabilizado;
                parcelaDB.DataEfetuacao = DateTime.Now;
                _bancoContext.Parcelas.Update(parcelaDB);
                if (!string.IsNullOrEmpty(parcelaDB.Financeiro.ContratoId.ToString())) {
                    ValidarInadimplenciaCliente(parcelaDB);
                }
                SetValoresPagosContratoAndCliente(parcelaDB);
                _bancoContext.SaveChanges();
                return parcelaDB;
            }
            catch (Exception erro) {
                throw new Exception(erro.Message);
            }
        }
        public void SetValoresPagosContratoAndCliente(Parcelas parcelaDB) {
            Financeiro financeiro = parcelaDB.Financeiro;
            if (!string.IsNullOrEmpty(financeiro.ContratoId.ToString())) {
                if (!string.IsNullOrEmpty(financeiro.ValorTotalPagoCliente.ToString())) {
                    financeiro.ValorTotalPagoCliente += financeiro.Contrato.ValorParcelaContratoPorCliente;
                }
                else {
                    financeiro.ValorTotalPagoCliente = financeiro.Contrato.ValorParcelaContratoPorCliente;
                }
                if (!string.IsNullOrEmpty(financeiro.Contrato.ValorTotalPagoContrato.ToString())) {
                    financeiro.Contrato.ValorTotalPagoContrato += financeiro.Contrato.ValorParcelaContratoPorCliente;
                }
                else {
                    financeiro.Contrato.ValorTotalPagoContrato = financeiro.Contrato.ValorParcelaContratoPorCliente;
                }
                if (!string.IsNullOrEmpty(parcelaDB.ValorJuros.ToString())) {
                    if (!string.IsNullOrEmpty(financeiro.ValorTotTaxaJurosPaga.ToString())) {
                        financeiro.ValorTotTaxaJurosPaga += parcelaDB.ValorJuros;
                    }
                    else {
                        financeiro.ValorTotTaxaJurosPaga = parcelaDB.ValorJuros;
                    }
                }
                _bancoContext.Contrato.Update(financeiro.Contrato);
            }
            else {
                if (!string.IsNullOrEmpty(financeiro.ValorTotalPagoCliente.ToString())) {
                    financeiro.ValorTotalPagoCliente += parcelaDB.Financeiro.ValorParcelaDR;
                }
                else {
                    financeiro.ValorTotalPagoCliente = parcelaDB.Financeiro.ValorParcelaDR;
                }
            }
            _bancoContext.Financeiro.Update(financeiro);
        }
        public void ValidarInadimplenciaCliente(Parcelas value) {
            var pessoaJuridica = _bancoContext.PessoaJuridica.Include(x => x.Financeiros).ThenInclude(x => x.Parcelas).FirstOrDefault(pessoa =>
               pessoa.Financeiros.Any(financeiro => financeiro.Parcelas.Any(parcelas =>
               parcelas.Id == value.Id)) && !string.IsNullOrEmpty(pessoa.Cnpj));

            var pessoaFisica = _bancoContext.PessoaFisica.Include(x => x.Financeiros).ThenInclude(x => x.Parcelas).FirstOrDefault(pessoa =>
                pessoa.Financeiros.Any(financeiro => financeiro.Parcelas.Any(parcelas =>
                parcelas.Id == value.Id) && !string.IsNullOrEmpty(pessoa.Cpf)));

            if (pessoaFisica != null) {
                int result = ReturnQtParcelasAtrasadaCliente(pessoaFisica.Financeiros);
                if (result == 0) {
                    if (!string.IsNullOrEmpty(pessoaFisica.IdVinculacaoContratual.ToString())) {
                        pessoaFisica.Adimplente = Adimplencia.Adimplente;
                        _bancoContext.PessoaFisica.Update(pessoaFisica);
                        //Chama o método que valida e seta com adimplente se passar na validação.
                        ValidarAndSetAdimplenteClienteResponsavel(pessoaFisica.IdVinculacaoContratual);
                    }
                    //Se o cliente for maior de idade, este método que é executado e realiza a validação se o cliente possui clientes vinculados em inadimplência.
                    else {
                        int clientesVinculadosInadimplentes = _bancoContext.PessoaFisica.Where(x => x.IdVinculacaoContratual == pessoaFisica.Id && x.Adimplente == Adimplencia.Inadimplente).ToList().Count;
                        if (clientesVinculadosInadimplentes == 0) {
                            pessoaFisica.Adimplente = Adimplencia.Adimplente;
                            _bancoContext.PessoaFisica.Update(pessoaFisica);
                        }
                    }
                }
            }
            else if (pessoaJuridica != null) {
                int result = ReturnQtParcelasAtrasadaCliente(pessoaJuridica.Financeiros);
                if (result == 0) {
                    int clientesVinculadosInadimplentes = _bancoContext.PessoaFisica.Where(x => x.IdVinculacaoContratual == pessoaJuridica.Id && x.Adimplente == Adimplencia.Inadimplente).ToList().Count;
                    if (clientesVinculadosInadimplentes == 0) {
                        pessoaJuridica.Adimplente = Adimplencia.Adimplente;
                        _bancoContext.PessoaJuridica.Update(pessoaJuridica);
                    }
                }
            }
        }
        public int ReturnQtParcelasAtrasadaCliente(List<Financeiro> financeiros) {
            int cont = 0;
            foreach (var item in financeiros) {
                foreach (var item2 in item.Parcelas) {
                    if (item2.StatusPagamento == SituacaoPagamento.Atrasada) cont++;
                }
            }
            return cont;
        }
        //Método que valida se o cliente tem parcelas atrasadas e outros clientes menores de idade com parcelas atrasadas,
        //caso não tenha, o mesmo é colocado em adimplência por não ter nenhuma infração das regras do contrato na aplicação. 
        public void ValidarAndSetAdimplenteClienteResponsavel(int? id) {
            PessoaFisica pessoaFisicaResponsavel = _bancoContext.PessoaFisica.Include(x => x.Financeiros).ThenInclude(x => x.Parcelas).FirstOrDefault(x => x.Id == id);
            if (pessoaFisicaResponsavel != null) {
                int resultParcelasAtrasadas = ReturnQtParcelasAtrasadaCliente(pessoaFisicaResponsavel.Financeiros);
                int clientesVinculadosInadimplentes = _bancoContext.PessoaFisica.Where(x => x.IdVinculacaoContratual == id && x.Adimplente == Adimplencia.Inadimplente).ToList().Count;
                if (resultParcelasAtrasadas == 0 && clientesVinculadosInadimplentes == 1) {
                    pessoaFisicaResponsavel.Adimplente = Adimplencia.Adimplente;
                    _bancoContext.Update(pessoaFisicaResponsavel);
                }
            }
            else {
                PessoaJuridica pessoaJuridicaResponsavel = _bancoContext.PessoaJuridica.Include(x => x.Financeiros).ThenInclude(x => x.Parcelas).FirstOrDefault(x => x.Id == id);
                if (pessoaJuridicaResponsavel != null) {
                    int resultParcelasAtrasadas = ReturnQtParcelasAtrasadaCliente(pessoaJuridicaResponsavel.Financeiros);
                    int clientesVinculadosInadimplentes = _bancoContext.PessoaFisica.Where(x => x.IdVinculacaoContratual == id && x.Adimplente == Adimplencia.Inadimplente).ToList().Count;
                    if (resultParcelasAtrasadas == 0 && clientesVinculadosInadimplentes == 1) {
                        pessoaJuridicaResponsavel.Adimplente = Adimplencia.Adimplente;
                        _bancoContext.Update(pessoaJuridicaResponsavel);
                    }
                }
            }
        }
        public void TaskMonitorParcelasLancamento() {
            var financeiros = _bancoContext.Financeiro
                .AsNoTracking().Include(x => x.Parcelas)
                .Where(x => x.Contrato == null).ToList();
            DateTime dateAtual = DateTime.Now.Date;
            foreach (var financeiro in financeiros) {
                foreach (var parcela in financeiro.Parcelas) {
                    if (dateAtual > parcela.DataVencimentoParcela && parcela.StatusPagamento != SituacaoPagamento.PagamentoContabilizado) {
                        Parcelas parcelaDB = _bancoContext.Parcelas.FirstOrDefault(x => x.Id == parcela.Id);
                        parcelaDB.StatusPagamento = SituacaoPagamento.Atrasada;
                        _bancoContext.Parcelas.Update(parcelaDB);
                    }
                }
            }
            _bancoContext.SaveChanges();
        }
        //Método agendado que executa sem interação com o usuário. 
        public void TaskMonitorParcelas() {
            var contratos = _bancoContext.Contrato.Where(x => x.Aprovacao == StatusAprovacao.Aprovado)
                .AsNoTracking().Include(x => x.Financeiros).ThenInclude(x => x.Parcelas)
                .AsNoTracking().Include(x => x.Financeiros).ThenInclude(x => x.PessoaFisica)
                .AsNoTracking().Include(x => x.Financeiros).ThenInclude(x => x.PessoaJuridica)
                .ToList();

            //Verifica se a parcela está atrasada e realiza as devidas medidas. 
            DateTime dateAtual = DateTime.Now.Date;
            foreach (var contrato in contratos) {
                foreach (var financeiro in contrato.Financeiros) {
                    foreach (var parcela in financeiro.Parcelas) {
                        if (dateAtual > parcela.DataVencimentoParcela && parcela.StatusPagamento != SituacaoPagamento.PagamentoContabilizado) {
                            Parcelas parcelaDB = _bancoContext.Parcelas.FirstOrDefault(x => x.Id == parcela.Id);
                            parcelaDB.StatusPagamento = SituacaoPagamento.Atrasada;
                            parcelaDB.ValorJuros = SetJurosParcela(parcelaDB, contrato);
                            var pessoaFisicaDB = _bancoContext.PessoaFisica.FirstOrDefault(x => x.Id == financeiro.PessoaFisicaId);
                            var pessoaJuridicaDB = _bancoContext.PessoaJuridica.FirstOrDefault(x => x.Id == financeiro.PessoaJuridicaId);
                            _bancoContext.Parcelas.Update(parcelaDB);
                            if (pessoaFisicaDB != null) {
                                pessoaFisicaDB.Adimplente = Adimplencia.Inadimplente;
                                _bancoContext.PessoaFisica.Update(pessoaFisicaDB);
                                if (!string.IsNullOrEmpty(pessoaFisicaDB.IdVinculacaoContratual.ToString())) {
                                    SetInadimplenciaClienteResponsavel(pessoaFisicaDB.IdVinculacaoContratual.Value);
                                }
                            }
                            else {
                                pessoaJuridicaDB.Adimplente = Adimplencia.Inadimplente;
                                _bancoContext.PessoaJuridica.Update(pessoaJuridicaDB);
                            }
                        }
                    }
                }
                //Realiza a validação se o contrato pode ser encerrado ou não. Caso a condição seja antendida, o contrato é encerrado.
                if (dateAtual > contrato.DataVencimento) {
                    int contParcelasAtrasadasOrPendente = contrato.Financeiros.Where(x => x.Parcelas.Any(x2 => x2.StatusPagamento == SituacaoPagamento.Atrasada
                    || x2.StatusPagamento == SituacaoPagamento.AguardandoPagamento)).ToList().Count;
                    if (contParcelasAtrasadasOrPendente == 0) {
                        Contrato contratoDB = _bancoContext.Contrato.FirstOrDefault(x => x.Id == contrato.Id);
                        contratoDB.Andamento = Andamento.Encerrado;
                        _bancoContext.Update(contratoDB);
                    }
                }
            }
            _bancoContext.SaveChanges();
        }
        public decimal? SetJurosParcela(Parcelas financeiro, Contrato contrato) {
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
                pessoaFisicaResponsavel.Adimplente = Adimplencia.Inadimplente;
                _bancoContext.PessoaFisica.Update(pessoaFisicaResponsavel);
            }
            else {
                PessoaJuridica pessoaJuridicaResponsavel = _bancoContext.PessoaJuridica.FirstOrDefault(x => x.Id == id);
                if (pessoaJuridicaResponsavel != null) {
                    pessoaJuridicaResponsavel.Adimplente = Adimplencia.Inadimplente;
                    _bancoContext.PessoaJuridica.Update(pessoaJuridicaResponsavel);
                }
            }
        }
        public int ReturnQtmMeses(DateTime date) {
            return date.Year * 12 + date.Month;
        }


        public ClientesContrato ConfirmarImpressaoPdf(ClientesContrato clientesContrato) {
            ClientesContrato clientesContratoDB = _bancoContext.ClientesContrato.FirstOrDefault(x => x.Id == clientesContrato.Id);
            clientesContratoDB.ProcessRescisao = ProcessRescendir.PdfBaixado;
            clientesContratoDB.DataEmissaoPdfRescisao = DateTime.Now.Date;
            _bancoContext.ClientesContrato.Update(clientesContratoDB);
            _bancoContext.SaveChanges();
            return clientesContratoDB;
        }
        public Financeiro ListFinanceiroPorContratoAndClientesContrato(int? id) {
            ClientesContrato clientesContrato = _bancoContext.ClientesContrato.FirstOrDefault(x => x.Id == id);
            if (clientesContrato != null) {
                if (!string.IsNullOrEmpty(clientesContrato.PessoaFisicaId.ToString())) {
                    Financeiro financeiro = _bancoContext.Financeiro
                        .AsNoTracking().Include(x => x.PessoaFisica)
                        .AsNoTracking().Include(x => x.PessoaJuridica)
                        .AsNoTracking().Include(x => x.Contrato)
                        .FirstOrDefault(x => x.ContratoId == clientesContrato.ContratoId
                        && x.PessoaFisicaId == clientesContrato.PessoaFisicaId);
                    return financeiro;
                }
                else {
                    Financeiro financeiro = _bancoContext.Financeiro
                        .AsNoTracking().Include(x => x.PessoaFisica)
                        .AsNoTracking().Include(x => x.PessoaJuridica)
                        .AsNoTracking().Include(x => x.Contrato)
                        .FirstOrDefault(x => x.ContratoId == clientesContrato.ContratoId
                        && x.PessoaJuridicaId == clientesContrato.PessoaJuridicaId);
                    return financeiro;
                }
            }
            else {
                return null;
            }
        }
        public Financeiro RescisaoContrato(Financeiro financeiro) {
            try {
                if (financeiro == null) throw new Exception("Desculpe, ID não foi encontrado!");
                if (financeiro.Parcelas.Any(x => x.StatusPagamento == SituacaoPagamento.Atrasada)) {
                    throw new Exception("Cliente tem parcelas atrasadas neste contrato!");
                }
                foreach (var parcela in financeiro.Parcelas) {
                    _bancoContext.Parcelas.Remove(parcela);
                }
                //chamando o método que cria a rescisão no lugar do clientes contrato.
                Rescisao rescisao = new Rescisao();
                rescisao.DataRescisao = DateTime.Now.Date;
                rescisao.Contrato = financeiro.Contrato;
                rescisao.CalcularMultaContrato();
                if (!string.IsNullOrEmpty(financeiro.ValorTotalPagoCliente.ToString())) {
                    rescisao.ValorPagoContrato = financeiro.ValorTotalPagoCliente;
                }
                if (!string.IsNullOrEmpty(financeiro.PessoaFisicaId.ToString())) {
                    rescisao.PessoaFisicaId = financeiro.PessoaFisicaId;
                    ClientesContrato clientesContrato = _bancoContext.ClientesContrato.FirstOrDefault(x => x.ContratoId == financeiro.ContratoId
                        && x.PessoaFisicaId == financeiro.PessoaFisicaId);
                    _bancoContext.ClientesContrato.Remove(clientesContrato);
                }
                else {
                    if (!string.IsNullOrEmpty(financeiro.PessoaJuridicaId.ToString())) {
                        rescisao.PessoaJuridicaId = financeiro.PessoaJuridicaId;
                        ClientesContrato clientesContrato = _bancoContext.ClientesContrato.FirstOrDefault(x => x.ContratoId == financeiro.ContratoId
                        && x.PessoaJuridicaId == financeiro.PessoaJuridicaId);
                        _bancoContext.ClientesContrato.Remove(clientesContrato);
                    }
                    else {
                        throw new Exception("Desculpe, ID não foi encontrado!");
                    }
                }
                _bancoContext.Rescisao.Add(rescisao);
                _bancoContext.Financeiro.Remove(financeiro);
                _bancoContext.SaveChanges();
                return financeiro;
            }
            catch (Exception erro) {
                throw new Exception(erro.Message);
            }
        }
        public void TaskMonitorPdfRescisao() {
            List<ClientesContrato> clientesContratos = _bancoContext.ClientesContrato.ToList();
            DateTime dataAtual = DateTime.Now.Date;
            dataAtual.AddDays(2);
            foreach (var clienteContrato in clientesContratos) {
                if (!string.IsNullOrEmpty(clienteContrato.DataEmissaoPdfRescisao.ToString())) {
                    if (dataAtual > clienteContrato.DataEmissaoPdfRescisao.Value.Date) {
                        clienteContrato.ProcessRescisao = ProcessRescendir.NoRescisao;
                        _bancoContext.ClientesContrato.Update(clienteContrato);
                    }
                }
            }
            _bancoContext.SaveChanges();
        }

        public Financeiro AdicionarDespesa(Financeiro financeiro) {
            try {
                financeiro.FinanceiroStatus = FinanceiroStatus.Ativo;
                financeiro.DespesaReceita = DespesaReceita.Despesa;
                _bancoContext.Financeiro.Add(financeiro);
                financeiro.ValorParcelaDR = financeiro.ValorTotDR / financeiro.QtParcelas;
                AdicionarParcelas(financeiro);
                _bancoContext.SaveChanges();
                return financeiro;
            }
            catch (Exception erro) {
                throw new Exception(erro.Message);
            }
        }
        public void AdicionarParcelas(Financeiro financeiro) {
            for (int parcelas = 1; parcelas <= financeiro.QtParcelas; parcelas++) {
                Parcelas parcela = new Parcelas {

                    Financeiro = financeiro, StatusPagamento = SituacaoPagamento.AguardandoPagamento,
                    DataVencimentoParcela = financeiro.DataEmissao.Value.AddMonths(parcelas - 1), NomeParcela = parcelas.ToString()
                };
                if (parcelas == 1) {
                    parcela.DataVencimentoParcela = financeiro.DataEmissao.Value.AddDays(3);
                }
                _bancoContext.Parcelas.Add(parcela);
            }
        }

        public Financeiro AdicionarReceita(Financeiro financeiro) {
            try {
                financeiro.FinanceiroStatus = FinanceiroStatus.Ativo;
                financeiro.DespesaReceita = DespesaReceita.Receita;
                _bancoContext.Financeiro.Add(financeiro);
                financeiro.ValorParcelaDR = financeiro.ValorTotDR / financeiro.QtParcelas;
                AdicionarParcelas(financeiro);
                _bancoContext.SaveChanges();
                return financeiro;
            }
            catch (Exception erro) {
                throw new Exception(erro.Message);
            }
        }

        public Financeiro EditarLancamento(Financeiro financeiro) {
            try {
                Financeiro financeiroDB = _bancoContext.Financeiro.FirstOrDefault(x => x.Id == financeiro.Id);
                if (financeiroDB == null) throw new Exception("Desculpe, ID não foi encontrado!");
                if (!string.IsNullOrEmpty(financeiroDB.ContratoId.ToString())) throw new Exception("Desculpe, ID não foi encontrado!");
                if (!string.IsNullOrEmpty(financeiroDB.ValorTotalPagoCliente.ToString())) throw new Exception("Lançamento possuí parcelas paga!");
                if (financeiroDB.FinanceiroStatus == FinanceiroStatus.Inativo) throw new Exception("Desculpe, financeiro inativado!");
                financeiroDB.Pagament = financeiro.Pagament;
                financeiroDB.ValorTotDR = financeiro.ValorTotDR;
                financeiroDB.ValorParcelaDR = financeiro.ValorTotDR / financeiro.QtParcelas;
                financeiroDB.TypeEfetuacao = financeiro.TypeEfetuacao;
                financeiroDB.Detalhamento = financeiro.Detalhamento;
                financeiroDB.DataVencimento = financeiro.DataVencimento;
                if (financeiro.QtParcelas > financeiroDB.QtParcelas) {
                    for (int parcelas = financeiroDB.QtParcelas.Value + 1; parcelas <= financeiro.QtParcelas.Value; parcelas++) {
                        Parcelas parcela = new Parcelas {
                            FinanceiroId = financeiro.Id, StatusPagamento = SituacaoPagamento.AguardandoPagamento,
                            DataVencimentoParcela = financeiro.DataEmissao.Value.AddMonths(parcelas - 1), NomeParcela = parcelas.ToString()
                        };
                        _bancoContext.Parcelas.Add(parcela);
                    }
                }
                else if (financeiro.QtParcelas != financeiroDB.QtParcelas) {
                    for (int? parcelas = financeiro.QtParcelas + 1; parcelas <= financeiroDB.QtParcelas; parcelas++) {
                        Parcelas parcela = _bancoContext.Parcelas.FirstOrDefault(x => x.FinanceiroId == financeiro.Id && x.NomeParcela == parcelas.ToString());
                        _bancoContext.Parcelas.Remove(parcela);
                    }
                }
                if (financeiroDB.DespesaReceita == DespesaReceita.Receita) {
                    if (!string.IsNullOrEmpty(financeiro.PessoaFisicaId.ToString())) {
                        financeiroDB.PessoaFisicaId = financeiro.PessoaFisicaId;
                        financeiroDB.PessoaJuridicaId = null;
                    }
                    else {
                        financeiroDB.PessoaJuridicaId = financeiro.PessoaJuridicaId;
                        financeiroDB.PessoaFisicaId = financeiro.PessoaFisicaId = null;
                    }
                }
                else {
                    if (!string.IsNullOrEmpty(financeiro.FornecedorFisicoId.ToString())) {
                        financeiroDB.FornecedorFisicoId = financeiro.FornecedorFisicoId;
                        financeiroDB.FornecedorJuridicoId = null;
                    }
                    else {
                        financeiroDB.FornecedorJuridicoId = financeiro.FornecedorJuridicoId;
                        financeiroDB.FornecedorFisicoId = null;
                    }
                }
                financeiroDB.QtParcelas = financeiro.QtParcelas;
                _bancoContext.Financeiro.Update(financeiroDB);
                _bancoContext.SaveChanges();
                return financeiroDB;

            }
            catch (Exception error) {
                throw new Exception(error.Message);
            }
        }
        //Atualiza a quantidade de parcelas de clientes que não foram excluídos, mas tiveram a quantidade de parcelas editadas.
        public void UpdateFinanceiro(Financeiro financeiro) {

        }


        public Financeiro InativarReceitaOrDespesa(Financeiro financeiro) {
            try {
                Financeiro financeiroDB = listPorIdFinanceiro(financeiro.Id);
                if (financeiroDB == null || !string.IsNullOrEmpty(financeiroDB.ContratoId.ToString())) throw new Exception("Desculpe, ID não foi encontrado!");
                if (financeiroDB.Parcelas.Any(x => x.StatusPagamento == SituacaoPagamento.PagamentoContabilizado)) {
                    throw new Exception("Desculpe, receitas/despesas com parcelas contabilizadas não pode ser inativado!");
                }
                financeiroDB.FinanceiroStatus = FinanceiroStatus.Inativo;
                _bancoContext.Financeiro.Update(financeiroDB);
                _bancoContext.SaveChanges();
                return financeiroDB;
            }
            catch (Exception error) {
                throw new Exception(error.Message);
            }
        }

        public List<Financeiro> ListFinanceirosFiltros(Filtros filtros) {
            if (filtros.ReceitasDespesas == "atrasados") {
                return _bancoContext.Financeiro
                        .AsNoTracking().Include(x => x.PessoaFisica)
                        .AsNoTracking().Include(x => x.PessoaJuridica)
                        .AsNoTracking().Include(x => x.FornecedorFisico)
                        .AsNoTracking().Include(x => x.FornecedorJuridico)
                        .AsNoTracking().Include(x => x.Contrato)
                        .AsNoTracking().Include(x => x.Parcelas)
                        .Where(x => x.Parcelas.Any(x => x.StatusPagamento == SituacaoPagamento.Atrasada == true)).ToList();
            }
            else if (filtros.DataFiltro == "não") {
                if (filtros.ReceitasDespesas == "todos") {
                    return _bancoContext.Financeiro
                        .AsNoTracking().Include(x => x.PessoaFisica)
                        .AsNoTracking().Include(x => x.PessoaJuridica)
                        .AsNoTracking().Include(x => x.FornecedorFisico)
                        .AsNoTracking().Include(x => x.FornecedorJuridico)
                        .AsNoTracking().Include(x => x.Contrato)
                        .AsNoTracking().Include(x => x.Parcelas)
                        .ToList();
                }
                else if (filtros.ReceitasDespesas == "receitas") {
                    return _bancoContext.Financeiro
                        .AsNoTracking().Include(x => x.PessoaFisica)
                        .AsNoTracking().Include(x => x.PessoaJuridica)
                        .AsNoTracking().Include(x => x.FornecedorFisico)
                        .AsNoTracking().Include(x => x.FornecedorJuridico)
                        .AsNoTracking().Include(x => x.Contrato)
                        .AsNoTracking().Include(x => x.Parcelas)
                        .Where(x => x.DespesaReceita == DespesaReceita.Receita)
                        .ToList();
                }
                else {
                    return _bancoContext.Financeiro
                    .AsNoTracking().Include(x => x.PessoaFisica)
                    .AsNoTracking().Include(x => x.PessoaJuridica)
                    .AsNoTracking().Include(x => x.FornecedorFisico)
                    .AsNoTracking().Include(x => x.FornecedorJuridico)
                    .AsNoTracking().Include(x => x.Contrato)
                    .AsNoTracking().Include(x => x.Parcelas)
                    .Where(x => x.DespesaReceita == DespesaReceita.Despesa)
                    .ToList();
                }
            }
            else {
                if (filtros.ReceitasDespesas == "todos") {
                    if (filtros.DataFiltro == "emissão") {
                        return _bancoContext.Financeiro
                            .AsNoTracking().Include(x => x.PessoaFisica)
                            .AsNoTracking().Include(x => x.PessoaJuridica)
                            .AsNoTracking().Include(x => x.FornecedorFisico)
                            .AsNoTracking().Include(x => x.FornecedorJuridico)
                            .AsNoTracking().Include(x => x.Contrato)
                            .AsNoTracking().Include(x => x.Parcelas)
                            .Where(x => x.DataEmissao.Value.Date >= filtros.DataInicial && x.DataEmissao <= filtros.DataTermino).ToList();
                    }
                    else if (filtros.DataFiltro == "efetuação") {
                        return _bancoContext.Financeiro
                            .AsNoTracking().Include(x => x.PessoaFisica)
                            .AsNoTracking().Include(x => x.PessoaJuridica)
                            .AsNoTracking().Include(x => x.FornecedorFisico)
                            .AsNoTracking().Include(x => x.FornecedorJuridico)
                            .AsNoTracking().Include(x => x.Contrato)
                            .AsNoTracking().Include(x => x.Parcelas)
                            .Where(x => x.Parcelas.Any(p => p.DataEfetuacao.Value.Date >= filtros.DataInicial && p.DataEfetuacao.Value.Date <= filtros.DataTermino)).ToList();
                    }
                    else {
                        return _bancoContext.Financeiro
                            .AsNoTracking().Include(x => x.PessoaFisica)
                            .AsNoTracking().Include(x => x.PessoaJuridica)
                            .AsNoTracking().Include(x => x.FornecedorFisico)
                            .AsNoTracking().Include(x => x.FornecedorJuridico)
                            .AsNoTracking().Include(x => x.Contrato)
                            .AsNoTracking().Include(x => x.Parcelas)
                            .Where(x => x.Parcelas.Any(p => p.DataVencimentoParcela.Value.Date >= filtros.DataInicial && p.DataVencimentoParcela.Value.Date <= filtros.DataTermino)).ToList();
                    }
                }
                else if (filtros.ReceitasDespesas == "receitas") {
                    if (filtros.DataFiltro == "emissão") {
                        return _bancoContext.Financeiro
                            .AsNoTracking().Include(x => x.PessoaFisica)
                            .AsNoTracking().Include(x => x.PessoaJuridica)
                            .AsNoTracking().Include(x => x.FornecedorFisico)
                            .AsNoTracking().Include(x => x.FornecedorJuridico)
                            .AsNoTracking().Include(x => x.Contrato)
                            .AsNoTracking().Include(x => x.Parcelas)
                            .Where(x => x.DespesaReceita == DespesaReceita.Receita && (x.DataEmissao.Value.Date >= filtros.DataInicial && x.DataEmissao.Value.Date <= filtros.DataTermino)).ToList();
                    }
                    else if (filtros.DataFiltro == "efetuação") {
                        return _bancoContext.Financeiro
                            .AsNoTracking().Include(x => x.PessoaFisica)
                            .AsNoTracking().Include(x => x.PessoaJuridica)
                            .AsNoTracking().Include(x => x.FornecedorFisico)
                            .AsNoTracking().Include(x => x.FornecedorJuridico)
                            .AsNoTracking().Include(x => x.Contrato)
                            .AsNoTracking().Include(x => x.Parcelas)
                            .Where(x => x.DespesaReceita == DespesaReceita.Receita && x.Parcelas.Any(p => p.DataEfetuacao.Value.Date >= filtros.DataInicial && p.DataEfetuacao.Value.Date <= filtros.DataTermino)).ToList();
                    }
                    else {
                        return _bancoContext.Financeiro
                            .AsNoTracking().Include(x => x.PessoaFisica)
                            .AsNoTracking().Include(x => x.PessoaJuridica)
                            .AsNoTracking().Include(x => x.FornecedorFisico)
                            .AsNoTracking().Include(x => x.FornecedorJuridico)
                            .AsNoTracking().Include(x => x.Contrato)
                            .AsNoTracking().Include(x => x.Parcelas)
                            .Where(x => x.DespesaReceita == DespesaReceita.Receita && x.Parcelas.Any(p => p.DataVencimentoParcela >= filtros.DataInicial && p.DataVencimentoParcela <= filtros.DataTermino)).ToList();
                    }
                }
                else {
                    if (filtros.DataFiltro == "emissão") {
                        return _bancoContext.Financeiro
                            .AsNoTracking().Include(x => x.PessoaFisica)
                            .AsNoTracking().Include(x => x.PessoaJuridica)
                            .AsNoTracking().Include(x => x.FornecedorFisico)
                            .AsNoTracking().Include(x => x.FornecedorJuridico)
                            .AsNoTracking().Include(x => x.Contrato)
                            .AsNoTracking().Include(x => x.Parcelas)
                            .Where(x => x.DespesaReceita == DespesaReceita.Despesa && (x.DataEmissao.Value.Date >= filtros.DataInicial && x.DataEmissao.Value.Date <= filtros.DataTermino)).ToList();
                    }
                    else if (filtros.DataFiltro == "efetuação") {
                        return _bancoContext.Financeiro
                            .AsNoTracking().Include(x => x.PessoaFisica)
                            .AsNoTracking().Include(x => x.PessoaJuridica)
                            .AsNoTracking().Include(x => x.FornecedorFisico)
                            .AsNoTracking().Include(x => x.FornecedorJuridico)
                            .AsNoTracking().Include(x => x.Contrato)
                            .AsNoTracking().Include(x => x.Parcelas)
                            .Where(x => x.DespesaReceita == DespesaReceita.Despesa && x.Parcelas.Any(p => p.DataEfetuacao.Value.Date >= filtros.DataInicial && p.DataEfetuacao.Value.Date <= filtros.DataTermino)).ToList();
                    }
                    else {
                        return _bancoContext.Financeiro
                            .AsNoTracking().Include(x => x.PessoaFisica)
                            .AsNoTracking().Include(x => x.PessoaJuridica)
                            .AsNoTracking().Include(x => x.FornecedorFisico)
                            .AsNoTracking().Include(x => x.FornecedorJuridico)
                            .AsNoTracking().Include(x => x.Contrato)
                            .AsNoTracking().Include(x => x.Parcelas)
                            .Where(x => x.DespesaReceita == DespesaReceita.Despesa && x.Parcelas.Any(p => p.DataVencimentoParcela.Value.Date >= filtros.DataInicial && p.DataVencimentoParcela.Value.Date <= filtros.DataTermino)).ToList();
                    }
                }
            }
        }
    }
}
