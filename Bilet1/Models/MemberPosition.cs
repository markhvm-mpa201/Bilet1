namespace Bilet1.Models;

public class MemberPosition : BaseEntity
{
    public string Name { get; set; } = null!;

    public ICollection<TeamMember> TeamMembers { get; set; } = [];
}
