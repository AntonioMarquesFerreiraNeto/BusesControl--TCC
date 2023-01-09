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

        public List<Contrato> ContratosEmAndamento() {
            return _bancoContext.Contrato
                .AsNoTracking().Include("Motorista")
                .AsNoTracking().Include("Onibus")
                .AsNoTracking().Include(x => x.ClientesContratos).ThenInclude(x => x.PessoaFisica)
                .AsNoTracking().Include(x => x.ClientesContratos).ThenInclude(x => x.PessoaJuridica)
                .AsNoTracking().Include(x => x.ClientesContratos).ThenInclude(x => x.ParcelasContrato)
                .Where(x => x.Aprovacao == StatusAprovacao.Aprovado && x.Andamento == Andamento.EmAndamento).ToList();
        }
        public List<Contrato> ContratosEncerrados() {
            return _bancoContext.Contrato
               .AsNoTracking().Include("Motorista")
               .AsNoTracking().Include("Onibus")
               .AsNoTracking().Include(x => x.ClientesContratos).ThenInclude(x => x.PessoaFisica)
               .AsNoTracking().Include(x => x.ClientesContratos).ThenInclude(x => x.PessoaJuridica)
               .AsNoTracking().Include(x => x.ClientesContratos).ThenInclude(x => x.ParcelasContrato)
               .Where(x => x.Aprovacao == StatusAprovacao.Aprovado && x.Andamento == Andamento.Encerrado).ToList();
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
                .AsNoTracking().Include(x => x.PessoaFisica)
                .AsNoTracking().Include(x => x.PessoaJuridica)
                .AsNoTracking().Include(x => x.Contrato)
                .AsNoTracking().Include(x => x.ParcelasContrato)
                .FirstOrDefault(x => x.Id == id);
        }

        public ParcelasCliente ListarFinanceiroPorId(int id) {
            return _bancoContext.ParcelasCliente.Include(x => x.ClientesContrato).ThenInclude(x => x.Contrato).FirstOrDefault(x => x.Id == id);
        }

        public ParcelasCliente ContabilizarFinanceiro(int id) {
            try {
                ParcelasCliente financeiroDB = ListarFinanceiroPorId(id);
                if (financeiroDB == null) throw new Exception("Desculpe, ID não foi encontrado!");
                financeiroDB.StatusPagamento = SituacaoPagamento.PagamentoContabilizado;
                _bancoContext.ParcelasCliente.Update(financeiroDB);
                ValidarInadimplenciaCliente(financeiroDB);
                SetValoresPagosContratoAndCliente(financeiroDB);
                _bancoContext.SaveChanges();
                return financeiroDB;
            }
            catch (Exception erro) {
                throw new Exception(erro.Message);
            }
        }
        public void SetValoresPagosContratoAndCliente(ParcelasCliente financeiroDB) {
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
        public void ValidarInadimplenciaCliente(ParcelasCliente value) {
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
                int result = ReturnQtParcelasAtrasadaCliente(pessoaJuridica.ClientesContratos);
                if (result == 0) {
                    int clientesVinculadosInadimplentes = _bancoContext.PessoaFisica.Where(x => x.IdVinculacaoContratual == pessoaJuridica.Id && x.Adimplente == Adimplencia.Inadimplente).ToList().Count;
                    if (clientesVinculadosInadimplentes == 0) {
                        pessoaJuridica.Adimplente = Adimplencia.Adimplente;
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
                int clientesVinculadosInadimplentes = _bancoContext.PessoaFisica.Where(x => x.IdVinculacaoContratual == id && x.Adimplente == Adimplencia.Inadimplente).ToList().Count;
                if (resultParcelasAtrasadas == 0 && clientesVinculadosInadimplentes == 1) {
                    pessoaFisicaResponsavel.Adimplente = Adimplencia.Adimplente;
                    _bancoContext.Update(pessoaFisicaResponsavel);
                }
            }
            else {
                PessoaJuridica pessoaJuridicaResponsavel = _bancoContext.PessoaJuridica.Include(x => x.ClientesContratos).ThenInclude(x => x.ParcelasContrato).FirstOrDefault(x => x.Id == id);
                if (pessoaJuridicaResponsavel != null) {
                    int resultParcelasAtrasadas = ReturnQtParcelasAtrasadaCliente(pessoaJuridicaResponsavel.ClientesContratos);
                    int clientesVinculadosInadimplentes = _bancoContext.PessoaFisica.Where(x => x.IdVinculacaoContratual == id && x.Adimplente == Adimplencia.Inadimplente).ToList().Count;
                    if (resultParcelasAtrasadas == 0 && clientesVinculadosInadimplentes == 1) {
                        pessoaJuridicaResponsavel.Adimplente = Adimplencia.Adimplente;
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
                            ParcelasCliente financeiroDB = _bancoContext.ParcelasCliente.FirstOrDefault(x => x.Id == financeiro.Id);
                            financeiroDB.StatusPagamento = SituacaoPagamento.Atrasada;
                            financeiroDB.ValorJuros = SetJurosParcela(financeiroDB, contrato);
                            var pessoaFisicaDB = _bancoContext.PessoaFisica.FirstOrDefault(x => x.Id == clientesContrato.PessoaFisicaId);
                            var pessoaJuridicaDB = _bancoContext.PessoaJuridica.FirstOrDefault(x => x.Id == clientesContrato.PessoaJuridicaId);
                            _bancoContext.ParcelasCliente.Update(financeiroDB);
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
                    int contParcelasAtrasadasOrPendente = contrato.ClientesContratos.Where(x => x.ParcelasContrato.Any(x2 => x2.StatusPagamento == SituacaoPagamento.Atrasada
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
        public decimal? SetJurosParcela(ParcelasCliente financeiro, Contrato contrato) {
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

        public ClientesContrato RescisaoContrato(ClientesContrato clientesContrato) {
            try {
                ClientesContrato clientesContratoDB = listPorIdClientesContrato(clientesContrato.Id);
                if (clientesContratoDB == null) throw new Exception("Desculpe, ID não foi encontrado!");
                if (clientesContratoDB.ParcelasContrato.Any(x => x.StatusPagamento == SituacaoPagamento.Atrasada)) {
                    throw new Exception("Cliente tem parcelas atrasadas neste contrato!");
                }
                foreach (ParcelasCliente parcela in clientesContratoDB.ParcelasContrato) {
                    _bancoContext.ParcelasCliente.Remove(parcela);
                }
                _bancoContext.ClientesContrato.Remove(clientesContratoDB);
                //chamando o método que cria a rescisão no lugar do clientes contrato.
                NewRescisao(clientesContratoDB);
                _bancoContext.SaveChanges();
                return clientesContratoDB;
            }
            catch (Exception erro) {
                throw new Exception(erro.Message);
            }
        }
        public void NewRescisao(ClientesContrato clientesContrato) {
            Rescisao rescisao = new Rescisao();
            rescisao.DataRescisao = DateTime.Now.Date;
            rescisao.Contrato = clientesContrato.Contrato;
            if (!string.IsNullOrEmpty(clientesContrato.PessoaFisicaId.ToString())) {
                rescisao.PessoaFisicaId = clientesContrato.PessoaFisicaId;
            }
            else {
                if (!string.IsNullOrEmpty(clientesContrato.PessoaJuridicaId.ToString())) {
                    rescisao.PessoaJuridicaId = clientesContrato.PessoaJuridicaId;
                }
                else {
                    throw new Exception("Desculpe, ID não foi encontrado!");
                }
            }
            rescisao.CalcularMultaContrato();
            _bancoContext.Rescisao.Add(rescisao);
        }
    }
}
