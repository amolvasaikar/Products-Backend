using MediSearch.Domain.Entities;

namespace MediSearch.Persistence.IRepositories
{
    public interface IAccountManager
    {
        Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
        Task<bool> CreateRoleAsync(ApplicationRole role, IEnumerable<string> claims);
        Task<bool> CreateUserAsync(ApplicationUser user, IEnumerable<string> roles, string password);
        Task<bool> DeleteRoleAsync(ApplicationRole role);
        Task<bool> DeleteRoleAsync(string roleName);
        Task<bool> DeleteUserAsync(ApplicationUser user);
        Task<bool> DeleteUserAsync(string userId);
        Task<ApplicationRole> GetRoleByIdAsync(long roleId);
        Task<ApplicationRole> GetRoleByNameAsync(string roleName);
        Task<ApplicationRole> GetRoleLoadRelatedAsync(string roleName);
        Task<List<ApplicationRole>> GetRolesLoadRelatedAsync(int page, int pageSize);
        Task<(ApplicationUser User, string[] Roles)?> GetUserAndRolesAsync(long userId);
        Task<ApplicationUser> GetUserByEmailAsync(string email);
        Task<ApplicationUser> GetUserByIdAsync(long userId);
        Task<ApplicationUser> GetUserByUserNameAsync(string userName);
        Task<IList<string>> GetUserRolesAsync(ApplicationUser user);
        Task<List<(ApplicationUser User, string[] Roles)>> GetUsersAndRolesAsync(int page, int pageSize);
        Task<bool> ResetPasswordAsync(ApplicationUser user, string newPassword);

        Task<bool> UpdatePasswordAsync(ApplicationUser user, string currentPassword, string newPassword);
        Task<bool> UpdateRoleAsync(ApplicationRole role, IEnumerable<string> claims);
        Task<bool> UpdateUserAsync(ApplicationUser user);
        Task<bool> UpdateUserAsync(ApplicationUser user, IEnumerable<string> roles);
    }
}
