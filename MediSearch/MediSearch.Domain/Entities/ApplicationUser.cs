using MediSearch.Domain.Common;
using System.Security.Claims;

namespace MediSearch.Domain.Entities
{
    public class ApplicationUser : BaseEntity, IAuditableEntity
    {
        public ApplicationUser(string username, string emailid, string phonenumber)
        {
            UserName = username;
            Email = emailid;
            PhoneNumber = phonenumber;
            NormalizedEmail = emailid.ToUpper();
            NormalizedUserName = username.ToUpper();
        }
        public virtual string FriendlyName
        {
            get
            {
                string friendlyName = string.IsNullOrWhiteSpace(FullName) ? UserName : FullName;

                if (!string.IsNullOrWhiteSpace(JobTitle))
                    friendlyName = $"{JobTitle} {friendlyName}";

                return friendlyName;
            }
        }
        public string? JobTitle { get; set; }
        public string? FullName { get; set; }
        public string? Configuration { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsLockedOut => this.LockoutEnabled && this.LockoutEnd >= DateTimeOffset.UtcNow;
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; } = Guid.NewGuid().ToString();
        public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
        public string? PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; } = 0;

        public DateTime? DateTimeCreated { get; set; }
        public DateTime? DateTimeUpdated { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        public virtual ICollection<ApplicationUserRole> Roles { get; set; }

        public virtual ICollection<ApplicationUserClaim> Claims { get; set; }
    }

    public class ApplicationUserRole
    {
        public long UserId { get; set; }
        public long RoleId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ApplicationRole Role { get; set; }
    }

    public class ApplicationUserClaim
    {
        public int Id { get; set; }
        public long UserId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public Claim ToClaim()
        {
            return new Claim(ClaimType, ClaimValue);
        }
        public void InitializeFromClaim(Claim claim)
        {
            ClaimType = claim.Type;
            ClaimValue = claim.Value;
        }

        public virtual ApplicationUser User { get; set; }
    }
}
