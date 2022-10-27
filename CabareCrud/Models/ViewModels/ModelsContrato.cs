using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusesControl.Models.ViewModels {
    public class ModelsContrato {

        public List<Onibus> OnibusList { get; set; }
        public List<Funcionario> MotoristaList { get; set; }
        public List<PessoaFisica> ClienteFisicoList { get; set; }
        public List<PessoaJuridica> ClienteJuridicoList { get; set; }
        public Contrato Contrato { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        public int? ClienteId { get; set; }

        //Construtor vazio para poder istânciar um objeto na controller. 
        public ModelsContrato() { }
    }
}
