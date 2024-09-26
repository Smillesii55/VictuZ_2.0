using VictuZ_2._0.Models.Users;

namespace VictuZ_2._0.Models.Sessions
{
    public class SessionRegistration
    {
        public int Id { get; set; }
        public int SessionId { get; set; }
        public int UserId { get; set; }
        public DateTime RegistrationDate { get; set; }

        // Navigatie-eigenschappen
        public Session? Session { get; set; }
        public User? User { get; set; }

        public SessionRegistration() { }
        // Constructor
        public SessionRegistration(int id, int sessionId, int userId, DateTime registrationDate)
        {
            Id = id;
            SessionId = sessionId;
            UserId = userId;
            RegistrationDate = registrationDate;
        }
    }
}
