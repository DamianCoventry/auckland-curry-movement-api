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
    public class ViolationController : ControllerBase
    {
        private readonly AcmDatabaseContext _context;

        public ViolationController(AcmDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Violation
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Violation>>> GetViolation([FromQuery(Name = "first")] int first, [FromQuery(Name = "count")] int count)
        {
            if (_context.Violation == null)
            {
                return NotFound();
            }
            return await _context.Violation
                .OrderBy(x => x.ID).Skip(first).Take(count)
                .ToListAsync();
        }

        // GET: api/Violation/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Violation>> GetViolation(int? id)
        {
            if (_context.Violation == null)
            {
                return NotFound();
            }

            var violation = await _context.Violation
                .Include(x => x.Dinner)
                .Include(x => x.FoundingFather)
                .Include(x => x.Member)
                .Include(x => x.Notifications)
                .Where(x => x.ID == id)
                .FirstOrDefaultAsync();
            if (violation == null)
            {
                return NotFound();
            }

            return violation;
        }

        // PUT: api/Violation/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutViolation(int? id, Violation violation)
        {
            if (id != violation.ID)
            {
                return BadRequest();
            }

            _context.Entry(violation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ViolationExists(id))
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

        // POST: api/Violation
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Violation>> PostViolation(Violation violation)
        {
            if (_context.Violation == null)
            {
                return Problem("Entity set 'AcmDatabaseContext.Violation'  is null.");
            }
            _context.Violation.Add(violation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetViolation", new { id = violation.ID }, violation);
        }

        // DELETE: api/Violation/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteViolation(int? id)
        {
            if (_context.Violation == null)
            {
                return NotFound();
            }
            var violation = await _context.Violation.FindAsync(id);
            if (violation == null)
            {
                return NotFound();
            }

            _context.Violation.Remove(violation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ViolationExists(int? id)
        {
            return (_context.Violation?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
