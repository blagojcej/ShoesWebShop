using Cart.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cart.API.Model
{
    public class CartRepository : ICartRepository
    {
        private readonly ILogger _logger;
        private readonly CartContext _context;

        public CartRepository(ILoggerFactory factory, CartContext context)
        {
            _logger = factory.CreateLogger<CartRepository>();
            _context = context;
        }

        public async Task<bool> DeleteCartAsync(string id)
        {
            var cart = await _context.Carts.SingleOrDefaultAsync(c => c.BuyerId == id);
            if (cart == null)
            {
                _logger.LogInformation($"Cart with id {id} doesn't exists.");
                return false;
            }

            _context.Remove(cart);
            int result = await _context.SaveChangesAsync();

            return result > 0;
        }

        public async Task<Cart> GetCartAsync(string cardId)
        {
            return await _context.Carts.SingleOrDefaultAsync(c => c.BuyerId == cardId);
        }

        public IEnumerable<string> GetUsers()
        {
            return _context.Carts.Select(cart => cart.BuyerId).ToList();
        }

        public async Task<Cart> UpdateCartAsync(Cart basket)
        {
            var cart = await _context.Carts.SingleOrDefaultAsync(c => c.BuyerId == basket.BuyerId);
            if (cart == null)
            {
                _logger.LogInformation($"Cart with id {basket.BuyerId} doesn't exists.");
                return null;
            }

            cart = basket;
            _context.Carts.Update(cart);
            await _context.SaveChangesAsync();

            return cart;
        }
    }
}
