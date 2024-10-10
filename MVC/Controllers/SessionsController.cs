using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using Core.Data;
using Core.Models.Sessions;
using Core.Models.Users;

namespace MVC.Controllers
{
    public class SessionsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<SessionsController> _logger;

        public SessionsController(ApplicationDbContext context, UserManager<User> userManager, ILogger<SessionsController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: Sessions
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Sessions.Include(s => s.CreatedBy).Include(s => s.Location);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Sessions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var session = await _context.Sessions
                .Include(s => s.CreatedBy)
                .Include(s => s.Location)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (session == null)
            {
                return NotFound();
            }

            return View(session);
        }

        // GET: Sessions/Create
        [Authorize(Roles = "BoardMember")]
        public IActionResult Create()
        {
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Name");
            return View();
        }

        // POST: Sessions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "BoardMember")]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,ActivityDate,EndDate,LocationId")] Session session)
        {
            _logger.LogInformation("Attempting to create a new session.");

            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    _logger.LogWarning("User is null during session creation.");
                    return Unauthorized();
                }

                session.CreatedById = user.Id;
                _context.Add(session);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Session '{session.Title}' created by user '{user.Email}'.");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // Log ModelState errors
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        _logger.LogError($"ModelState error for {state.Key}: {error.ErrorMessage}");
                    }
                }
            }

            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Name", session.LocationId);
            return View(session);
        }

        // GET: Sessions/Edit/5
        [Authorize(Roles = "BoardMember")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var session = await _context.Sessions.FindAsync(id);
            if (session == null)
            {
                return NotFound();
            }

            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Name", session.LocationId);
            return View(session);
        }

        // POST: Sessions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "BoardMember")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,ActivityDate,EndDate,LocationId")] Session session)
        {
            if (id != session.Id)
            {
                return NotFound();
            }

            _logger.LogInformation($"Attempting to edit session with ID {id}.");

            if (ModelState.IsValid)
            {
                try
                {
                    var existingSession = await _context.Sessions.FindAsync(id);
                    if (existingSession == null)
                    {
                        return NotFound();
                    }

                    // Validatie van datums
                    if (session.EndDate < session.ActivityDate)
                    {
                        ModelState.AddModelError(string.Empty, "Einddatum moet na de activiteitsdatum liggen.");
                        ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Name", session.LocationId);
                        return View(session);
                    }

                    // Update de velden
                    existingSession.Title = session.Title;
                    existingSession.Description = session.Description;
                    existingSession.ActivityDate = session.ActivityDate;
                    existingSession.EndDate = session.EndDate;
                    existingSession.LocationId = session.LocationId;

                    _context.Update(existingSession);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation($"Session '{existingSession.Title}' updated by user '{User.Identity.Name}'.");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SessionExists(session.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError("Concurrency exception tijdens het bewerken van de sessie.");
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // Log ModelState errors
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        _logger.LogError($"ModelState error for {state.Key}: {error.ErrorMessage}");
                    }
                }
            }

            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Name", session.LocationId);
            return View(session);
        }


        // GET: Sessions/Delete/5
        [Authorize(Roles = "BoardMember")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var session = await _context.Sessions
                .Include(s => s.CreatedBy)
                .Include(s => s.Location)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (session == null)
            {
                return NotFound();
            }

            return View(session);
        }

        // POST: Sessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "BoardMember")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var session = await _context.Sessions.FindAsync(id);
            if (session != null)
            {
                _context.Sessions.Remove(session);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SessionExists(int id)
        {
            return _context.Sessions.Any(e => e.Id == id);
        }
    }
}
