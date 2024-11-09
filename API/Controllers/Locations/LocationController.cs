using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Core.Models.Locations;
using Core.Data;

namespace API.Controllers.Locations
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;  // Vervang met de naam van jouw DbContext

        public LocationController()
        {
            ApplicationDbContextFactory factory = new ApplicationDbContextFactory();
            _context = factory.CreateDbContext([]);
        }

        // GET: api/Location
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Location>>> GetLocations()
        {
            return await _context.Locations
                                 .Include(l => l.ScheduledActivities)
                                 .ToListAsync();
        }

        // GET: api/Location/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Location>> GetLocation(int id)
        {
            var location = await _context.Locations
                                         .Include(l => l.ScheduledActivities)
                                         .FirstOrDefaultAsync(l => l.Id == id);

            if (location == null)
            {
                return NotFound();
            }

            return location;
        }

        // POST: api/Location
        [HttpPost]
        public async Task<ActionResult<Location>> PostLocation(Location location)
        {
            _context.Locations.Add(location);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLocation), new { id = location.Id }, location);
        }

        // PUT: api/Location/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLocation(int id, Location location)
        {
            if (id != location.Id)
            {
                return BadRequest();
            }

            _context.Entry(location).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LocationExists(id))
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

        // DELETE: api/Location/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            var location = await _context.Locations.FindAsync(id);
            if (location == null)
            {
                return NotFound();
            }

            _context.Locations.Remove(location);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LocationExists(int id)
        {
            return _context.Locations.Any(e => e.Id == id);
        }
    }
}
