using InventoryMg.DAL.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryMg.DAL.Seed
{
    public class SeedData
    {




        public static async Task EnsurePopulatedAsync(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var _roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
            var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserProfile>>();

            var existingUser = await _userManager.FindByEmailAsync("admin@domain.com");
            if (existingUser == null)
            {
                UserProfile user = new()
                {
                    FullName = "Adim User",
                    Phone = "09096026989",
                    UserName = "admin",
                    Email = "admin@domain.com",
                    Password = "Pass12345@"
                };

                IdentityResult result = await _userManager.CreateAsync(user, "Pass12345@");
                if (!result.Succeeded)
                {
                    Console.WriteLine($"Failed to create admin: {(result.Errors.FirstOrDefault())?.Description}");
                };
                await _roleManager.CreateAsync(new AppRole { Name = "admin", Id = Guid.NewGuid().ToString() });
                var getRole = _roleManager.Roles.Where(r => r.Name == "admin").FirstOrDefault();

                await _userManager.AddToRoleAsync(user, "admin");
                //generate token
                Console.WriteLine(user);
            }
            Console.WriteLine("Already Exist");

            /*ApplicationDbContext appDbContext = app.ApplicationServices.CreateScope().ServiceProvider
                .GetRequiredService<ApplicationDbContext>();
            UserProfile admin = new UserProfile()
            {
                FullName = "Admin One",
                Email = "admin@domain.com",
                Phone = "2233445566"
            };
            using var scope = app.ApplicationServices.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserProfile>>();

            var result = await userManager.CreateAsync(admin, "Pass12345@");

            await roleManager.CreateAsync(new AppRole { Name = "admin", Id = Guid.NewGuid().ToString() });
            var getRole = roleManager.Roles.Where(r => r.Name == "admin").FirstOrDefault();

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "admin");
            }
*/

        }






    }
}
