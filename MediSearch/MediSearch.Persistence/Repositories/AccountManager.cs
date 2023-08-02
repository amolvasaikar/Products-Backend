using MediSearch.Domain.Entities;
using MediSearch.Persistence.Context;
using MediSearch.Persistence.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace MediSearch.Persistence.Repositories
{
    public class AccountManager : IAccountManager
    {
        private readonly ApplicationDbContext _context;
        public AccountManager(
            ApplicationDbContext context)
        {
            _context = context;


        }
        public async Task<ApplicationUser> GetUserByIdAsync(long userId)
        {
            return await _context.ApplicationUsers.SingleOrDefaultAsync(s => s.Id == userId);

        }

        public async Task<ApplicationUser> GetUserByUserNameAsync(string userName)
        {
            return await _context.ApplicationUsers.SingleOrDefaultAsync(s => s.UserName.Equals(userName, StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            return await _context.ApplicationUsers
                .SingleOrDefaultAsync(s => s.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase));
        }

        public async Task<IList<string>> GetUserRolesAsync(ApplicationUser user)
        {
            var appuser = await _context.ApplicationUsers
                .Include(u => u.Roles)
                .Where(u => u.Id == user.Id)
                .SingleOrDefaultAsync();

            if (user == null)
                return null;

            var userRoleIds = user.Roles.Select(r => r.RoleId).ToList();

            return await _context.ApplicationRoles
                .Where(r => userRoleIds.Contains(r.Id))
                .Select(r => r.Name)
                .ToListAsync();
        }


        public async Task<(ApplicationUser User, string[] Roles)?> GetUserAndRolesAsync(long userId)
        {
            var user = await _context.ApplicationUsers
                .Include(u => u.Roles)
                .Where(u => u.Id == userId)
                .SingleOrDefaultAsync();

            if (user == null)
                return null;

            var userRoleIds = user.Roles.Select(r => r.RoleId).ToList();

            var roles = await _context.ApplicationRoles
                .Where(r => userRoleIds.Contains(r.Id))
                .Select(r => r.Name)
                .ToArrayAsync();

            return (user, roles);
        }


        public async Task<List<(ApplicationUser User, string[] Roles)>> GetUsersAndRolesAsync(int page, int pageSize)
        {
            IQueryable<ApplicationUser> usersQuery = _context.ApplicationUsers
                .Include(u => u.Roles)
                .OrderBy(u => u.UserName);

            if (page != -1)
                usersQuery = usersQuery.Skip((page - 1) * pageSize);

            if (pageSize != -1)
                usersQuery = usersQuery.Take(pageSize);

            var users = await usersQuery.ToListAsync();

            var userRoleIds = users.SelectMany(u => u.Roles.Select(r => r.RoleId)).ToList();

            var roles = await _context.ApplicationRoles
                .Where(r => userRoleIds.Contains(r.Id))
                .ToArrayAsync();

            return users
                .Select(u => (u, roles.Where(r => u.Roles.Select(ur => ur.RoleId).Contains(r.Id)).Select(r => r.Name).ToArray()))
                .ToList();
        }


        public async Task<bool> CreateUserAsync(ApplicationUser user, IEnumerable<string> userroles, string password)
        {
            var newuser = await _context.AddAsync(user);
            await _context.SaveChangesAsync();

            try
            {
                var roles = await _context.ApplicationRoles
                .Where(r => userroles.Contains(r.Id.ToString()))
                .Select(r => r.Id)
                .ToArrayAsync();

                foreach (var item in roles)
                {
                    ApplicationUserRole applicationUserRole =
                        new ApplicationUserRole
                        {
                            RoleId = item,
                            UserId = newuser.Entity.Id
                        };

                    await _context.AddAsync(applicationUserRole);
                    await _context.SaveChangesAsync();
                }

            }
            catch
            {
                await DeleteUserAsync(user);
                throw;
            }
            return true;
        }


        public async Task<bool> UpdateUserAsync(ApplicationUser user)
        {
            return await UpdateUserAsync(user, null);
        }


        public async Task<bool> UpdateUserAsync(ApplicationUser user, IEnumerable<string> roles)
        {
            var result = _context.Update(user);
            await _context.SaveChangesAsync();

            if (roles != null)
            {
                var userRoles = await GetUserRolesAsync(user);

                var rolesToRemove = userRoles.Except(roles).ToArray();
                var rolesToAdd = roles.Except(userRoles).Distinct().ToArray();

                if (rolesToRemove.Any())
                {
                    foreach (var item in rolesToRemove)
                    {
                        var role = await GetRoleByNameAsync(item);
                        var applicationrole = await _context.ApplicationUserRoles.SingleOrDefaultAsync(s => s.UserId == user.Id && s.RoleId == role.Id);
                        _context.ApplicationUserRoles.Remove(applicationrole);
                        await _context.SaveChangesAsync();
                    }
                }

                if (rolesToAdd.Any())
                {
                    foreach (var item in rolesToAdd)
                    {
                        var role = await GetRoleByNameAsync(item);
                        ApplicationUserRole applicationUserRole =
                        new ApplicationUserRole
                        {
                            RoleId = role.Id,
                            UserId = user.Id
                        };

                        await _context.AddAsync(applicationUserRole);
                        await _context.SaveChangesAsync();
                    }
                }
            }

            return true;
        }


        public async Task<bool> ResetPasswordAsync(ApplicationUser user, string newPassword)
        {
            var result = _context.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdatePasswordAsync(ApplicationUser user, string currentPassword, string newPassword)
        {
            var result = _context.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            var result = _context.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var appuser = await _context.ApplicationUsers.SingleOrDefaultAsync(u => u.Id.ToString() == userId);

            _context.Remove(appuser);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> DeleteUserAsync(ApplicationUser user)
        {
            _context.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ApplicationRole> GetRoleByIdAsync(long roleId)
        {
            return await _context.ApplicationRoles.SingleOrDefaultAsync(u => u.Id.ToString() == roleId.ToString());

        }


        public async Task<ApplicationRole> GetRoleByNameAsync(string roleName)
        {
            return await _context.ApplicationRoles.SingleOrDefaultAsync(u => u.Name == roleName);
        }


        public async Task<ApplicationRole> GetRoleLoadRelatedAsync(string roleName)
        {
            var role = await _context.ApplicationRoles
                .Include(r => r.Claims)
                .Include(r => r.Users)
                .Where(r => r.Name == roleName)
                .SingleOrDefaultAsync();

            return role;
        }
        public async Task<List<ApplicationRole>> GetRolesLoadRelatedAsync(int page, int pageSize)
        {
            IQueryable<ApplicationRole> rolesQuery = _context.ApplicationRoles
                .Include(r => r.Claims)
                .Include(r => r.Users)
                .OrderBy(r => r.Name);

            if (page != -1)
                rolesQuery = rolesQuery.Skip((page - 1) * pageSize);

            if (pageSize != -1)
                rolesQuery = rolesQuery.Take(pageSize);

            var roles = await rolesQuery.ToListAsync();

            return roles;
        }
        public async Task<bool> CreateRoleAsync(ApplicationRole role, IEnumerable<string> claims)
        {
            //if (claims == null)
            //    claims = new string[] { };

            //string[] invalidClaims = claims.Where(c => ApplicationPermissions.GetPermissionByValue(c) == null).ToArray();
            //if (invalidClaims.Any())
            //    return false;

            await _context.AddAsync(role);
            await _context.SaveChangesAsync();
            //role = await _roleManager.FindByNameAsync(role.Name);

            //foreach (string claim in claims.Distinct())
            //{
            //    result = await this._roleManager.AddClaimAsync(role, new Claim(ClaimConstants.Permission, ApplicationPermissions.GetPermissionByValue(claim)));

            //    if (!result.Succeeded)
            //    {
            //        await DeleteRoleAsync(role);
            //        return false;
            //    }
            //}

            return true;
        }

        public async Task<bool> UpdateRoleAsync(ApplicationRole role, IEnumerable<string> claims)
        {
            //if (claims != null)
            //{
            //    string[] invalidClaims = claims.Where(c => ApplicationPermissions.GetPermissionByValue(c) == null).ToArray();
            //    if (invalidClaims.Any())
            //        return false;
            //}


            _context.Update(role);
            await _context.SaveChangesAsync();


            //if (claims != null)
            //{
            //    var roleClaims = (await _roleManager.GetClaimsAsync(role)).Where(c => c.Type == ClaimConstants.Permission);
            //    var roleClaimValues = roleClaims.Select(c => c.Value).ToArray();

            //    var claimsToRemove = roleClaimValues.Except(claims).ToArray();
            //    var claimsToAdd = claims.Except(roleClaimValues).Distinct().ToArray();

            //    if (claimsToRemove.Any())
            //    {
            //        foreach (string claim in claimsToRemove)
            //        {
            //            result = await _roleManager.RemoveClaimAsync(role, roleClaims.Where(c => c.Value == claim).FirstOrDefault());
            //            if (!result.Succeeded)
            //                return false;
            //        }
            //    }

            //    if (claimsToAdd.Any())
            //    {
            //        foreach (string claim in claimsToAdd)
            //        {
            //            result = await _roleManager.AddClaimAsync(role, new Claim(ClaimConstants.Permission, ApplicationPermissions.GetPermissionByValue(claim)));
            //            if (!result.Succeeded)
            //                return false;
            //        }
            //    }
            //}

            return true;
        }
        public async Task<bool> DeleteRoleAsync(string roleName)
        {
            var role = await _context.ApplicationRoles.SingleOrDefaultAsync(u => u.Name == roleName);
            _context.Remove(role);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteRoleAsync(ApplicationRole role)
        {
            _context.Remove(role);
            await _context.SaveChangesAsync();
            return true;
        }
    }

}
