using System.ComponentModel.DataAnnotations;

namespace BusesControl.Models.ValidacoesDados.ModelValidarParcela {
    public class ValidationMinParcela : ValidationAttribute {
        public override bool IsValid(object value) {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return false;
            return ValidarMinParcela(value.ToString());
        }
        public bool ValidarMinParcela(string value) {
            int qtParcela = int.Parse(value);
            if (qtParcela < 1) {
                return false;
            }
            else {
                return true;
            }
        }
    }
}
