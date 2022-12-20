﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BusesControl.Models.Enums;
using BusesControl.Models.ModelValidarCPF;
using BusesControl.Models.ValidacoesCliente.ModelValidarDate;
using BusesControl.Models.ValidacoesDados.ModelValidarEmail;

namespace BusesControl.Models {
    public class Cliente {
        public int Id { get; set; }

        [ValidarEmail(ErrorMessage = "Campo inválido!")]
        [MinLength(5, ErrorMessage = "Campo inválido!")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Campo inválido!")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        [MinLength(9, ErrorMessage = "Campo inválido!")]
        [MaxLength(9, ErrorMessage = "Campo inválido!")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [MinLength(8, ErrorMessage = "Campo inválido!")]
        [MaxLength(8, ErrorMessage = "Campo inválido!")]
        public string Cep { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        public string NumeroResidencial { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [MinLength(3, ErrorMessage = "Campo inválido!")]
        public string Logradouro { get; set; }


        [Required(ErrorMessage = "Campo obrigatório!")]
        [MinLength(2, ErrorMessage = "Campo inválido!")]
        public string ComplementoResidencial { get; set; }


        [Required(ErrorMessage = "Campo obrigatório!")]
        [MinLength(2, ErrorMessage = "Campo inválido!")]
        public string Bairro { get; set; }

        [MinLength(3, ErrorMessage = "Campo inválido!")]
        [Required(ErrorMessage = "Campo obrigatório!")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]

        [MinLength(2, ErrorMessage = "Campo inválido!")]
        public string Estado { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [MinLength(2, ErrorMessage = "Campo inválido!")]
        public string Ddd { get; set; }

        public Adimplente Adimplente { get; set; }

        public virtual List<ClientesContrato> ClientesContratos { get; set; }

        public string ReturnTelefoneCliente() {
            string tel = Telefone;
            return $"{long.Parse(tel).ToString(@"00000-0000")}";
        }

        public string ReturnAdimplenciaCliente() {
            string situacao = (Adimplente == Adimplente.Adimplente) ? "Cliente adimplente" : "Cliente inadimplente";
            return situacao;
        }
    }
}
