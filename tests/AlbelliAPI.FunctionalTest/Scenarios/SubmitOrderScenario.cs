using Albelli.Core.Handlers.CommandHandlers;
using Albelli.Core.Models.MongoEntities;
using AlbelliAPI.FunctionalTest.CollectionDefinitions;
using AlbelliAPI.FunctionalTest.Extensions;
using AlbelliAPI.FunctionalTest.Fixtures;
using AlbelliAPI.FunctionalTest.Routes;
using AlbelliAPI.FunctionalTest.SeedData;
using Funda.FunctionalTests.Scenarios;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AlbelliAPI.FunctionalTest.Scenarios
{
    [Collection(nameof(FunctionalTestCollection))]
    public class SubmitOrderScenario : BaseScenario
    {
        public SubmitOrderScenario(TestServerFixture testServerFixture) : base(testServerFixture)
        {
        }

        [Fact]
        public async Task SubmitOrder_Should_Return_404_When_Product_Not_Found()
        {
            SubmitOrderCommand command = new SubmitOrderCommand
            {
                Details = new List<OrderDetails>
                {
                    new OrderDetails
                    {
                        ProductTypeId = ObjectId.GenerateNewId().ToString(),
                        Quantity = 2
                    }
                }
            };

            var httpResponseMessage = await HttpClient.PostAsync(OrderRoutes.SubmitOrder(), command);

            Assert.Equal(HttpStatusCode.NotFound, httpResponseMessage.StatusCode);
        }

        [Fact]
        public async Task SubmitOrder_Should_Return_200()
        {
            SubmitOrderCommand command = new SubmitOrderCommand
            {
                Details = new List<OrderDetails>
                {
                    new OrderDetails
                    {
                        ProductTypeId = ProductTypeSeeder.canvasPk,
                        Quantity = 2
                    }
                }
            };

            var httpResponseMessage = await HttpClient.PostAsync(OrderRoutes.SubmitOrder(), command);

            Assert.Equal(HttpStatusCode.OK, httpResponseMessage.StatusCode);
        }
    }
}
