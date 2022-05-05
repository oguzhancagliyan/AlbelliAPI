using Albelli.Core.Helpers;
using Albelli.Core.Models.Exceptions;
using Albelli.Core.Models.MongoEntities;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Albelli.Core.Handlers.QueryHandlers;

public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, GetOrderResponseModel>
{
    private readonly AlbelliContext _context;
    private readonly IDistributedCache _distributedCache;
    private readonly ILogger<GetOrderQueryHandler> _logger;

    public GetOrderQueryHandler(AlbelliContext context, IDistributedCache distributedCache, ILogger<GetOrderQueryHandler> logger)
    {
        _context = context;
        _distributedCache = distributedCache;
        _logger = logger;
    }

    public async Task<GetOrderResponseModel> Handle(GetOrderQuery query, CancellationToken token = default)
    {
        string cacheKey = $"{nameof(GetOrderQuery)}-{query.OrderId}";

        _logger.LogInformation("Requested {@request}", query);

        return await _distributedCache.GetOrCreateAsync(cacheKey, async () =>
        {

            OrderEntity order = await _context.Orders.Find(a => a.Id == query.OrderId).FirstOrDefaultAsync(token);

            if (order is null)
            {
                throw new OrderNotFoundException($"Order not found id : {query.OrderId}");
            }

            List<OrderDetailEntity> orderDetailsEntity = await _context.OrderDetails.Find(a => a.OrderId == query.OrderId).ToListAsync(token);

            if (!orderDetailsEntity.Any())
            {
                throw new OrderDetailNotFoundException($"Orderdetail not found OrderId : {query.OrderId}");
            }

            List<GetOrderOrderDetailModel> orderDetails = new();

            foreach (OrderDetailEntity item in orderDetailsEntity)
            {
                ProductTypeEntity product = await _context.ProductTypes.Find(c => c.Id == item.ProductTypeId).FirstOrDefaultAsync(token);

                if (product is null)
                {
                    throw new ProductTypeNotFoundException($"Product not found OrderId : {order.Id} , OrderDetailId : {item.Id} , ProductId : {item.ProductTypeId}");
                }

                GetOrderProductTypeModel productTypeModel = new()
                {
                    Id = product.Id,
                    Name = product.Name,
                    PackageWidth = product.PackageWidth,
                };

                GetOrderOrderDetailModel orderDetailModel = new GetOrderOrderDetailModel
                {
                    Id = item.Id,
                    OrderId = item.OrderId,
                    Product = productTypeModel,
                    Quantity = item.Quantity,
                };

                orderDetails.Add(orderDetailModel);
            }


            GetOrderResponseModel getOrderResponseModel = new GetOrderResponseModel
            {
                OrderId = query.OrderId,
                BinWidth = order.BinWidth,
                Details = orderDetails
            };

            return getOrderResponseModel;
        }, TimeSpan.FromHours(1));
    }
}

public record GetOrderQuery : IRequest<GetOrderResponseModel>
{
    public string OrderId { get; init; }
}

public class GetPopularMakelaarsQueryValidator : AbstractValidator<GetOrderQuery>
{
    public GetPopularMakelaarsQueryValidator()
    {
        RuleFor(c => c.OrderId).NotEmpty();
    }
}

public record GetOrderResponseModel
{
    public string OrderId { get; init; }

    public double BinWidth { get; init; }

    public List<GetOrderOrderDetailModel> Details { get; init; }
}

public record GetOrderOrderDetailModel
{
    public string Id { get; set; }

    public string OrderId { get; init; }

    public int Quantity { get; set; }

    public GetOrderProductTypeModel Product { get; init; }
}

public record GetOrderProductTypeModel
{
    public string Id { get; init; }

    public string Name { get; init; }

    public double PackageWidth { get; init; }
}