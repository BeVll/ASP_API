﻿using ASP_API.Constants;
using ASP_API.Data.Entities;
using ASP_API.Data.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ASP_API.Data
{
    public static class SeederDB
    {
        public static void SeedData(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var service = scope.ServiceProvider;
                var context = service.GetRequiredService<AppEFContext>();
                var userNamager = service.GetRequiredService<UserManager<UserEntity>>();
                var roleNamager = service.GetRequiredService<RoleManager<RoleEntity>>();
                context.Database.Migrate(); //автоматично запускає міграції на БД

                if (!context.Categories.Any())
                {
                    CategoryEntity categoryEntity = new CategoryEntity()
                    {
                        Name = "Піжами",
                        Image = "1.jpg",
                        Priority = 1,
                        DateCreated = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                        Description = "Для нічних прогулок"
                    };
                    context.Categories.Add(categoryEntity);
                    context.SaveChanges();
                }
                if (!context.Roles.Any())
                {
                    foreach (string name in Roles.All)
                    {
                        var role = new RoleEntity
                        {
                            Name = name
                        };
                        var result = roleNamager.CreateAsync(role).Result;
                    }
                }
                if (!context.Users.Any())
                {
                    var user = new UserEntity()
                    {
                        FirstName = "Вова",
                        LastName = "Новак",
                        Email = "admin@gmail.com",
                        UserName = "admin@gmail.com"
                    };
                    var result = userNamager.CreateAsync(user, "123456").Result;
                    if (result.Succeeded)
                    {
                        result = userNamager.AddToRoleAsync(user, Roles.Admin).Result;
                    }
                }

            }
        }
    }
}
