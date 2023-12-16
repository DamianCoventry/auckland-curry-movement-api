using acm_models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;

namespace acm_rest_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly AcmDatabaseContext _context;

        public MemberController(AcmDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Member
        [HttpGet]
        public async Task<ActionResult<PageOfData<Member>>> GetMember([FromQuery(Name = "first")] int first, [FromQuery(Name = "count")] int pageSize)
        {
            if (_context.Member == null || pageSize <= 0)
            {
                return NotFound();
            }

            int rowCount = await _context.Member.CountAsync();
            int totalPages = rowCount / pageSize;
            if (rowCount % pageSize > 0)
                ++totalPages;

            return new PageOfData<Member>()
            {
                CurrentPage = first,
                TotalPages = totalPages,
                PageItems = await _context.Member
                .OrderBy(x => x.ID)
                    .Skip(first).Take(pageSize)
                    .ToListAsync()
            };
        }

        // GET: api/Member/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Member>> GetMember(int? id)
        {
            if (_context.Member == null)
            {
                return NotFound();
            }

            var member = await _context.Member
                .Include(x => x.Sponsor)
                .Include(x => x.Clubs)
                .Include(x => x.Dinners)
                .Include(x => x.Attendees)
                .Include(x => x.CurrentLevel)
                .Include(x => x.ExemptionsGiven)
                .Include(x => x.ExemptionsReceived)
                .Include(x => x.KotCs)
                .Include(x => x.Reservations)
                .Include(x => x.RotYs)
                .Include(x => x.ViolationsGiven)
                .Include(x => x.ViolationsReceived)
                .Include(x => x.Notifications)
                .Where(x => x.ID == id)
                .FirstOrDefaultAsync();
            if (member == null)
            {
                return NotFound();
            }

            if (_context.Attendee != null)
            {
                // Update the cached attendance count if it's changed.
                int count = await _context.Attendee.CountAsync(x => x.MemberID == id);
                if (count != member.AttendanceCount)
                {
                    member.AttendanceCount = count;
                    _context.Entry(member).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }
            }

            return member;
        }

        // PUT: api/Member/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMember(int? id, Member member)
        {
            if (id != member.ID)
            {
                return BadRequest();
            }

            _context.Entry(member).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberExists(id))
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

        // POST: api/Member
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Member>> PostMember(Member member)
        {
            if (_context.Member == null)
            {
                return Problem("Entity set 'AcmDatabaseContext.Member' is null.");
            }
            _context.Member.Add(member);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMember", new { id = member.ID }, member);
        }

        // DELETE: api/Member/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(int? id)
        {
            if (_context.Member == null)
            {
                return NotFound();
            }
            var member = await _context.Member.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }

            _context.Member.Remove(member);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MemberExists(int? id)
        {
            return (_context.Member?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
