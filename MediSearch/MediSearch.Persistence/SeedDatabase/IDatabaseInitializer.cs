using MediSearch.Domain.Entities;
using MediSearch.Infrastructure;
using MediSearch.Persistence.Context;
using MediSearch.Persistence.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MediSearch.Persistence.SeedDatabase
{
    public interface IDatabaseInitializer
    {
        Task SeedAsync();
    }

    public class DatabaseInitializer : IDatabaseInitializer
    {
        readonly ApplicationDbContext _context;
        readonly IAccountManager _accountManager;
        readonly ILogger _logger;

        public DatabaseInitializer(ApplicationDbContext context, IAccountManager accountManager, ILogger<DatabaseInitializer> logger)
        {
            _accountManager = accountManager;
            _context = context;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            await _context.Database.MigrateAsync().ConfigureAwait(false);
            await SeedDefaultUsersAsync();
        }

        private async Task SeedDefaultUsersAsync()
        {
            if (!await _context.ApplicationUsers.AnyAsync())
            {
                _logger.LogInformation("Generating inbuilt accounts");

                const string adminRoleName = "administrator";
                const string userRoleName = "user";

                await EnsureRoleAsync(adminRoleName, "Default administrator", ApplicationPermissions.GetAllPermissionValues());
                await EnsureRoleAsync(userRoleName, "Default user", new string[] { });

                await CreateUserAsync("admin", "!TMtm123", "Inbuilt Administrator", "admin@ebenmonney.com", "+1 (123) 000-0000", new string[] { adminRoleName });
                await CreateUserAsync("user", "!TMtm123", "Inbuilt Standard User", "user@ebenmonney.com", "+1 (123) 000-0001", new string[] { userRoleName });

                _logger.LogInformation("Inbuilt account generation completed");
            }
        }

        private async Task EnsureRoleAsync(string roleName, string description, string[] claims)
        {
            if ((await _accountManager.GetRoleByNameAsync(roleName)) == null)
            {
                _logger.LogInformation($"Generating default role: {roleName}");

                ApplicationRole applicationRole = new ApplicationRole
                {
                    Name = roleName,
                    NormalizedName = roleName.ToUpper(),
                    Description = description,

                };


                var user = _context.AddAsync(applicationRole);
                _context.SaveChangesAsync();
            }
        }

        private async Task<ApplicationUser> CreateUserAsync(string userName, string password, string fullName, string email, string phoneNumber, string[] roles)
        {
            _logger.LogInformation($"Generating default user: {userName}");
            var hasher = new PasswordHasher<string>();
            ApplicationUser applicationUser = new ApplicationUser(userName, email, phoneNumber);
            applicationUser.PasswordHash = hasher.HashPassword(userName, password);
            applicationUser.FullName = fullName;


            var user = _context.AddAsync(applicationUser);
            _context.SaveChangesAsync();

            return applicationUser;
        }
    }

}
