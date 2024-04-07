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

        protected override Tuple<Pizzaorder[],Pizzaorder[]> filterOutExisting(List<Pizzaorder> items)
        {
            var forSaving = new Pizzaorder[]{};
            var forUpdate = new Pizzaorder[]{};

            var existing = _context.Pizzaorders.Select(price => price.Id);
            items.ForEach(item => {
                if(existing.Contains(item.Id)){forUpdate.Append(item);}
                else{forSaving.Append(item);}
            });
            return new Tuple<Pizzaorder[],Pizzaorder[]>(forSaving,forUpdate);
        }

        protected override Pizzaorder parseModel(string[] vals)
        {
            return new Pizzaorder(){
                Id = uint.Parse(vals[0]),
                Datetime = DateTime.Parse(vals[1] + " " + vals[2])
            };
        }

    }
}