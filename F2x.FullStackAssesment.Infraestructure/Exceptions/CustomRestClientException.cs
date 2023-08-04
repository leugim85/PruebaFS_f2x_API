using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Runtime.Serialization;

namespace F2xFullStackAssesment.Infraestructure.Exceptions
{
    [Serializable]
    public class CustomRestClientException : Exception
    {
        public HttpResponseMessage HttpResponseMessage { get; }

        public CustomRestClientException()
        {
        }

        public CustomRestClientException(string message) : base(message)
        {
        }

        public CustomRestClientException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public CustomRestClientException(HttpResponseMessage httpResponseMessage)
        {
            HttpResponseMessage = httpResponseMessage;
        }

        public CustomRestClientException(string message, HttpResponseMessage httpResponseMessage) : base(message)
        {
            HttpResponseMessage = httpResponseMessage;
        }

        public CustomRestClientException(string message, Exception innerException, HttpResponseMessage httpResponseMessage) : base(message, innerException)
        {
            HttpResponseMessage = httpResponseMessage;
        }

        protected CustomRestClientException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
