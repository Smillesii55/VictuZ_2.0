using Core.Models.Users;

namespace Core.Models.Sessions
{
    public class SessionRegistration
    {
        public int Id { get; set; }
        public int SessionId { get; set; }
        public int UserId { get; set; }
        public DateTime RegistrationDate { get; set; }

        public bool HasCancelled { get; set; } = false;
        public bool IsPresent { get; set; } = false;
        public bool HasAttended { get; set; } = false;

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
