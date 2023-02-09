using BusesControl.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusesControl.Data.Map {
    public class MapFinanceiro : IEntityTypeConfiguration<Parcelas> {
        public void Configure(EntityTypeBuilder<Parcelas> builder) {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Financeiro);
        }
    }
}
