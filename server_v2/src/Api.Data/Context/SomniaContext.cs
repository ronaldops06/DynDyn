using Data.Mapping;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Data.Context
{
    public class SomniaContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }

        public SomniaContext(DbContextOptions<SomniaContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserEntity>(new UserMap().Configure);

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
