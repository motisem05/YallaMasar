using System;
using System.Runtime.Serialization;

namespace YallaMasar.Exceptions
{
    public class MaximumSizeLimiteException : BadRequestException
    {
        public MaximumSizeLimiteException()
        {
        }

        public MaximumSizeLimiteException(string message) : base(message)
        {
        }

        public MaximumSizeLimiteException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MaximumSizeLimiteException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}