using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Core.Data;
using Core.Models.Sessions;
using MVC.Models;
using System.Security.Claims;

namespace MVC.Controllers
{
    public class MyReservationsController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public MyReservationsController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        
        // GET: MyReservations
        public async Task<IActionResult> Index()
        {
            int? huidigeGebruikerId = null;
            if (User.Identity.IsAuthenticated)
            {
                string gebruikerIdWaarde = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!string.IsNullOrEmpty(gebruikerIdWaarde))
                {
                    huidigeGebruikerId = int.Parse(gebruikerIdWaarde);
                }
            }

            var user = await _context.Users
                                .FirstOrDefaultAsync(m => m.Id == huidigeGebruikerId);
            if (user == null)
            {
                return NotFound();
            }

            // Haal de sessies op waarvan de huidige gebruiker een reservering heeft
            var MySessions = await _context.Sessions
                .Include(s => s.Location)
                .Include(s => s.SessionRegistrations)
                    .ThenInclude(r => r.User)  // Voeg de gebruiker van de reservering toe
                .Where(s => s.ActivityDate >= DateTime.Now)
                .Where(s => s.SessionRegistrations.Any(r => r.User == user)) // Filter op reserveringen van de huidige gebruiker
                .OrderBy(s => s.ActivityDate)
                .Take(4)
                .ToListAsync();

            // Maak een ViewModel aan en vul het met de MySessions
            var viewModel = new MyReservationsViewModel
            {
                MyReservations = MySessions
            };

            return View(viewModel);  // Geef de ViewModel door naar de view
        }

    }
}
