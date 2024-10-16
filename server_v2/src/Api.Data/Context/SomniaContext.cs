﻿using Api.Data.Mapping;
using Api.Domain.Entities;
using Data.Mapping;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Data.Context
{
    public class SomniaContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<CategoryEntity> Category { get; set; }
        public DbSet<AccountEntity> Account { get; set; }
        public DbSet<OperationEntity> Operation { get; set; }
        public DbSet<TransactionEntity> Transaction { get; set; }

        public SomniaContext(DbContextOptions<SomniaContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserEntity>(new UserMap().Configure);
            modelBuilder.Entity<CategoryEntity>(new CategoryMap().Configure);
            modelBuilder.Entity<AccountEntity>(new AccountMap().Configure);
            modelBuilder.Entity<OperationEntity>(new OperationMap().Configure);
            modelBuilder.Entity<TransactionEntity>(new TransactionMap().Configure);

            modelBuilder.Entity<UserEntity>().HasData(
                new UserEntity
                {
                    Id = 1,
                    Name = "Administrador",
                    Login = "admin@gmail.com",
                    Password = "pgadmin",
                    DataCriacao = DateTime.Now,
                    Role = String.Empty
                }
            );
        }
    }
}
