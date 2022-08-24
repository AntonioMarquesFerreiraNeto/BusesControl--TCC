using System;
using System.ComponentModel.DataAnnotations;

namespace BusesControl.Models.ValidacoesDados.ModelValidarAnoFab {
    public class ValidarAnoFab : ValidationAttribute {
        public override bool IsValid(object value) {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return false;
            return validarAno(value.ToString());
        }
        public bool validarAno(string ano) {
            DateTime anoAtual = DateTime.Now;
            long anoFab = Int64.Parse(ano);
            long ianoAtual = anoAtual.Year;
            if (anoFab > ianoAtual || anoFab < 1970) {
                return false;
            }
            return true;
        }
    }
}
