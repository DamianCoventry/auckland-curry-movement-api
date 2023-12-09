using acm_models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;

namespace acm_rest_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendeeController : ControllerBase
    {
        private readonly AcmDatabaseContext _context;

        public AttendeeController(AcmDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Attendee
        [HttpGet]
        public async Task<ActionResult<PageOfData<Attendee>>> GetAttendee([FromQuery(Name = "first")] int first, [FromQuery(Name = "count")] int pageSize)
        {
            if (_context.Attendee == null || pageSize <= 0)
            {
                return NotFound();
            }

            int rowCount = await _context.Attendee.CountAsync();
            int totalPages = rowCount / pageSize;
            if (rowCount % pageSize > 0)
                ++totalPages;

            return new PageOfData<Attendee>()
            {
                CurrentPage = first,
                TotalPages = totalPages,
                PageItems = await _context.Attendee
                .OrderBy(x => x.ID)
                    .Skip(first).Take(pageSize)
                    .ToListAsync()
            };
        }

        // GET: api/Attendee/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Attendee>> GetAttendee(int? id)
        {
            if (_context.Attendee == null)
            {
                return NotFound();
            }

            var attendee = await _context.Attendee
                .Include(x => x.Dinner)
                .Include(x => x.Member)
                .Include(x => x.Level)
                .Include(x => x.Notifications)
                .Where(x => x.ID == id)
                .FirstOrDefaultAsync();
            if (attendee == null)
            {
                return NotFound();
            }

            return attendee;
        }

        // PUT: api/Attendee/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAttendee(int? id, Attendee attendee)
        {
            if (id != attendee.ID)
            {
                return BadRequest();
            }

            _context.Entry(attendee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AttendeeExists(id))
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

        // POST: api/Attendee
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Attendee>> PostAttendee(Attendee attendee)
        {
            if (_context.Attendee == null)
            {
                return Problem("Entity set 'AcmDatabaseContext.Attendee' is null.");
            }
            _context.Attendee.Add(attendee);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAttendee", new { id = attendee.ID }, attendee);
        }

        // DELETE: api/Attendee/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAttendee(int? id)
        {
            if (_context.Attendee == null)
            {
                return NotFound();
            }
            var attendee = await _context.Attendee.FindAsync(id);
            if (attendee == null)
            {
                return NotFound();
            }

            _context.Attendee.Remove(attendee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AttendeeExists(int? id)
        {
            return (_context.Attendee?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
