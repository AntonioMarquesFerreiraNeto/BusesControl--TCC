using System.ComponentModel.DataAnnotations;

namespace BusesControl.Models {
    public class Login {

        [Required(ErrorMessage = "Campo obrigatório!")]
        [MinLength(11, ErrorMessage = "Campo inválido!")]
        [MaxLength(11, ErrorMessage = "Campo inválido!")]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        public string Senha { get; set; }
    }
}
