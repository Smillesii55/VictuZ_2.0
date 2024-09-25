using VictuZ_2._0.Models.Sessions;
using VictuZ_2._0.Models.Users;

namespace VictuZ_2._0.Models.Feedbacks
{
    public class Feedback
    {
        public int Id { get; set; }
        public int SessionId { get; set; }
        public int MemberId { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
        public DateTime SubmittedOn { get; set; }

        // Navigatie-eigenschappen
        public Session? Session { get; set; }
        public Member? Member { get; set; }

        // Constructor
        public Feedback(int id, int sessionId, int memberId, string content, int rating, DateTime submittedOn)
        {
            Id = id;
            SessionId = sessionId;
            MemberId = memberId;
            Content = content;
            Rating = rating;
            SubmittedOn = submittedOn;
        }
    }
}
