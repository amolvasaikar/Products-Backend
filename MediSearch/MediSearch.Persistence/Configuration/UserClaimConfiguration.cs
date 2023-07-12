using MediSearch.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MediSearch.Persistence.Configuration
{
    public class UserClaimConfiguration : IEntityTypeConfiguration<ApplicationUserClaim>
    {
        public void Configure(EntityTypeBuilder<ApplicationUserClaim> builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.HasOne(userClaim => userClaim.User)
                .WithMany(user => user.Claims)
                .HasForeignKey(userClaim => userClaim.UserId);

            builder.ToTable("ApplicationUserClaim");
        }
    }
}
