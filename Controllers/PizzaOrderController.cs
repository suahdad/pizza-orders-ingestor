using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Extensions.ObjectPool;
using NuGet.Packaging;
using NuGet.Common;
using System.Data.Entity.Core.Metadata.Edm;
using Mysqlx;
using System.Data.Entity;

namespace pizza_orders_ingestor.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PizzaOrderController : IPizzaPlaceControllerBase<Pizzaorder>
    {
        protected PizzaPlaceDbContext _context;
        
        public PizzaOrderController(PizzaPlaceDbContext context) : base(context)
        {
            this._context = context;
        }

        protected override List<Pizzaorder> filterOutExisting(List<Pizzaorder> items)
        {
            var existing = _context.Pizzaorders.Select(order => order.Id);
            return items.Where(order => !existing.Contains(order.Id)).ToList();
        }

        protected override Pizzaorder parseModel(string[] vals, List<Pizzaorder> items)
        {
            return new Pizzaorder(){
                Id = uint.Parse(vals[0]),
                Datetime = DateTime.Parse(vals[1] + " " + vals[2])
            };
        }

        protected override IActionResult result(List<Pizzaorder> saved)
        {
            return CreatedAtAction(nameof(PizzaOrderController),new {count = saved.Count});
        }
    }
}