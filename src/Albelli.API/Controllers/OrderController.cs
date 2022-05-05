using Albelli.Core.Handlers.CommandHandlers;
using Albelli.Core.Handlers.QueryHandlers;
using Albelli.Core.Models.MongoEntities;
using Albelli.Core.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Albelli.API.Controllers
{
    [Route("api")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IProductTypeService _productTypeService;

        public OrderController(IMediator mediator, IProductTypeService productTypeService)
        {
            _mediator = mediator;
            _productTypeService = productTypeService;
        }

        [HttpPost]
        [Route("")]
        [ProducesResponseType(typeof(SubmitOrderResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> SubmitOrder([FromBody] SubmitOrderCommand command, CancellationToken token = default)
        {

            SubmitOrderResponseModel result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpGet]
        [Route("{order_id}")]
        [ProducesResponseType(typeof(GetOrderResponseModel), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOrder([FromRoute(Name = "order_id")] string orderId, CancellationToken token = default)
        {
            GetOrderQuery query = new GetOrderQuery
            {
                OrderId = orderId,
            };

            GetOrderResponseModel result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpGet]
        [Route("populate_data")]
        public async Task<IActionResult> PopulateData()
        {
            List<ProductTypeEntity> result = await _productTypeService.PopulateProductTypesAsync();

            return Ok(result);
        }
    }
}
