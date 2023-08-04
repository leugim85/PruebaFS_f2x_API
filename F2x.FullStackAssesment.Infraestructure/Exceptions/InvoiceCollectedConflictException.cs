using System;
using System.Runtime.Serialization;

namespace F2xFullStackAssesment.Infraestructure.Exceptions
{
    public class InvoiceCollectedConflictException : Exception
    {
        public InvoiceCollectedConflictException()
        {
        }

        public InvoiceCollectedConflictException(string message) : base(message)
        {
        }

        public InvoiceCollectedConflictException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvoiceCollectedConflictException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
