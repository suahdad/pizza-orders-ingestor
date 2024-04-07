using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mysqlx.Crud;
using pizza_orders_ingestor;

namespace pizza_orders_ingestor.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrderDetailController : IPizzaPlaceControllerBase<Orderdetail>
    {
        private readonly PizzaPlaceDbContext _context;

        public OrderDetailController(PizzaPlaceDbContext context):base(context)
        {
            _context = context;
        }

        // GET: OrderDetail
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Orderdetail>>> GetOrderdetails()
        {
            return await _context.Orderdetails.ToListAsync();
        }

        // GET: OrderDetail/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Orderdetail>> GetOrderdetail(uint id)
        {
            var orderdetail = await _context.Orderdetails.FindAsync(id);

            if (orderdetail == null)
            {
                return NotFound();
            }

            return orderdetail;
        }

        // PUT: OrderDetail/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrderdetail(uint id, Orderdetail orderdetail)
        {
            if (id != orderdetail.Id)
            {
                return BadRequest();
            }

            _context.Entry(orderdetail).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderdetailExists(id))
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

        // POST: OrderDetail
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Orderdetail>> PostOrderdetail(Orderdetail orderdetail)
        {
            _context.Orderdetails.Add(orderdetail);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrderdetail", new { id = orderdetail.Id }, orderdetail);
        }

        // DELETE: OrderDetail/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderdetail(uint id)
        {
            var orderdetail = await _context.Orderdetails.FindAsync(id);
            if (orderdetail == null)
            {
                return NotFound();
            }

            _context.Orderdetails.Remove(orderdetail);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderdetailExists(uint id)
        {
            return _context.Orderdetails.Any(e => e.Id == id);
        }

        protected override Orderdetail parseModel(string[] vals)
        {
            return new Orderdetail(){
                Id = uint.Parse(vals[0]),
                OrderId = uint.Parse(vals[1]),
                PizzaId = vals[2],
                Quantity = Byte.Parse(vals[3]),
                Price = _context.Pizzaprices.Find(vals[2]).Price
            };;
        }

        protected override Tuple<Orderdetail[],Orderdetail[]> filterOutExisting(List<Orderdetail> items)
        {
            var forSaving = new List<Orderdetail>();
            var forUpdate = new List<Orderdetail>();

            var existing = _context.Orderdetails.Select(price => price.Id);
            items.ForEach(item => {
                if(existing.Contains(item.Id)){forUpdate.Add(item);}
                else{forSaving.Add(item);}
            });
            return new Tuple<Orderdetail[],Orderdetail[]>(forSaving.ToArray(),forUpdate.ToArray());
        }
    }
}
