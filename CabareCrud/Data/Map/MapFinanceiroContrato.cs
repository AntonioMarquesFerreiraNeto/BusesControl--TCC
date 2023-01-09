using BusesControl.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BusesControl.Data.Map {
    public class MapFinanceiroContrato : IEntityTypeConfiguration<ParcelasCliente> {
        public void Configure(EntityTypeBuilder<ParcelasCliente> builder) {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.ClientesContrato);
        }
    }
}
