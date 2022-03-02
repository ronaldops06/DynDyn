using Somnia.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Somnia.API.Data
{
    public class SomniaContext : DbContext
    {
        public SomniaContext() : base() { }

        public SomniaContext(DbContextOptions<SomniaContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public virtual DbSet<Categoria> Categorias { get; set; }
        public DbSet<Conta> Contas { get; set; }
        public DbSet<Operacao> Operacoes { get; set; }
        public DbSet<Movimento> Movimentos { get; set; }
        public DbSet<Saldo> Saldos { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<Conta>().HasOne(c => c.Categoria).WithOne().HasForeignKey<Conta>(fk => fk.CategoriaID);
            //builder.Entity<Conta>().Ignore(parent => parent.Categoria);
            var user = new User();
            user.ID = 1;
            user.Name = "Admin";
            user.Login = "admin";
            user.Password = "pgadmin";
            user.Role = "";

            builder.Entity<User>()
                .HasData(new List<User>{
                    user
                });
        }
    }
}
