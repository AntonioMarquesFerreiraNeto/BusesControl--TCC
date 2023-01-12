using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BusesControl.Models.ViewModels {
    public class ModelsFinanceiroRD {
        
        public List<FornecedorFisico> CredorFisicoList { get; set; }
        
        public List<FornecedorJuridico> CredorJuridicoList { get; set; }
        
        [Required(ErrorMessage = "Campo obrigatório!")]
        public int? CredorDevedorId { get; set; }
        
        public Financeiro Financeiro { get; set; }
    }
}
