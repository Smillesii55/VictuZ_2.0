using VictuZ_2._0.Models.Sessions;

namespace VictuZ_2._0.Models.Locations
{
    public class Location
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int Capacity { get; set; }

        // Navigatie-eigenschappen
        public ICollection<Session>? ScheduledActivities { get; set; }

        // Constructor
        public Location(int id, string name, string address, int capacity)
        {
            Id = id;
            Name = name;
            Address = address;
            Capacity = capacity;
        }
    }
}
