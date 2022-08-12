using System;
using System.ComponentModel.DataAnnotations;
using BusesControl.Models.ModelValidarCPF;
using BusesControl.Models.ValidacoesCliente.ModelValidarDate;


namespace BusesControl.Models {
    public class Cliente {
        public long Id { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [MinLength(5, ErrorMessage = "Campo inferior a cinco dígitos.")]
        [MaxLength(60, ErrorMessage = "Campo superior a 60 dígitos.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Campo obrigatório")]
        [CpfValidation(ErrorMessage = "Campo inválido!")]
        public string Cpf { get; set; }


        [Required(ErrorMessage = "Campo obrigatório!")]
        [DataType(DataType.Date, ErrorMessage = "Campo inválido!")]
        [ValidarData(ErrorMessage = "Data inválida!")]
        [Display(Name = "Data de nascimento")]
        public DateTime? DataNascimento { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [MinLength(5, ErrorMessage = "Campo inferior a cinco digitos.")]
        [MaxLength(14, ErrorMessage = "Campo superior a quatorze dígitos.")]
        public string Rg { get; set; }

        [EmailAddress(ErrorMessage = "Campo inválido!")]
        [MinLength(10, ErrorMessage = "Campo inferior a dez dígitos.")]
        [MaxLength(60, ErrorMessage = "Campo superior a sessenta dígitos.")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Campo inválido!")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        [MinLength(8, ErrorMessage ="Campo inferior a oito dígitos.")]
        [MaxLength(9, ErrorMessage = "Campo superior a nove dígitos.")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [MinLength(10, ErrorMessage = "Campo inferior a dez dígitos.")]
        [MaxLength(60, ErrorMessage = "Campo inferior a sessenta dígitos.")]
        public string NameMae { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [MinLength(8, ErrorMessage = "Campo inferior a oito dígitos.")]
        [MaxLength(8, ErrorMessage = "Campo superior a oito dígitos.")]
        public string Cep { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [MaxLength(10, ErrorMessage = "Campo superior a dez dígitos.")]
        public string NumeroResidencial { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [MinLength(5, ErrorMessage = "Campo inferior a quatro dígitos.")]
        [MaxLength(20, ErrorMessage = "Campo superior a vinte dígitos.")]
        public string Logradouro { get; set; }

    
        [Required(ErrorMessage = "Campo obrigatório!")]
        [MinLength(5, ErrorMessage = "Campo inferior a cinco dígitos.")]
        [MaxLength(50, ErrorMessage = "Campo superior a cinquenta dígitos.")]
        public string ComplementoResidencial { get; set; }

      
        [Required(ErrorMessage = "Campo obrigatório!")]
        [MinLength(4, ErrorMessage = "Tamanho em dígitos inválido.")]
        [MaxLength(20, ErrorMessage = "Campo superior a vinte dígitos.")]
        public string Bairro { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [MinLength(4, ErrorMessage = "Tamanho em dígitos inválido.")]
        [MaxLength(20, ErrorMessage = "Campo superior a vinte dígitos.")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [MinLength(2, ErrorMessage = "Campo inferior a dois dígitos.")]
        [MaxLength(20, ErrorMessage = "Campo superior a vinte dígitos.")]
        public string Estado { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [MinLength(2, ErrorMessage = "Campo inferior a dois dígitos.")]
        [MaxLength(2, ErrorMessage = "Campo superior a dois dígitos.")]
        public string Ddd { get; set; }
    }
}
