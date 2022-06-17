using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Domain.Entities.Base;

namespace Domain.Interfaces;

public interface ISpecification<T> where T : BaseEntity
{
    Expression<Func<T, bool>>? Criteria { get; }
    Expression<Func<T, object>>? OrderBy { get; }
    Expression<Func<T, object>>? OrderByDescending { get; }
    List<Expression<Func<T, object>>> Includes { get; }
    List<string> IncludesByString { get; }
    int Take { get; }
    int Skip { get; }
    bool IsPaginationEnabled { get; }
}
