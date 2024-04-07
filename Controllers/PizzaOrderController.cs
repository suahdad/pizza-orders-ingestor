using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pizza_orders_ingestor;

namespace pizza_orders_ingestor.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PizzaOrderController : IPizzaPlaceControllerBase<Pizzaorder>
    {
        private readonly PizzaPlaceDbContext _context;

        public PizzaOrderController(PizzaPlaceDbContext context) : base(context)
        {
            _context = context;
        }

        // GET: api/PizzaOrder
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pizzaorder>>> GetPizzaorders()
        {
            return await _context.Pizzaorders.ToListAsync();
        }

        // GET: api/PizzaOrder/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pizzaorder>> GetPizzaorder(uint id)
        {
            var pizzaorder = await _context.Pizzaorders.FindAsync(id);

            if (pizzaorder == null)
            {
                return NotFound();
            }

            return pizzaorder;
        }

        // PUT: api/PizzaOrder/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPizzaorder(uint id, Pizzaorder pizzaorder)
        {
            if (id != pizzaorder.Id)
            {
                return BadRequest();
            }

            _context.Entry(pizzaorder).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PizzaorderExists(id))
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

        // POST: api/PizzaOrder
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Pizzaorder>> PostPizzaorder(Pizzaorder pizzaorder)
        {
            _context.Pizzaorders.Add(pizzaorder);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPizzaorder", new { id = pizzaorder.Id }, pizzaorder);
        }

        // DELETE: api/PizzaOrder/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePizzaorder(uint id)
        {
            var pizzaorder = await _context.Pizzaorders.FindAsync(id);
            if (pizzaorder == null)
            {
                return NotFound();
            }

            _context.Pizzaorders.Remove(pizzaorder);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PizzaorderExists(uint id)
        {
            return _context.Pizzaorders.Any(e => e.Id == id);
        }

        protected override Pizzaorder parseModel(string[] vals)
        {
            return new Pizzaorder()
            {
                Id = uint.Parse(vals[0]),
                Datetime = DateTime.Parse(vals[1] + " " + vals[2])
            };

        }

        protected override Tuple<Pizzaorder[], Pizzaorder[]> filterOutExisting(List<Pizzaorder> items)
        {
            var forSaving = new List<Pizzaorder>();
            var forUpdate = new List<Pizzaorder>();

            var existing = _context.Pizzaorders.Select(price => price.Id);
            items.ForEach(item =>
            {
                if (existing.Contains(item.Id)) { forUpdate.Add(item); }
                else { forSaving.Add(item); }
            });
            return new Tuple<Pizzaorder[], Pizzaorder[]>(forSaving.ToArray(), forUpdate.ToArray());

        }
    }
}
