using VictuZ_2._0.Models.Users;

namespace VictuZ_2._0.Models.Suggestions
{
    public class SuggestionLike
    {
        public int Id { get; set; }
        public int SuggestionId { get; set; }
        public int MemberId { get; set; }

        // Navigatie-eigenschappen
        public Suggestion? Suggestion { get; set; }
        public Member? Member { get; set; }

        // Constructor
        public SuggestionLike(int id, int suggestionId, int memberId)
        {
            Id = id;
            SuggestionId = suggestionId;
            MemberId = memberId;
        }
    }
}
