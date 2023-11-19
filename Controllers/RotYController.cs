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
    public class RotYController : ControllerBase
    {
        private readonly AcmDatabaseContext _context;

        public RotYController(AcmDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/RotY
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RotY>>> GetRotY()
        {
            if (_context.RotY == null)
            {
                return NotFound();
            }
            return await _context.RotY.ToListAsync();
        }

        // GET: api/RotY/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RotY>> GetRotY(int? id)
        {
            if (_context.RotY == null)
            {
                return NotFound();
            }

            var rotY = await _context.RotY
                .Include(x => x.Restaurant)
                .Include(x => x.Presenter)
                .Include(x => x.Notifications)
                .Where(x => x.Year == id)
                .FirstOrDefaultAsync();
            if (rotY == null)
            {
                return NotFound();
            }

            return rotY;
        }

        // PUT: api/RotY/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRotY(int? id, RotY rotY)
        {
            if (id != rotY.Year)
            {
                return BadRequest();
            }

            _context.Entry(rotY).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RotYExists(id))
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

        // POST: api/RotY
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RotY>> PostRotY(RotY rotY)
        {
            if (_context.RotY == null)
            {
                return Problem("Entity set 'AcmDatabaseContext.RotY'  is null.");
            }
            _context.RotY.Add(rotY);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRotY", new { id = rotY.Year }, rotY);
        }

        // DELETE: api/RotY/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRotY(int? id)
        {
            if (_context.RotY == null)
            {
                return NotFound();
            }
            var rotY = await _context.RotY.FindAsync(id);
            if (rotY == null)
            {
                return NotFound();
            }

            _context.RotY.Remove(rotY);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RotYExists(int? id)
        {
            return (_context.RotY?.Any(e => e.Year == id)).GetValueOrDefault();
        }
    }
}
