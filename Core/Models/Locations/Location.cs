using Core.Models.Sessions;
using System.ComponentModel.DataAnnotations;

namespace Core.Models.Locations
{
    public class Location
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Naam is verplicht.")]
        [StringLength(100, ErrorMessage = "Naam mag maximaal 100 tekens bevatten.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Adres is verplicht.")]
        [StringLength(200, ErrorMessage = "Adres mag maximaal 200 tekens bevatten.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Capaciteit is verplicht.")]
        [Range(1, 10000, ErrorMessage = "Capaciteit moet tussen 1 en 10.000 liggen.")]
        public int Capacity { get; set; }

        // Navigatie-eigenschappen
        public ICollection<Session>? ScheduledActivities { get; set; }

        // Parameterloze Constructor
        public Location()
        {
            ScheduledActivities = new HashSet<Session>();
        }

        // Overige Constructor (optioneel)
        public Location(int id, string name, string address, int capacity)
        {
            Id = id;
            Name = name;
            Address = address;
            Capacity = capacity;
            ScheduledActivities = new HashSet<Session>();
        }
    }
}
