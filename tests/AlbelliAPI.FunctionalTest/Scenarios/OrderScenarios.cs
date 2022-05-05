using Albelli.Core.Handlers.QueryHandlers;
using Albelli.Core.Models.MongoEntities;
using AlbelliAPI.FunctionalTest.CollectionDefinitions;
using AlbelliAPI.FunctionalTest.Extensions;
using AlbelliAPI.FunctionalTest.Fixtures;
using AlbelliAPI.FunctionalTest.Routes;
using AlbelliAPI.FunctionalTest.SeedData;
using MongoDB.Bson;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Funda.FunctionalTests.Scenarios;

[Collection(nameof(FunctionalTestCollection))]
public class OrderScenarios : BaseScenario
{

    [Fact]
    public async Task GetOrder_Should_Return_404_When_There_Is_No_Order()
    {
        var orderId = ObjectId.GenerateNewId().ToString();

        var httpResponseMessage = await HttpClient.GetAsync(OrderRoutes.GetOrder(orderId));

        Assert.Equal(HttpStatusCode.NotFound, httpResponseMessage.StatusCode);
    }

    [Fact]
    public async Task GetOrder_Should_Return_404_When_There_Is_No_Order_Detail()
    {
        var orderId = ObjectId.GenerateNewId().ToString();

        var order = new OrderEntity
        {
            Id = orderId,
            BinWidth = 1
        };

        await DbContext.Orders.InsertOneAsync(order);

        var httpResponseMessage = await HttpClient.GetAsync(OrderRoutes.GetOrder(orderId));

        Assert.Equal(HttpStatusCode.NotFound, httpResponseMessage.StatusCode);
    }

    [Fact]

    public async Task GetOrder_Should_Return_404_When_There_Is_No_ProductType()
    {
        var orderId = ObjectId.GenerateNewId().ToString();

        var httpResponseMessage = await HttpClient.GetAsync(OrderRoutes.GetOrder(orderId));

        Assert.Equal(HttpStatusCode.NotFound, httpResponseMessage.StatusCode);
    }

    [Fact]

    public async Task GetOrder_Should_Return_200()
    {
        var httpResponseMessage = await HttpClient.GetAsync(OrderRoutes.GetOrder(OrderSeeder.OrderPk));
        var responseModel = await httpResponseMessage.Content.GetAsync<GetOrderResponseModel>();

        Assert.Equal(HttpStatusCode.OK, httpResponseMessage.StatusCode);
        Assert.NotNull(responseModel);        
    }

    public OrderScenarios(TestServerFixture testServerFixture) : base(testServerFixture)
    {
    }
}