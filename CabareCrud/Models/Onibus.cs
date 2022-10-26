using BusesControl.Models.Enums;
using BusesControl.Models.ValidacoesDados.ModelValidarAnoFab;
using BusesControl.Models.ValidacoesDados.ModelValidarAssentos;
using System;
using System.Collections.Generic;
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

        public virtual List<Contrato> Contratos { get; set; }

        public string ReturnCorBus() {
            if (corBus == CorBus.Branco) {
                return "Branco";
            }
            else if (corBus == CorBus.Azul) {
                return "Azul";
            }
            else if (corBus == CorBus.Cinza) {
                return "Cinza";
            }
            else if (corBus == CorBus.Prata) {
                return "Prata";
            }
            else if (corBus == CorBus.Amarelo) {
                return "Amarelo";
            }
            else if (corBus == CorBus.Verde) {
                return "Verde";
            }
            else if (corBus == CorBus.Preto) {
                return "Preto";
            }
            else if (corBus == CorBus.Vermelho) {
                return "Vermelho";
            }
            return "";
        }
        public string ReturnStatusOnibus() {
            if (StatusOnibus == OnibusStatus.Habilitado) {
                return "Habilitado";
            }
            return "Desabilitado";
        }
    }
}
