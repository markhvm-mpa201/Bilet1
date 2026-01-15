using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bilet1.Configurations;

public class MemberPositionConfiguration : IEntityTypeConfiguration<MemberPosition>
{
    public void Configure(EntityTypeBuilder<MemberPosition> builder)
    {
        builder.Property(x => x.Name).IsRequired().HasMaxLength(265);       
    }
}
