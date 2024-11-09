using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Core.Data;
using Core.Models.Sessions;

namespace API.Controllers.Sessions
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionRegistrationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;


        public SessionRegistrationController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/SessionRegistration
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SessionRegistration>>> GetSessionRegistrations()
        {
            return await _context.SessionRegistrations
                                 .Include(sr => sr.Session)
                                 .Include(sr => sr.User)
                                 .ToListAsync();
        }

        // GET: api/SessionRegistration/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SessionRegistration>> GetSessionRegistration(int id)
        {
            var sessionRegistration = await _context.SessionRegistrations
                                                    .Include(sr => sr.Session)
                                                    .Include(sr => sr.User)
                                                    .FirstOrDefaultAsync(sr => sr.Id == id);

            if (sessionRegistration == null)
            {
                return NotFound();
            }

            return sessionRegistration;
        }

        // POST: api/SessionRegistration
        [HttpPost]
        public async Task<ActionResult<SessionRegistration>> PostSessionRegistration(SessionRegistration sessionRegistration)
        {
            sessionRegistration.RegistrationDate = DateTime.UtcNow;  // Stel de registratie datum in op de huidige tijd
            _context.SessionRegistrations.Add(sessionRegistration);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSessionRegistration), new { id = sessionRegistration.Id }, sessionRegistration);
        }

        // PUT: api/SessionRegistration/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSessionRegistration(int id, SessionRegistration sessionRegistration)
        {
            if (id != sessionRegistration.Id)
            {
                return BadRequest();
            }

            _context.Entry(sessionRegistration).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SessionRegistrationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/SessionRegistration/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSessionRegistration(int id)
        {
            var sessionRegistration = await _context.SessionRegistrations.FindAsync(id);
            if (sessionRegistration == null)
            {
                return NotFound();
            }

            _context.SessionRegistrations.Remove(sessionRegistration);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SessionRegistrationExists(int id)
        {
            return _context.SessionRegistrations.Any(e => e.Id == id);
        }
    }
}
