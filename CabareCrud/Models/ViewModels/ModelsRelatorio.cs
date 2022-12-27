using System.Collections.Generic;

namespace BusesControl.Models.ViewModels {
    public class ModelsRelatorio {
        public Relatorio Relatorio { get; set; }
        public List<Contrato> Contratos { get; set; }

        public ModelsRelatorio() { }
    }
}
