using Core.Data;
using Core.Models.Sessions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Service
{
    public class SessionService
    {
        private readonly ApplicationDbContext _context;

        public SessionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Session?> GetSessionById(int id)
        {
            return await _context.Sessions
                .Include(s => s.CreatedBy)
                .Include(s => s.Location)
                .Include(s => s.SessionRegistrations)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Session>> GetAllSessions()
        {
            return await _context.Sessions
                .Include(s => s.CreatedBy)
                .Include(s => s.Location)
                .Include(s => s.SessionRegistrations)
                .ToListAsync();
        }
    }
}
