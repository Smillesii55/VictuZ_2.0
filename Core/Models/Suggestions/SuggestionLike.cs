using Core.Models.Users;

namespace Core.Models.Suggestions
{
    public class SuggestionLike
    {
        public int? Id { get; set; }
        public int? SuggestionId { get; set; }
        public int? UserId { get; set; }

        // Navigatie-eigenschappen
        public Suggestion? Suggestion { get; set; }
        public User? User { get; set; }

        // Constructor
        public SuggestionLike()
        {
        }
    }
}
