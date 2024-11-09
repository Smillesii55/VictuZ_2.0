using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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

            // Haal de sessies op waarvoor de huidige gebruiker een reservering heeft
            var MySessions = await _context.Sessions
                .Include(s => s.Location)
                .Include(s => s.SessionRegistrations)
                    .ThenInclude(r => r.User)
                .Where(s => s.ActivityDate >= DateTime.Now)
                .Where(s => s.SessionRegistrations.Any(r => r.User == user))
                .OrderBy(s => s.ActivityDate)
                .Take(4)
                .ToListAsync();

            // Haal de IDs van sessies waarvoor de gebruiker al heeft gereserveerd
            var userReservedSessionIds = MySessions.Select(s => s.Id).ToList();

            // Haal populaire sessies op, exclusief de sessies waarvoor de gebruiker is ingeschreven
            var PopularSessions = await _context.Sessions
                .Include(s => s.SessionRegistrations)
                .Where(s => !userReservedSessionIds.Contains(s.Id))  // Filter de sessies waarvoor de gebruiker al ingeschreven is
                .OrderByDescending(s => s.SessionRegistrations.Count())  // Sorteer op het aantal inschrijvingen
                .Take(3)  // Neem de top 3 populairste sessies
                .ToListAsync();

            // Maak een ViewModel aan en vul het met de MySessions en gefilterde PopularSessions
            var viewModel = new MyReservationsViewModel
            {
                MyReservations = MySessions,
                PopularSessions = PopularSessions
            };

            return View(viewModel);
        }

        public async Task<IActionResult> AllReservations()
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

            // Haal alle sessies op waarvoor de huidige gebruiker een reservering heeft
            var MySessions = await _context.Sessions
                .Include(s => s.Location)
                .Include(s => s.SessionRegistrations)
                    .ThenInclude(r => r.User)
                .Where(s => s.SessionRegistrations.Any(r => r.User == user))
                .OrderBy(s => s.ActivityDate)
                .ToListAsync();

            // Maak een ViewModel aan en vul het met MySessions
            var viewModel = new MyReservationsViewModel
            {
                MyReservations = MySessions
            };

            return View(viewModel);
        }
    }
}
