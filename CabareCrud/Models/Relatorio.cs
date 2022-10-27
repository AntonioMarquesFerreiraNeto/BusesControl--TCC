using System.ComponentModel.DataAnnotations;

namespace BusesControl.Models {
    public class Relatorio {

        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? ValorMonetarioTotal { get; set; }
    }
}
