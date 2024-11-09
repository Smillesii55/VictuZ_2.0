using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Models.Suggestions;
using Core.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers.Suggestions
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuggesionLikeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SuggesionLikeController(ApplicationDbContext context)
        {
            ApplicationDbContextFactory factory = new ApplicationDbContextFactory();
            _context = factory.CreateDbContext([]);
        }

        // GET: api/SuggestionLike
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SuggestionLike>>> GetSuggestionLikes()
        {
            return await _context.SuggestionLikes
                                 .Include(sl => sl.Suggestion)
                                 .Include(sl => sl.User)
                                 .ToListAsync();
        }

        // GET: api/SuggestionLike/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SuggestionLike>> GetSuggestionLike(int id)
        {
            var suggestionLike = await _context.SuggestionLikes
                                               .Include(sl => sl.Suggestion)
                                               .Include(sl => sl.User)
                                               .FirstOrDefaultAsync(sl => sl.Id == id);

            if (suggestionLike == null)
            {
                return NotFound();
            }

            return suggestionLike;
        }

        // POST: api/SuggestionLike
        [HttpPost]
        public async Task<ActionResult<SuggestionLike>> PostSuggestionLike(SuggestionLike suggestionLike)
        {
            _context.SuggestionLikes.Add(suggestionLike);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSuggestionLike), new { id = suggestionLike.Id }, suggestionLike);
        }

        // PUT: api/SuggestionLike/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSuggestionLike(int id, SuggestionLike suggestionLike)
        {
            if (id != suggestionLike.Id)
            {
                return BadRequest();
            }

            _context.Entry(suggestionLike).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SuggestionLikeExists(id))
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

        // DELETE: api/SuggestionLike/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSuggestionLike(int id)
        {
            var suggestionLike = await _context.SuggestionLikes.FindAsync(id);
            if (suggestionLike == null)
            {
                return NotFound();
            }

            _context.SuggestionLikes.Remove(suggestionLike);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SuggestionLikeExists(int id)
        {
            return _context.SuggestionLikes.Any(e => e.Id == id);
        }
    }
}
