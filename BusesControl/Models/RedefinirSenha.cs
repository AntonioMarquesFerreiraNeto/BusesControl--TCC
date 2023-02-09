using BusesControl.Models.ValidacoesDados.ModelValidarEmail;
using System.ComponentModel.DataAnnotations;

namespace BusesControl.Models {
    public class RedefinirSenha {

        [Required(ErrorMessage = "Campo obrigatório!")]
        [MinLength(11, ErrorMessage = "Campo inválido!")]
        [MaxLength(11, ErrorMessage = "Campo inválido!")]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [ValidarEmail (ErrorMessage = "Campo inválido!")]
        public string Email { get; set; }
    }
}
