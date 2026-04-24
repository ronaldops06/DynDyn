using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mapping
{
    public class TotalizerRoleLinkMap
    {
        public void Configure(EntityTypeBuilder<TotalizerRoleLinkEntity> builder)
        {
            builder.ToTable("TotalizerRoleLink");
            
            builder.HasKey(x => new { x.TotalizerRoleId, x.OperationRoleId });
        }
    }
}