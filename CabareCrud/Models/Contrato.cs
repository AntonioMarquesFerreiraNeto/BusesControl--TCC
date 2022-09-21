using BusesControl.Models.ValidacoesDados.ModelValidarNum;
using System.ComponentModel.DataAnnotations;

namespace BusesControl.Models {
    public class Contrato {
        public int Id { get; set; }

        [ValidarCampo(ErrorMessage = "Campo obrigatório!")]
        public int IdMotorista { get; set; }

        [ValidarCampo(ErrorMessage = "Campo obrigatório!")]
        public int IdCliente { get; set; }

        [ValidarCampo(ErrorMessage = "Campo obrigatório!")]
        public int IdOnibus { get; set; }
    }
}
