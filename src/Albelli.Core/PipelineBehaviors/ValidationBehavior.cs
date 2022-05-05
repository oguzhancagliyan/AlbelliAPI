using MediatR;

namespace Albelli.Core.PipelineBehaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly IModelValidator _validator;

    public ValidationBehavior(IModelValidator validator)
    {
        _validator = validator;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        await _validator.ValidateAndThrowAsync(request);

        return await next();
    }
}