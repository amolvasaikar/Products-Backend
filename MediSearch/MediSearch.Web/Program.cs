using MediSearch.Persistence.Context;
using MediSearch.Persistence.IRepositories;
using MediSearch.Persistence.Repositories;
using MediSearch.Persistence.SeedDatabase;
using Microsoft.EntityFrameworkCore;

namespace MediSearch.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            AddServices(builder);// Add services to the container.

            var app = builder.Build();
            ConfigureRequestPipeline(app); // Configure the HTTP request pipeline.

            SeedDatabase(app); //Seed initial database

            app.Run();
        }

        static void AddServices(WebApplicationBuilder builder)
        {

            var Configuration = builder.Configuration;
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                            throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            // var authServerUrl = builder.Configuration["AuthServerUrl"].TrimEnd('/');


            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(connectionString));



            builder.Services.AddScoped<IAccountManager, AccountManager>();
            builder.Services.AddTransient<IDatabaseInitializer, DatabaseInitializer>();
            // Add services to the container.

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
                    //logger.LogCritical(LoggingEvents.INIT_DATABASE, ex, LoggingEvents.INIT_DATABASE.Name);
                    throw ex;
                }
            }
        }
    }
}