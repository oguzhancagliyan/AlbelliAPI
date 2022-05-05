using System.Runtime.Serialization;

namespace Albelli.Core.Models.Exceptions;

public class OrderNotFoundException : Exception
{
    public OrderNotFoundException()
    {
    }

    public OrderNotFoundException(string message)
        : base(message)
    {
    }

    public OrderNotFoundException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public OrderNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}