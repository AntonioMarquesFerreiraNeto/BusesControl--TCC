using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusesControl.Models {
    public class ClientesContrato {
        public int Id { get; set; }
        public int? ContratoId { get; set; }
        public int? PessoaJuridicaId { get; set; }
        public int? PessoaFisicaId { get; set; }
        public virtual PessoaFisica PessoaFisica { get; set; }
        public virtual PessoaJuridica PessoaJuridica { get; set; }
        public virtual List<ParcelasCliente> ParcelasContrato { get; set; }
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? ValorTotalPagoCliente { get; set; }
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? ValorTotTaxaJurosPaga { get; set; }
        public virtual Contrato Contrato { get; set; }

        public string ReturnValorMultaRescisao() {
            decimal? valorTotCliente = Contrato.ValorParcelaContratoPorCliente * Contrato.QtParcelas;
            decimal valorMulta = (valorTotCliente.Value * 3) / 100;
            return $"{valorMulta.ToString("C2")}";
        }

        public ClientesContrato() { }
    }
}
