using Cart.API.Model;
using Microsoft.EntityFrameworkCore;

namespace Cart.API.Data
{
    public class CartContext : DbContext
    {
        public DbSet<Model.Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        public CartContext(DbContextOptions options) : base(options)
        {
            
        }
    }
}
