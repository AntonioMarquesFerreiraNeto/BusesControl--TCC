using BusesControl.Data.Map;
using BusesControl.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace BusesControl.Data {
    public class BancoContext : DbContext {
        public BancoContext(DbContextOptions<BancoContext> options) : base(options) {
        }

        //Tabelas estão sendo criadas e depois acessadas.
        public DbSet<PessoaFisica> PessoaFisica { get; set; }
        public DbSet<PessoaJuridica> PessoaJuridica { get; set; }
        public DbSet<Funcionario> Funcionario { get; set; }
        public DbSet<Onibus> Onibus { get; set; }
        public DbSet<Contrato> Contrato { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            //Evitar duplicatas dos atributos de cliente físico e jurídico. 
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

            modelBuilder.ApplyConfiguration(new MapContrato());
            base.OnModelCreating(modelBuilder);
        }
    }
}
