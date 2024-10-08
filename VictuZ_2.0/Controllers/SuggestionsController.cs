using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VictuZ_2._0.Data;
using VictuZ_2._0.Models.Suggestions;
using VictuZ_2._0.Models.Users;
using VictuZ_2._0.ViewModels;

namespace VictuZ_2._0.Controllers
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
            // Check if the user is authenticated
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            // Get the current user's ID
            int currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Retrieve the suggestion along with its likes
            var suggestion = _context.Suggestions
                .Include(s => s.SuggestionLikes)
                .FirstOrDefault(s => s.Id == id);

            if (suggestion == null)
            {
                return NotFound();
            }

            // Check if the current user has already liked the suggestion
            var existingLike = suggestion.SuggestionLikes
                .FirstOrDefault(like => like.UserId == currentUserId);

            if (existingLike != null)
            {
                // User has liked it before; remove the like
                suggestion.SuggestionLikes.Remove(existingLike);
                suggestion.LikeCount--;

                // Optionally, remove the like from the database context
                _context.SuggestionLikes.Remove(existingLike);
            }
            else
            {
                // User has not liked it before; add a new like
                var newLike = new SuggestionLike
                {
                    SuggestionId = id,
                    UserId = currentUserId
                };

                suggestion.SuggestionLikes.Add(newLike);
                suggestion.LikeCount++;
            }

            // Save changes to the database
            _context.SaveChanges();

            // Redirect back to the page where the user came from
            return RedirectToAction("Index", "Home");
        }

        private bool SuggestionExists(int id)
        {
            return _context.Suggestions.Any(e => e.Id == id);
        }
    }
}
