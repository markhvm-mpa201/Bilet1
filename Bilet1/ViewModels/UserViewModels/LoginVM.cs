using System.ComponentModel.DataAnnotations;

namespace Bilet1.ViewModels.UserViewModels;

public class LoginVM
{
    [Required, MinLength(3)]
    public string UserName { get; set; } = string.Empty;

    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}
