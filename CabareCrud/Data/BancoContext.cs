using BusesControl.Models;
using Microsoft.EntityFrameworkCore;

namespace BusesControl.Data {
    public class BancoContext : DbContext {
        public BancoContext(DbContextOptions<BancoContext> options) : base(options) {
        }

        //Tabela cliente está sendo criada e depois acessada.
        public DbSet<PessoaFisica> PessoaFisica { get; set; }
        public DbSet<PessoaJuridica> PessoaJuridica { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<PessoaFisica>()
                .HasIndex(p => p.Cpf)
                .IsUnique(true);
            modelBuilder.Entity<Cliente>()
                .HasIndex(p => p.Id)
                .IsUnique(true);
            modelBuilder.Entity<PessoaFisica>()
                .HasIndex(p => p.Rg)
                .IsUnique(true);
            modelBuilder.Entity<Cliente>()
                .HasIndex(p => p.Email)
                .IsUnique(true);
            modelBuilder.Entity<Cliente>()
                .HasIndex(p => p.Telefone)
                .IsUnique(true);
            modelBuilder.Entity<PessoaJuridica>()
           .HasIndex(p => p.Cnpj)
           .IsUnique(true);
            modelBuilder.Entity<PessoaJuridica>()
               .HasIndex(p => p.InscricaoEstadual)
               .IsUnique(true);
            modelBuilder.Entity<PessoaJuridica>()
               .HasIndex(p => p.RazaoSocial)
               .IsUnique(true);
            modelBuilder.Entity<PessoaJuridica>()
               .HasIndex(p => p.NomeFantasia)
               .IsUnique(true);
        }
    }
}
