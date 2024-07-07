using Library.Model.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Library.Data.Configurations;

public static class SeedData
{
    public async static Task SeedDataAsync(IApplicationBuilder applicationBuilder)
    {
        string[] roles = ["Pending", "Admin", "Manager", "Librarian", "IT"];

        using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
        {
            var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (!await roleManager.RoleExistsAsync(roles[0]))
                await roleManager.CreateAsync(new IdentityRole(roles[0]));
            if (!await roleManager.RoleExistsAsync(roles[1]))
                await roleManager.CreateAsync(new IdentityRole(roles[1]));
            if (!await roleManager.RoleExistsAsync(roles[2]))
                await roleManager.CreateAsync(new IdentityRole(roles[2]));
            if (!await roleManager.RoleExistsAsync(roles[3]))
                await roleManager.CreateAsync(new IdentityRole(roles[3]));
            if (!await roleManager.RoleExistsAsync(roles[4]))
                await roleManager.CreateAsync(new IdentityRole(roles[4]));

            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<Employee>>();

            var employee1 = new Employee()
            {
                Name = "Giorgi",
                Surname = "Otinashvili",
                CreationDate = DateTime.UtcNow,
                Email = "giorgiotinashvili12@gmail.com",
                NormalizedEmail = "GIORGIOTINASHVILI12@GMAIL.COM",
                UserName = "Giorgi",
                NormalizedUserName = "GIORGI",
                PhoneNumber = "595951546"
            };

            await userManager.CreateAsync(employee1, "Pass123!");
            await userManager.AddToRolesAsync(employee1, [roles[0], roles[3]]);


            var employee2 = new Employee()
            {
                Name = "John",
                Surname = "Doe",
                CreationDate = DateTime.UtcNow,
                Email = "johndoe@gmail.com",
                NormalizedEmail = "JOHNDOE@GMAIL.COM",
                UserName = "JohnDoe",
                NormalizedUserName = "JOHNDOE",
                PhoneNumber = "595951547"
            };

            await userManager.CreateAsync(employee2, "Pass123!");
            await userManager.AddToRolesAsync(employee2, [roles[2]]);


            var employee3 = new Employee()
            {
                Name = "Kylian",
                Surname = "Doe",
                CreationDate = DateTime.UtcNow,
                Email = "kyliandoe@gmail.com",
                NormalizedEmail = "KYLIANDOE@GMAIL.COM",
                UserName = "KylianDoe",
                NormalizedUserName = "KYLIANDOE",
                PhoneNumber = "595951548"
            };

            await userManager.CreateAsync(employee1, "Pass123!");
            await userManager.AddToRolesAsync(employee1, [roles[1], roles[3]]);

        }
    }
}
