using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VictuZ_2._0.Models.Feedbacks;
using VictuZ_2._0.Models.Sessions;
using VictuZ_2._0.Models.Suggestions;

namespace VictuZ_2._0.Models.Users
{
    public class User : IdentityUser<int>
    {
        [Required]
        public string Name { get; set; }

        public DateTime RegistrationDate { get; set; }

        // Navigatie-eigenschappen
        public ICollection<SessionRegistration>? ActivityRegistrations { get; set; }
        public ICollection<Feedback>? Feedbacks { get; set; }
        public ICollection<Suggestion>? Suggestions { get; set; }
        public ICollection<Session>? CreatedActivities { get; set; }

        public User()
        {
            ActivityRegistrations = new HashSet<SessionRegistration>();
            Feedbacks = new HashSet<Feedback>();
            Suggestions = new HashSet<Suggestion>();
            CreatedActivities = new HashSet<Session>();
        }
    }
}
