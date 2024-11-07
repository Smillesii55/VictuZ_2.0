using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Core.Data;

using Core.Models.Sessions;

namespace API.Controllers.Sessions
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;  // Vervang met de naam van jouw DbContext

        public SessionController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Session
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Session>>> GetSessions()
        {
            return await _context.Sessions
                                 .Include(s => s.CreatedBy)
                                 .Include(s => s.Location)
                                 .Include(s => s.SessionRegistrations)
                                 .Include(s => s.Feedbacks)
                                 .ToListAsync();
        }

        // GET: api/Session/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Session>> GetSession(int id)
        {
            var session = await _context.Sessions
                                        .Include(s => s.CreatedBy)
                                        .Include(s => s.Location)
                                        .Include(s => s.SessionRegistrations)
                                        .Include(s => s.Feedbacks)
                                        .FirstOrDefaultAsync(s => s.Id == id);

            if (session == null)
            {
                return NotFound();
            }

            return session;
        }

        // POST: api/Session
        [HttpPost]
        public async Task<ActionResult<Session>> PostSession(Session session)
        {
            _context.Sessions.Add(session);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSession), new { id = session.Id }, session);
        }

        // PUT: api/Session/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSession(int id, Session session)
        {
            if (id != session.Id)
            {
                return BadRequest();
            }

            _context.Entry(session).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SessionExists(id))
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

        // DELETE: api/Session/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSession(int id)
        {
            var session = await _context.Sessions.FindAsync(id);
            if (session == null)
            {
                return NotFound();
            }

            _context.Sessions.Remove(session);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SessionExists(int id)
        {
            return _context.Sessions.Any(e => e.Id == id);
        }
    }
}
