using System.Collections.Generic;
using VictuZ_2._0.Models.Newss;
using VictuZ_2._0.Models.Sessions;
using VictuZ_2._0.Models.Suggestions;

namespace VictuZ_2._0.Models.ViewModels
{
    public class HomeIndexViewModel
    {
        public IEnumerable<Session>? UpcomingSessions { get; set; }
        public IEnumerable<News>? News { get; set; }
        public IEnumerable<Suggestion>? Suggestions { get; set; }

    }
}
