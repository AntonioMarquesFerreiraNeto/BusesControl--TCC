using System;
using System.ComponentModel.DataAnnotations;
using Correios.NET;
namespace BusesControl.Models.ValidacoesCliente.ModelValidarRG {
    public class ValidarCEP : ValidationAttribute {
        public override bool IsValid(object value) {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return false;

            return ValidadaCEP(value.ToString());
        }

        public bool ValidadaCEP(string value) {
            return true;
        }
    }
}
