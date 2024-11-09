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
            int? currentUserId = null;
            if (User.Identity.IsAuthenticated)
            {
                string userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!string.IsNullOrEmpty(userIdValue) && int.TryParse(userIdValue, out int parsedUserId))
                {
                    currentUserId = parsedUserId;
                }
            }

            // 1. Top 3 Suggesties met de hoogste LikeCount
            var topSuggestions = await _context.Suggestions
                .Where(s => !s.IsDeleted) // Veronderstel een soft delete mechanisme
                .OrderByDescending(s => s.LikeCount)
                .Take(3)
                .ToListAsync();

            // 2. Trending 3 Suggesties: Meeste likes binnen de kortste tijd sinds creatie, exclusief Top suggesties
            var trendingSuggestions = await _context.Suggestions
                .Where(s => !s.IsDeleted && !topSuggestions.Select(ts => ts.Id).Contains(s.Id))
                .OrderByDescending(s => s.LikeCount)
                .ThenBy(s => s.SubmittedOn) // Prioriteit aan nieuwere suggesties met veel likes
                .Take(3)
                .ToListAsync();

            // 3. Alle overige Suggesties, exclusief Top en Trending
            var allSuggestions = await _context.Suggestions
                .Where(s => !s.IsDeleted &&
                            !topSuggestions.Select(ts => ts.Id).Contains(s.Id) &&
                            !trendingSuggestions.Select(ts => ts.Id).Contains(s.Id))
                .OrderByDescending(s => s.SubmittedOn) // Of een andere gewenste volgorde
                .ToListAsync();

            var viewModel = new SuggestionsViewModel
            {
                TopSuggestions = topSuggestions,
                TrendingSuggestions = trendingSuggestions,
                AllSuggestions = allSuggestions
            };

            return View(viewModel);
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

        // POST: Suggestions/ToggleLike
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleLike(int id)
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

            var suggestion = await _context.Suggestions
                .Include(s => s.SuggestionLikes)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (suggestion == null)
            {
                return NotFound();
            }

            var existingLike = suggestion.SuggestionLikes.FirstOrDefault(l => l.UserId == userId);
            if (existingLike != null)
            {
                // Unlike
                _context.SuggestionLikes.Remove(existingLike);
                suggestion.LikeCount = Math.Max(0, suggestion.LikeCount - 1); // Voorkom negatieve likes
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
                suggestion.LikeCount += 1;
            }

            await _context.SaveChangesAsync();

            // Redirect naar de vorige pagina of naar Index als referer niet beschikbaar is
            var referer = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(referer))
            {
                return Redirect(referer);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        private bool SuggestionExists(int id)
        {
            return _context.Suggestions.Any(e => e.Id == id);
        }
    }
}
