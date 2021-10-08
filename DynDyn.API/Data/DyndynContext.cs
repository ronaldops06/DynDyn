using DynDyn.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DynDyn.API.Data
{
    public class DyndynContext : DbContext
    {
        public DyndynContext(DbContextOptions<DyndynContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Conta> Contas { get; set; }
        public DbSet<Operacao> Operacoes { get; set; }
        public DbSet<Movimento> Movimentos { get; set; }
        public DbSet<Saldo> Saldos { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            //builder.Entity<Conta>().HasOne(c => c.Categoria).WithOne().HasForeignKey<Conta>(fk => fk.CategoriaID);
            //builder.Entity<Conta>().Ignore(parent => parent.Categoria);

            builder.Entity<User>()
                .HasData(new List<User>{
                    new User(1, "admin", "pgadmin", "")
                });
        }
    }
}
