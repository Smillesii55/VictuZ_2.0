using Core.Models.Sessions;
namespace MVC.Models
{
    public class MyReservationsViewModel
    {
        public IEnumerable<Session> MyReservations { get; set; }
        public IEnumerable<Session> PopularSessions { get; set; }
    }
}
