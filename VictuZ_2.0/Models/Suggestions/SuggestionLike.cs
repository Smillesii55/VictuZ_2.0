using VictuZ_2._0.Models.Users;

namespace VictuZ_2._0.Models.Suggestions
{
    public class SuggestionLike
    {
        public int Id { get; set; }
        public int SuggestionId { get; set; }
        public int UserId { get; set; }

        // Navigatie-eigenschappen
        public Suggestion? Suggestion { get; set; }
        public User? User { get; set; }

        // Constructor
        public SuggestionLike(int id, int suggestionId, int userId)
        {
            Id = id;
            SuggestionId = suggestionId;
            UserId = userId;
        }
    }
}
