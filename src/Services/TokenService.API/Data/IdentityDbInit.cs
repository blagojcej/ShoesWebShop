using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TokenService.API.Models;

namespace TokenService.API.Data
{
    public class IdentityDbInit
    {
        //This example just creates an Administrator role and Admin user
        public static async void Initialize(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            //Create database schema if not exists
            context.Database.Migrate();

            //Create the Administrator role
            if (context.Users.Any(r => r.UserName == "admin@test.com"))
            {
                return;
            }

            //Create the default Admin account and apply the Administrator role
            string user = "admin@test.com";
            string password = "Qw3rty~123";

            await userManager.CreateAsync(new ApplicationUser() { UserName = user, Email = user, EmailConfirmed = true },
                password);
        }
    }
}
