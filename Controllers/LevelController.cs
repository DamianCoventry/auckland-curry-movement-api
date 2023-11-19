using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using auckland_curry_movement_api;
using auckland_curry_movement_api.Models;

namespace auckland_curry_movement_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LevelController : ControllerBase
    {
        private readonly AcmDatabaseContext _context;

        public LevelController(AcmDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Level
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Level>>> GetLevel()
        {
            if (_context.Level == null)
            {
                return NotFound();
            }
            return await _context.Level.ToListAsync();
        }

        // GET: api/Level/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Level>> GetLevel(int? id)
        {
            if (_context.Level == null)
            {
                return NotFound();
            }

            var level = await _context.Level
                .Include(x => x.Attendees)
                .Include(x => x.Members)
                .Include(x => x.Notifications)
                .Where(x => x.ID == id)
                .FirstOrDefaultAsync();
            if (level == null)
            {
                return NotFound();
            }

            return level;
        }

        // PUT: api/Level/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLevel(int? id, Level level)
        {
            if (id != level.ID)
            {
                return BadRequest();
            }

            _context.Entry(level).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LevelExists(id))
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

        // POST: api/Level
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Level>> PostLevel(Level level)
        {
            if (_context.Level == null)
            {
                return Problem("Entity set 'AcmDatabaseContext.Level'  is null.");
            }
            _context.Level.Add(level);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLevel", new { id = level.ID }, level);
        }

        // DELETE: api/Level/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLevel(int? id)
        {
            if (_context.Level == null)
            {
                return NotFound();
            }
            var level = await _context.Level.FindAsync(id);
            if (level == null)
            {
                return NotFound();
            }

            _context.Level.Remove(level);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LevelExists(int? id)
        {
            return (_context.Level?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
