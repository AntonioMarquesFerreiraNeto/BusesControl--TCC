using System;
using System.ComponentModel.DataAnnotations;

namespace BusesControl.Models.ValidacoesDados.ModelValidarDate {
    public class ValidarDataEmissao : ValidationAttribute {

        public override bool IsValid(object value) {
            if (value == null || string.IsNullOrEmpty(value.ToString())) {
                return false;
            }
            return ValidarDateEmissao(value.ToString());
        }
        public bool ValidarDateEmissao(string value) {
            DateTime dataEmissao = DateTime.Parse(value).Date;
            DateTime dataAtual = DateTime.Now.Date;

            if (dataEmissao != dataAtual) {
                return false;
            }

            return true;
        }
    }
}
