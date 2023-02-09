using Microsoft.EntityFrameworkCore;
using BusesControl.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusesControl.Data.Map {
    public class MapRescisao : IEntityTypeConfiguration<Rescisao> {
        public void Configure(EntityTypeBuilder<Rescisao> builder) {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Contrato);
            builder.HasOne(x => x.PessoaFisica);
            builder.HasOne(x => x.PessoaJuridica);
        }
    }
}
