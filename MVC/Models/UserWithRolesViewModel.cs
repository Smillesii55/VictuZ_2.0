namespace MVC.Models
{
    public class UserWithRolesViewModel
    {
        public Core.Models.Users.User User { get; set; }
        public List<string> Roles { get; set; }
    }
}
