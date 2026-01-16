using Microsoft.AspNetCore.Identity;

namespace Bilet1.Models;

public class AppUser : IdentityUser
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;


}
