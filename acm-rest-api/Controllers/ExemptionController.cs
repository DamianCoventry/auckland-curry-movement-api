using acm_models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace acm_rest_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExemptionController : ControllerBase
    {
        private readonly AcmDatabaseContext _context;

        public ExemptionController(AcmDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Exemption
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Exemption>>> GetExemption([FromQuery(Name = "first")] int first, [FromQuery(Name = "count")] int count)
        {
            if (_context.Exemption == null)
            {
                return NotFound();
            }
            return await _context.Exemption
                .OrderBy(x => x.ID).Skip(first).Take(count)
                .ToListAsync();
        }

        // GET: api/Exemption/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Exemption>> GetExemption(int? id)
        {
            if (_context.Exemption == null)
            {
                return NotFound();
            }

            var exemption = await _context.Exemption
                .Include(x => x.FoundingFather)
                .Include(x => x.Member)
                .Include(x => x.Notifications)
                .Where(x => x.ID == id)
                .FirstOrDefaultAsync();
            if (exemption == null)
            {
                return NotFound();
            }

            return exemption;
        }

        // PUT: api/Exemption/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExemption(int? id, Exemption exemption)
        {
            if (id != exemption.ID)
            {
                return BadRequest();
            }

            _context.Entry(exemption).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExemptionExists(id))
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

        // POST: api/Exemption
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Exemption>> PostExemption(Exemption exemption)
        {
            if (_context.Exemption == null)
            {
                return Problem("Entity set 'AcmDatabaseContext.Exemption' is null.");
            }
            _context.Exemption.Add(exemption);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExemption", new { id = exemption.ID }, exemption);
        }

        // DELETE: api/Exemption/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExemption(int? id)
        {
            if (_context.Exemption == null)
            {
                return NotFound();
            }
            var exemption = await _context.Exemption.FindAsync(id);
            if (exemption == null)
            {
                return NotFound();
            }

            _context.Exemption.Remove(exemption);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExemptionExists(int? id)
        {
            return (_context.Exemption?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
