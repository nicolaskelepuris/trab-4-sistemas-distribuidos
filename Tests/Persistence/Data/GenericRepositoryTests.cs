using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Domain.Entities.Base;
using Domain.Interfaces;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Persistence.Data;
using Persistence.Interfaces;
using Xunit;

namespace Tests.Persistence.Data;
public class GenericRepositoryTests
{
    public class SomeEntity : BaseEntity
    {
    }

    public class SomeDbContext : DbContext
    {
        public SomeDbContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<SomeEntity> Entities { get; set; } = null!;
    }

    private DbContextOptions<SomeDbContext> dbContextOptions
    {
        get
        {
            var optionsBuilder = new DbContextOptionsBuilder<SomeDbContext>();
            optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            return optionsBuilder.Options;
        }
    }

    private Mock<ISpecificationEvaluator<SomeEntity>> CreateSpecificationEvaluatorMock(SomeDbContext dbContext)
    {
        var mock = new Mock<ISpecificationEvaluator<SomeEntity>>();
        mock.Setup(_ => _.Evaluate(dbContext.Entities, It.IsAny<ISpecification<SomeEntity>>())).Returns(dbContext.Entities);
        mock.Setup(_ => _.EvaluateForCount(dbContext.Entities, It.IsAny<ISpecification<SomeEntity>>())).Returns(dbContext.Entities);

        return mock;
    }

    private Mock<ISpecification<SomeEntity>> specificationMock
    {
        get
        {
            var mock = new Mock<ISpecification<SomeEntity>>();
            mock.Setup(_ => _.Criteria).Returns<Expression<Func<SomeEntity, bool>>>(null);
            mock.Setup(_ => _.OrderBy).Returns<Expression<Func<SomeEntity, object>>>(null);
            mock.Setup(_ => _.OrderByDescending).Returns<Expression<Func<SomeEntity, object>>>(null);
            mock.Setup(_ => _.Includes).Returns(new List<Expression<Func<SomeEntity, object>>>());
            mock.Setup(_ => _.IncludesByString).Returns(new List<string>());
            mock.Setup(_ => _.IsPaginationEnabled).Returns(false);
            return mock;
        }
    }

    [Fact]
    public void Constructor_Valid_ShouldConstruct()
    {
        var dbContext = new SomeDbContext(dbContextOptions);

        var genericRepository = () => new GenericRepository<SomeEntity>(dbContext, CreateSpecificationEvaluatorMock(dbContext).Object, new Mock<IDateTimeProvider>().Object);

        genericRepository.Should().NotThrow();
    }

