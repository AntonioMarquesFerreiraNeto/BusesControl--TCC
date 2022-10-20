using System.Collections.Generic;

namespace BusesControl.Models.ViewModels {
    public class ModelsContrato {

        public List<Onibus> OnibusList { get; set; }
        public List<Funcionario> MotoristaList { get; set; }
        public List<PessoaFisica> ClienteFisicoList { get; set; }
        public List<PessoaJuridica> ClienteJuridicoList { get; set; }
        public Contrato Contrato { get; set; }

        public string DetalhesClienteView { get; set; }
        public string DetalhesMotoristaView { get; set; }
        public string DetalheOnibusView { get; set; }
        public string PlacaOnibusView { get; set; }

        //Construtor vazio para poder istânciar um objeto na controller. 
        public ModelsContrato() { }
    }
}
