using Albelli.Core.Handlers.QueryHandlers;
using Albelli.UnitTests.Base;
using Xunit;
using FluentValidation.TestHelper;
using Albelli.Core.Handlers.CommandHandlers;
using System.Collections.Generic;

namespace Albelli.UnitTests.Validators;

public class SubmitOrderCommandValidatorTests : ComponentTestBase<SubmitOrderCommandHandler>
{
    private readonly SubmitOrderCommandValidator _validator;

    public SubmitOrderCommandValidatorTests(ComponentFixtureBase fixture) : base(fixture)
    {
        _validator = new SubmitOrderCommandValidator();
    }

    [Theory]
    [InlineData("", 0)]
    [InlineData(null, 0)]
    public void SubmitOrderCommandValidator_Should_Not_Validate(string productTypeId, int quantity)
    {
        SubmitOrderCommand command = new SubmitOrderCommand
        {
            Details = new List<OrderDetails>
            {
                new OrderDetails
                {
                    ProductTypeId =productTypeId,
                    Quantity = quantity
                }
            }
        };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor("Details[0].Quantity");
        result.ShouldHaveValidationErrorFor("Details[0].ProductTypeId");
    }

    [Theory]
    [InlineData("1234", "1234", 2)]
    public void SubmitOrderCommandValidator_Should_Validate(string orderId, string productTypeId, int quantity)
    {
        SubmitOrderCommand command = new SubmitOrderCommand
        {
            Details = new List<OrderDetails>
            {
                new OrderDetails
                {
                    ProductTypeId =productTypeId,
                    Quantity = quantity
                }
            }
        };
        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(c => c.Details);
    }
}