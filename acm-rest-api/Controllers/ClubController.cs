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
                    .Include(x => x.Memberships)
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
                .Include(x => x.Memberships)
                .Include(x => x.Notifications)
                .Where(x => x.ID == id)
                .FirstOrDefaultAsync();
            if (club == null)
            {
                return NotFound();
            }

            return club;
        }

        // GET: api/Club/5/MemberStats
        [HttpGet("{id}/MemberStats")]
        public async Task<ActionResult<PageOfData<MemberStats>>> GetMemberStats(int? id, [FromQuery(Name = "first")] int first, [FromQuery(Name = "count")] int pageSize)
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
                @"SELECT m.[ID]
                         ,m.[Name]
                         ,ms3.[LevelID]
                         ,(SELECT COUNT(*) FROM [dbo].[Membership] ms1 WHERE ms1.[MemberID] = m.ID) AS MembershipCount
                         ,(SELECT COUNT(*) FROM [dbo].[Membership] ms2 WHERE ms2.[MemberID] = m.ID AND ms2.ClubID = " + id.ToString() + @" AND ms2.IsFoundingFather <> 0) AS IsFoundingFather
                         ,(SELECT COUNT(*) FROM [dbo].[Attendee] a WHERE a.[MemberID] = m.ID) AS DinnersAttendedCount
                         ,(SELECT COUNT(*) FROM [dbo].[Exemption] e1 WHERE e1.[FoundingFatherID] = m.ID) AS ExemptionsAwardedCount
                         ,(SELECT COUNT(*) FROM [dbo].[Exemption] e2 WHERE e2.[MemberID] = m.ID) AS ExemptionsReceivedCount
                         ,(SELECT COUNT(*) FROM [dbo].[KotC] k WHERE k.[MemberID] = m.ID) AS KotCCount
                         ,(SELECT COUNT(*) FROM [dbo].[Reservation] re WHERE re.[OrganiserID] = m.ID) AS ReservationOrganiserCount
                         ,(SELECT COUNT(*) FROM [dbo].[RotY] r WHERE r.[PresenterID] = m.ID) AS RotYPresenterCount
                         ,(SELECT COUNT(*) FROM [dbo].[Violation] v1 WHERE v1.[FoundingFatherID] = m.ID) AS ViolationsAwardedCount
                         ,(SELECT COUNT(*) FROM [dbo].[Violation] v2 WHERE v2.[MemberID] = m.ID) AS ViolationsReceivedCount
                  FROM [dbo].[Member] m
                  INNER JOIN [dbo].[Membership] ms3 ON ms3.MemberID = m.ID AND ms3.ClubID = " + id.ToString();

            int rowCount = await _context.Set<MemberStats>().FromSqlRaw(sql).CountAsync();
            int totalPages = rowCount / pageSize;
            if (rowCount % pageSize > 0)
                ++totalPages;

            return new PageOfData<MemberStats>()
            {
                CurrentPage = first,
                TotalPages = totalPages,
                PageItems = await _context
                                        .Set<MemberStats>()
                                        .FromSqlRaw(sql)
                                        .OrderBy(x => x.ID).Skip(first).Take(pageSize)
                                        .ToListAsync()
            };
        }

        // GET: api/Club/5/Meals
        [HttpGet("{id}/Meals")]
        public async Task<ActionResult<PageOfData<Meal>>> GetClubMeals(int? id, [FromQuery(Name = "first")] int first, [FromQuery(Name = "count")] int pageSize)
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
                "SELECT r.[id] ReservationID,r.[OrganiserID],m.[Name] OrganiserName,r.[RestaurantID]," +
                "rst.[Name] RestaurantName,r.[Year],r.[Month],r.[ExactDateTime],r.[NegotiatedBeerPrice]," +
                "r.[NegotiatedBeerDiscount],r.[IsAmnesty],d.[ID] DinnerID,d.[CostPerPerson],d.[NumBeersConsumed] " +
                "FROM [dbo].[Reservation] r " +
                "LEFT OUTER JOIN [dbo].[Dinner] d ON d.[ReservationID] = r.[ID] " +
                "INNER JOIN [dbo].[Member] m ON m.ID = r.[OrganiserID] " +
                "INNER JOIN [dbo].[Restaurant] rst ON rst.ID = r.[RestaurantID] " +
                "INNER JOIN [dbo].[Membership] mc ON mc.MemberID = m.ID AND mc.ClubID = " + id.ToString();

            int rowCount = await _context.Set<Meal>().FromSqlRaw(sql).CountAsync();
            int totalPages = rowCount / pageSize;
            if (rowCount % pageSize > 0)
                ++totalPages;

            return new PageOfData<Meal>()
            {
                CurrentPage = first,
                TotalPages = totalPages,
                PageItems = await _context
                                        .Set<Meal>()
                                        .FromSqlRaw(sql)
                                        .OrderByDescending(x => x.ExactDateTime).Skip(first).Take(pageSize)
                                        .ToListAsync()
            };
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
                @"SELECT d.ID, m.ID AS OrganiserID, m.Name AS OrganiserName, mc.LevelID AS OrganiserLevelID, mc.IsFoundingFather AS IsOrganiserFoundingFather,
                        rs.ID AS RestaurantID, rs.Name AS RestaurantName,
                        rv.ExactDateTime, rv.NegotiatedBeerPrice, rv.NegotiatedBeerDiscount, rv.IsAmnesty,
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
                                        .OrderByDescending(x => x.ExactDateTime).Skip(first).Take(pageSize)
                                        .ToListAsync()
            };
        }

        // GET: api/Club/43/FoundingFathers
        [HttpGet("{id}/FoundingFathers")]
        public async Task<ActionResult<PageOfData<Membership>>> GetClubFoundingFathers(int? id, [FromQuery(Name = "first")] int first, [FromQuery(Name = "count")] int pageSize)
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

            return new PageOfData<Membership>()
            {
                CurrentPage = first,
                TotalPages = totalPages,
                PageItems = await _context
                                    .Membership.Where(x => x.ClubID == id && x.IsFoundingFather)
                                    .OrderBy(x => x.MemberID).Skip(first).Take(pageSize)
                                    .ToListAsync()
            };
        }

        // GET: api/Club/65/Memberships
        [HttpGet("{id}/Memberships")]
        public async Task<ActionResult<PageOfData<Membership>>> GetClubMemberships(int? id, [FromQuery(Name = "first")] int first, [FromQuery(Name = "count")] int pageSize)
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

            return new PageOfData<Membership>()
            {
                CurrentPage = first,
                TotalPages = totalPages,
                PageItems = await _context.Membership
                                    .Where(x => x.ClubID == id)
                                    .OrderBy(x => x.MemberID).Skip(first).Take(pageSize)
                                    .ToListAsync()
            };
        }

        // GET: api/Club/43/Membership/18
        [HttpGet("{clubId}/Membership/{memberId}")]
        public async Task<ActionResult<Membership>> GetClubMembership(int? clubId, int? memberId)
        {
            if (_context.Club == null || _context.Membership == null)
            {
                return NotFound();
            }

            var club = await _context.Club.Where(x => x.ID == clubId).FirstOrDefaultAsync();
            if (club == null)
            {
                return NotFound();
            }

            var membership = await _context.Membership
                .Where(x => x.ClubID == clubId && x.MemberID == memberId)
                .FirstOrDefaultAsync();
            if (membership == null)
            {
                return NotFound();
            }
            return membership;
        }

        // PUT: api/Club/4/Membership/87
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{clubId}/Membership/{memberId}")]
        public async Task<IActionResult> PutClubMembership(int? clubId, int? memberId, Membership membership)
        {
            if (clubId != membership.ClubID || memberId != membership.MemberID)
            {
                return BadRequest();
            }

            membership.Clubs = null;
            membership.Dinners = null;
            membership.Attendees = null;
            membership.ExemptionsGiven = null;
            membership.ExemptionsReceived = null;
            membership.KotCs = null;
            membership.Reservations = null;
            membership.RotYs = null;
            membership.ViolationsGiven = null;
            membership.ViolationsReceived = null;
            membership.Notifications = null;
            membership.Inductees = null;

            _context.Entry(membership).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClubExists(clubId))
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

        // PUT: api/Club/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClub(int? id, Club club)
        {
            if (id != club.ID)
            {
                return BadRequest();
            }

            var foundingFathers = club.Memberships;
            club.Memberships = null;
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
                    membership.IsFoundingFather = foundingFathers.Where(x => x.MemberID == membership.MemberID).Any();
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
                return Problem("Entity set 'AcmDatabaseContext.Club' is null.");
            if (_context.Membership == null)
                return Problem("Entity set 'AcmDatabaseContext.Membership' is null.");
            if (_context.Member == null)
                return Problem("Entity set 'AcmDatabaseContext.Member' is null.");
            if (club.Memberships == null || club.Memberships.Count == 0)
                return Problem("No founding father names were supplied");

            var requestedFFs = club.Memberships;
            club.ID = null;
            club.Memberships = null;
            club.Notifications = null;
            _context.Club.Add(club);

            await _context.SaveChangesAsync(); // if this succeeds then club.ID becomes valid to use.
            if (club.ID == null)
                return Problem("The RDBMS was unable to save the new club.");

            foreach (var ff in requestedFFs)
            {
                if (ff.Member == null)
                    continue;

                Member newMember = new() { Name = ff.Member.Name };
                _context.Member.Add(newMember);
                await _context.SaveChangesAsync(); // if this succeeds then newMember.ID becomes valid to use.

                if (newMember.ID != null)
                {
                    Membership newMembership = new()
                    {
                        MemberID = (int)newMember.ID,
                        ClubID = (int)club.ID,
                        LevelID = 1,
                        AttendanceCount = 0,
                        IsAdmin = false,
                        IsFoundingFather = true,
                        IsAuditor = ff.IsAuditor,
                    };

                    _context.Membership.Add(newMembership);
                    await _context.SaveChangesAsync();
                }
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
