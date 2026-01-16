using System.ComponentModel.DataAnnotations;

namespace Bilet1.ViewModels.MemberPositions;

public class MemberPositionUpdateVM
{
    public int Id { get; set; }

    [Required, MaxLength(256), MinLength(3)]
    public string? Name { get; set; }
}
