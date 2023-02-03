using BusesControl.Data;
using BusesControl.Models;
using System.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using BusesControl.Models.Enums;
using Microsoft.EntityFrameworkCore;
using BusesControl.Models.ViewModels;

namespace BusesControl.Repositorio {
    public class ContratoRepositorio : IContratoRepositorio {

        private readonly BancoContext _bancoContext;

        public ContratoRepositorio(BancoContext bancoContext) {
            _bancoContext = bancoContext;
        }

        public ClientesContrato ListarClientesContratoId(int id) {
            return _bancoContext.ClientesContrato
                .AsNoTracking().Include(x => x.PessoaFisica)
                .AsNoTracking().Include(x => x.PessoaJuridica)
                .AsNoTracking().Include(x => x.Contrato)
                .FirstOrDefault(x => x.Id == id);
        }

        public Contrato ListarPorId(int id) {
            return _bancoContext.Contrato.FirstOrDefault(x => x.Id == id);
        }
        public Contrato ListarJoinPorId(int id) {
            return _bancoContext.Contrato
                .AsNoTracking().Include("Motorista")
                .AsNoTracking().Include("Onibus")
                .AsNoTracking().Include(x => x.ClientesContratos).ThenInclude(x => x.PessoaFisica)
                .AsNoTracking().Include(x => x.ClientesContratos).ThenInclude(x => x.PessoaJuridica)
                .AsNoTracking().Include(x => x.Financeiros).ThenInclude(x => x.Parcelas)
                .AsNoTracking().Include(x => x.Financeiros).ThenInclude(x => x.PessoaFisica)
                .AsNoTracking().Include(x => x.Financeiros).ThenInclude(x => x.PessoaJuridica)
                .FirstOrDefault(x => x.Id == id);
        }
        public Contrato ListarJoinPorIdAprovado(int? id) {
            return _bancoContext.Contrato
                .AsNoTracking().Include("Motorista")
                .AsNoTracking().Include("Onibus")
                .AsNoTracking().Include(x => x.ClientesContratos).ThenInclude(x => x.PessoaFisica)
                .AsNoTracking().Include(x => x.ClientesContratos).ThenInclude(x => x.PessoaJuridica)
                .AsNoTracking().Include(x => x.Financeiros).ThenInclude(x => x.Parcelas)
                .AsNoTracking().Include(x => x.Financeiros).ThenInclude(x => x.PessoaFisica)
                .AsNoTracking().Include(x => x.Financeiros).ThenInclude(x => x.PessoaJuridica)
                .AsNoTracking().Include(x => x.Rescisoes).ThenInclude(x => x.PessoaFisica)
                .AsNoTracking().Include(x => x.Rescisoes).ThenInclude(x => x.PessoaJuridica)
                .FirstOrDefault(x => x.Id == id && x.Aprovacao == StatusAprovacao.Aprovado);
        }
        public List<Contrato> ListContratoAtivo() {
            return _bancoContext.Contrato.Where(x => x.StatusContrato == ContratoStatus.Ativo)
                .AsNoTracking().Include("Motorista")
                .AsNoTracking().Include("Onibus")
                .AsNoTracking().Include(x => x.ClientesContratos).ThenInclude(x => x.PessoaFisica)
                .AsNoTracking().Include(x => x.ClientesContratos).ThenInclude(x => x.PessoaJuridica)
                .AsNoTracking().Include(x => x.Financeiros).ThenInclude(x => x.Parcelas)
                .ToList();
        }
        public List<Contrato> ListContratoInativo() {
            return _bancoContext.Contrato.Where(x => x.StatusContrato == ContratoStatus.Inativo)
                .AsNoTracking().Include("Motorista")
                .AsNoTracking().Include("Onibus")
                .AsNoTracking().Include(x => x.ClientesContratos).ThenInclude(x => x.PessoaFisica)
                .AsNoTracking().Include(x => x.ClientesContratos).ThenInclude(x => x.PessoaJuridica)
                .AsNoTracking().Include(x => x.Financeiros).ThenInclude(x => x.Parcelas)
                .ToList();
        }
        public List<Contrato> ListContratoEmAnalise() {
            return _bancoContext.Contrato.Where(x => x.Aprovacao == StatusAprovacao.EmAnalise && x.StatusContrato == ContratoStatus.Ativo)
                .AsNoTracking().Include("Motorista")
                .AsNoTracking().Include("Onibus")
                .AsNoTracking().Include(x => x.ClientesContratos).ThenInclude(x => x.PessoaFisica)
                .AsNoTracking().Include(x => x.ClientesContratos).ThenInclude(x => x.PessoaJuridica)
                .AsNoTracking().Include(x => x.Financeiros).ThenInclude(x => x.Parcelas)
                .ToList();
        }
        public List<Contrato> ListContratoNegados() {
            return _bancoContext.Contrato.Where(x => x.Aprovacao == StatusAprovacao.Negado)
                .AsNoTracking().Include("Motorista")
                .AsNoTracking().Include("Onibus")
                .AsNoTracking().Include(x => x.ClientesContratos).ThenInclude(x => x.PessoaFisica)
                .AsNoTracking().Include(x => x.ClientesContratos).ThenInclude(x => x.PessoaJuridica)
                .AsNoTracking().Include(x => x.Financeiros).ThenInclude(x => x.Parcelas)
                .ToList();
        }
        public List<Contrato> ListContratoAprovados() {
            return _bancoContext.Contrato.Where(x => x.Aprovacao == StatusAprovacao.Aprovado && x.StatusContrato == ContratoStatus.Ativo)
                .AsNoTracking().Include("Motorista")
                .AsNoTracking().Include("Onibus")
                .AsNoTracking().Include(x => x.ClientesContratos).ThenInclude(x => x.PessoaFisica)
                .AsNoTracking().Include(x => x.ClientesContratos).ThenInclude(x => x.PessoaJuridica)
                .AsNoTracking().Include(x => x.Financeiros).ThenInclude(x => x.Parcelas)
                .AsNoTracking().Include(x => x.Rescisoes)
                .ToList();
        }

