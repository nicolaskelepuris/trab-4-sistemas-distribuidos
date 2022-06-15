using System.Linq;
using Domain.Entities.Base;
using Domain.Interfaces;

namespace Persistence.Interfaces
{
    public interface ISpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        IQueryable<TEntity> EvaluateForCount(IQueryable<TEntity> query, ISpecification<TEntity> specification);
        IQueryable<TEntity> Evaluate(IQueryable<TEntity> query, ISpecification<TEntity> specification);
    }
}