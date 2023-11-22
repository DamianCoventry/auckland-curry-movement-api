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
    public class NotificationController : ControllerBase
    {
        private readonly AcmDatabaseContext _context;

        public NotificationController(AcmDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Notification
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Notification>>> GetNotification([FromQuery(Name = "first")] int first, [FromQuery(Name = "count")] int count)
        {
            if (_context.Notification == null)
            {
                return NotFound();
            }
            return await _context.Notification
                .OrderBy(x => x.ID).Skip(first).Take(count)
                .ToListAsync();
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
                return Problem("Entity set 'AcmDatabaseContext.Notification'  is null.");
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
