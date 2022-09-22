using System.Collections.Generic;

namespace BusesControl.Models.ViewModels {
    public class ModelsCliente {
        public List<PessoaFisica> ClienteFisicoList { get; set; }
        public List<PessoaJuridica> ClienteJuridicoList { get; set; }
        public PessoaFisica ClienteFisico { get; set; }

        public ModelsCliente() {

        }
    }
}
