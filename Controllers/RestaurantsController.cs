using auckland_curry_movement_api.DatabaseContext;
using auckland_curry_movement_api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace auckland_curry_movement_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {
        //private readonly AcmDatabaseContext _context;

        //public RestaurantsController(AcmDatabaseContext context)
        //{
        //    _context = context;
        //}

        // GET: api/Restaurants
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Restaurant>>> GetRestaurants()
        {
            //if (_context.Restaurant == null)
            //{
            //    return NotFound();
            //}
            //return await _context.Restaurant.ToListAsync();
            return NoContent();
        }

        // GET: api/Restaurants/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Restaurant>> GetRestaurant(int? id)
        {
            //if (_context.Restaurant == null)
            //{
            //    return NotFound();
            //}
            //var restaurant = await _context.Restaurant.FindAsync(id);

            //if (restaurant == null)
            //{
            //    return NotFound();
            //}

            //return restaurant;
            return NoContent();
        }

        // PUT: api/Restaurants/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRestaurant(int? id, Restaurant restaurant)
        {
            //if (id != restaurant.ID)
            //{
            //    return BadRequest();
            //}

            //_context.Entry(restaurant).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!RestaurantExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            //return NoContent();
            return NoContent();
        }

        // POST: api/Restaurants
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Restaurant>> PostRestaurant(Restaurant restaurant)
        {
            //if (_context.Restaurant == null)
            //{
            //    return Problem("Entity set 'AcmDatabaseContext.Restaurants'  is null.");
            //}
            //_context.Restaurant.Add(restaurant);
            //await _context.SaveChangesAsync();

            //return CreatedAtAction("GetRestaurant", new { id = restaurant.ID }, restaurant);
            return NoContent();
        }

        // DELETE: api/Restaurants/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRestaurant(int? id)
        {
            //if (_context.Restaurant == null)
            //{
            //    return NotFound();
            //}
            //var restaurant = await _context.Restaurant.FindAsync(id);
            //if (restaurant == null)
            //{
            //    return NotFound();
            //}

            //_context.Restaurant.Remove(restaurant);
            //await _context.SaveChangesAsync();

            //return NoContent();
            return NoContent();
        }

        //private bool RestaurantExists(int? id)
        //{
        //    return (_context.Restaurant?.Any(e => e.ID == id)).GetValueOrDefault();
        //}
    }
}
