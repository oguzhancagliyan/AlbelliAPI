using System.Runtime.Serialization;

namespace Albelli.Core.Models.Exceptions
{
    public class ProductTypeNotFoundException : Exception
    {
        public ProductTypeNotFoundException()
        {
        }

        public ProductTypeNotFoundException(string message)
            : base(message)
        {
        }

        public ProductTypeNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ProductTypeNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
