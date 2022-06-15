using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.ModelConfigurations;
using System;

namespace Persistence.Contexts
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public virtual DbSet<Transaction> Transactions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            if (builder is null) throw new ArgumentNullException(nameof(builder));

            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new TransactionModelConfiguration());
        }
    }
}
