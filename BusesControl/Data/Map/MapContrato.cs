using BusesControl.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusesControl.Data.Map {
    public class MapContrato : IEntityTypeConfiguration<Contrato> {
        public void Configure(EntityTypeBuilder<Contrato> builder) {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Motorista);
            builder.HasOne(x => x.Onibus);
        }
    }
}
