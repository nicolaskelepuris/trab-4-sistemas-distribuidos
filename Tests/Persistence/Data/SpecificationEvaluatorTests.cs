using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities.Base;
using Domain.Interfaces;
using FluentAssertions;
using Moq;
using Persistence.Data;
using Xunit;

namespace Tests.Persistence.Data;
public class SpecificationEvaluatorTests
{
    public class SomeEntity : BaseEntity
    {
        
    }

    [Fact]
    public void EvaluateForCount_NotNullQueryAndSpecification_ShouldNotThrow()
    {
        var query = new List<SomeEntity>().AsQueryable();
        var specification = new Mock<ISpecification<SomeEntity>>().Object;
        var evaluator = new SpecificationEvaluator<SomeEntity>();
        
        var evaluateForCount = () => evaluator.EvaluateForCount(query, specification);

        evaluateForCount.Should().NotThrow<ArgumentNullException>();
    }

    [Fact]
    public void EvaluateForCount_NullQuery_ShouldThrow()
    {
        var specification = new Mock<ISpecification<SomeEntity>>().Object;
        var evaluator = new SpecificationEvaluator<SomeEntity>();

        var evaluateForCount = () => evaluator.EvaluateForCount(query: null!, specification);

        evaluateForCount.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void EvaluateForCount_NullSpecification_ShouldThrow()
    {
        var query = new List<SomeEntity>().AsQueryable();
        var evaluator = new SpecificationEvaluator<SomeEntity>();

        var evaluateForCount = () => evaluator.EvaluateForCount(query, specification: null!);

        evaluateForCount.Should().ThrowExactly<ArgumentNullException>();
    }
}
