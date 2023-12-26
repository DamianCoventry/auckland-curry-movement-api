using acm_models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
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

        // GET: api/Dinner/5/Attendees
        [HttpGet("{id}/Attendees")]
        public async Task<ActionResult<IEnumerable<AttendeeStats>>> GetDinnerAttendeeStats(int? id, [FromQuery(Name = "clubID")] int clubID)
        {
            if (_context.Attendee == null || _context.Membership == null || _context.Exemption == null || _context.Violation == null || id == null)
            {
                return NotFound();
            }

            List<Attendee> attendees = await _context.Attendee
                .Include(x => x.Dinner)
                .Include(x => x.Member)
                .Include(x => x.Level)
                .Where(x => x.DinnerID == id)
                .ToListAsync();

            List<AttendeeStats> attendeeStats = new();
            foreach (var a in attendees)
            {
                AttendeeStats att = new() { Attendee = a };

                int count = await _context.Attendee.Where(x => x.MemberID == a.MemberID && x.DinnerID <= id).CountAsync();
                att.NthAttendance = (count % 10) switch
                {
                    1 => count.ToString() + "st",
                    2 => count.ToString() + "nd",
                    3 => count.ToString() + "rd",
                    _ => count.ToString() + "th",
                };

                att.IsFoundingFather = await _context.Membership.Where(x => x.ClubID == clubID && x.MemberID == a.MemberID && x.IsFoundingFather).AnyAsync();

                att.IsExemptionUsed = await _context.Exemption.Where(x => x.MemberID == a.MemberID).AnyAsync();

                att.IsReceivedViolation = await _context.Violation.Where(x => x.MemberID == a.MemberID && x.DinnerID == id).AnyAsync();

                attendeeStats.Add(att);
            }

            return attendeeStats;
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
