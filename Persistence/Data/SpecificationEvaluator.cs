using System;
using System.Linq;
using Domain.Entities.Base;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Interfaces;

namespace Persistence.Data;
public class SpecificationEvaluator<TEntity> : ISpecificationEvaluator<TEntity> where TEntity : BaseEntity
{
    private IQueryable<TEntity>? query;
    private ISpecification<TEntity>? specification;

    private void Initialize(IQueryable<TEntity> query, ISpecification<TEntity> specification)
    {
        ArgumentNullException.ThrowIfNull(query);
        ArgumentNullException.ThrowIfNull(specification);

        this.query = query;
        this.specification = specification;
    }

    public IQueryable<TEntity> EvaluateForCount(IQueryable<TEntity> query, ISpecification<TEntity> specification)
    {
        Initialize(query, specification);

        ApplyCriteria();

        return query;
    }

    public IQueryable<TEntity> Evaluate(IQueryable<TEntity> query, ISpecification<TEntity> specification)
    {
        Initialize(query, specification);
        
        ApplyCriteria();
        ApplyOrderBy();
        ApplyOrderByDescending();
        ApplyIncludes();
        ApplyPagination();

        return query;
    }

    private void ApplyCriteria()
    {
        if (specification!.Criteria != null)
        {
            query = query!.Where(specification.Criteria);
        }
    }

    private void ApplyOrderBy()
    {
        if (specification!.OrderBy != null)
        {
            query = query!.OrderBy(specification.OrderBy);
        }
    }

    private void ApplyOrderByDescending()
    {
        if (specification!.OrderByDescending != null)
        {
            query = query!.OrderByDescending(specification.OrderByDescending);
        }
    }

    private void ApplyPagination()
    {
        if (specification!.IsPaginationEnabled)
        {
            query = query!.Skip(specification.Skip).Take(specification.Take);
        }
    }

    private void ApplyIncludes()
    {
        if (specification!.Includes.Any())
        {
            query = specification.Includes.Aggregate(query, (current, include) => current!.Include(include));
        }

        if (specification.IncludesByString.Any())
        {
            query = specification.IncludesByString.Aggregate(query, (current, include) => current!.Include(include));
        }
    }
}
