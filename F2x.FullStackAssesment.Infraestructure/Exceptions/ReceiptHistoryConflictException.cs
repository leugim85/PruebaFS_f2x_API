using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace F2xFullStackAssesment.Infraestructure.Exceptions
{
    [Serializable, ExcludeFromCodeCoverage]
    public class ReceiptHistoryConflictException : Exception
    {
        public ReceiptHistoryConflictException()
        {
        }

        public ReceiptHistoryConflictException(string message) : base(message)
        {
        }

        public ReceiptHistoryConflictException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ReceiptHistoryConflictException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
