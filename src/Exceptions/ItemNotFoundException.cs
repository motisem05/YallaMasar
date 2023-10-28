using System;
using System.Runtime.Serialization;

namespace YallaMasar.Exceptions
{
    public class ItemNotFoundException : BadRequestException
    {
        public ItemNotFoundException() : base(404)
        {
        }

        public ItemNotFoundException(string message) : base(message, 404)
        {
        }

        public ItemNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ItemNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
