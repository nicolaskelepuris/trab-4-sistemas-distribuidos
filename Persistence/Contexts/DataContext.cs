using Domain.Entities;
using Domain.Entities.Base;
using Microsoft.EntityFrameworkCore;
using Persistence.ModelConfigurations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Persistence.Contexts
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public virtual DbSet<AppUser> AppUsers { get; set; } = null!;
        public virtual DbSet<Transaction> Transactions { get; set; } = null!;
        public virtual DbSet<OrganRequest> OrganRequests { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            if (builder is null) throw new ArgumentNullException(nameof(builder));

            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new TransactionModelConfiguration());
            builder.ApplyConfiguration(new AppUserModelConfiguration());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((BaseEntity)entityEntry.Entity).SetUpdatedAt();

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).SetCreatedAt();
                }
            }

            return await base.SaveChangesAsync();
        }
    }
}
