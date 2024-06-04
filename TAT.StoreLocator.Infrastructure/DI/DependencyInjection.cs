using log4net;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository.Hierarchy;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text;
using TAT.StoreLocator.Core.Entities;
using TAT.StoreLocator.Core.Helpers;
using TAT.StoreLocator.Core.Interface.ILogger;
using TAT.StoreLocator.Core.Interface.IServices;
using TAT.StoreLocator.Infrastructure.Mapper;
using TAT.StoreLocator.Infrastructure.Persistence.EF;
using TAT.StoreLocator.Infrastructure.Services;

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
            #region Logging 
            ConfigureLog4Net(config.GetSection("Logging:Log4Net"));
            #endregion

            #endregion Cors

            #region SQL Connection

            _ = services.AddDbContext<AppDbContext>(options =>
            {
                _ = options.UseNpgsql(config.GetConnectionString("DefaultConnection"));
            });
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            #endregion SQL Connection

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
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                };

                cfg.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.StatusCode = 400; // Return bad request
                            context.Response.ContentType = "application/json";
                            var message = new { error = "Token expired" };
                            return context.Response.WriteAsync(JsonConvert.SerializeObject(message));
                        }
                        return Task.CompletedTask;
                    }
                };
            });
            _ = services.AddAuthorization();

            #endregion authentication and JWt

            #region Services

            _ = services.AddScoped(typeof(IJwtService), typeof(JwtService));
            _ = services.AddScoped(typeof(IUserService), typeof(UserService));
            _ = services.AddTransient(typeof(ILoggerService), typeof(LoggerService));
            _ = services.AddTransient(typeof(IPhotoService), typeof(PhotoService));
            _ = services.AddScoped(typeof(IAuthenticationService), typeof(AuthenticationService));
            _ = services.AddTransient<SignInManager<User>, SignInManager<User>>();
            _ = services.AddTransient<UserManager<User>, UserManager<User>>();
            _ = services.AddTransient<RoleManager<Role>, RoleManager<Role>>();
            //huy_dev
            _ = services.AddScoped(typeof(IProductService), typeof(ProductService));
            _ = services.AddScoped(typeof(ICategoryService), typeof(CategoryService));
            _ = services.AddScoped(typeof(IWishlistService), typeof(WishlistService));

            //PhucThinh-dev
            _ = services.AddScoped(typeof(IStoreService), typeof(StoreService));
            _ = services.AddScoped(typeof(IReviewService), typeof(ReviewService));

            #endregion Services

            #region Mapper

            _ = services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

            #endregion Mapper

            #region Cloudinary

            _ = services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));

            #endregion Cloudinary

            return services;
        }

        private static void ConfigureLog4Net(IConfigurationSection log4NetConfig)
        {
            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();

            Logger rootLogger = hierarchy.Root;
            rootLogger.Level = log4net.Core.Level.All;

            IConfigurationSection appenders = log4NetConfig.GetSection("Appenders");
            foreach (IConfigurationSection? appenderSection in appenders.GetChildren())
            {
                Type? appenderType = Type.GetType(appenderSection.GetValue<string>("Type"));
                if (appenderType == null)
                {
                    continue;
                }

                AppenderSkeleton? appender = (AppenderSkeleton)Activator.CreateInstance(appenderType)!;
                if (appender == null)
                {
                    continue;
                }

                if (appender is RollingFileAppender rollingFileAppender)
                {
                    rollingFileAppender.File = appenderSection.GetValue<string>("File");
                    rollingFileAppender.DatePattern = appenderSection.GetValue<string>("DatePattern");
                    rollingFileAppender.StaticLogFileName = appenderSection.GetValue<bool>("StaticLogFileName");
                    rollingFileAppender.AppendToFile = appenderSection.GetValue<bool>("AppendToFile");
                    rollingFileAppender.RollingStyle = (RollingFileAppender.RollingMode)Enum.Parse(typeof(RollingFileAppender.RollingMode), appenderSection.GetValue<string>("RollingStyle"));
                    rollingFileAppender.MaxSizeRollBackups = appenderSection.GetValue<int>("MaxSizeRollBackups");
                    rollingFileAppender.MaximumFileSize = appenderSection.GetValue<string>("MaximumFileSize");

                    Type? layoutType = Type.GetType(appenderSection.GetSection("Layout").GetValue<string>("Type"));
                    if (layoutType != null)
                    {
                        SerializedLayout? layout = (SerializedLayout)Activator.CreateInstance(layoutType)!;
                        Type? decoratorType = Type.GetType(appenderSection.GetSection("Layout:Decorator").GetValue<string>("Type"));
                        if (decoratorType != null)
                        {
                            log4net.Layout.Decorators.StandardTypesDecorator? decorator = (log4net.Layout.Decorators.StandardTypesDecorator)Activator.CreateInstance(decoratorType)!;
                            layout.AddDecorator(decorator);
                        }
                        layout.ActivateOptions();
                        rollingFileAppender.Layout = layout;
                    }

                    rollingFileAppender.ActivateOptions();
                }

                hierarchy.Root.AddAppender(appender);
            }

            hierarchy.Configured = true;

        }



    }
}