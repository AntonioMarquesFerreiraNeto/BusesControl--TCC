using BusesControl.Models.Enums;
using BusesControl.Models.ValidacoesDados.ModelValidarDate;
using System;
using System.ComponentModel.DataAnnotations;

namespace BusesControl.Models {
    public class Contrato {

        public int Id { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        public int? IdMotorista { get; set; }
        
        [Required(ErrorMessage = "Campo obrigatório!")]
        public int? IdCliente { get; set; }
        
        [Required(ErrorMessage = "Campo obrigatório!")]
        public int? IdOnibus { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        public decimal? ValorMonetario { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        public int? QtParcelas { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [ValidarDataEmissao(ErrorMessage = "Campo inválido!")]
        public DateTime? DataEmissao { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        public DateTime? DataVencimento { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        public string Detalhamento { get; set; }

        public ContratoStatus StatusContrato { get; set; }

        public StatusAprovacao Aprovacao { get; set; }
    }
}
