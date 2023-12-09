using acm_models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;

namespace acm_rest_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DinnerController : ControllerBase
    {
        private readonly AcmDatabaseContext _context;

        public DinnerController(AcmDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Dinner
        [HttpGet]
        public async Task<ActionResult<PageOfData<Dinner>>> GetDinner([FromQuery(Name = "first")] int first, [FromQuery(Name = "count")] int pageSize)
        {
            if (_context.Dinner == null || pageSize <= 0)
            {
                return NotFound();
            }

            int rowCount = await _context.Dinner.CountAsync();
            int totalPages = rowCount / pageSize;
            if (rowCount % pageSize > 0)
                ++totalPages;

            return new PageOfData<Dinner>()
            {
                CurrentPage = first,
                TotalPages = totalPages,
                PageItems = await _context.Dinner
                    .OrderBy(x => x.ID)
                    .Skip(first).Take(pageSize)
                    .ToListAsync()
            };
        }

        // GET: api/Dinner/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Dinner>> GetDinner(int? id)
        {
            if (_context.Dinner == null)
            {
                return NotFound();
            }

            var dinner = await _context.Dinner
                .Include(x => x.Reservation)
                .Include(x => x.Attendees)
                .Include(x => x.Members)
                .Include(x => x.KotC)
                .Include(x => x.Violations)
                .Include(x => x.Notifications)
                .Where(x => x.ID == id)
                .FirstOrDefaultAsync();
            if (dinner == null)
            {
                return NotFound();
            }

            return dinner;
        }

        // PUT: api/Dinner/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDinner(int? id, Dinner dinner)
        {
            if (id != dinner.ID)
            {
                return BadRequest();
            }

            _context.Entry(dinner).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DinnerExists(id))
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

        // POST: api/Dinner
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Dinner>> PostDinner(Dinner dinner)
        {
            if (_context.Dinner == null)
            {
                return Problem("Entity set 'AcmDatabaseContext.Dinner' is null.");
            }
            _context.Dinner.Add(dinner);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDinner", new { id = dinner.ID }, dinner);
        }

        // DELETE: api/Dinner/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDinner(int? id)
        {
            if (_context.Dinner == null)
            {
                return NotFound();
            }
            var dinner = await _context.Dinner.FindAsync(id);
            if (dinner == null)
            {
                return NotFound();
            }

            _context.Dinner.Remove(dinner);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DinnerExists(int? id)
        {
            return (_context.Dinner?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
