using System;
using System.Threading.Tasks;
using Domain.Entities.Base;
using Domain.Interfaces;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using Persistence.Data;
using Xunit;

namespace Tests.Persistence.Data;
public class UnitOfWorkTests
{
    public class SomeEntity : BaseEntity
    {
    }

    public class SomeDbContext : DataContext
    {
        public SomeDbContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public virtual DbSet<SomeEntity> Entities { get; set; } = null!;
    }

    private DbContextOptions<DataContext> dbContextOptions
    {
        get
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            return optionsBuilder.Options;
        }
    }

    [Fact]
    public async Task Complete_ShouldCallContextSaveChanges()
    {
        var savedChanges = false;
        var dbContext = new SomeDbContext(dbContextOptions);
        dbContext.SavedChanges += (sender, args) => savedChanges = true;
        var unitOfWork = new UnitOfWork(dbContext);

        await unitOfWork.Complete();

        savedChanges.Should().BeTrue();
    }

    [Fact]
    public void Dispose_ShouldDisposeContext()
    {
        var dbContext = new SomeDbContext(dbContextOptions);
        var unitOfWork = new UnitOfWork(dbContext);
        var useDbContextAction = () => dbContext.Database;

        unitOfWork.Dispose();

        useDbContextAction.Should().ThrowExactly<ObjectDisposedException>();
    }

    [Fact]
    public void Repository_ShouldCreateRepository()
    {
        var sut = new UnitOfWork(new SomeDbContext(dbContextOptions));

        var result = sut.Repository<SomeEntity>();

        result.Should().NotBeNull();
        result.Should().BeAssignableTo<IGenericRepository<SomeEntity>>();
    }
}