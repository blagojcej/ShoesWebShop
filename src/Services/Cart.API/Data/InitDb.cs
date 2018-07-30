using Microsoft.EntityFrameworkCore;

namespace Cart.API.Data
{
    public class InitDb
    {
        //This example just creates an Administrator role and Admin user
        public static void Initialize(CartContext context) =>
             //Create database schema if not exists
             context.Database.Migrate();
    }
}
