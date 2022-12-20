using BusesControl.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace BusesControl.Models {
    public class Financeiro {
        public int Id { get; set; }
        public int? ClientesContratoId { get; set; }
        public ClientesContrato ClientesContrato { get; set; }
        public string NomeParcela { get; set; }
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? ValorJuros { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DataVencimentoParcela { get; set; }
        public SituacaoPagamento StatusPagamento { get; set; }

        public string ReturnNomeParcela() {
            return $"{NomeParcela}º parcela";
        }

        public string ReturnStatusPagamento() {
            if (StatusPagamento == SituacaoPagamento.AguardandoPagamento) return "Aguardando pagamento";
            else if (StatusPagamento == SituacaoPagamento.PagamentoContabilizado) return "Pagamento contabilizado";
            else return "Atrasada";
        }
    }
}
