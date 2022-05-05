using Albelli.Core.Models.Exceptions;
using Albelli.Core.Models.MongoEntities;
using FluentValidation;
using MediatR;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Albelli.Core.Handlers.CommandHandlers
{
    public class SubmitOrderCommandHandler : IRequestHandler<SubmitOrderCommand, SubmitOrderResponseModel>
    {
        private readonly AlbelliContext _context;

        public SubmitOrderCommandHandler(AlbelliContext context)
        {
            _context = context;
        }

        // TODO : we should do in a mongosession. In that way we can provide transaction
        public async Task<SubmitOrderResponseModel> Handle(SubmitOrderCommand request, CancellationToken cancellationToken)
        {
            double binWidth = await CalculateBinWidthAsync(request.Details, cancellationToken);

            var objectId = ObjectId.GenerateNewId().ToString();

            var entity = new OrderEntity
            {
                BinWidth = binWidth,
                Id = objectId
            };

            await _context.Orders.InsertOneAsync(entity, null, cancellationToken);

            var orderDetailEntity = request.Details.Select(x => new OrderDetailEntity
            {
                OrderId = objectId.ToString(),
                ProductTypeId = x.ProductTypeId,
                Quantity = x.Quantity,
            });

            await _context.OrderDetails.InsertManyAsync(orderDetailEntity, null, cancellationToken);

            var submitOrderResponseModel = new SubmitOrderResponseModel
            {
                OrderId = entity.Id.ToString(),
            };

            return submitOrderResponseModel;
        }

        public async Task<double> CalculateBinWidthAsync(List<OrderDetails> orderRequest, CancellationToken token = default)
        {
            var minimumBinWidth = 0.0;

            foreach (var item in orderRequest)
            {
                ProductTypeEntity productType = await _context.ProductTypes.Find(b => b.Id == item.ProductTypeId).FirstOrDefaultAsync(token);
                if (productType == null)
                {
                    throw new ProductTypeNotFoundException($"product couldn't found productTypeId : {item.ProductTypeId}");
                }

                if (productType.Name == "mug")
                {
                    int mugQuantity = 0;
                    if (item.Quantity % 4 == 0)
                    {
                        mugQuantity = (item.Quantity / 4);
                    }
                    else
                    {
                        mugQuantity = (item.Quantity / 4) + 1;
                    }

                    minimumBinWidth += productType.PackageWidth * mugQuantity;
                }
                else
                {
                    minimumBinWidth += productType.PackageWidth * item.Quantity;
                }
            }

            return minimumBinWidth;
        }
    }

    public record SubmitOrderCommand : IRequest<SubmitOrderResponseModel>
    {
        public List<OrderDetails> Details { get; init; }
    }

    public record SubmitOrderResponseModel
    {
        public string OrderId { get; init; }
    }

    public record OrderDetails
    {
        public int Quantity { get; init; }

        public string ProductTypeId { get; init; }
    }

    public class SubmitOrderCommandValidator : AbstractValidator<SubmitOrderCommand>
    {
        public SubmitOrderCommandValidator()
        {
            RuleFor(b => b.Details).NotEmpty();
            RuleFor(x => x).Must(x => IsUnique(x.Details)).WithMessage("Duplicated items error");
            RuleForEach(b => b.Details).SetValidator(new OrderDetailsValidator());
        }

        private bool IsUnique(List<OrderDetails> orderDetails)
        {
            var group = orderDetails.ToList().GroupBy(x => x.ProductTypeId).Where(x => x.Count() > 1);
            return !group.Any();
        }
    }

    public class OrderDetailsValidator : AbstractValidator<OrderDetails>
    {
        public OrderDetailsValidator()
        {
            RuleFor(x => x.Quantity).GreaterThan(0);
            RuleFor(b => b.ProductTypeId).NotEmpty();
        }
    }
}
