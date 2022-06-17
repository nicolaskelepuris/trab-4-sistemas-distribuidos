using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Entities.Base;
using Domain.Interfaces;
using Persistence.Contexts;

namespace Persistence.Data;
public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _context;
    private Dictionary<string, object> _repositories;

    public UnitOfWork(DataContext context)
    {
        _context = context;
        _repositories = new Dictionary<string, object>();
    }

    public async Task<int> Complete()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
    {
        var typeName = typeof(TEntity).Name;

        if (!_repositories.ContainsKey(typeName))
        {
            var repositoryType = typeof(GenericRepository<>);

            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context, new SpecificationEvaluator<TEntity>());
            if (repositoryInstance == null) throw new Exception($"Could not create generic repository for {typeName}");

            _repositories.Add(typeName, repositoryInstance);
        }

        return (GenericRepository<TEntity>)_repositories[typeName];
    }
}