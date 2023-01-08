using BusesControl.Models.Enums;
using BusesControl.Models.ValidacoesCliente.ModelValidarCnpj;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusesControl.Models {
    public class PessoaJuridica : Cliente {

        [Required(ErrorMessage = "Campo obrigatório!")]
        [MinLength(2, ErrorMessage = "Campo inválido!")]
        public string NomeFantasia { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [ValidaCnpj(ErrorMessage = "Campo inválido!")]
        public string Cnpj { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [MinLength(3, ErrorMessage = "Campo inválido!")]
        public string RazaoSocial { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [MinLength(9, ErrorMessage = "Campo inválido!")]
        public string InscricaoEstadual { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [MinLength(11, ErrorMessage = "Campo inválido!")]
        public string InscricaoMunicipal { get; set; }

        public virtual List<Rescisao> Rescisoes { get; set; }

        public StatuCliente Status { get; set; }

        public string ReturnCnpjCliente() {
            return $"{Convert.ToUInt64(Cnpj): 00\\.000\\.000\\/0000-00}";
        }
    }
}
