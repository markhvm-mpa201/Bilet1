using System.ComponentModel.DataAnnotations;

namespace Bilet1.ViewModels.TeamMember;

public class TeamMemberUpdateVM
{
    public int Id { get; set; }

    [Required, MaxLength(256), MinLength(3)]
    public string? Name { get; set; }

    [Required, MaxLength(1024), MinLength(3)]
    public string? Description { get; set; }

    [Required]
    public int MemberPositionId { get; set; }

    [Required]
    public IFormFile? Image { get; set; }
}
