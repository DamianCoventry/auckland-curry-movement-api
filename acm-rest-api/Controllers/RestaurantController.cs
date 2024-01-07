using acm_models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace acm_rest_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly AcmDatabaseContext _context;

        public RestaurantController(AcmDatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Restaurant
        [HttpGet]
        public async Task<ActionResult<PageOfData<Restaurant>>> GetRestaurant([FromQuery(Name = "first")] int first, [FromQuery(Name = "count")] int pageSize)
        {
            if (_context.Restaurant == null || pageSize <= 0)
            {
                return NotFound();
            }

            int rowCount = await _context.Restaurant.CountAsync();
            int totalPages = rowCount / pageSize;
            if (rowCount % pageSize > 0)
                ++totalPages;

            return new PageOfData<Restaurant>()
            {
                CurrentPage = first,
                TotalPages = totalPages,
                PageItems = await _context.Restaurant
                .OrderBy(x => x.ID)
                    .Skip(first).Take(pageSize)
                    .ToListAsync()
            };
        }

        // GET: api/Restaurant/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Restaurant>> GetRestaurant(int? id)
        {
            if (_context.Restaurant == null)
            {
                return NotFound();
            }

            var restaurant = await _context.Restaurant
                .Include(x => x.Reservations)
                .Include(x => x.RotYs)
                .Include(x => x.Notifications)
                .Where(x => x.ID == id)
                .FirstOrDefaultAsync();
            if (restaurant == null)
            {
                return NotFound();
            }

            return restaurant;
        }

        // GET: api/Restaurant/5/RotYStats
        [HttpGet("{id}/RotYStats")]
        public async Task<ActionResult<RotYStats>> GetRestaurantRotYStats(int? id)
        {
            if (_context.Restaurant == null || _context.RotY == null || id == null)
            {
                return NotFound();
            }

            int year = DateTime.Now.Year;

            bool isCurrentRotY = await _context.RotY
                .Where(x => x.Year == year && x.RestaurantID == (int)id)
                .AnyAsync();

            List<int> formerYears = await _context.RotY
                .Where(x => x.Year != year && x.RestaurantID == (int)id)
                .Select(x => x.Year.GetValueOrDefault())
                .ToListAsync();

            return new RotYStats()
            {
                RestaurantID = (int)id,
                CurrentYear = year,
                IsCurrentRotY = isCurrentRotY,
                FormerYears = formerYears,
            };
        }

        // PUT: api/Restaurant/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRestaurant(int? id, Restaurant restaurant)
        {
            if (id != restaurant.ID)
            {
                return BadRequest();
            }

            _context.Entry(restaurant).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RestaurantExists(id))
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

        // POST: api/Restaurant
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Restaurant>> PostRestaurant(Restaurant restaurant)
        {
            if (_context.Restaurant == null)
            {
                return Problem("Entity set 'AcmDatabaseContext.Restaurant' is null.");
            }
            _context.Restaurant.Add(restaurant);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRestaurant", new { id = restaurant.ID }, restaurant);
        }

        // DELETE: api/Restaurant/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRestaurant(int? id)
        {
            if (_context.Restaurant == null)
            {
                return NotFound();
            }
            var restaurant = await _context.Restaurant.FindAsync(id);
            if (restaurant == null)
            {
                return NotFound();
            }

            _context.Restaurant.Remove(restaurant);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RestaurantExists(int? id)
        {
            return (_context.Restaurant?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
