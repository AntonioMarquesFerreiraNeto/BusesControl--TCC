using BusesControl.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusesControl.Models {
    public class Financeiro {

        public int Id { get; set; }

        public int? ContratoId { get; set; }

        public int? PessoaJuridicaId { get; set; }

        public int? PessoaFisicaId { get; set; }

        public int? FornecedorFisicoId { get; set; }

        public int? FornecedorJuridicoId { get; set; }

        public virtual PessoaFisica PessoaFisica { get; set; }

        public virtual PessoaJuridica PessoaJuridica { get; set; }

        public virtual FornecedorFisico FornecedorFisico { get; set; }

        public virtual FornecedorJuridico FornecedorJuridico { get; set; }

        public virtual List<Parcelas> Parcelas { get; set; }

        public virtual Contrato Contrato { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DataVencimento { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? ValorParcelaDR { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? ValorTotDR { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? ValorTotalPagoCliente { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? ValorTotTaxaJurosPaga { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DataEmissao { get; set; }

        public int? QtParcelas { get; set; }

        public TypeEfetuacao TypeEfetuacao { get; set; }

        public DespesaReceita DespesaReceita { get; set; }

        public ModelPagament Pagament { get; set; }

        public FinanceiroStatus FinanceiroStatus { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [MinLength(30, ErrorMessage = "Campo inválido!")]
        public string Detalhamento { get; set; }

        public string ReturnValorMultaRescisao() {
            decimal? valorTotCliente = Contrato.ValorParcelaContratoPorCliente * Contrato.QtParcelas;
            decimal valorMulta = (valorTotCliente.Value * 3) / 100;
            return $"{valorMulta.ToString("C2")}";
        }

        public string ReturnTypePagament() {
            string msgPagament = (Pagament == ModelPagament.Avista) ? "À vista" : "Parcelado";
            return msgPagament;
        }

        public string ReturnNameClienteOrCredor() {
            if (!string.IsNullOrEmpty(ContratoId.ToString())) {
                return (!string.IsNullOrEmpty(PessoaFisicaId.ToString())) ? $"{PessoaFisica.Name}" : $"{PessoaJuridica.RazaoSocial}";
            }
            else {
                if (!string.IsNullOrEmpty(PessoaFisicaId.ToString()) || !string.IsNullOrEmpty(PessoaJuridicaId.ToString())) {
                    return (!string.IsNullOrEmpty(PessoaFisicaId.ToString())) ? $"{PessoaFisica.Name}" : $"{PessoaJuridica.RazaoSocial}";
                }
                else {
                    return (!string.IsNullOrEmpty(FornecedorFisicoId.ToString())) ? $"{FornecedorFisico.Name}" : $"{FornecedorJuridico.RazaoSocial}";

                }
            }
        }
        public string ReturnTypeFinanceiro() {
            string type = (DespesaReceita == DespesaReceita.Receita) ? "Receita" : "Despesa";
            return type;
        }
        public string ReturnTypeEfetuacao() {
            if (TypeEfetuacao == TypeEfetuacao.Debito) {
                return $"Débito";
            }
            else if (TypeEfetuacao == TypeEfetuacao.Credito) {
                return $"Crédito";
            }
            else {
                return "Em espécie";
            }
        }
        public string ReturnStatusFinanceiro() {
            if (FinanceiroStatus == FinanceiroStatus.Ativo) return "Ativado";
            return "Inativo";
        }
        public string ReturnValorTot() {
            return $"{ValorTotDR.Value.ToString("C2")}";
        }
        public string ReturnValorParcela() {
            return $"{ValorParcelaDR.Value.ToString("C2")}";
        }
        public string ReturnValorTotEfetuado() {
            if (!string.IsNullOrEmpty(ValorTotalPagoCliente.ToString())) {
                return $"{ValorTotalPagoCliente.Value.ToString("C2")}";
            }
            return "R$ 0,00";
        }
    }
}
