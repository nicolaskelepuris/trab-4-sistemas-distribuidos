using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.ModelConfigurations;

    public class AppUserModelConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Cpf).IsRequired().HasMaxLength(11).IsFixedLength();
            builder.HasIndex(x => x.Cpf).IsUnique();

            builder.HasMany(x => x.OrganRequests).WithOne(x => x.Requester);
        }
    }
