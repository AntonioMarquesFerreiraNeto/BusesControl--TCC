using System;

namespace BusesControl.Models {
    public class Filtros {
        public string ReceitasDespesas { get; set; }
        public string DataFiltro { get; set; }
        public DateTime? DataInicial { get; set; }
        public DateTime? DataTermino { get; set; }
    }
}
