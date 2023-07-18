using MediSearch.Domain.Entities;
using MediSearch.Infrastructure;
using MediSearch.Persistence.Context;
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
        readonly ILogger _logger;

        public DatabaseInitializer(ApplicationDbContext context, ILogger<DatabaseInitializer> logger)
        {
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
            if (_context.ApplicationRoles.Where(s => s.Name.ToUpper() == roleName.ToLower()).Count() == 0)
            {
                _logger.LogInformation($"Generating default role: {roleName}");

                ApplicationRole applicationRole = new ApplicationRole
                {
                    Name = roleName,
                    NormalizedName = roleName.ToUpper(),
                    Description = description,

                };


                var user = _context.AddAsync(applicationRole);
                await _context.SaveChangesAsync();
            }
        }

        private async Task<ApplicationUser> CreateUserAsync(string userName, string password, string fullName, string email, string phoneNumber, string[] roles)
        {

            if (_context.ApplicationUsers.Where(s => s.UserName.ToUpper() == userName.ToLower()).Count() == 0)
            {
                _logger.LogInformation($"Generating default user: {userName}");
                var hasher = new PasswordHasher<string>();
                ApplicationUser applicationUser = new ApplicationUser()
                {
                    UserName = userName,
                    Email = email,
                    PhoneNumber = phoneNumber,
                    NormalizedEmail = email.ToUpper(),
                    NormalizedUserName = userName.ToUpper(),
                    PasswordHash = hasher.HashPassword(userName, password),
                    FullName = fullName
                };

                var user = await _context.AddAsync(applicationUser);
                await _context.SaveChangesAsync();


                foreach (var item in roles)
                {
                    var role = await _context.ApplicationRoles.SingleOrDefaultAsync(s => s.Name == item);

                    var userrole = new ApplicationUserRole { RoleId = role.Id, UserId = user.Entity.Id };
                    await _context.AddAsync(userrole);
                    await _context.SaveChangesAsync();
                }
                return applicationUser;
            }
            return null;

        }
    }

}
