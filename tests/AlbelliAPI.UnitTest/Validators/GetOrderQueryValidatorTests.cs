using FluentValidation.TestHelper;
using Albelli.Core.Handlers.QueryHandlers;
using Albelli.UnitTests.Base;
using Xunit;

namespace Albelli.UnitTests.Validators;
public class GetOrderQueryValidatorTests : ComponentTestBase<GetOrderQueryHandler>
{
    private readonly GetPopularMakelaarsQueryValidator _validator;

    public GetOrderQueryValidatorTests(ComponentFixtureBase fixture) : base(fixture)
    {
        _validator = new GetPopularMakelaarsQueryValidator();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void GetOrderQueryValidator_Should_Not_Validate(string orderId)
    {
        GetOrderQuery query = new GetOrderQuery
        {
            OrderId = orderId
        };
        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(c => c.OrderId);
    }

    [Theory]
    [InlineData("12")]    
    public void GetOrderQueryValidator_Should_Validate(string orderId)
    {
        GetOrderQuery query = new GetOrderQuery
        {
            OrderId = orderId
        };
        var result = _validator.TestValidate(query);

        result.ShouldNotHaveValidationErrorFor(c => c.OrderId);
    }
}