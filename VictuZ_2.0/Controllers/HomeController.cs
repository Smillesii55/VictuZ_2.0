using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using VictuZ_2._0.Data;
using VictuZ_2._0.Models;
using VictuZ_2._0.Models.ViewModels;

namespace VictuZ_2._0.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context; // Toegevoegd

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context) // Aangepast
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Haal de eerstvolgende 3 sessies op
            var upcomingSessions = await _context.Sessions
                .Include(s => s.Location) // Include Location gegevens
                .Where(s => s.ActivityDate >= DateTime.Now) // Toekomstige sessies
                .OrderBy(s => s.ActivityDate) // Orden op datum
                .Take(3) // Neem de eerste 3
                .ToListAsync();

            // Vul het ViewModel
            var viewModel = new HomeIndexViewModel
            {
                UpcomingSessions = upcomingSessions
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
