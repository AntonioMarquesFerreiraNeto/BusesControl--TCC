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
            long ano_long = Int64.Parse(ano);
            DateTime dataAtual = DateTime.Now;
            string data = dataAtual.ToString();
            long dataAtual_long = Int64.Parse(data);

            if (ano_long > dataAtual_long || ano_long < 1980) {
                return false;
            }
            else {
                return true;
            }
        }
    }
}