        public ModelsContrato Adicionar(ModelsContrato modelsContrato) {
            try {
                Contrato contrato = modelsContrato.Contrato;
                contrato = ContratoTrim(contrato);
                AddClienteFisico(contrato, modelsContrato.ListPessoaFisicaSelect);
                AddClienteJuridico(contrato, modelsContrato.ListPessoaJuridicaSelect);
                contrato.ReturnValorParcela();
                int qtClient = modelsContrato.ListPessoaFisicaSelect.Count + modelsContrato.ListPessoaJuridicaSelect.Count;
                contrato.ReturnValorParcelaPorCliente(qtClient);
                contrato.Andamento = Andamento.Aguardando;
                _bancoContext.Contrato.Add(contrato);
                _bancoContext.SaveChanges();
                return modelsContrato;
            }
            catch (Exception erro) {
                throw new Exception(erro.Message);
            }
        }
        public void AddClienteFisico(Contrato contrato, List<PessoaFisica> list) {
            try {
                if (list.Count > 0) {
                    foreach (var item in list) {
                        ClientesContrato clientesContrato = new ClientesContrato { PessoaFisicaId = item.Id, Contrato = contrato };
                        _bancoContext.AddRange(clientesContrato);
                    }
                }
            }
            catch (Exception erro) {
                throw new Exception(erro.Message);
            }
        }
        public void AddClienteJuridico(Contrato contrato, List<PessoaJuridica> list) {
            try {
                if (list.Count > 0) {
                    foreach (var item in list) {
                        ClientesContrato clientesContrato = new ClientesContrato { PessoaJuridicaId = item.Id, Contrato = contrato };
                        _bancoContext.AddRange(clientesContrato);
                    }
                }
            }
            catch (Exception erro) {
                throw new Exception(erro.Message);
            }
        }

