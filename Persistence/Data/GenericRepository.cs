using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities.Base;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Interfaces;

namespace Persistence.Data;
public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
{
    private readonly DbContext _context;
    private readonly ISpecificationEvaluator<T> _specificationEvaluator;

    public GenericRepository(DbContext context, ISpecificationEvaluator<T> specificationEvaluator)
    {
        ArgumentNullException.ThrowIfNull(context);

        _context = context;
        _specificationEvaluator = specificationEvaluator;
    }

    public void Add(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _context.Set<T>().Add(entity);
    }

    public void Update(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        _context.Set<T>().Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
    }

    public void Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

    public async Task<T?> GetEntityByIdAsync(Guid id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    public async Task<IReadOnlyList<T>> ListAllAsync()
    {
        return await _context.Set<T>().ToListAsync();
    }

    public async Task<T?> GetEntityAsyncWithSpec(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).FirstOrDefaultAsync();
    }

    public async Task<int> CountAsync(ISpecification<T> spec)
    {
        return await ApplySpecificationForCount(spec).CountAsync();
    }

    public async Task<IReadOnlyList<T>> ListAsyncWithSpec(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).ToListAsync();
    }

    private IQueryable<T> ApplySpecification(ISpecification<T> spec)
    {
        return _specificationEvaluator.Evaluate(_context.Set<T>().AsQueryable(), spec);
    }

    private IQueryable<T> ApplySpecificationForCount(ISpecification<T> spec)
    {
        return _specificationEvaluator.EvaluateForCount(_context.Set<T>().AsQueryable(), spec);
    }
}
