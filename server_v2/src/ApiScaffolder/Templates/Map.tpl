using Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Data.Mapping
{
    /// <summary>
    /// Mapeamento de campos para a tabela de {{name}} no banco de dados.
    /// </summary>
    public class {{model}}Map
    {
        public void Configure(EntityTypeBuilder<{{model}}Entity> builder)
        {
            builder.ToTable("{{model}}");

            builder.HasKey(u => u.Id);

            //Adicionar demais campos
            
            builder.HasOne(u => u.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .IsRequired();
        }
    }
}