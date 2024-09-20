﻿namespace VictuZ_2._0.Models.Sessions
{
    public class Session
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime ActivityDate { get; set; }   // Starttijd
        public DateTime EndDate { get; set; }        // Eindtijd
        public int CreatedById { get; set; }
        public int LocationId { get; set; }

        // Navigatie-eigenschappen
        //public BoardMember? CreatedBy { get; set; }
        //public Location? Location { get; set; }
        //public ICollection<SessionRegistration>? ActivityRegistrations { get; set; }
        //public ICollection<Feedback>? Feedbacks { get; set; }

        public Session()
        {

        }
    }
}
