using System.Collections.Generic;

namespace BusesControl.Models {
    public class Cliente {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Cpf { get; set; }
        public string Rg { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string NameMae { get; set; }
        public string Cep { get; set; }
        public string NumeroResidencial { get; set; }
        public string Logradouro { get; set; }
        public string ComplementoResidencial { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
    }
}
