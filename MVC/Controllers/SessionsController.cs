using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using Core.Data;
using Core.Models.Sessions;
using Core.Models.Users;
using System.Security.Claims;

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
            if (id == null || _context.Sessions == null)
            {
                return NotFound();
            }

            var session = await _context.Sessions
                .Include(s => s.CreatedBy)
                .Include(s => s.Location)
                .Include(s => s.SessionRegistrations)
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
            Session session = new Session();
            ViewData["LocationId"] = new SelectList(_context.Locations, "Id", "Name");
            return View(session);
        }

        // POST: Sessions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "BoardMember")]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,ActivityDate,EndDate,LocationId,Host,MaxParticipants,ImageUrl,IsEarlyAccess")] Session session, IFormFile ?imageFile)
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

                if (imageFile != null && imageFile.Length > 0)
                {
                    // Validatie en opslag van de afbeelding
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var extension = Path.GetExtension(imageFile.FileName).ToLower();
                    if (!allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("ImageUrl", "Only JPG, JPEG, PNG, and GIF files are allowed.");
                        return View(session);
                    }

                    var fileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
                    var uniqueFileName = $"{fileName}_{Guid.NewGuid()}{extension}";
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "sessions", uniqueFileName);

                    Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    session.ImageUrl = $"/images/sessions/{uniqueFileName}";
                }

                session.CreatedById = user.Id;
                session.CreatedAt = DateTime.Now;
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,ActivityDate,EndDate,LocationId,Host,MaxParticipants,ImageUrl,IsEarlyAccess")] Session session, IFormFile ?imageFile)
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

                    if (imageFile != null && imageFile.Length > 0)
                    {
                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                        var extension = Path.GetExtension(imageFile.FileName).ToLower();
                        if (!allowedExtensions.Contains(extension))
                        {
                            ModelState.AddModelError("ImageUrl", "Only JPG, JPEG, PNG, and GIF files are allowed.");
                            return View(session);
                        }

                        var fileName = Path.GetFileNameWithoutExtension(imageFile.FileName);
                        var uniqueFileName = $"{fileName}_{Guid.NewGuid()}{extension}";
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "sessions", uniqueFileName);

                        Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }

                        if (!string.IsNullOrEmpty(existingSession.ImageUrl))
                        {
                            var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingSession.ImageUrl.TrimStart('/'));
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }

                        existingSession.ImageUrl = $"/images/sessions/{uniqueFileName}";
                    }

                    // Update de velden
                    existingSession.Title = session.Title;
                    existingSession.Description = session.Description;
                    existingSession.ActivityDate = session.ActivityDate;
                    existingSession.EndDate = session.EndDate;
                    existingSession.LocationId = session.LocationId;
                    existingSession.Host = session.Host;
                    existingSession.MaxParticipants = session.MaxParticipants;
                    existingSession.IsEarlyAccess = existingSession.IsEarlyAccess;

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

        // POST: Sessions/Register/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(int id)
        {
            var session = await _context.Sessions.FindAsync(id);
            if (session == null)
            {
                return NotFound();
            }

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString == null)
            {
                return Unauthorized();
            }

            int userId = int.Parse(userIdString);

            // Haal de gebruiker op inclusief zijn SessionRegistrations
            var user = await _userManager.Users
                .Include(u => u.SessionRegistrations)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return Unauthorized();
            }

            // Controleer of de gebruiker al is geregistreerd
            if (user.SessionRegistrations.Any(r => r.SessionId == id))
            {
                // Gebruiker is al geregistreerd
                return RedirectToAction(nameof(Details), new { id = id });
            }

            var registration = new SessionRegistration
            {
                SessionId = id,
                UserId = userId,
                RegistrationDate = DateTime.Now,
                Session = session,
                User = user
            };

            // Voeg de registratie toe aan de gebruiker
            user.SessionRegistrations.Add(registration);

            // Voeg de registratie toe aan de context
            _context.SessionRegistrations.Add(registration);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = id });
        }


        // POST: Sessions/Unregister/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Unregister(int id)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdString == null)
            {
                return Unauthorized();
            }

            int userId = int.Parse(userIdString);

            // Haal de gebruiker op inclusief zijn SessionRegistrations
            var user = await _userManager.Users
                .Include(u => u.SessionRegistrations)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return Unauthorized();
            }

            // Vind de registratie die moet worden verwijderd
            var registration = user.SessionRegistrations.FirstOrDefault(r => r.SessionId == id);
            if (registration == null)
            {
                // Gebruiker is niet geregistreerd
                return RedirectToAction(nameof(Details), new { id = id });
            }

            // Verwijder de registratie uit de gebruiker
            user.SessionRegistrations.Remove(registration);

            // Verwijder de registratie uit de context
            _context.SessionRegistrations.Remove(registration);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = id });
        }
    }
}
