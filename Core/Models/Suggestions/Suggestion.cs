using System.ComponentModel.DataAnnotations;
using Core.Models.Users;

namespace Core.Models.Suggestions
{
    public class Suggestion
    {
        public int Id { get; set; }

        public int? CreatedById { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime SubmittedOn { get; set; }
        public bool IsAnonymous { get; set; }
        public bool IsAnonymousToBoard { get; set; }
        public int LikeCount { get; set; }
        public bool IsDeleted { get; set; }

        // Navigation properties
        public User? CreatedBy { get; set; }
        public ICollection<SuggestionLike>? SuggestionLikes { get; set; }

        public Suggestion()
        {
            SuggestionLikes = new HashSet<SuggestionLike>();
        }

        public bool LikedByCurrentUser()
        {
            return false;
        }
    }
}
