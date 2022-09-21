using System.ComponentModel.DataAnnotations;

namespace BusesControl.Models.ValidacoesDados.ModelValidarNum {
    public class ValidarCampo : ValidationAttribute {
        public override bool IsValid(object value) {
            return ValidaCampo(value.ToString());
        }
        public bool ValidaCampo(string valor) {
            if (string.IsNullOrEmpty(valor)) {
                return false;
            }
            return true;
        }
    }
}
