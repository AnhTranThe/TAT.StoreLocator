using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TAT.StoreLocator.Core.Entities;
using TAT.StoreLocator.Core.Helpers;
using TAT.StoreLocator.Core.Interface.ILogger;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Infrastructure.Mapper;
using TAT.StoreLocator.Infrastructure.Persistence.EF;
using TAT.StoreLocator.Infrastructure.Services;
using TAT.StoreLocator.Infrastructure.UnitOfWork;


namespace TAT.StoreLocator.Infrastructure.DI
{
    public static class DependencyInjection

    {

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {


            #region Cors
            _ = services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            });
            #endregion

            #region SQL Connection
            _ = services.AddDbContext<AppDbContext>(options =>
            {
                _ = options.UseNpgsql(config.GetConnectionString("DefaultConnection"));
            });

            #endregion

            #region authentication and JWt

            //Authen and author
            _ = services.Configure<JwtTokenSettings>(config.GetSection("JwtTokenSettings"));



            _ = services.AddIdentity<User, Role>(opt => { opt.Password.RequireNonAlphanumeric = false; })
               .AddEntityFrameworkStores<AppDbContext>();


            _ = services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = false;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });


            _ = services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {

                    ValidIssuer = config["JwtTokenSettings:Issuer"],
                    ValidAudience = config["JwtTokenSettings:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtTokenSettings:Key"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true
                };
            });
            _ = services.AddAuthorization();

            #endregion

            #region Services
            _ = services.AddTransient(typeof(IJwtService), typeof(JwtService));
            _ = services.AddTransient(typeof(IUserService), typeof(UserService));
            _ = services.AddTransient(typeof(ILogger), typeof(LoggerService));
            _ = services.AddTransient(typeof(IPhotoService), typeof(PhotoService));
            _ = services.AddTransient(typeof(IAuthenticationService), typeof(AuthenticationService));
            _ = services.AddTransient<SignInManager<User>, SignInManager<User>>();
            _ = services.AddTransient<UserManager<User>, UserManager<User>>();
            _ = services.AddTransient<RoleManager<Role>, RoleManager<Role>>();

            #endregion


            #region Mapper
            _ = services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);


            #endregion

            #region Cloudinary
            _ = services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            #endregion


            return services;
        }

    }
}
