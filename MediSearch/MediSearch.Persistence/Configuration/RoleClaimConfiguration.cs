using MediSearch.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediSearch.Persistence.Configuration
{
    public class RoleClaimConfiguration : IEntityTypeConfiguration<ApplicationRoleClaim>
    {
        public void Configure(EntityTypeBuilder<ApplicationRoleClaim> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.HasOne(roleClaim => roleClaim.Role)
                .WithMany(role => role.Claims)
                .HasForeignKey(roleClaim => roleClaim.RoleId);

            builder.ToTable("ApplicationRoleClaim");
        }
    }
}
