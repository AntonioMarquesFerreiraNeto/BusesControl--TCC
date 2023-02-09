using System.ComponentModel.DataAnnotations;

namespace BusesControl.Models.ValidacoesDados.ModelValidarAssentos {
    public class ValAssentos : ValidationAttribute {
        public override bool IsValid(object value) {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return false;
            return ValidarAssento(value.ToString());
        }
        public bool ValidarAssento(string value) {
            long assentos = long.Parse(value);
            if (assentos > 200 || assentos < 10) {
                return false;
            }
            else {
                return true;
            }
        }
    }
}
