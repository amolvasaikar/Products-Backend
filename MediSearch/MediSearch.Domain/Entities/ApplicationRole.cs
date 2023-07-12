using MediSearch.Domain.Common;
using System.Security.Claims;

namespace MediSearch.Domain.Entities
{
    public class ApplicationRole : BaseEntity, IAuditableEntity
    {
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public string? Description { get; set; }
        public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
        public DateTime? DateTimeCreated { get; set; }
        public DateTime? DateTimeUpdated { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public override string ToString()
        {
            return Name;
        }

        public virtual ICollection<ApplicationUserRole> Users { get; set; }

        public virtual ICollection<ApplicationRoleClaim> Claims { get; set; }
    }

    public class ApplicationRoleClaim
    {
        public int Id { get; set; }
        public long RoleId { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
        public Claim ToClaim()
        {
            return new Claim(ClaimType, ClaimValue);
        }
        public void InitializeFromClaim(Claim other)
        {
            ClaimType = other?.Type;
            ClaimValue = other?.Value;
        }
        public virtual ApplicationRole Role { get; set; }
    }
}
