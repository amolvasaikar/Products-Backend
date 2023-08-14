using MediSearch.Application;
using MediSearch.Infrastructure;
using MediSearch.Persistence.Context;
using MediSearch.Persistence.IRepositories;
using MediSearch.Persistence.Repositories;
using MediSearch.Persistence.SeedDatabase;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

namespace MediSearch.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var builder = WebApplication.CreateBuilder(args);
                AddServices(builder);// Add services to the container.

                var app = builder.Build();
                ConfigureRequestPipeline(app); // Configure the HTTP request pipeline.
                SeedDatabase(app); //Seed initial database
                app.Run();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static void AddServices(WebApplicationBuilder builder)
        {
            var logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(builder.Configuration)
                    .Enrich.FromLogContext()
                    .CreateLogger();
            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(logger);


            var Configuration = builder.Configuration;
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                            throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");


            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));
            // Add services to the container.

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                    ValidAudience = builder.Configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"])),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true
                };
            });

            builder.Services.AddApplicationServices();
            builder.Services.AddTransient<IDatabaseInitializer, DatabaseInitializer>();
            builder.Services.AddScoped<IAccountManager, AccountManager>();

            //builder.Services.AddSingleton<IHttpContextAccessor, IHttpContextAccessor>();
            //builder.Services.AddSingleton(typeof(IGenericRepository<>), typeof(GenericRepository<ApplicationDbContext>));
            //builder.Services.AddSingleton(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

        }

        static void ConfigureRequestPipeline(WebApplication app)
        {
            using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.Migrate();
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            //app.UseSerilogRequestLogging();
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
        }

        private static void SeedDatabase(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var databaseInitializer = services.GetRequiredService<IDatabaseInitializer>();
                    databaseInitializer.SeedAsync().Wait();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError("Enable to seed databse");
                    throw ex;
                }
            }
        }
    }
}