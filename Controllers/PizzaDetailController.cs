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
    public class PizzaDetailController : IPizzaPlaceControllerBase<Pizzadetail>
    {
        private readonly PizzaPlaceDbContext _context;

        public PizzaDetailController(PizzaPlaceDbContext context) : base(context)
        {
            _context = context;
        }

        // GET: PizzaDetail
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pizzadetail>>> GetPizzadetails()
        {
            return await _context.Pizzadetails.ToListAsync();
        }

        // GET: PizzaDetail/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pizzadetail>> GetPizzadetail(string id)
        {
            var pizzadetail = await _context.Pizzadetails.FindAsync(id);

            if (pizzadetail == null)
            {
                return NotFound();
            }

            return pizzadetail;
        }

        // PUT: PizzaDetail/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPizzadetail(string id, Pizzadetail pizzadetail)
        {
            if (id != pizzadetail.Id)
            {
                return BadRequest();
            }

            _context.Entry(pizzadetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PizzadetailExists(id))
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

        // POST: PizzaDetail
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Pizzadetail>> PostPizzadetail(Pizzadetail pizzadetail)
        {
            _context.Pizzadetails.Add(pizzadetail);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PizzadetailExists(pizzadetail.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPizzadetail", new { id = pizzadetail.Id }, pizzadetail);
        }

        // DELETE: PizzaDetail/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePizzadetail(string id)
        {
            var pizzadetail = await _context.Pizzadetails.FindAsync(id);
            if (pizzadetail == null)
            {
                return NotFound();
            }

            _context.Pizzadetails.Remove(pizzadetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PizzadetailExists(string id)
        {
            return _context.Pizzadetails.Any(e => e.Id == id);
        }

        protected override Pizzadetail parseModel(string[] vals)
        {
            return new Pizzadetail()
            {
                Id = vals[0],
                Name = vals[1],
                Category = vals[2]
            };
        }

        protected override Tuple<Pizzadetail[],Pizzadetail[]> filterOutExisting(List<Pizzadetail> items)
        {
            var forSaving = new List<Pizzadetail>();
            var forUpdate = new List<Pizzadetail>();

            var existing = _context.Pizzadetails.Select(price => price.Id);
            items.ForEach(item => {
                if(existing.Contains(item.Id)){forUpdate.Add(item);}
                else{forSaving.Add(item);}
            });
            return new Tuple<Pizzadetail[],Pizzadetail[]>(forSaving.ToArray(),forUpdate.ToArray());
        }
    }
}

