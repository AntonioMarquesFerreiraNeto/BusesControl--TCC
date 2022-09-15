﻿using System.ComponentModel.DataAnnotations;

namespace BusesControl.Models {
    public class MudarSenha {
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [MinLength(8, ErrorMessage = "Senha atual inválida!")]
        public string SenhaAtual { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [MinLength(8, ErrorMessage = "Nova senha inválida!")]
        public string NovaSenha { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [MinLength(8, ErrorMessage = "Confirmar senha inválida!")]
        public string ConfirmarSenha { get; set; }

        public bool ValNovaSenhaConfirmSenha() {
            bool result = (ConfirmarSenha == NovaSenha) ? true : false;
            return result;
        }
    }
}
