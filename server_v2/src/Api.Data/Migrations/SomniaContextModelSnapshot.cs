﻿// <auto-generated />
using System;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Data.Migrations
{
    [DbContext(typeof(SomniaContext))]
    partial class SomniaContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.17")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("Api.Domain.Entities.AccountEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("CategoryId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("DataAlteracao")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("DataCriacao")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int?>("ParentAccountId")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("ParentAccountId");

                    b.ToTable("Account");
                });

            modelBuilder.Entity("Api.Domain.Entities.BalanceEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("AccountId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("BalanceDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<double>("Credit")
                        .HasColumnType("double precision");

                    b.Property<DateTime?>("DataAlteracao")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("DataCriacao")
                        .HasColumnType("timestamp without time zone");

                    b.Property<double>("Debit")
                        .HasColumnType("double precision");

                    b.Property<double?>("Dividends")
                        .HasColumnType("double precision");

                    b.Property<double?>("Income")
                        .HasColumnType("double precision");

                    b.Property<double?>("PercentageIncome")
                        .HasColumnType("double precision");

                    b.Property<double?>("PercentageValuation")
                        .HasColumnType("double precision");

                    b.Property<double?>("SalaryCredit")
                        .HasColumnType("double precision");

                    b.Property<double?>("SalaryDebit")
                        .HasColumnType("double precision");

                    b.Property<double?>("Valuation")
                        .HasColumnType("double precision");

                    b.Property<double>("Value")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("BalanceDate")
                        .IsUnique();

                    b.ToTable("Balance");
                });

            modelBuilder.Entity("Api.Domain.Entities.CategoryEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime?>("DataAlteracao")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("DataCriacao")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Category");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Transferência",
                            Status = 1,
                            Type = 2
                        });
                });

            modelBuilder.Entity("Api.Domain.Entities.OperationEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("CategoryId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("DataAlteracao")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("DataCriacao")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<bool>("Recurrent")
                        .HasColumnType("boolean");

                    b.Property<bool>("Salary")
                        .HasColumnType("boolean");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Operation");
                });

            modelBuilder.Entity("Api.Domain.Entities.TransactionEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("AccountId")
                        .HasColumnType("integer");

                    b.Property<int>("Consolidated")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("DataAlteracao")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("DataCriacao")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("DestinationAccountId")
                        .HasColumnType("integer");

                    b.Property<int?>("Installment")
                        .HasColumnType("integer");

                    b.Property<string>("Observation")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<int>("OperationId")
                        .HasColumnType("integer");

                    b.Property<int?>("ParentTransactionId")
                        .HasColumnType("integer");

                    b.Property<int?>("TotalInstallments")
                        .HasColumnType("integer");

                    b.Property<double>("Value")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("DestinationAccountId");

                    b.HasIndex("OperationId");

                    b.HasIndex("ParentTransactionId");

                    b.ToTable("Transaction");
                });

            modelBuilder.Entity("Domain.Entities.UserEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime?>("DataAlteracao")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("DataCriacao")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Role")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.HasKey("Id");

                    b.HasIndex("Login")
                        .IsUnique();

                    b.ToTable("User");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            DataCriacao = new DateTime(2025, 1, 9, 16, 30, 51, 490, DateTimeKind.Local).AddTicks(272),
                            Login = "admin@gmail.com",
                            Name = "Administrador",
                            Password = "pgadmin",
                            Role = ""
                        });
                });

            modelBuilder.Entity("Api.Domain.Entities.AccountEntity", b =>
                {
                    b.HasOne("Api.Domain.Entities.CategoryEntity", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Api.Domain.Entities.AccountEntity", "ParentAccount")
                        .WithMany()
                        .HasForeignKey("ParentAccountId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Category");

                    b.Navigation("ParentAccount");
                });

            modelBuilder.Entity("Api.Domain.Entities.BalanceEntity", b =>
                {
                    b.HasOne("Api.Domain.Entities.AccountEntity", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("Api.Domain.Entities.OperationEntity", b =>
                {
                    b.HasOne("Api.Domain.Entities.CategoryEntity", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Api.Domain.Entities.TransactionEntity", b =>
                {
                    b.HasOne("Api.Domain.Entities.AccountEntity", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Api.Domain.Entities.AccountEntity", "DestinationAccount")
                        .WithMany()
                        .HasForeignKey("DestinationAccountId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Api.Domain.Entities.OperationEntity", "Operation")
                        .WithMany()
                        .HasForeignKey("OperationId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Api.Domain.Entities.TransactionEntity", "ParentTransaction")
                        .WithMany()
                        .HasForeignKey("ParentTransactionId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Account");

                    b.Navigation("DestinationAccount");

                    b.Navigation("Operation");

                    b.Navigation("ParentTransaction");
                });
#pragma warning restore 612, 618
        }
    }
}
