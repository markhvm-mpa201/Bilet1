using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bilet1.Configurations;

public class TeamMemberConfiguration : IEntityTypeConfiguration<TeamMember>
{
    public void Configure(EntityTypeBuilder<TeamMember> builder) 
    {
        builder.Property(x => x.Name).IsRequired().HasMaxLength(265);
        builder.Property(x => x.Description).IsRequired().HasMaxLength(1024);
        builder.Property(x => x.ImagePath).IsRequired().HasMaxLength(1024);

        builder.HasOne(x => x.MemberPosition).WithMany(x => x.TeamMembers).HasForeignKey(x => x.MemberPositionId).HasPrincipalKey(x => x.Id).OnDelete(DeleteBehavior.Cascade);
    }
}
