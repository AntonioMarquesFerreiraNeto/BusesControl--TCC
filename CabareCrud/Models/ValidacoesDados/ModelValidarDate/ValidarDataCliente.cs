using System;
using System.ComponentModel.DataAnnotations;

namespace BusesControl.Models.ValidacoesDados.ModelValidarDate {
    public class ValidarDataCliente : ValidationAttribute {
        public override bool IsValid(object value) {
            if (value == null || string.IsNullOrEmpty(value.ToString())) {
                return false;
            }
            return ValidationDateInvalid(value.ToString());
        }
        public bool ValidationDateInvalid(string date) {
            DateTime dataNascimento = DateTime.Parse(date);
            DateTime dataAtual = DateTime.Now;

            long dias = (int)dataAtual.Subtract(dataNascimento).TotalDays;
            long idade = dias / 365;
            if (idade <= 0 || idade > 132) {
                return false;
            }
            else {
                return true;
            }
        }
    }
}
