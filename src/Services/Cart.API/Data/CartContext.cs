using Cart.API.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Cart.API.Data
{
    public class CartContext : DbContext
    {
        public DbSet<Model.Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        public CartContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Model.Cart>(ConfigureCart);
            modelBuilder.Entity<CartItem>(ConfigureCartItem);
        }

        private void ConfigureCartItem(EntityTypeBuilder<CartItem> builder)
        {
            builder.ToTable("CartItems");

            builder.HasOne(ci => ci.Cart)
                .WithMany(c => c.Items);
        }

        private void ConfigureCart(EntityTypeBuilder<Model.Cart> builder)
        {
            builder.ToTable("Cart");

            builder.HasMany(c => c.Items)
                .WithOne(ci => ci.Cart);
        }
    }
}
