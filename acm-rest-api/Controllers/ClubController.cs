using System.Linq;
using acm_rest_api.Models;
using auckland_curry_movement_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace auckland_curry_movement_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClubController : ControllerBase
    {
        private readonly AcmDatabaseContext _context;

        public ClubController(AcmDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Club
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Club>>> GetClub([FromQuery(Name = "first")] int first, [FromQuery(Name = "count")] int count)
        {
            if (_context.Club == null)
            {
                return NotFound();
            }
            return await _context.Club
                .Include(x => x.Members)
                .OrderBy(x => x.ID).Skip(first).Take(count)
                .ToListAsync();
        }

        // GET: api/Club/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Club>> GetClub(int? id)
        {
            if (_context.Club == null)
            {
                return NotFound();
            }

            var club = await _context.Club
                .Include(x => x.Members)
                .Include(x => x.Notifications)
                .Where(x => x.ID == id)
                .FirstOrDefaultAsync();
            if (club == null)
            {
                return NotFound();
            }

            return club;
        }

        // GET: api/Club/5/PastDinners
        [HttpGet("{id}/PastDinners")]
        public async Task<ActionResult<IEnumerable<PastDinner>>> GetClubPastDinners(int? id, [FromQuery(Name = "first")] int first, [FromQuery(Name = "count")] int count)
        {
            if (_context.Club == null || _context.Dinner == null)
            {
                return NotFound();
            }

            var club = await _context.Club.Where(x => x.ID == id).FirstOrDefaultAsync();
            if (club == null)
            {
                return NotFound();
            }

            return await _context
                .Set<PastDinner>()
                .FromSqlRaw(
                    "SELECT d.ID, m.ID AS OrganiserID, m.Name AS OrganiserName, " +
                    "       rs.ID AS RestaurantID, rs.Name AS RestaurantName, " +
                    "       rv.ExactDateTime, rv.NegotiatedBeerPrice, rv.NegotiatedBeerDiscount, " +
                    "       d.CostPerPerson, d.NumBeersConsumed, " +
                    "       (SELECT IIF(COUNT(*) > 0, 1, 0) FROM KotC k WHERE k.DinnerID = d.ID) AS IsNewKotC, " +
                    "       (SELECT IIF(COUNT(*) > 0, 1, 0) FROM RotY r1 WHERE r1.RestaurantID = rs.ID AND r1.Year < YEAR(GETDATE())) AS IsFormerRotY, " +
                    "       (SELECT IIF(COUNT(*) > 0, 1, 0) FROM RotY r2 WHERE r2.RestaurantID = rs.ID AND r2.Year = YEAR(GETDATE())) AS IsCurrentRotY, " +
                    "       (SELECT IIF(COUNT(*) > 0, 1, 0) FROM Violation v WHERE v.DinnerID = d.ID) AS IsRulesViolation " +
                    "FROM Dinner d " +
                    "INNER JOIN Reservation rv ON rv.ID = d.ReservationID " +
                    "INNER JOIN Restaurant rs ON rs.ID = rv.RestaurantID " +
                    "INNER JOIN Member m ON m.ID = rv.OrganiserID " +
                    $"INNER JOIN MemberClub mc ON mc.MemberID = m.ID AND mc.ClubID = {id}")
                .OrderBy(x => x.ID).Skip(first).Take(count)
                .ToListAsync();
        }

        // PUT: api/Club/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClub(int? id, Club club)
        {
            if (id != club.ID)
            {
                return BadRequest();
            }

            _context.Entry(club).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClubExists(id))
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

        // POST: api/Club
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Club>> PostClub(Club club)
        {
            if (_context.Club == null)
            {
                return Problem("Entity set 'AcmDatabaseContext.Club'  is null.");
            }
            _context.Club.Add(club);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClub", new { id = club.ID }, club);
        }

        // DELETE: api/Club/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClub(int? id)
        {
            if (_context.Club == null)
            {
                return NotFound();
            }
            var club = await _context.Club.FindAsync(id);
            if (club == null)
            {
                return NotFound();
            }

            _context.Club.Remove(club);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClubExists(int? id)
        {
            return (_context.Club?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
