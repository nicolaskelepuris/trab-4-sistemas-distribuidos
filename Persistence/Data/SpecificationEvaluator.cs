using System.Linq;
using Domain.Entities.Base;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Interfaces;

namespace Persistence.Data;
public class SpecificationEvaluator<TEntity> : ISpecificationEvaluator<TEntity> where TEntity : BaseEntity
{
    public IQueryable<TEntity> EvaluateForCount(IQueryable<TEntity> query, ISpecification<TEntity> specification)
    {
        return ApplyCriteria(query, specification);
    }

    public IQueryable<TEntity> Evaluate(IQueryable<TEntity> query, ISpecification<TEntity> specification)
    {
        var result = query;
        
        result = ApplyCriteria(result, specification);
        result = ApplyOrderBy(result, specification);
        result = ApplyOrderByDescending(result, specification);
        result = ApplyIncludes(result, specification);
        result = ApplyPagination(result, specification);

        return result;
    }

    private IQueryable<TEntity> ApplyCriteria(IQueryable<TEntity> query, ISpecification<TEntity> specification)
    {
        if (specification!.Criteria != null)
        {
            return query!.Where(specification.Criteria);
        }

        return query;
    }

    private IQueryable<TEntity> ApplyOrderBy(IQueryable<TEntity> query, ISpecification<TEntity> specification)
    {
        if (specification!.OrderBy != null)
        {
            return query!.OrderBy(specification.OrderBy);
        }
        
        return query;
    }

    private IQueryable<TEntity> ApplyOrderByDescending(IQueryable<TEntity> query, ISpecification<TEntity> specification)
    {
        if (specification!.OrderByDescending != null)
        {
            return query!.OrderByDescending(specification.OrderByDescending);
        }

        return query;
    }

    private IQueryable<TEntity> ApplyPagination(IQueryable<TEntity> query, ISpecification<TEntity> specification)
    {
        if (specification!.IsPaginationEnabled)
        {
            return query!.Skip(specification.Skip).Take(specification.Take);
        }

        return query;
    }

    private IQueryable<TEntity> ApplyIncludes(IQueryable<TEntity> query, ISpecification<TEntity> specification)
    {
        var result = query;
        if (specification!.Includes.Any())
        {
            result = specification.Includes.Aggregate(query, (current, include) => current!.Include(include));
        }

        if (specification.IncludesByString.Any())
        {
            result = specification.IncludesByString.Aggregate(query, (current, include) => current!.Include(include));
        }

        return result;
    }
}