        public ModelsContrato EditarContrato(ModelsContrato modelsContrato) {
            try {
                Contrato contrato = modelsContrato.Contrato;
                Contrato contratoDB = ListarPorId(contrato.Id);
                if (contratoDB == null) throw new Exception($"Desculpe, ID não foi encontrado.");
                if (contratoDB.Andamento == Andamento.Encerrado) throw new Exception("Desculpe, contrato encerrado!");
                contratoDB.ValorMonetario = contrato.ValorMonetario;
                contratoDB.QtParcelas = contrato.QtParcelas;
                contratoDB.ReturnValorParcela();
                contratoDB.MotoristaId = contrato.MotoristaId;
                contratoDB.OnibusId = contrato.OnibusId;
                contratoDB.DataVencimento = contrato.DataVencimento;
                contratoDB.Detalhamento = contrato.Detalhamento.Trim();
                if (contratoDB.Aprovacao != StatusAprovacao.Aprovado) {
                    contratoDB.Pagament = contrato.Pagament;
                    UpdateClienteFisico(contratoDB, modelsContrato.ListPessoaFisicaSelect);
                    UpdateClienteJuridico(contratoDB, modelsContrato.ListPessoaJuridicaSelect);
                    int qtClient = modelsContrato.ListPessoaFisicaSelect.Count + modelsContrato.ListPessoaJuridicaSelect.Count;
                    contratoDB.ReturnValorParcelaPorCliente(qtClient);
                }
                else {
                    contratoDB.ReturnValorParcelaPorCliente(modelsContrato.TotClientes);
                }
                _bancoContext.Contrato.Update(contratoDB);
                _bancoContext.SaveChanges();
                return modelsContrato;
            }
            catch (Exception erro) {
                throw new Exception(erro.Message);
            }
        }
        public void UpdateClienteFisico(Contrato contrato, List<PessoaFisica> list) {
            try {
                var contratoCliente = ListarJoinPorId(contrato.Id);
                foreach (var item in contratoCliente.ClientesContratos) {
                    if (!string.IsNullOrEmpty(item.PessoaFisicaId.ToString())) {
                        bool clienteValidation = list.Any(x => x.Id == item.PessoaFisicaId) ? true : false;
                        if (clienteValidation != true) {
                            _bancoContext.ClientesContrato.Remove(new ClientesContrato { PessoaFisicaId = item.PessoaFisicaId, Id = item.Id, ContratoId = item.ContratoId });
                        }
                    }
                }
                if (list.Count > 0) {
                    foreach (var item in list) {
                        bool clienteValidation = (contratoCliente.ClientesContratos.Any(x => x.PessoaFisicaId == item.Id)) ? true : false;
                        if (clienteValidation != true) {
                            ClientesContrato clientesContrato = new ClientesContrato { PessoaFisicaId = item.Id, ContratoId = contrato.Id };
                            _bancoContext.ClientesContrato.Add(clientesContrato);
                        }
                    }
                }
            }
            catch (Exception erro) {
                throw new Exception(erro.Message);
            }
        }
        public void UpdateClienteJuridico(Contrato contrato, List<PessoaJuridica> list) {
            try {
                var contratoCliente = ListarJoinPorId(contrato.Id);
                foreach (var item in contratoCliente.ClientesContratos) {
                    if (!string.IsNullOrEmpty(item.PessoaJuridicaId.ToString())) {
                        bool clienteValidation = list.Any(x => x.Id == item.PessoaJuridicaId) ? true : false;
                        if (clienteValidation != true) {
                            _bancoContext.ClientesContrato.Remove(new ClientesContrato { PessoaJuridicaId = item.PessoaJuridicaId, Id = item.Id, ContratoId = item.ContratoId });
                        }
                    }
                }
                if (list.Count > 0) {
                    foreach (var item in list) {
                        bool clienteValidation = (contratoCliente.ClientesContratos.Any(x => x.PessoaJuridicaId == item.Id)) ? true : false;
                        if (clienteValidation != true) {
                            ClientesContrato clientesContrato = new ClientesContrato { PessoaJuridicaId = item.Id, ContratoId = contrato.Id };
                            _bancoContext.ClientesContrato.Add(clientesContrato);
                        }
                    }
                }
            }
            catch (Exception erro) {
                throw new Exception(erro.Message);
            }
        }

        public Contrato InativarContrato(Contrato contrato) {
            try {
                Contrato contratoDB = ListarJoinPorId(contrato.Id);
                if (contratoDB == null) throw new Exception("Desculpe, ID não foi encontrado.");
                if (contratoDB.Aprovacao == StatusAprovacao.Aprovado) {
                    throw new Exception("Não é possível inativar contratos em andamento.");
                }
                contratoDB.StatusContrato = ContratoStatus.Inativo;
                _bancoContext.Contrato.Update(contratoDB);
                _bancoContext.SaveChanges();
                return contrato;
            }
            catch (Exception erro) {
                throw new Exception(erro.Message);
            }
        }
        public Contrato AtivarContrato(Contrato contrato) {
            try {
                Contrato contratoDB = ListarJoinPorId(contrato.Id);
                if (contratoDB == null) throw new Exception("Desculpe, ID não foi encontrado.");
                contratoDB.StatusContrato = ContratoStatus.Ativo;
                _bancoContext.Update(contratoDB);
                _bancoContext.SaveChanges();
                return contrato;
            }
            catch (Exception erro) {
                throw new Exception(erro.Message);
            }
        }
        public Contrato AprovarContrato(Contrato contrato) {
            try {
                Contrato contratoDB = ListarJoinPorId(contrato.Id);
                if (contratoDB == null) {
                    throw new Exception("Desculpe, ID não foi encontrado.");
                }
                //Realizando tal medida logo abaixo para que os espertinhos não consiga aprovar um contrato já aprovado pela URL.
                if (contratoDB.Aprovacao == StatusAprovacao.Aprovado) {
                    throw new Exception("Desculpe, ID não foi encontrado.");         
                }
                if (contratoDB.StatusContrato == ContratoStatus.Inativo) {
                    throw new Exception("Não é possível aprovar contratos inativos!");
                }

                //para não ter problemas com referência de objetos, já que um contrato pode ter clientes físicos ou jurídicos.
                if (ValidarClientDesabilitado(contrato) != false) {
                    throw new Exception("Não é possível aprovar contrato de clientes desabilitados!");
                }
                if (contratoDB.Motorista.Status == StatuFuncionario.Desabilitado) {
                    throw new Exception("Não é possível aprovar contrato com motorista vinculado desabilitado!");
                }
                if (contratoDB.Onibus.StatusOnibus == OnibusStatus.Desabilitado) {
                    throw new Exception("Não é possível aprovar contrato com ônibus vinculado desabilitado!");
                }
                contratoDB.Aprovacao = StatusAprovacao.Aprovado;
                contratoDB.Andamento = Andamento.EmAndamento;
                contratoDB.DataEmissao = DateTime.Now.Date;
                _bancoContext.Update(contratoDB);
                AddFinanceiro(contratoDB);
                _bancoContext.SaveChanges();
                return contratoDB;
            }
            catch (Exception erro) {
                throw new Exception(erro.Message);
            }
        }
        public Contrato RevogarContrato(Contrato contrato) {
            try {
                Contrato contratoDB = ListarJoinPorId(contrato.Id);
                if (contratoDB == null) {
                    throw new Exception("Desculpe, ID não foi encontrado.");
                }
                if (contratoDB.Aprovacao == StatusAprovacao.Aprovado) {
                    throw new Exception("Contratos aprovados não podem ser negados!");
                }
                contratoDB.Aprovacao = StatusAprovacao.Negado;
                _bancoContext.Update(contratoDB);
                _bancoContext.SaveChanges();
                return contratoDB;
            }
            catch (Exception erro) {
                throw new Exception(erro.Message);
            }
        }

