using BusesControl.Data;
using BusesControl.Models.Enums;
using BusesControl.Models.ValidacoesDados.ModelValidarDate;
using BusesControl.Models.ValidacoesDados.ModelValidarParcela;
using BusesControl.Models.ValidacoesDados.ModelValidarValorMonetario;
using BusesControl.Repositorio;
using System;
using System.ComponentModel.DataAnnotations;

namespace BusesControl.Models {
    public class Contrato {

        public int Id { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        public int? MotoristaId { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        public int? ClienteId { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        public int? OnibusId { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [ValidarValorMonetario(ErrorMessage = "Campo inválido!")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? ValorMonetario { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [ValidationMinParcela(ErrorMessage = "Campo inválido!")]
        public int? QtParcelas { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DataEmissao { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DataVencimento { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [MinLength(30, ErrorMessage = "Campo inválido!")]
        public string Detalhamento { get; set; }

        public ContratoStatus StatusContrato { get; set; }

        public StatusAprovacao Aprovacao { get; set; }

        public Cliente Cliente { get; set; }
        
        public Funcionario Motorista { get; set; }

        public Onibus Onibus { get; set; }

        public bool ValidarValorMonetario() {
            if (ValorMonetario < 150) {
                return false;
            }
            return true;
        }
    }
}
