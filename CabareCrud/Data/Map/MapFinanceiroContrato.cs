using BusesControl.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusesControl.Data.Map {
    public class MapFinanceiroContrato : IEntityTypeConfiguration<Financeiro> {
        public void Configure(EntityTypeBuilder<Financeiro> builder) {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.ClientesContrato);
        }
    }
}
