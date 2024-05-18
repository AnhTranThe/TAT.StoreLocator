using log4net;
using log4net.Config;
using Microsoft.OpenApi.Models;
using TAT.StoreLocator.API.MiddleWares;
using TAT.StoreLocator.Core.DI;
using TAT.StoreLocator.Infrastructure.DI;
using TAT.StoreLocator.Infrastructure.Persistence.Seeding;


namespace TAT.StoreLocator.API
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            #region Logging
            _ = XmlConfigurator.Configure(new FileInfo("log4net.config"));
            _ = builder.Services.AddSingleton(LogManager.GetLogger(typeof(Program)));


            #endregion
            _ = builder.Services.AddCore();
            _ = builder.Services.AddInfrastructure(builder.Configuration);
            _ = builder.Services.AddAutoMapper(typeof(Program));



            _ = builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            _ = builder.Services.AddEndpointsApiExplorer();

            _ = builder.Services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "TAT.StoreLocator.API", Version = "v1" });
                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
    });
                //string xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //opt.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                SeedData.InitializeAsync(app);
                _ = app.UseSwagger();
                _ = app.UseSwaggerUI();
            }


            //_ = app.UseSwagger();
            //_ = app.UseSwaggerUI();
            _ = app.UseStaticFiles();
            _ = app.UseCors("CorsPolicy");
            _ = app.UseHttpsRedirection();
            //_ = app.UseAuthentication();
            //_ = app.UseMiddleware<JwtMiddleWare>();
            //_ = app.UseAuthorization();
            _ = app.MapControllers();
            app.Run();
        }
    }
}
