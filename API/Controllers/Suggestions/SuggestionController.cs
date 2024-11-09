using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Models.Suggestions;

using Core.Data;

namespace API.Controllers.Suggestions
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuggestionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SuggestionController(ApplicationDbContext context)
        {
            ApplicationDbContextFactory factory = new ApplicationDbContextFactory();
            _context = factory.CreateDbContext([]);
        }

        // GET: api/Suggestion
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Suggestion>>> GetSuggestions()
        {
            return await _context.Suggestions.ToListAsync();
        }

        // GET: api/Suggestion/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Suggestion>> GetSuggestion(int id)
        {
            var suggestion = await _context.Suggestions
                                           .Include(s => s.CreatedBy)
                                           .Include(s => s.SuggestionLikes)
                                           .FirstOrDefaultAsync(s => s.Id == id);

            if (suggestion == null)
            {
                return NotFound();
            }

            return suggestion;
        }

        // POST: api/Suggestion
        [HttpPost]
        public async Task<ActionResult<Suggestion>> PostSuggestion(Suggestion suggestion)
        {
            suggestion.SubmittedOn = DateTime.UtcNow;  // Stel de datum in wanneer de suggestie wordt ingediend
            _context.Suggestions.Add(suggestion);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSuggestion), new { id = suggestion.Id }, suggestion);
        }

        // PUT: api/Suggestion/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSuggestion(int id, Suggestion suggestion)
        {
            if (id != suggestion.Id)
            {
                return BadRequest();
            }

            _context.Entry(suggestion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SuggestionExists(id))
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

        // DELETE: api/Suggestion/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSuggestion(int id)
        {
            var suggestion = await _context.Suggestions.FindAsync(id);
            if (suggestion == null)
            {
                return NotFound();
            }

            _context.Suggestions.Remove(suggestion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SuggestionExists(int id)
        {
            return _context.Suggestions.Any(e => e.Id == id);
        }
    }
}

