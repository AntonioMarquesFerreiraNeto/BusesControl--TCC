using BusesControl.Models;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Data {
    public class BancoContext : DbContext {
        public BancoContext(DbContextOptions<BancoContext> options) : base(options) {
        }

        //Tabela cliente está sendo criada e depois acessada.
        public DbSet<Cliente> Cliente { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Cliente>()
                .HasIndex(p => p.Cpf)
                .IsUnique(true);
            modelBuilder.Entity<Cliente>()
                .HasIndex(p => p.Id)
                .IsUnique(true);
            modelBuilder.Entity<Cliente>()
                .HasIndex(p => p.Rg)
                .IsUnique(true);
            modelBuilder.Entity<Cliente>()
                .HasIndex(p => p.Email)
                .IsUnique(true);
        }

    }
}
