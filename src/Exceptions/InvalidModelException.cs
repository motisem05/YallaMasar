
using System;
using System.Runtime.Serialization;

namespace YallaMasar.Exceptions
{
    public class InvalidModelException : BadRequestException
    {
        public string FieldName { get; set; }

        public InvalidModelException()
        {
        }

        public InvalidModelException(string fieldName, string message) : base(message)
        {
            FieldName = fieldName;
        }


        public InvalidModelException(string message) : base(message)
        {
        }

        public InvalidModelException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidModelException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}