using System;
using System.ComponentModel.DataAnnotations;

namespace BusesControl.Models.ValidacoesCliente.ModelValidarDate {
    public class ValidarData : ValidationAttribute {
        public override bool IsValid(object value) {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return false;

            return ValidarDataNascimento(value.ToString());
        }
        public bool ValidarDataNascimento(string data) {
            DateTime dataNascimento = DateTime.Parse(data);
            DateTime dataAtual = DateTime.Now;

            long iDataAtual = dataAtual.Year;
            long iDataNascimento = dataNascimento.Year;

            long resultado = iDataAtual - iDataNascimento;
            if (resultado < 18 || resultado > 132) {
                return false;
            }
            return true;
        }
    }
}
