using acm_models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Drawing.Printing;

namespace acm_rest_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly AcmDatabaseContext _context;

        public NotificationController(AcmDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Notification
        [HttpGet]
        public async Task<ActionResult<PageOfData<Notification>>> GetNotification([FromQuery(Name = "first")] int first, [FromQuery(Name = "count")] int pageSize)
        {
            if (_context.Notification == null || pageSize <= 0)
            {
                return NotFound();
            }

            int rowCount = await _context.Notification.CountAsync();
            int totalPages = rowCount / pageSize;
            if (rowCount % pageSize > 0)
                ++totalPages;

            return new PageOfData<Notification>()
            {
                CurrentPage = first,
                TotalPages = totalPages,
                PageItems = await _context.Notification
                .OrderBy(x => x.ID)
                    .Skip(first).Take(pageSize)
                    .ToListAsync()
            };
        }

        // GET: api/Notification/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Notification>> GetNotification(int? id)
        {
            if (_context.Notification == null)
            {
                return NotFound();
            }

            var notification = await _context.Notification
                .Include(x => x.Attendee)
                .Include(x => x.Club)
                .Include(x => x.Dinner)
                .Include(x => x.Exemption)
                .Include(x => x.KotC)
                .Include(x => x.Level)
                .Include(x => x.Member)
                .Include(x => x.Reservation)
                .Include(x => x.Restaurant)
                .Include(x => x.RotY)
                .Include(x => x.Violation)
                .Where(x => x.ID == id)
                .FirstOrDefaultAsync();
            if (notification == null)
            {
                return NotFound();
            }

            return notification;
        }

        // PUT: api/Notification/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNotification(int? id, Notification notification)
        {
            if (id != notification.ID)
            {
                return BadRequest();
            }

            _context.Entry(notification).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotificationExists(id))
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

        // POST: api/Notification
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Notification>> PostNotification(Notification notification)
        {
            if (_context.Notification == null)
            {
                return Problem("Entity set 'AcmDatabaseContext.Notification' is null.");
            }
            _context.Notification.Add(notification);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNotification", new { id = notification.ID }, notification);
        }

        // DELETE: api/Notification/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(int? id)
        {
            if (_context.Notification == null)
            {
                return NotFound();
            }
            var notification = await _context.Notification.FindAsync(id);
            if (notification == null)
            {
                return NotFound();
            }

            _context.Notification.Remove(notification);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NotificationExists(int? id)
        {
            return (_context.Notification?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
