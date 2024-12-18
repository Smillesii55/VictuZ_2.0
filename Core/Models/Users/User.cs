﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Models.Feedbacks;
using Core.Models.Sessions;
using Core.Models.Suggestions;
using Core.Models.Newss;

namespace Core.Models.Users
{
    public class User : IdentityUser<int>
    {
        [Required]
        public string Name { get; set; }

        public DateTime RegistrationDate { get; set; }

        // Navigatie-eigenschappen
        public ICollection<SessionRegistration>? SessionRegistrations { get; set; }
        public ICollection<Feedback>? Feedbacks { get; set; }
        public ICollection<Suggestion>? Suggestions { get; set; }
        public ICollection<Session>? CreatedActivities { get; set; }
        public ICollection<News>? CreatedNews { get; set; }

        public User()
        {
            SessionRegistrations = new HashSet<SessionRegistration>();
            Feedbacks = new HashSet<Feedback>();
            Suggestions = new HashSet<Suggestion>();
            CreatedActivities = new HashSet<Session>();
        }
    }
}
