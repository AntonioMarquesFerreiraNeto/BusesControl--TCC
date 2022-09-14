using System.ComponentModel.DataAnnotations;

namespace BusesControl.Models {
    public class MudarSenha {
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [MinLength(8, ErrorMessage = "Senha inválida!")]
        public string SenhaAtual { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [MinLength(8, ErrorMessage = "Senha inválida!")]
        public string NovaSenha { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [MinLength(8, ErrorMessage = "Senha inválida!")]
        public string ConfirmarSenha { get; set; }

        public bool ValidarSenhaAtual() {
            bool result = (ConfirmarSenha == NovaSenha) ? true : false;
            return result;
        }
    }
}
