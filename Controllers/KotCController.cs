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
    public class KotCController : ControllerBase
    {
        private readonly AcmDatabaseContext _context;

        public KotCController(AcmDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/KotC
        [HttpGet]
        public async Task<ActionResult<IEnumerable<KotC>>> GetKotC([FromQuery(Name = "first")] int first, [FromQuery(Name = "count")] int count)
        {
            if (_context.KotC == null)
            {
                return NotFound();
            }
            return await _context.KotC
                .OrderBy(x => x.ID).Skip(first).Take(count)
                .ToListAsync();
        }

        // GET: api/KotC/5
        [HttpGet("{id}")]
        public async Task<ActionResult<KotC>> GetKotC(int? id)
        {
            if (_context.KotC == null)
            {
                return NotFound();
            }

            var kotC = await _context.KotC
                .Include(x => x.Member)
                .Include(x => x.Dinner)
                .Include(x => x.Notifications)
                .Where(x => x.ID == id)
                .FirstOrDefaultAsync();
            if (kotC == null)
            {
                return NotFound();
            }

            return kotC;
        }

        // PUT: api/KotC/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutKotC(int? id, KotC kotC)
        {
            if (id != kotC.ID)
            {
                return BadRequest();
            }

            _context.Entry(kotC).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!KotCExists(id))
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

        // POST: api/KotC
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<KotC>> PostKotC(KotC kotC)
        {
            if (_context.KotC == null)
            {
                return Problem("Entity set 'AcmDatabaseContext.KotC'  is null.");
            }
            _context.KotC.Add(kotC);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetKotC", new { id = kotC.ID }, kotC);
        }

        // DELETE: api/KotC/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKotC(int? id)
        {
            if (_context.KotC == null)
            {
                return NotFound();
            }
            var kotC = await _context.KotC.FindAsync(id);
            if (kotC == null)
            {
                return NotFound();
            }

            _context.KotC.Remove(kotC);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool KotCExists(int? id)
        {
            return (_context.KotC?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
