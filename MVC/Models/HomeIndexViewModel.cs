using Core.Models.Newss;
using Core.Models.Sessions;
using Core.Models.Suggestions;

namespace MVC.Models
{
    public class HomeIndexViewModel
    {
        public IEnumerable<Session>? UpcomingSessions { get; set; }
        public IEnumerable<News>? News { get; set; }
        public IEnumerable<Suggestion>? Suggestions { get; set; }

    }
}
