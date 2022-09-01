﻿using System;
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
            long dias = (int)dataAtual.Subtract(dataNascimento).TotalDays;
            long idade = dias / 365;
            if (idade < 18 || idade > 132) {
                return false;
            }
            else {
                string year = dataNascimento.Year.ToString();
                return ValidarAno(year);
            }
        }

        public bool ValidarAno(string ano) {
            if (ano.Length < 4 || ano.Length > 4) {
                return false;
            }
            else {
                return true;
            }
        }
    }
}
