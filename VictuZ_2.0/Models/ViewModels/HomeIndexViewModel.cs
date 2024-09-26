using System.Collections.Generic;
using VictuZ_2._0.Models.Sessions;

namespace VictuZ_2._0.Models.ViewModels
{
    public class HomeIndexViewModel
    {
        public IEnumerable<Session>? UpcomingSessions { get; set; }
    }
}
