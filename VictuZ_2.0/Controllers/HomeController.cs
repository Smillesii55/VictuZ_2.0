using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using VictuZ_2._0.Data;
using VictuZ_2._0.Models;
using VictuZ_2._0.Models.ViewModels;

namespace VictuZ_2._0.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Haal de eerstvolgende 3 sessies op
            var upcomingSessions = await _context.Sessions
                .Include(s => s.Location)
                .Where(s => s.ActivityDate >= DateTime.Now)
                .OrderBy(s => s.ActivityDate)
                .Take(3)
                .ToListAsync();

            // Haal de laatste 3 nieuwsberichten op
            var LatestNews = await _context.News
                .OrderByDescending(n => n.PublicationDate)
                .Take(3)
                .ToListAsync();

            // Haal de laatste 3 suggesties op en sorteer deze op like count
            var suggestions = await _context.Suggestions
                .Include(s => s.CreatedBy)
                .Include(s => s.SuggestionLikes)
                .OrderByDescending(s => s.LikeCount)
                .Take(3)
                .ToListAsync();

            // Vul het ViewModel
            var viewModel = new HomeIndexViewModel
            {
                UpcomingSessions = upcomingSessions,
                News = LatestNews,
                Suggestions = suggestions
            };

            return View(viewModel);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
