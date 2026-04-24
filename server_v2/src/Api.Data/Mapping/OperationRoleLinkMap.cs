using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Mapping
{
    public class OperationRoleLinkMap
    {
        public void Configure(EntityTypeBuilder<OperationRoleLinkEntity> builder)
        {
            builder.ToTable("OperationRoleLink");
            
            builder.HasKey(x => new { x.OperationId, x.OperationRoleId });
        }
    }
}