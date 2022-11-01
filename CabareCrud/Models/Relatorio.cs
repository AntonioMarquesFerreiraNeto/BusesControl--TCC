using System.ComponentModel.DataAnnotations;

namespace BusesControl.Models {
    public class Relatorio {

        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? ValTotAprovados { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? ValTotEmAnalise { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal? ValTotContratos { get; set; }
    }
}
