﻿using BusesControl.Models.ValidacoesDados.ModelValidarAnoFab;
using BusesControl.Models.ValidacoesDados.ModelValidarAssentos;
using System;
using System.ComponentModel.DataAnnotations;

namespace BusesControl.Models {
    public class Onibus {
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [MinLength(3, ErrorMessage = "Campo inválido.")]
        public string Marca { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [MinLength(3, ErrorMessage = "Campo inválido.")]
        public string NameBus { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [ValidarAnoFab(ErrorMessage = "Campo inválido!")]
        [MinLength(4, ErrorMessage = "Campo inválido!")]
        public string DataFabricacao { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [MinLength(8, ErrorMessage = "Campo inválido.")]
        public string Renavam { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [MinLength(7, ErrorMessage = "Campo inválido.")]
        public string Placa { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [MinLength(17, ErrorMessage = "Campo inválido.")]
        public string Chassi { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        [ValAssentos(ErrorMessage = "Campo inválido!")]
        public string Assentos { get; set; }

        public OnibusStatus StatusOnibus { get; set; }
        public CorBus corBus { get; set; }
    }
}
