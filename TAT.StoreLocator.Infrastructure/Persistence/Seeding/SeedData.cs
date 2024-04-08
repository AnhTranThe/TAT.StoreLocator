using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TAT.StoreLocator.Core.Entities;
using TAT.StoreLocator.Core.Helpers;
using TAT.StoreLocator.Core.Interface.ILogger;
using TAT.StoreLocator.Infrastructure.Persistence.EF;

namespace TAT.StoreLocator.Infrastructure.Persistence.Seeding
{
    public static class SeedData
    {

        public static async Task InitializeAsync(IHost host)
        {

            using IServiceScope scope = host.Services.CreateScope();
            IServiceProvider serviceProvider = scope.ServiceProvider;
            AppDbContext? context = serviceProvider.GetService<AppDbContext>();
            ILogger? logger = serviceProvider.GetService<ILogger>();
            try
            {
                if (context != null)
                {
                    PasswordHasher<User> passwordHasher = new();


                    if (!context.Roles.Any())
                    {
                        await context.Roles.AddRangeAsync(
                            new Role()
                            {
                                Id = GlobalConstants.RoleAdminId,
                                Name = "Admin",
                                NormalizedName = "ADMIN",

                            },
                            new Role()
                            {
                                Id = GlobalConstants.RoleUserId,
                                Name = "User",
                                NormalizedName = "USER",

                            }

                            );
                        _ = await context.SaveChangesAsync();
                    }


                    if (!context.Users.Any())
                    {

                        List<User> UserLs = new()
                                        {
                        new User
                        {
                            Id = GlobalConstants.AdminId,
                            FirstName = "admin",
                            LastName = "admin",
                            FullName = "Administrator",
                            Email = "admin@gmail.com",
                            NormalizedEmail = "ADMIN@GMAIL.COM",
                            UserName = "admin",
                            NormalizedUserName = "ADMIN",
                            IsActive = true,
                            SecurityStamp = Guid.NewGuid().ToString(),
                            LockoutEnabled = false,

                        },


                        new User
                        {
                            Id = GlobalConstants.UserId,
                            FirstName = "tester",
                            LastName = "tester",
                            Email = "tester@gmail.com",
                            NormalizedEmail = "TRANTHEANH@GMAIL.COM",
                            UserName = "tester",
                            NormalizedUserName = "TESTER",
                            IsActive = true,
                            SecurityStamp = Guid.NewGuid().ToString(),
                            LockoutEnabled = false,

                        }
                        };
                        foreach (User user in UserLs)
                        {
                            user.PasswordHash = passwordHasher.HashPassword(user, "Admin@123");
                        }


                        await context.Users.AddRangeAsync(UserLs);

                        _ = await context.SaveChangesAsync();


                    }

                    if (!context.UserRoles.Any())
                    {

                        await context.UserRoles.AddRangeAsync(
                            new IdentityUserRole<string>()
                            {
                                RoleId = GlobalConstants.RoleAdminId,
                                UserId = GlobalConstants.AdminId,


                            },
                            new IdentityUserRole<string>()
                            {
                                RoleId = GlobalConstants.RoleUserId,
                                UserId = GlobalConstants.UserId,

                            }

                            );
                        _ = await context.SaveChangesAsync();

                    }


                }



                logger?.LogInfo("Data initialization completed successfully.");


            }
            catch (Exception ex)
            {
                logger?.LogError(ex.Message);
            }

        }


    }
}


