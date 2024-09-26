using VictuZ_2._0.Models.Users;

namespace VictuZ_2._0.Models.Suggestions
{
    public class Suggestion
    {
        public int Id { get; set; }
        public int? UserId { get; set; }  // Kan null zijn voor anonieme suggesties
        public string Content { get; set; }
        public DateTime SubmittedOn { get; set; }
        public bool IsAnonymous { get; set; }  // Anoniem voor andere leden
        public bool IsAnonymousToBoard { get; set; }  // Anoniem voor bestuursleden
        public int LikeCount { get; set; }

        // Navigatie-eigenschappen
        public User? User { get; set; }
        public ICollection<SuggestionLike>? SuggestionLikes { get; set; }

        // Constructor
        public Suggestion(int id, int? userId, string content, DateTime submittedOn, bool isAnonymous, bool isAnonymousToBoard, int likeCount)
        {
            Id = id;
            UserId = userId;
            Content = content;
            SubmittedOn = submittedOn;
            IsAnonymous = isAnonymous;
            IsAnonymousToBoard = isAnonymousToBoard;
            LikeCount = likeCount;
        }
    }
}
