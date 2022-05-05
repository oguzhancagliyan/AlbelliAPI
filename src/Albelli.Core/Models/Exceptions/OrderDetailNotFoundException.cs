using System.Runtime.Serialization;

namespace Albelli.Core.Models.Exceptions
{

    public class OrderDetailNotFoundException : Exception
    {
        public OrderDetailNotFoundException()
        {
        }

        public OrderDetailNotFoundException(string message)
            : base(message)
        {
        }

        public OrderDetailNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public OrderDetailNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
