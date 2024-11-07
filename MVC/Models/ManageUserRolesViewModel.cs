using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace MVC.Models
{
    public class ManageUserRolesViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }

        // Lijst van alle rollen
        public List<IdentityRole<int>> Roles { get; set; }

        // IDs van rollen die de gebruiker momenteel heeft
        public List<int> AssignedRoleIds { get; set; }

        // IDs van rollen geselecteerd in het formulier
        public List<int> SelectedRoleIds { get; set; }
    }
}
