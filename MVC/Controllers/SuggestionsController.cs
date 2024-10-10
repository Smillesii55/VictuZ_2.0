using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using Core.Data;
using Core.Models.Suggestions;
using MVC.Models;

namespace MVC.Controllers
{
    public class SuggestionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SuggestionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Suggestions
        public async Task<IActionResult> Index()
        {
            // Create an instance of SuggestionsViewModel and populate it
            var viewModel = new SuggestionsViewModel
            {
                TopSuggestions = await GetTopSuggestions(),
                TrendingSuggestions = await GetTrendingSuggestions(),
                AllSuggestions = await GetAllSuggestions()
            };

            return View(viewModel);
        }

        private async Task<List<Suggestion>> GetTopSuggestions()
        {
            // Logic to retrieve top suggestions, you can customize this.
            // For example, top could be based on the highest like count.
            return await _context.Suggestions
                .OrderByDescending(s => s.LikeCount)
                .Take(5)
                .ToListAsync();
        }

        private async Task<List<Suggestion>> GetTrendingSuggestions()
        {
            // Logic to retrieve trending suggestions, e.g., those with increasing likes over time.
            // You could implement a trend algorithm here.
            return await _context.Suggestions
                .OrderByDescending(s => s.SubmittedOn)
                .Take(5)
                .ToListAsync();
        }

        private async Task<List<Suggestion>> GetAllSuggestions()
        {
            // Retrieve all suggestions.
            return await _context.Suggestions.ToListAsync();
        }

        // GET: Suggestions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suggestion = await _context.Suggestions
                .Include(s => s.CreatedBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (suggestion == null)
            {
                return NotFound();
            }

            return View(suggestion);
        }

        // GET: Suggestions/Create
        public IActionResult Create()
        {
            ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Name");
            return View();
        }

        // POST: Suggestions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CreatedById,Content,SubmittedOn,IsAnonymous,IsAnonymousToBoard,LikeCount")] Suggestion suggestion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(suggestion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Name", suggestion.CreatedById);
            return View(suggestion);
        }

        // Other methods remain unchanged...
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleLike(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdValue, out int userId))
            {
                return Unauthorized();
            }

            var suggestion = _context.Suggestions
                .Include(s => s.SuggestionLikes)
                .FirstOrDefault(s => s.Id == id);

            if (suggestion == null)
            {
                return NotFound();
            }

            var existingLike = suggestion.SuggestionLikes.FirstOrDefault(l => l.UserId == userId);
            if (existingLike != null)
            {
                // Unlike
                _context.SuggestionLikes.Remove(existingLike);
                suggestion.LikeCount--;
            }
            else
            {
                // Like
                var like = new SuggestionLike
                {
                    SuggestionId = id,
                    UserId = userId
                };
                _context.SuggestionLikes.Add(like);
                suggestion.LikeCount++;
            }

            _context.SaveChanges();

            // Haal de URL van de vorige pagina op uit de Referer-header
            var referer = Request.Headers["Referer"].ToString();

            if (!string.IsNullOrEmpty(referer))
            {
                // Redirect naar de vorige pagina
                return Redirect(referer);
            }
            else
            {
                // Als de Referer-header niet beschikbaar is, redirect naar een fallback actie
                return RedirectToAction("Index");
            }
        }


        private bool SuggestionExists(int id)
        {
            return _context.Suggestions.Any(e => e.Id == id);
        }
    }
}
