using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using Core.Data;
using Core.Models.Suggestions;

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
            var applicationDbContext = _context.Suggestions.Include(s => s.CreatedBy);
            return View(await applicationDbContext.ToListAsync());
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Suggestions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var suggestion = await _context.Suggestions.FindAsync(id);
            if (suggestion == null)
            {
                return NotFound();
            }
            ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Name", suggestion.CreatedById);
            return View(suggestion);
        }

        // POST: Suggestions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CreatedById,Content,SubmittedOn,IsAnonymous,IsAnonymousToBoard,LikeCount")] Suggestion suggestion)
        {
            if (id != suggestion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(suggestion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SuggestionExists(suggestion.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Name", suggestion.CreatedById);
            return View(suggestion);
        }

        // GET: Suggestions/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Suggestions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var suggestion = await _context.Suggestions.FindAsync(id);
            if (suggestion != null)
            {
                _context.Suggestions.Remove(suggestion);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SuggestionExists(int id)
        {
            return _context.Suggestions.Any(e => e.Id == id);
        }

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
    }
}
