using Albelli.Core.Handlers.QueryHandlers;
using Albelli.Core.Models.Exceptions;
using Albelli.Core.Models.MongoEntities;
using Albelli.UnitTests.Base;
using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Bson;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Albelli.UnitTests.Handlers.QueryHandlers;

public class GetOrderQueryHandlerTests : ComponentTestBase<GetOrderQueryHandler>
{

    [Fact]
    public async Task GetOrderQueryHandler_Should_Throw_OrderNotFound_Exception_When_There_Is_No_Record_In_Db()
    {
        GetOrderQuery query = new GetOrderQuery
        {
            OrderId = ObjectId.GenerateNewId().ToString(),
        };

        MockFor<IDistributedCache>().Setup(m => m.GetAsync(It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(() => null);

        CancellationToken cancellationToken = new CancellationToken(false);

        await Assert.ThrowsAsync<OrderNotFoundException>(() => ClassUnderTest.Handle(query, cancellationToken));

        MockFor<IDistributedCache>().Verify(c => c.GetAsync(It.IsAny<string>(), CancellationToken.None), Times.Once);
    }

    public GetOrderQueryHandlerTests(ComponentFixtureBase fixture) : base(fixture)
    {
        SetupILoggerByCurrentService();
    }

    [Fact]
    public async Task GetOrderQueryHandler_Should_Throw_OrderDetailNotFoundException_Exception_When_There_Is_No_Record_In_Db()
    {
        var orderId = ObjectId.GenerateNewId().ToString();
        OrderEntity order = new OrderEntity
        {
            Id = orderId,
            BinWidth = 1
        };


        await Context.Orders.InsertOneAsync(order);

        GetOrderQuery query = new GetOrderQuery
        {
            OrderId = orderId,
        };

        MockFor<IDistributedCache>().Setup(m => m.GetAsync(It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(() => null);

        CancellationToken cancellationToken = new CancellationToken(false);

        await Assert.ThrowsAsync<OrderDetailNotFoundException>(() => ClassUnderTest.Handle(query, cancellationToken));

        MockFor<IDistributedCache>().Verify(c => c.GetAsync(It.IsAny<string>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task GetOrderQueryHandler_Should_Throw_ProductTypeNotFoundException_Exception_When_There_Is_No_Record_In_Db()
    {
        var orderId = ObjectId.GenerateNewId().ToString();
        OrderEntity order = new OrderEntity
        {
            Id = orderId,
            BinWidth = 1
        };


        await Context.Orders.InsertOneAsync(order);

        var orderDetailId = ObjectId.GenerateNewId().ToString();

        var productTypeId = ObjectId.GenerateNewId().ToString();

        OrderDetailEntity orderDetailEntity = new OrderDetailEntity
        {
            Id = orderDetailId,
            OrderId = orderId,
            Quantity = 1,
            ProductTypeId = productTypeId
        };

        await Context.OrderDetails.InsertOneAsync(orderDetailEntity);

        GetOrderQuery query = new GetOrderQuery
        {
            OrderId = orderId,
        };

        MockFor<IDistributedCache>().Setup(m => m.GetAsync(It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(() => null);

        CancellationToken cancellationToken = new CancellationToken(false);

        await Assert.ThrowsAsync<ProductTypeNotFoundException>(() => ClassUnderTest.Handle(query, cancellationToken));

        MockFor<IDistributedCache>().Verify(c => c.GetAsync(It.IsAny<string>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task GetOrderQueryHandler_Should_Get_Order()
    {
        var orderId = ObjectId.GenerateNewId().ToString();
        OrderEntity order = new OrderEntity
        {
            Id = orderId,
            BinWidth = 1
        };


        await Context.Orders.InsertOneAsync(order);

        var orderDetailId = ObjectId.GenerateNewId().ToString();

        var productTypeId = ObjectId.GenerateNewId().ToString();

        OrderDetailEntity orderDetailEntity = new OrderDetailEntity
        {
            Id = orderDetailId,
            OrderId = orderId,
            Quantity = 1,
            ProductTypeId = productTypeId
        };

        await Context.OrderDetails.InsertOneAsync(orderDetailEntity);

        ProductTypeEntity productTypeEntity = new ProductTypeEntity
        {
            Id = productTypeId,
            Name = "photoBook",

            PackageWidth = 1,
        };

        await Context.ProductTypes.InsertOneAsync(productTypeEntity);

        GetOrderQuery query = new GetOrderQuery
        {
            OrderId = orderId,
        };

        MockFor<IDistributedCache>().Setup(m => m.GetAsync(It.IsAny<string>(), CancellationToken.None)).ReturnsAsync(() => null);

        CancellationToken cancellationToken = new CancellationToken(false);

        var result = ClassUnderTest.Handle(query, cancellationToken);

        Assert.NotNull(result);

        MockFor<IDistributedCache>().Verify(c => c.GetAsync(It.IsAny<string>(), CancellationToken.None), Times.Once);
    }
}