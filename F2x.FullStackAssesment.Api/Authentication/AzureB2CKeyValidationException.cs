using System;
using System.Runtime.Serialization;

namespace F2xFullStackAssesment.Api.Authentication
{
    public class AzureB2CKeyValidationException : Exception
    {
        public AzureB2CKeyValidationException()
        {
        }

        public AzureB2CKeyValidationException(string message)
            : base(message)
        {
        }

        public AzureB2CKeyValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected AzureB2CKeyValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
