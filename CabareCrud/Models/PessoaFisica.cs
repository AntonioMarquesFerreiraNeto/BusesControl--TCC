using BusesControl.Models.ModelValidarCPF;
using BusesControl.Models.ValidacoesCliente.ModelValidarDate;
using System;
using System.ComponentModel.DataAnnotations;

namespace BusesControl.Models {
    public class PessoaFisica : Cliente {

        [Required(ErrorMessage = "Campo obrigatório!")]
        [MinLength(5, ErrorMessage = "Campo inválido.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [CpfValidation(ErrorMessage = "Campo inválido!")]
        public string Cpf { get; set; }


        [Required(ErrorMessage = "Campo obrigatório!")]
        [DataType(DataType.Date, ErrorMessage = "Campo inválido!")]
        [ValidarData(ErrorMessage = "Data de nascimento inválida!")]
        [Display(Name = "Data de nascimento")]
        public DateTime? DataNascimento { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        public string Rg { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [MinLength(5, ErrorMessage = "Campo inválido")]
        public string NameMae { get; set; }

        public StatuCliente Status { get; set; }
    }
}
