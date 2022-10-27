﻿// <auto-generated />
using System;
using BusesControl.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BusesControl.Migrations
{
    [DbContext(typeof(BancoContext))]
    [Migration("20221027035243_Finish join")]
    partial class Finishjoin
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.25")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("BusesControl.Models.Cliente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Bairro")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Cep")
                        .IsRequired()
                        .HasColumnType("varchar(8) CHARACTER SET utf8mb4")
                        .HasMaxLength(8);

                    b.Property<string>("Cidade")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ComplementoResidencial")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Ddd")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Email")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Logradouro")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("NumeroResidencial")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Telefone")
                        .IsRequired()
                        .HasColumnType("varchar(9) CHARACTER SET utf8mb4")
                        .HasMaxLength(9);

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("Telefone")
                        .IsUnique();

                    b.ToTable("Cliente");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Cliente");
                });

            modelBuilder.Entity("BusesControl.Models.Contrato", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("Aprovacao")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DataEmissao")
                        .IsRequired()
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("DataVencimento")
                        .IsRequired()
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Detalhamento")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int?>("MotoristaId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int?>("OnibusId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int?>("PessoaFisicaId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int?>("PessoaJuridicaId")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int?>("QtParcelas")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int>("StatusContrato")
                        .HasColumnType("int");

                    b.Property<decimal?>("ValorMonetario")
                        .IsRequired()
                        .HasColumnType("decimal(65,30)");

                    b.HasKey("Id");

                    b.HasIndex("MotoristaId");

                    b.HasIndex("OnibusId");

                    b.HasIndex("PessoaFisicaId");

                    b.HasIndex("PessoaJuridicaId");

                    b.ToTable("Contrato");
                });

            modelBuilder.Entity("BusesControl.Models.Funcionario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Apelido")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Bairro")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("Cargos")
                        .HasColumnType("int");

                    b.Property<string>("Cep")
                        .IsRequired()
                        .HasColumnType("varchar(8) CHARACTER SET utf8mb4")
                        .HasMaxLength(8);

                    b.Property<string>("Cidade")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ComplementoResidencial")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Cpf")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime?>("DataNascimento")
                        .IsRequired()
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Ddd")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Estado")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Logradouro")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("NumeroResidencial")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Senha")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("StatusUsuario")
                        .HasColumnType("int");

                    b.Property<string>("Telefone")
                        .IsRequired()
                        .HasColumnType("varchar(9) CHARACTER SET utf8mb4")
                        .HasMaxLength(9);

                    b.HasKey("Id");

                    b.ToTable("Funcionario");
                });

            modelBuilder.Entity("BusesControl.Models.Onibus", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Assentos")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Chassi")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("DataFabricacao")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Marca")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("NameBus")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Placa")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Renavam")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("StatusOnibus")
                        .HasColumnType("int");

                    b.Property<int>("corBus")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Onibus");
                });

            modelBuilder.Entity("BusesControl.Models.PessoaFisica", b =>
                {
                    b.HasBaseType("BusesControl.Models.Cliente");

                    b.Property<string>("Cpf")
                        .IsRequired()
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<DateTime?>("DataNascimento")
                        .IsRequired()
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("IdVinculacaoContratual")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("NameMae")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Rg")
                        .IsRequired()
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasIndex("Cpf")
                        .IsUnique();

                    b.HasIndex("Rg")
                        .IsUnique();

                    b.HasDiscriminator().HasValue("PessoaFisica");
                });

            modelBuilder.Entity("BusesControl.Models.PessoaJuridica", b =>
                {
                    b.HasBaseType("BusesControl.Models.Cliente");

                    b.Property<string>("Cnpj")
                        .IsRequired()
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("InscricaoEstadual")
                        .IsRequired()
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("InscricaoMunicipal")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("NomeFantasia")
                        .IsRequired()
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("RazaoSocial")
                        .IsRequired()
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<int>("Status")
                        .HasColumnName("PessoaJuridica_Status")
                        .HasColumnType("int");

                    b.HasIndex("Cnpj")
                        .IsUnique();

                    b.HasIndex("InscricaoEstadual")
                        .IsUnique();

                    b.HasIndex("NomeFantasia")
                        .IsUnique();

                    b.HasIndex("RazaoSocial")
                        .IsUnique();

                    b.HasDiscriminator().HasValue("PessoaJuridica");
                });

            modelBuilder.Entity("BusesControl.Models.Contrato", b =>
                {
                    b.HasOne("BusesControl.Models.Funcionario", "Motorista")
                        .WithMany("Contratos")
                        .HasForeignKey("MotoristaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BusesControl.Models.Onibus", "Onibus")
                        .WithMany("Contratos")
                        .HasForeignKey("OnibusId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BusesControl.Models.PessoaFisica", "PessoaFisica")
                        .WithMany("Contratos")
                        .HasForeignKey("PessoaFisicaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BusesControl.Models.PessoaJuridica", "PessoaJuridica")
                        .WithMany("Contratos")
                        .HasForeignKey("PessoaJuridicaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
