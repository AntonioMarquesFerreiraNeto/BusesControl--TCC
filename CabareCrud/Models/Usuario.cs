using BusesControl.Models.ModelValidarCPF;
using System.ComponentModel.DataAnnotations;

namespace BusesControl.Models {
    public class Usuario {

        public string Cpf { get; set; }

        public string Email { get; set; }

        public string Senha { get; set; }
    }
}
