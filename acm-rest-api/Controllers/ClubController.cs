using acm_models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace acm_rest_api.Controllers
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
        public async Task<ActionResult<PageOfData<Club>>> GetClub([FromQuery(Name = "first")] int first, [FromQuery(Name = "count")] int pageSize)
        {
            if (_context.Club == null || pageSize <= 0)
            {
                return NotFound();
            }

            int rowCount = await _context.Club.CountAsync();
            int totalPages = rowCount / pageSize;
            if (rowCount % pageSize > 0)
                ++totalPages;

            return new PageOfData<Club>()
            {
                CurrentPage = first,
                TotalPages = totalPages,
                PageItems = await _context.Club
                    .Include(x => x.Members)
                    .OrderBy(x => x.ID)
                    .Skip(first).Take(pageSize)
                    .ToListAsync()
            };
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
        public async Task<ActionResult<PageOfData<PastDinner>>> GetClubPastDinners(int? id, [FromQuery(Name = "first")] int first, [FromQuery(Name = "count")] int pageSize)
        {
            if (_context.Club == null || _context.Dinner == null || id == null || pageSize <= 0)
            {
                return NotFound();
            }

            var club = await _context.Club.Where(x => x.ID == id).FirstOrDefaultAsync();
            if (club == null)
            {
                return NotFound();
            }

            string sql =
                @"SELECT d.ID, m.ID AS OrganiserID, m.Name AS OrganiserName,
                        rs.ID AS RestaurantID, rs.Name AS RestaurantName,
                        rv.ExactDateTime, rv.NegotiatedBeerPrice, rv.NegotiatedBeerDiscount,
                        d.CostPerPerson, d.NumBeersConsumed,
                        (SELECT IIF(COUNT(*) > 0, 1, 0) FROM KotC k WHERE k.DinnerID = d.ID) AS IsNewKotC,
                        (SELECT IIF(COUNT(*) > 0, 1, 0) FROM RotY r1 WHERE r1.RestaurantID = rs.ID AND r1.Year < YEAR(GETDATE())) AS IsFormerRotY,
                        (SELECT IIF(COUNT(*) > 0, 1, 0) FROM RotY r2 WHERE r2.RestaurantID = rs.ID AND r2.Year = YEAR(GETDATE())) AS IsCurrentRotY,
                        (SELECT IIF(COUNT(*) > 0, 1, 0) FROM Violation v WHERE v.DinnerID = d.ID) AS IsRulesViolation
                FROM [dbo].[Dinner] d
                INNER JOIN [dbo].[Reservation] rv ON rv.ID = d.ReservationID
                INNER JOIN [dbo].[Restaurant] rs ON rs.ID = rv.RestaurantID
                INNER JOIN [dbo].[Member] m ON m.ID = rv.OrganiserID
                INNER JOIN [dbo].[Membership] mc ON mc.MemberID = m.ID AND mc.ClubID = " + id.ToString();

            int rowCount = await _context.Set<PastDinner>().FromSqlRaw(sql).CountAsync();
            int totalPages = rowCount / pageSize;
            if (rowCount % pageSize > 0)
                ++totalPages;

            return new PageOfData<PastDinner>()
            {
                CurrentPage = first,
                TotalPages = totalPages,
                PageItems = await _context
                                        .Set<PastDinner>()
                                        .FromSqlRaw(sql)
                                        .OrderBy(x => x.ID).Skip(first).Take(pageSize)
                                        .ToListAsync()
            };
        }

        // GET: api/Club/43/FoundingFathers
        [HttpGet("{id}/FoundingFathers")]
        public async Task<ActionResult<PageOfData<Member>>> GetClubFoundingFathers(int? id, [FromQuery(Name = "first")] int first, [FromQuery(Name = "count")] int pageSize)
        {
            if (_context.Club == null || _context.Member == null || _context.Membership == null || id == null || pageSize <= 0)
            {
                return NotFound();
            }

            var club = await _context.Club.Where(x => x.ID == id).FirstOrDefaultAsync();
            if (club == null)
            {
                return NotFound();
            }

            int rowCount = await _context.Membership.Where(x => x.ClubID == id && x.IsFoundingFather).CountAsync();
            int totalPages = rowCount / pageSize;
            if (rowCount % pageSize > 0)
                ++totalPages;

            return new PageOfData<Member>()
            {
                CurrentPage = first,
                TotalPages = totalPages,
                PageItems = await _context
                                    .Set<Member>()
                                    .FromSqlRaw(
                                        "SELECT [m].[ID], [m].[Name], [m].[SponsorID], [m].[CurrentLevelID], [m].[AttendanceCount], [m].[IsArchived], [m].[ArchiveReason] " +
                                        "FROM [dbo].[Member] m " +
                                        "INNER JOIN Membership mc ON [mc].[MemberID] = [m].[ID] AND [mc].[IsFoundingFather] <> 0 " +
                                       $"AND [mc].[ClubID] = {id}")
                                    .OrderBy(x => x.ID).Skip(first).Take(pageSize)
                                    .ToListAsync()
            };
        }

        // GET: api/Club/65/Members
        [HttpGet("{id}/Members")]
        public async Task<ActionResult<PageOfData<Member>>> GetClubMembers(int? id, [FromQuery(Name = "first")] int first, [FromQuery(Name = "count")] int pageSize)
        {
            if (_context.Club == null || _context.Member == null || _context.Membership == null || pageSize <= 0)
            {
                return NotFound();
            }

            var club = await _context.Club.Where(x => x.ID == id).FirstOrDefaultAsync();
            if (club == null)
            {
                return NotFound();
            }

            int rowCount = await _context.Membership.Where(x => x.ClubID == id).CountAsync();
            int totalPages = rowCount / pageSize;
            if (rowCount % pageSize > 0)
                ++totalPages;

            return new PageOfData<Member>()
            {
                CurrentPage = first,
                TotalPages = totalPages,
                PageItems = await _context
                                    .Set<Member>()
                                    .FromSqlRaw(
                                        "SELECT [m].[ID], [m].[Name], [m].[SponsorID], [m].[CurrentLevelID], [m].[AttendanceCount], [m].[IsArchived], [m].[ArchiveReason] " +
                                        "FROM [dbo].[Member] m " +
                                        "INNER JOIN Membership mc ON [mc].[MemberID] = [m].[ID] " +
                                       $"AND [mc].[ClubID] = {id}")
                                    .OrderBy(x => x.ID).Skip(first).Take(pageSize)
                                    .ToListAsync()
            };
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

            var foundingFathers = club.Members;
            club.Members = null;
            club.Notifications = null;

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

            if (_context.Membership != null && foundingFathers != null && id != null)
            {
                var memberships = await _context.Membership.Where(x => x.ClubID == id).ToListAsync();
                foreach (var membership in memberships)
                {
                    membership.IsFoundingFather = foundingFathers.Where(x => x.ID == membership.MemberID).Any();
                    _context.Entry(membership).State = EntityState.Modified;
                }

                await _context.SaveChangesAsync();
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
                return Problem("Entity set 'AcmDatabaseContext.Club' is null.");
            }

            var foundingFathers = club.Members;
            club.ID = null;
            club.Members = null;
            club.Notifications = null;
            _context.Club.Add(club);

            await _context.SaveChangesAsync(); // if this succeeds then club.ID becomes valid to use.

            if (foundingFathers != null && club.ID != null && _context.Membership != null)
            {
                foreach (var ff in foundingFathers)
                {
                    if (ff.ID != null)
                        _context.Membership.Add(new Membership() { ClubID = (int)club.ID, MemberID = (int)ff.ID, IsFoundingFather = true });
                }

                await _context.SaveChangesAsync();
            }

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

            if (_context.Membership != null)
            {
                var memberships = await _context.Membership.Where(x => x.ClubID == id).ToListAsync();
                foreach (var membership in memberships)
                    _context.Membership.Remove(membership);
                await _context.SaveChangesAsync();
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
