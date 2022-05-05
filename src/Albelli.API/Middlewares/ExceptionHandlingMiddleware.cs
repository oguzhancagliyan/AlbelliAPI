using Albelli.Core.Models;
using Albelli.Core.Models.Exceptions;
using System.Net;
using System.Net.Mime;
using System.Text.Json;

namespace Albelli.API.Middlewares;
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception e)
        {
            (ErrorMessage error, HttpStatusCode statusCode) = HandleException(e);

            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = MediaTypeNames.Application.Json;

            var serializedError = JsonSerializer.Serialize(error);
            await context.Response.WriteJsonAsync(serializedError);
        }
    }

    private (ErrorMessage error, HttpStatusCode statusCode) HandleException(Exception exception)
    {
        var httpStatusCode = HttpStatusCode.InternalServerError;

        var errorMessage = new ErrorMessage
        {
            Message = "Error"
        };
        if (exception is OrderDetailNotFoundException)
        {
            httpStatusCode = HttpStatusCode.NotFound;
            errorMessage.Message = "Not Found";
            errorMessage.ErrorDetails = new List<ErrorDetail>
                {
                    new()
                    {
                        ErrorCode = "OrderDetailNotFound",
                        PropertyName = exception.Message
                    }
                };
        }
        else if (exception is OrderNotFoundException)
        {
            httpStatusCode = HttpStatusCode.NotFound;
            errorMessage.Message = "Not Found";
            errorMessage.ErrorDetails = new List<ErrorDetail>
            {
                new()
                {
                    ErrorCode = "OrderNotFound",
                    PropertyName = exception.Message
                }
            };
        }
        else if (exception is ProductTypeNotFoundException)
        {
            httpStatusCode = HttpStatusCode.NotFound;
            errorMessage.Message = "Not Found";
            errorMessage.ErrorDetails = new List<ErrorDetail>
            {
                new()
                {
                    ErrorCode = "ProductTypeNotFound",
                    PropertyName = exception.Message
                }
            };
        }
        else if (exception is FluentValidation.ValidationException validationException)
        {
            httpStatusCode = HttpStatusCode.BadRequest;
            errorMessage.Message = "Couldn't validate";

            var errorDetails = new List<ErrorDetail>();
            var errors = validationException.Errors;

            foreach (var error in errors)
            {
                errorDetails.Add(new ErrorDetail()
                {
                    ErrorCode = error.ErrorMessage,
                    PropertyName = error.PropertyName
                });
            }

            errorMessage.ErrorDetails = errorDetails;
        }
        else if (exception is TaskCanceledException)
        {
            httpStatusCode = HttpStatusCode.BadRequest;
        }
        else
        {
            httpStatusCode = HttpStatusCode.InternalServerError;
        }

        if (httpStatusCode == HttpStatusCode.InternalServerError)
        {
            _logger.LogError(exception, "{@ErrorMessage} {HttpStatusCode}", errorMessage, httpStatusCode);
        }
        else
        {
            _logger.LogWarning(exception, "{@ErrorMessage} {HttpStatusCode}", errorMessage, httpStatusCode);
        }

        return (errorMessage, httpStatusCode);
    }
}

public static class MiddlewareExtensions
{
    public static void UseCustomExceptionHandler(this IApplicationBuilder applicationBuilder)
    {
        applicationBuilder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}

public static class HttpResponseExtensions
{
    public static Task WriteJsonAsync(this HttpResponse response, string json)
    {
        response.ContentType = "application/json; charset=UTF-8";
        return response.WriteAsync(json);
    }
}

