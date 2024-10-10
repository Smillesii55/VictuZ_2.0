﻿using Core.Models.Feedbacks;
using Core.Models.Locations;
using Core.Models.Users;
using System.ComponentModel.DataAnnotations;

namespace Core.Models.Sessions
{
    public class Session
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Titel is verplicht.")]
        public string? Title { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Activiteitsdatum is verplicht.")]
        [DataType(DataType.DateTime)]
        public DateTime ActivityDate { get; set; }   // Starttijd

        [Required(ErrorMessage = "Einddatum is verplicht.")]
        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }        // Eindtijd

        public int CreatedById { get; set; }
        public int LocationId { get; set; }

        // Navigatie-eigenschappen
        public User? CreatedBy { get; set; }
        public Location? Location { get; set; }
        public ICollection<SessionRegistration>? ActivityRegistrations { get; set; }
        public ICollection<Feedback>? Feedbacks { get; set; }

        public Session()
        {
            ActivityRegistrations = new HashSet<SessionRegistration>();
            Feedbacks = new HashSet<Feedback>();
        }
    }
}