    [Fact]
    public void Constructor_NullContext_ShouldThrow()
    {
        var dbContext = new SomeDbContext(dbContextOptions);
        var genericRepository = () => new GenericRepository<SomeEntity>(context: null!, CreateSpecificationEvaluatorMock(dbContext).Object, new Mock<IDateTimeProvider>().Object);

        genericRepository.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void Add_ValidEntity_ShouldEnterAddedState()
    {
        var dbContext = new SomeDbContext(dbContextOptions);
        var genericRepository = new GenericRepository<SomeEntity>(dbContext, CreateSpecificationEvaluatorMock(dbContext).Object, new Mock<IDateTimeProvider>().Object);
        var entity = new SomeEntity();

        genericRepository.Add(entity);

        dbContext.Entry(entity).State.Should().Be(EntityState.Added);
        dbContext.Entities.Should().BeEmpty();
    }

    [Fact]
    public void Add_ValidEntity_ShouldSetCreatedAt()
    {
        var dateTimeProviderMock = new Mock<IDateTimeProvider>();
        var utcNow = DateTime.UtcNow;
        dateTimeProviderMock.Setup(x => x.UtcNow).Returns(utcNow);
        var dbContext = new SomeDbContext(dbContextOptions);
        var genericRepository = new GenericRepository<SomeEntity>(dbContext, CreateSpecificationEvaluatorMock(dbContext).Object, dateTimeProviderMock.Object);
        var entity = new SomeEntity();

        genericRepository.Add(entity);

        entity.CreatedAt.Should().Be(utcNow);
    }

    [Fact]
    public void Add_NullEntity_ShouldThrow()
    {
        var dbContext = new SomeDbContext(dbContextOptions);
        var genericRepository = new GenericRepository<SomeEntity>(dbContext, CreateSpecificationEvaluatorMock(dbContext).Object, new Mock<IDateTimeProvider>().Object);

        var add = () => genericRepository.Add(entity: null!);

        add.Should().ThrowExactly<ArgumentNullException>();
    }   

    [Fact]
    public void Update_ValidEntity_ShouldEnterModifiedState()
    {
        var dbContext = new SomeDbContext(dbContextOptions);
        var genericRepository = new GenericRepository<SomeEntity>(dbContext, CreateSpecificationEvaluatorMock(dbContext).Object, new Mock<IDateTimeProvider>().Object);
        var entity = new SomeEntity();

        genericRepository.Update(entity);

        dbContext.Entry(entity).State.Should().Be(EntityState.Modified);
        dbContext.Entities.Should().BeEmpty();
    }

    [Fact]
    public void Update_ValidEntity_ShouldSetUpdatedAt()
    {
        var dateTimeProviderMock = new Mock<IDateTimeProvider>();
        var utcNow = DateTime.UtcNow;
        dateTimeProviderMock.Setup(x => x.UtcNow).Returns(utcNow);
        var dbContext = new SomeDbContext(dbContextOptions);
        var genericRepository = new GenericRepository<SomeEntity>(dbContext, CreateSpecificationEvaluatorMock(dbContext).Object, dateTimeProviderMock.Object);
        var entity = new SomeEntity();

        genericRepository.Update(entity);

        entity.UpdatedAt.Should().Be(utcNow);
    }

    [Fact]
    public void Update_NullEntity_ShouldThrow()
    {
        var dbContext = new SomeDbContext(dbContextOptions);
        var genericRepository = new GenericRepository<SomeEntity>(dbContext, CreateSpecificationEvaluatorMock(dbContext).Object, new Mock<IDateTimeProvider>().Object);

        var update = () => genericRepository.Update(entity: null!);

        update.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public async Task Delete_ValidEntity_ShouldEnterDeletedState()
    {
        var dbContext = new SomeDbContext(dbContextOptions);
        var genericRepository = new GenericRepository<SomeEntity>(dbContext, CreateSpecificationEvaluatorMock(dbContext).Object, new Mock<IDateTimeProvider>().Object);
        var entity = new SomeEntity();
        await dbContext.Entities.AddAsync(entity);
        await dbContext.SaveChangesAsync();

        genericRepository.Delete(entity);

        dbContext.Entry(entity).State.Should().Be(EntityState.Deleted);
        dbContext.Entities.Should().HaveCount(1);
    }

    [Fact]
    public void Delete_NullEntity_ShouldThrow()
    {
        var dbContext = new SomeDbContext(dbContextOptions);
        var genericRepository = new GenericRepository<SomeEntity>(dbContext, CreateSpecificationEvaluatorMock(dbContext).Object, new Mock<IDateTimeProvider>().Object);

        var delete = () => genericRepository.Delete(entity: null!);

        delete.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public async Task GetEntityByIdAsync_ValidId_ShouldGetEntity()
    {
        var dbContext = new SomeDbContext(dbContextOptions);
        var genericRepository = new GenericRepository<SomeEntity>(dbContext, CreateSpecificationEvaluatorMock(dbContext).Object, new Mock<IDateTimeProvider>().Object);
        var entity = new SomeEntity();
        await dbContext.Entities.AddAsync(entity);
        await dbContext.SaveChangesAsync();
        var id = entity.Id;

        var entityFound = await genericRepository.GetEntityByIdAsync(id);

        entityFound.Should().NotBeNull();
        entityFound!.Id.Should().Be(id);
    }

    [Fact]
    public async Task GetEntityByIdAsync_NotFoundEntity_ShouldReturnNull()
    {
        var dbContext = new SomeDbContext(dbContextOptions);
        var genericRepository = new GenericRepository<SomeEntity>(dbContext, CreateSpecificationEvaluatorMock(dbContext).Object, new Mock<IDateTimeProvider>().Object);

        var entityFound = await genericRepository.GetEntityByIdAsync(Guid.NewGuid());

        entityFound.Should().BeNull();
    }

    [Fact]
    public async Task ListAllAsync_ContainItems_ShouldList()
    {
        var dbContext = new SomeDbContext(dbContextOptions);
        var genericRepository = new GenericRepository<SomeEntity>(dbContext, CreateSpecificationEvaluatorMock(dbContext).Object, new Mock<IDateTimeProvider>().Object);
        var entities = new List<SomeEntity>()
        {
            new SomeEntity(),
            new SomeEntity(),
            new SomeEntity()
        };
        await dbContext.Entities.AddRangeAsync(entities);
        await dbContext.SaveChangesAsync();

        var entitiesFound = await genericRepository.ListAllAsync();

        entitiesFound.Should().HaveCount(entities.Count);
    }

    [Fact]
    public async Task ListAllAsync_Empty_ShouldReturnEmptyList()
    {
        var dbContext = new SomeDbContext(dbContextOptions);
        var genericRepository = new GenericRepository<SomeEntity>(dbContext, CreateSpecificationEvaluatorMock(dbContext).Object, new Mock<IDateTimeProvider>().Object);

        var entitiesFound = await genericRepository.ListAllAsync();

        entitiesFound.Should().BeEmpty();
    }

    [Fact]
    public async Task GetEntityAsyncWithSpec_NotFound_ShouldReturnNull()
    {
        var dbContext = new SomeDbContext(dbContextOptions);
        var genericRepository = new GenericRepository<SomeEntity>(dbContext, CreateSpecificationEvaluatorMock(dbContext).Object, new Mock<IDateTimeProvider>().Object);

        var entity = await genericRepository.GetEntityAsyncWithSpec(new Mock<ISpecification<SomeEntity>>().Object);

        entity.Should().BeNull();
    }

    [Fact]
    public async Task GetEntityAsyncWithSpec_ShouldEvaluateSpecification()
    {
        var dbContext = new SomeDbContext(dbContextOptions);
        var specificationMock = new Mock<ISpecification<SomeEntity>>();
        var specificationEvaluatorMock = CreateSpecificationEvaluatorMock(dbContext);
        var genericRepository = new GenericRepository<SomeEntity>(dbContext, specificationEvaluatorMock.Object, new Mock<IDateTimeProvider>().Object);

        await genericRepository.GetEntityAsyncWithSpec(specificationMock.Object);

        specificationEvaluatorMock.Verify(_ => _.Evaluate(dbContext.Entities, specificationMock.Object), Times.Once);
    }

    [Fact]
    public async Task GetEntityAsyncWithSpec_EntityFound_ShouldReturnEntity()
    {
        var dbContext = new SomeDbContext(dbContextOptions);
        var entities = new List<SomeEntity>()
        {
            new SomeEntity()
        };
        dbContext.Entities.AddRange(entities);
        await dbContext.SaveChangesAsync();
        var genericRepository = new GenericRepository<SomeEntity>(dbContext, CreateSpecificationEvaluatorMock(dbContext).Object, new Mock<IDateTimeProvider>().Object);

        var entity = await genericRepository.GetEntityAsyncWithSpec(specificationMock.Object);

        entity.Should().Be(entities[0]);
    }

    [Fact]
    public async Task CountAsync_ShouldEvaluateSpecificationForCount()
    {
        var dbContext = new SomeDbContext(dbContextOptions);
        var specificationMock = new Mock<ISpecification<SomeEntity>>();
        var specificationEvaluatorMock = CreateSpecificationEvaluatorMock(dbContext);
        var genericRepository = new GenericRepository<SomeEntity>(dbContext, specificationEvaluatorMock.Object, new Mock<IDateTimeProvider>().Object);

        await genericRepository.CountAsync(specificationMock.Object);

        specificationEvaluatorMock.Verify(_ => _.EvaluateForCount(dbContext.Entities, specificationMock.Object), Times.Once);
    }

    [Fact]
    public async Task CountAsync_NoEntity_ShouldReturnZero()
    {
        var dbContext = new SomeDbContext(dbContextOptions);
        var specificationMock = new Mock<ISpecification<SomeEntity>>();
        var specificationEvaluatorMock = CreateSpecificationEvaluatorMock(dbContext);
        var genericRepository = new GenericRepository<SomeEntity>(dbContext, specificationEvaluatorMock.Object, new Mock<IDateTimeProvider>().Object);

        var count = await genericRepository.CountAsync(specificationMock.Object);

        count.Should().Be(0);
    }

    [Fact]
    public async Task CountAsync_EntitiesFound_ShouldReturnEntitiesCount()
    {
        var dbContext = new SomeDbContext(dbContextOptions);
        var entities = new List<SomeEntity>()
        {
            new SomeEntity(),
            new SomeEntity(),
            new SomeEntity()
        };
        dbContext.Entities.AddRange(entities);
        await dbContext.SaveChangesAsync();
        var specificationMock = new Mock<ISpecification<SomeEntity>>();
        var specificationEvaluatorMock = CreateSpecificationEvaluatorMock(dbContext);
        var genericRepository = new GenericRepository<SomeEntity>(dbContext, specificationEvaluatorMock.Object, new Mock<IDateTimeProvider>().Object);

        var count = await genericRepository.CountAsync(specificationMock.Object);

        count.Should().Be(entities.Count);
    }

    [Fact]
    public async Task ListAsyncWithSpec_ShouldEvaluateSpecification()
    {
        var dbContext = new SomeDbContext(dbContextOptions);
        var specificationMock = new Mock<ISpecification<SomeEntity>>();
        var specificationEvaluatorMock = CreateSpecificationEvaluatorMock(dbContext);
        var genericRepository = new GenericRepository<SomeEntity>(dbContext, specificationEvaluatorMock.Object, new Mock<IDateTimeProvider>().Object);

        await genericRepository.ListAsyncWithSpec(specificationMock.Object);

        specificationEvaluatorMock.Verify(_ => _.Evaluate(dbContext.Entities, specificationMock.Object), Times.Once);
    }

    [Fact]
    public async Task ListAsyncWithSpec_NoEntities_ShouldReturnEmptyList()
    {
        var dbContext = new SomeDbContext(dbContextOptions);
        var specificationMock = new Mock<ISpecification<SomeEntity>>();
        var specificationEvaluatorMock = CreateSpecificationEvaluatorMock(dbContext);
        var genericRepository = new GenericRepository<SomeEntity>(dbContext, specificationEvaluatorMock.Object, new Mock<IDateTimeProvider>().Object);

        var entities = await genericRepository.ListAsyncWithSpec(specificationMock.Object);

        entities.Should().BeEmpty();
    }

    [Fact]
    public async Task ListAsyncWithSpec_EntitiesFound_ShouldReturnListWithEntities()
    {
        var dbContext = new SomeDbContext(dbContextOptions);
        var seed = new List<SomeEntity>()
        {
            new SomeEntity(),
            new SomeEntity(),
            new SomeEntity()
        };
        dbContext.Entities.AddRange(seed);
        await dbContext.SaveChangesAsync();
        var specificationMock = new Mock<ISpecification<SomeEntity>>();
        var specificationEvaluatorMock = CreateSpecificationEvaluatorMock(dbContext);
        var genericRepository = new GenericRepository<SomeEntity>(dbContext, specificationEvaluatorMock.Object, new Mock<IDateTimeProvider>().Object);

        var entities = await genericRepository.ListAsyncWithSpec(specificationMock.Object);

        entities.Should().HaveCount(seed.Count);
    }
}
