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
using System.Windows.Markup;
using Google.Protobuf.WellKnownTypes;

namespace pizza_orders_ingestor.Controllers
{
    [Route("[controller]")]
    public abstract class IPizzaPlaceControllerBase<T> : ControllerBase
    {
        protected abstract T parseModel(String[] vals,List<T> items);

        private readonly PizzaPlaceDbContext _context;
        protected abstract IActionResult result(List<T> savedObjects);
        protected abstract List<T> filterOutExisting(List<T> items);
        public IPizzaPlaceControllerBase(PizzaPlaceDbContext context)
        { this._context = context; }

        [HttpPost]
        [Route("")]
        [Consumes(MediaTypeNames.Multipart.FormData)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostFile(IFormFile file)
        {
            var csvFileHasHeader = Request.Query["hasHeader"].First();
            var readStream = file.OpenReadStream();
            List<T> items = new List<T>();
            string line;
            using (TextReader tr = new StreamReader(readStream))
            {
                if (bool.Parse(csvFileHasHeader)) { tr.ReadLine(); }
                while ((line = await tr.ReadLineAsync()) != null)
                {
                    try
                    {
                        var vals = line.Split(',');
                        var model = this.parseModel(vals,items);
                        items.Add(model);
                    }
                    catch (Exception e)
                    {
                        return BadRequest(e);
                    }
                }
            }

            var forSaving = filterOutExisting(items);
            try
            {
                // var tn = await _context.Database.BeginTransactionAsync();
                // await _context.AddRangeAsync(forSaving);
                // await tn.CommitAsync();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
            // await _context.SaveChangesAsync();
            return this.result(forSaving);
        }

    }
}
