﻿namespace LearningSystem.Web.Infrastructure.Extensions
{
    using Data;
    using Data.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Threading.Tasks;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseDatabaseMigration(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetService<LearningSystemDbContext>().Database.Migrate();

                var userManger = serviceScope.ServiceProvider.GetService<UserManager<User>>();
                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

                Task
                    .Run(async () =>
                    {
                        var adminName = WebConstants.AdministratorRole;

                        var roles = new[]
                        {
                            adminName,
                            WebConstants.BlogAuthorRole,
                            WebConstants.TrainerRole
                        };

                        foreach (var role in roles)
                        {
                            var roleExests = await roleManager.RoleExistsAsync(role);

                            if (!roleExests)
                            {
                                await roleManager.CreateAsync(new IdentityRole
                                {
                                    Name = role
                                });
                            }
                        }

                        var adminEmail = "admin@mysite.com";

                        var adminUser = await userManger.FindByNameAsync(adminEmail);

                        if (adminUser is null)
                        {
                            adminUser = new User
                            {
                                Email = adminEmail,
                                UserName = adminEmail,
                                Name = adminName,
                                Birthdate = DateTime.UtcNow
                            };

                            await userManger.CreateAsync(adminUser, "admin12");

                            await userManger.AddToRoleAsync(adminUser, adminName);
                        }
                    })
                    .Wait();
            }

            return app;
        }
    }
}
