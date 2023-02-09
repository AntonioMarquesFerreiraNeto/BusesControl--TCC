using BusesControl.Models.Enums;
using BusesControl.Models.ModelValidarCPF;
using BusesControl.Models.ValidacoesDados.ModelValidarDate;
using System;
using System.Collections.Generic;
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
        [ValidarDataCliente(ErrorMessage = "Data de nascimento inválida!")]
        [Display(Name = "Data de nascimento")]
        public DateTime? DataNascimento { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [MinLength(5, ErrorMessage = "Campo inválido!")]
        public string Rg { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [MinLength(5, ErrorMessage = "Campo inválido")]
        public string NameMae { get; set; }

        public int? IdVinculacaoContratual { get; set; }

        public virtual List<Rescisao> Rescisoes { get; set; }

        public StatuCliente Status { get; set; }

        public bool Validation(DateTime dataNascimento) {
            DateTime dataAtual = DateTime.Now;

            long dias = (int)dataAtual.Subtract(dataNascimento).TotalDays;

            long idade = dias / 365;

            if (idade >= 0 && idade < 18) {
                return true;
            }
            else {
                return false;
            }
        }
        public string ReturnCpfCliente() {
            return $"{Convert.ToUInt64(Cpf):000\\.000\\.000\\-00}";
        }
    }
}
