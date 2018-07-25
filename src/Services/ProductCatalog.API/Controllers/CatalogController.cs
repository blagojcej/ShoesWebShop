using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ProductCatalog.API.Data;
using ProductCatalog.API.Domain;
using ProductCatalog.API.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalog.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Catalog")]
    public class CatalogController : Controller
    {
        private readonly CatalogContext _context;
        private readonly IOptionsSnapshot<CatalogSettings> _settings;

        public CatalogController(CatalogContext context, IOptionsSnapshot<CatalogSettings> settings)
        {
            _context = context;
            _settings = settings;
            ((DbContext)context).ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        private List<CatalogItem> ChangeUrlPlaceHolder(List<CatalogItem> items)
        {
            items.ForEach(x => x.PictureUrl = x.PictureUrl.Replace("http://externalcatalogbaseurltobereplaced/",
                _settings.Value.ExternalCatalogBaseUrl));

            return items;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> CatalogTypes()
        {
            var items = await _context.CatalogTypes.ToListAsync();

            return Ok(items);
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> CatalogBrands()
        {
            var items = await _context.CatalogBrands.ToListAsync();

            return Ok(items);
        }

        [HttpGet]
        [Route("items/{id:int}")]
        public async Task<IActionResult> GetItemById(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var item = await _context.CatalogItems.SingleOrDefaultAsync(c => c.Id == id);
            if (item != null)
            {
                item.PictureUrl = item.PictureUrl.Replace("http://externalcatalogbaseurltobereplaced/",
                    _settings.Value.ExternalCatalogBaseUrl);
                return Ok(item);
            }

            return NotFound();
        }

        //GET api/Catalog/items[?pageSize=4&pageIndex=3]
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> Items([FromQuery] int pageSize = 6, [FromQuery] int pageIndex = 0)
        {
            var totalItems = await _context.CatalogItems.LongCountAsync();
            var itemsOnPage = await _context.CatalogItems.OrderBy(c => c.Name).Skip((pageIndex - 1) * pageSize).Take(pageSize)
                .ToListAsync();

            itemsOnPage = ChangeUrlPlaceHolder(itemsOnPage);

            var model = new PaginatedItemsViewModel<CatalogItem>(pageIndex, pageSize, totalItems, itemsOnPage);

            return Ok(model);
        }

        [HttpGet]
        [Route("[action]/withname/{name:minlength(1)}")]
        public async Task<IActionResult> Items(string name, [FromQuery] int pageSize = 6, [FromQuery] int pageIndex = 0)
        {
            var totalItems = await _context.CatalogItems.Where(c => c.Name.StartsWith(name)).LongCountAsync();
            var itemsOnPage = await _context.CatalogItems.Where(c => c.Name.StartsWith(name)).OrderBy(c => c.Name)
                .Skip((pageIndex - 1) * pageSize).Take(pageSize)
                .ToListAsync();

            itemsOnPage = ChangeUrlPlaceHolder(itemsOnPage);

            var model = new PaginatedItemsViewModel<CatalogItem>(pageIndex, pageSize, totalItems, itemsOnPage);

            return Ok(model);
        }

        // GET api/Catalog/items/type/1/brand/null[?pageSize=4&pageIndex=0]
        [HttpGet]
        [Route("[action]/type/{catalogTypId}/brand/{catalogBrandId}")]
        public async Task<IActionResult> Items(int? catalogTypeId, int? catalogBrandId, [FromQuery] int pageSize = 6, [FromQuery] int pageIndex = 0)
        {
            var root = (IQueryable<CatalogItem>)_context.CatalogItems;

            if (catalogTypeId.HasValue)
            {
                root = root.Where(c => c.CatalogTypeId == catalogTypeId);
            }

            if (catalogBrandId.HasValue)
            {
                root = root.Where(c => c.CatalogBrandId == catalogBrandId);
            }

            var totalItems = await root.LongCountAsync();
            var itemsOnPage = await root.OrderBy(c => c.Name)
                .Skip((pageIndex - 1) * pageSize).Take(pageSize)
                .ToListAsync();

            itemsOnPage = ChangeUrlPlaceHolder(itemsOnPage);

            var model = new PaginatedItemsViewModel<CatalogItem>(pageIndex, pageSize, totalItems, itemsOnPage);

            return Ok(model);
        }

        [HttpPost]
        [Route("items")]
        public async Task<IActionResult> CreateProduct([FromBody]CatalogItem product)
        {
            var item = new CatalogItem
            {
                CatalogBrandId = product.CatalogBrandId,
                CatalogTypeId = product.CatalogTypeId,
                Description = product.Description,
                Name = product.Name,
                PictureFileName = product.PictureFileName,
                Price = product.Price
            };

            _context.CatalogItems.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetItemById), new { id = item.Id });
        }

        [HttpPut]
        [Route("items")]
        public async Task<IActionResult> UpdateProduct([FromBody] CatalogItem productToUpdate)
        {
            var catalogItem = await _context.CatalogItems.SingleOrDefaultAsync(c => c.Id == productToUpdate.Id);
            if (catalogItem == null)
                return NotFound(new { Message = $"Item with id {productToUpdate.Id} not found" });

            catalogItem = productToUpdate;
            _context.CatalogItems.Update(catalogItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetItemById), new { id = productToUpdate.Id });
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var catalogItem = await _context.CatalogItems.SingleOrDefaultAsync(c => c.Id == id);
            if (catalogItem == null)
                return NotFound(new { Message = $"Item with id {id} not found" });

            _context.CatalogItems.Remove(catalogItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}