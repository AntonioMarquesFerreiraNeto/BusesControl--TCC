using System.ComponentModel.DataAnnotations;

namespace BusesControl.Models {
    public class Contrato {
        public int Id { get; set; }

        [Required(ErrorMessage = "Campo obrigatório!")]
        public int? IdMotorista { get; set; }
        
        [Required(ErrorMessage = "Campo obrigatório!")]
        public int? IdCliente { get; set; }
        
        [Required(ErrorMessage = "Campo obrigatório!")]
        public int? IdOnibus { get; set; }
    }
}
