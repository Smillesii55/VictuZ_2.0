using VictuZ_2._0.Models.Sessions;

namespace VictuZ_2._0.Models.Users
{
    public class BoardMember : User
    {
        // Extra eigenschappen voor bestuursleden kunnen hier worden toegevoegd
        // Bijvoorbeeld: Functie, StartDatum, etc.

        // Navigatie-eigenschappen
        public ICollection<Session>? CreatedActivities { get; set; }
    }
}
