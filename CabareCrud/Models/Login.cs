using System.ComponentModel.DataAnnotations;

namespace BusesControl.Models {
    public class Login {

        [Required(ErrorMessage = "Campo obrigatório!")]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        public string Cep { get; set; }
    }
}
