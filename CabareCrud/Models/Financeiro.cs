using BusesControl.Models.Enums;
using System;

namespace BusesControl.Models {
    public class Financeiro {
        public int Id { get; set; }
        public int? ClientesContratoId { get; set; }
        public ClientesContrato ClientesContrato { get; set; }
        public string NomeParcela { get; set; }
        public DateTime? DataVencimentoParcela { get; set; }
        public SituacaoPagamento StatusPagamento { get; set; }
    }
}
