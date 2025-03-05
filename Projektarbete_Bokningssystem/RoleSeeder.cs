using Microsoft.AspNetCore.Identity;

namespace Projektarbete_Bokningssystem
{
    public class RoleSeeder
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            // Seeda databasen med roller
            await roleManager.CreateAsync(new IdentityRole("Admin"));
            await roleManager.CreateAsync(new IdentityRole("User"));
        }
    }
}
