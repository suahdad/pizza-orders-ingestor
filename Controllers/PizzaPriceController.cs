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
    public class PizzaPriceController : IPizzaPlaceControllerBase<Pizzaprice>
    {
        private readonly PizzaPlaceDbContext _context;

        public PizzaPriceController(PizzaPlaceDbContext context) : base(context)
        {
            _context = context;
        }

        // GET: PizzaPrice
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pizzaprice>>> GetPizzaprices()
        {
            return await _context.Pizzaprices.ToListAsync();
        }

        // GET: PizzaPrice/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pizzaprice>> GetPizzaprice(string id)
        {
            var pizzaprice = await _context.Pizzaprices.FindAsync(id);

            if (pizzaprice == null)
            {
                return NotFound();
            }

            return pizzaprice;
        }

        // PUT: PizzaPrice/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPizzaprice(string id, Pizzaprice pizzaprice)
        {
            if (id != pizzaprice.Id)
            {
                return BadRequest();
            }

            _context.Entry(pizzaprice).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PizzapriceExists(id))
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

        // POST: PizzaPrice
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Pizzaprice>> PostPizzaprice(Pizzaprice pizzaprice)
        {
            _context.Pizzaprices.Add(pizzaprice);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PizzapriceExists(pizzaprice.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPizzaprice", new { id = pizzaprice.Id }, pizzaprice);
        }

        // DELETE: PizzaPrice/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePizzaprice(string id)
        {
            var pizzaprice = await _context.Pizzaprices.FindAsync(id);
            if (pizzaprice == null)
            {
                return NotFound();
            }

            _context.Pizzaprices.Remove(pizzaprice);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PizzapriceExists(string id)
        {
            return _context.Pizzaprices.Any(e => e.Id == id);
        }

        protected override Pizzaprice parseModel(string[] vals)
        {
            return new Pizzaprice(){
                 Id = vals[0],
                 PizzaId = vals[1],
                 Price = Double.Parse(vals[3]),
                 Size = vals[2]
            };
        }

        protected override Tuple<Pizzaprice[],Pizzaprice[]> filterOutExisting(List<Pizzaprice> items)
        {
            var forSaving = new Pizzaprice[]{};
            var forUpdate = new Pizzaprice[]{};

            var existing = _context.Pizzaprices.Select(price => price.Id);
            items.ForEach(item => {
                if(existing.Contains(item.Id)){forUpdate.Append(item);}
                else{forSaving.Append(item);}
            });
            return new Tuple<Pizzaprice[],Pizzaprice[]>(forSaving,forUpdate);
        }
    }
}
