using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Threading.Tasks;

namespace pizza_orders_ingestor.Namespace
{
    [Route("[controller]")]
    [ApiController]
    public class PizzaOrderController : ControllerBase
    {
        private readonly PizzaPlaceDbContext _context;
        public PizzaOrderController(PizzaPlaceDbContext context)
        {this._context = context;}

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostTransactions(){
            return new CreatedAtActionResult(null,null,null,null);
        }

        [HttpGet]
        [Route("PizzaOrder/Greet")]
        public async Task<string> SampleGet(){
            return "HelloWorld";
        }

    }
}
