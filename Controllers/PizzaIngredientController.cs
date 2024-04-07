using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using pizza_orders_ingestor;

namespace pizza_orders_ingestor.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PizzaIngredientController : IPizzaPlaceControllerBase<Pizzaingredient>
    {
        private readonly PizzaPlaceDbContext _context;

        public PizzaIngredientController(PizzaPlaceDbContext context) : base(context)
        {
            _context = context;
        }

        // GET: PizzaIngredient
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pizzaingredient>>> GetPizzaingredients()
        {
            return await _context.Pizzaingredients.ToListAsync();
        }

        // GET: PizzaIngredient/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pizzaingredient>> GetPizzaingredient(string id)
        {
            var pizzaingredient = await _context.Pizzaingredients.FindAsync(id);

            if (pizzaingredient == null)
            {
                return NotFound();
            }

            return pizzaingredient;
        }

        // PUT: PizzaIngredient/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPizzaingredient(string id, Pizzaingredient pizzaingredient)
        {
            if (id != pizzaingredient.Id)
            {
                return BadRequest();
            }

            _context.Entry(pizzaingredient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PizzaingredientExists(id))
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

        // POST: PizzaIngredient
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Pizzaingredient>> PostPizzaingredient(Pizzaingredient pizzaingredient)
        {
            _context.Pizzaingredients.Add(pizzaingredient);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PizzaingredientExists(pizzaingredient.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPizzaingredient", new { id = pizzaingredient.Id }, pizzaingredient);
        }

        // DELETE: PizzaIngredient/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePizzaingredient(string id)
        {
            var pizzaingredient = await _context.Pizzaingredients.FindAsync(id);
            if (pizzaingredient == null)
            {
                return NotFound();
            }

            _context.Pizzaingredients.Remove(pizzaingredient);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PizzaingredientExists(string id)
        {
            return _context.Pizzaingredients.Any(e => e.Id == id);
        }

        protected override Pizzaingredient parseModel(string[] vals)
        {
            var joined = String.Join(",",vals);
            return new Pizzaingredient(){
                Id = vals[0],
                Ingredients = joined.Substring(vals[0].Length+vals[1].Length+vals[2].Length+3) //3 commas
            };
        }

        protected override Tuple<Pizzaingredient[],Pizzaingredient[]> filterOutExisting(List<Pizzaingredient> items)
        {
            var forSaving = new List<Pizzaingredient>();
            var forUpdate = new List<Pizzaingredient>();

            var existing = _context.Pizzaingredients.Select(price => price.Id);
            items.ForEach(item => {
                if(existing.Contains(item.Id)){forUpdate.Add(item);}
                else{forSaving.Add(item);}
            });
            return new Tuple<Pizzaingredient[],Pizzaingredient[]>(forSaving.ToArray(),forUpdate.ToArray());
        }
    }
}
