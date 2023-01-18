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
        public DbSet<ClientesContrato> ClientesContrato { get; set; }
        public DbSet<Financeiro> Financeiro { get; set; }
        public DbSet<Parcelas> Parcelas { get; set; }
        public DbSet<Rescisao> Rescisao { get; set; }
        public DbSet<FornecedorFisico> FornecedorFisico { get; set; }
        public DbSet<FornecedorJuridico> FornecedorJuridico { get; set; }
        //Criado para adicionar o fornecedor juridico e fisico na mesma tabela. 
        public DbSet<Fornecedor> Fornecedor { get; set; }

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


            //Configurações de movimentações(relacionamento entre tabelas) do sistema.
            modelBuilder.ApplyConfiguration(new MapContrato());
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new MapFinanceiro());
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new MapRescisao());
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ClientesContrato>()
                .HasOne(x => x.Contrato)
                .WithMany(x => x.ClientesContratos)
                .HasForeignKey(x => x.ContratoId);

            modelBuilder.Entity<ClientesContrato>()
                .HasOne(x => x.PessoaFisica)
                .WithMany(x => x.ClientesContratos)
                .HasForeignKey(x => x.PessoaFisicaId);

            modelBuilder.Entity<ClientesContrato>()
                .HasOne(x => x.PessoaJuridica)
                .WithMany(x => x.ClientesContratos)
                .HasForeignKey(x => x.PessoaJuridicaId);
        }
    }
}
