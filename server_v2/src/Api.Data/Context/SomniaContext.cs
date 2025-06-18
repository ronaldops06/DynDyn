using Api.Data.Mapping;
using Api.Domain.Entities;
using Data.Mapping;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using Api.Domain.Enums;

namespace Data.Context
{
    public class SomniaContext : DbContext
    {
        public DbSet<TransientUserEntity> TransientUsers { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<CategoryEntity> Category { get; set; }
        public DbSet<PortfolioEntity> Portfolio { get; set; }
        public DbSet<OperationEntity> Operation { get; set; }
        public DbSet<TransactionEntity> Transaction { get; set; }
        public DbSet<BalanceEntity> Balance { get; set; }

        public SomniaContext(DbContextOptions<SomniaContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TransientUserEntity>(new TransientUserMap().Configure);
            modelBuilder.Entity<UserEntity>(new UserMap().Configure);
            modelBuilder.Entity<CategoryEntity>(new CategoryMap().Configure);
            modelBuilder.Entity<OperationEntity>(new OperationMap().Configure);
            modelBuilder.Entity<TransactionEntity>(new TransactionMap().Configure);
            modelBuilder.Entity<BalanceEntity>(new BalanceMap().Configure);
            modelBuilder.Entity<PortfolioEntity>(new PortfolioMap().Configure);

            modelBuilder.Entity<UserEntity>().HasData(
                new UserEntity
                {
                    Id = 1,
                    Name = "Administrador",
                    Login = "admin@gmail.com",
                    Password = "pgadmin",
                    DataCriacao = DateTime.Now,
                    Role = String.Empty
                },
                new UserEntity
                {
                    Id = 2,
                    Name = "Operação",
                    Login = "ope@gmail.com",
                    Password = "pgadmin",
                    DataCriacao = DateTime.Now,
                    Role = String.Empty
                }
            );

            /*modelBuilder.Entity<CategoryEntity>().HasData(
                new CategoryEntity
                {
                    Id = 1,
                    Name = "Transferência",
                    Type = CategoryType.Operação,
                    Status = StatusType.Ativo,
                });*/
        }
    }
}