        public Contrato ContratoTrim(Contrato contrato) {
            contrato.Detalhamento = contrato.Detalhamento.Trim();
            return contrato;
        }

        public bool ValidarClientDesabilitado(Contrato value) {
            List<PessoaFisica> pessoaFisicas = _bancoContext.PessoaFisica.Where(x => x.Status == StatuCliente.Desabilitado)
                .AsNoTracking().Include(x => x.ClientesContratos).ThenInclude(x => x.Contrato).ToList();

            List<PessoaJuridica> pessoaJuridicas = _bancoContext.PessoaJuridica.Where(x => x.Status == StatuCliente.Desabilitado)
                .AsNoTracking().Include(x => x.ClientesContratos).ThenInclude(x => x.Contrato).ToList();
            foreach (var item in pessoaFisicas) {
                if (item.ClientesContratos.Any(x => x.ContratoId == value.Id)) {
                    return true;
                }
            }
            foreach (var item in pessoaJuridicas) {
                if (item.ClientesContratos.Any(x => x.ContratoId == value.Id)) {
                    return true;
                }
            }
            return false;
        }
        public void AddFinanceiro(Contrato contrato) {
            //Incluindo financeiro no clientesContrato.
            foreach (var clientesContrato in contrato.ClientesContratos) {
                Financeiro financeiroDB = new Financeiro();
                financeiroDB.Contrato = contrato;
                financeiroDB.PessoaFisica = clientesContrato.PessoaFisica;
                financeiroDB.PessoaJuridica = clientesContrato.PessoaJuridica;
                financeiroDB.DataVencimento = contrato.DataVencimento;
                financeiroDB.DespesaReceita = DespesaReceita.Receita;
                financeiroDB.ValorTotDR = contrato.ValorParcelaContratoPorCliente * contrato.QtParcelas;
                financeiroDB.ValorParcelaDR = contrato.ValorParcelaContratoPorCliente;
                financeiroDB.DataEmissao = contrato.DataEmissao;
                financeiroDB.QtParcelas = contrato.QtParcelas;
                financeiroDB.Detalhamento = contrato.Detalhamento;
                financeiroDB.Pagament = contrato.Pagament;
                financeiroDB.FinanceiroStatus = FinanceiroStatus.Ativo;
                _bancoContext.Financeiro.Add(financeiroDB);
                for (int parcelas = 1; parcelas <= contrato.QtParcelas; parcelas++) {
                    Parcelas parcela = new Parcelas {
                        Financeiro = financeiroDB, StatusPagamento = SituacaoPagamento.AguardandoPagamento,
                        DataVencimentoParcela = contrato.DataEmissao.Value.AddMonths(parcelas - 1), NomeParcela = parcelas.ToString()
                    };
                    if (parcelas == 1) {
                        parcela.DataVencimentoParcela = contrato.DataEmissao.Value.AddDays(3);
                    }
                    _bancoContext.Parcelas.Add(parcela);
                }
            }
        }
    }
}
