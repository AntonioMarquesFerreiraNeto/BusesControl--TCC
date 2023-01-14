﻿using BusesControl.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace BusesControl.Models {
    public class Parcelas {
        
        public int Id { get; set; }
        
        public int? FinanceiroId { get; set; }
        
        public Financeiro Financeiro { get; set; }
        
        public string NomeParcela { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? ValorJuros { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DataVencimentoParcela { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DataEfetuacao { get; set; }

        public SituacaoPagamento StatusPagamento { get; set; }

        public string ReturnNomeParcela() {
            return $"{NomeParcela}º parcela";
        }

        public string ReturnStatusPagamento() {
            if (StatusPagamento == SituacaoPagamento.AguardandoPagamento) return "Aguardando pagamento";
            else if (StatusPagamento == SituacaoPagamento.PagamentoContabilizado) return "Pagamento efetuado";
            else return "Atrasada";
        }
    }
}
