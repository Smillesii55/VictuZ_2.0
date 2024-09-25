using VictuZ_2._0.Models.Feedbacks;
using VictuZ_2._0.Models.Sessions;
using VictuZ_2._0.Models.Suggestions;

namespace VictuZ_2._0.Models.Users
{
    public class Member : User
    {
        public DateTime RegistrationDate { get; set; }

        // Navigatie-eigenschappen
        public ICollection<SessionRegistration>? ActivityRegistrations { get; set; }
        public ICollection<Feedback>? Feedbacks { get; set; }
        public ICollection<Suggestion>? Suggestions { get; set; }

        // Constructor
        public Member()
        {

        }
    }
}
