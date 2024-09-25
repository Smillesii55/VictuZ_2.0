using Microsoft.AspNetCore.Identity;
using VictuZ_2._0.Models.Enums;

namespace VictuZ_2._0.Models.Users
{
    public abstract class User : IdentityUser<int>
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }  // Beveiligd opslaan
        public string? PasswordHash { get; set; }  // Hashed wachtwoorden
        public RoleType Role { get; set; }

        public User() { }
    }
}
