using System.Net;
using System.Threading.Tasks;
using Cart.API.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Cart.API.Controllers
{
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly ILogger _logger;
        private readonly ICartRepository _repository;

        public CartController(ILoggerFactory factory, ICartRepository repository)
        {
            _logger = factory.CreateLogger<CartController>();
            _repository = repository;
        }

        //GET api/values/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Model.Cart), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(string id)
        {
            var basket = await _repository.GetCartAsync(id);

            return Ok(basket);
        }

        //POST apivalues
        [HttpPost]
        [ProducesResponseType(typeof(Model.Cart), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Post([FromBody] Model.Cart value)
        {
            var basket = await _repository.UpdateCartAsync(value);

            return Ok(basket);
        }

        //DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            _logger.LogInformation("Delete method in Cart controller reached.");
            _repository.DeleteCartAsync(id);
        }
    }
}