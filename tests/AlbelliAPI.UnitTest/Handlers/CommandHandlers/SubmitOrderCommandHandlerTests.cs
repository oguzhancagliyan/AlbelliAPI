using Albelli.Core.Handlers.CommandHandlers;
using Albelli.Core.Models.Exceptions;
using Albelli.Core.Models.MongoEntities;
using Albelli.UnitTests.Base;
using MongoDB.Bson;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AlbelliAPI.UnitTest.Handlers.CommandHandlers
{
    public class SubmitOrderCommandHandlerTests : ComponentTestBase<SubmitOrderCommandHandler>
    {
        public SubmitOrderCommandHandlerTests(ComponentFixtureBase fixture) : base(fixture)
        {
            SetupILoggerByCurrentService();
        }

        [Fact]
        public async Task SubmitOrderCommandHandler_Should_Throw_ProductTypeNotFoundException_When_Product_Doesnt_Exist()
        {
            var orderId = ObjectId.GenerateNewId().ToString();
            var productTypeId = ObjectId.GenerateNewId().ToString();
            List<OrderDetails> orderDetails = new List<OrderDetails>
            {
                new OrderDetails
                {
                    ProductTypeId = productTypeId,
                    Quantity = 1
                },
            };

            SubmitOrderCommand submitOrderCommand = new SubmitOrderCommand
            {
                Details = orderDetails
            };
            CancellationToken cancellationToken = new CancellationToken(false);
            await Assert.ThrowsAsync<ProductTypeNotFoundException>((async () => await ClassUnderTest.Handle(submitOrderCommand, cancellationToken)));
        }

        [Fact]
        public async Task SubmitOrderCommandHandler_Should_Add_Order()
        {

            var productTypeId = ObjectId.GenerateNewId().ToString();

            var productTypeEntity = new ProductTypeEntity
            {
                Id = productTypeId,
                Name = "photoBook",
                PackageWidth =1,                
            };

            await Context.ProductTypes.InsertOneAsync(productTypeEntity);


            List<OrderDetails> orderDetails = new List<OrderDetails>
            {
                new OrderDetails
                {
                    ProductTypeId = productTypeId,
                    Quantity = 1,
                },
            };

            SubmitOrderCommand submitOrderCommand = new SubmitOrderCommand
            {
                Details = orderDetails
            };

            var result = await ClassUnderTest.Handle(submitOrderCommand, new CancellationToken());

            Assert.NotNull(result);
        }
    }
}
