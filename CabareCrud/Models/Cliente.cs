using System;
using System.ComponentModel.DataAnnotations;
using BusesControl.Models.ModelValidarCPF;
using BusesControl.Models.ValidacoesCliente.ModelValidarDate;


namespace BusesControl.Models {
    public class Cliente {
        public long Id { get; set; }

        [EmailAddress(ErrorMessage = "Campo inválido!")]
        [MinLength(5, ErrorMessage = "Campo inválido!")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Campo inválido!")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        [MinLength(8, ErrorMessage ="Campo inválido!")]
        [MaxLength(9, ErrorMessage = "Campo inválido!")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [MinLength(8, ErrorMessage = "Campo inválido!")]
        [MaxLength(8, ErrorMessage = "Campo inválido!")]
        public string Cep { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        public string NumeroResidencial { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [MinLength(5, ErrorMessage = "Campo inválido!")]
        public string Logradouro { get; set; }

    
        [Required(ErrorMessage = "Campo obrigatório!")]
        public string ComplementoResidencial { get; set; }

      
        [Required(ErrorMessage = "Campo obrigatório!")]
        public string Bairro { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]

        public string Estado { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [MinLength(2, ErrorMessage = "Campo inválido!")]
        public string Ddd { get; set; }
    }
}
