using System;
using System.Runtime.Serialization;

namespace F2xFullStackAssesment.Infraestructure.Exceptions
{
    public class PhotoFineDocumentConflictException : Exception
    {
        public PhotoFineDocumentConflictException()
        {
        }

        public PhotoFineDocumentConflictException(string message) : base(message)
        {
        }

        public PhotoFineDocumentConflictException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PhotoFineDocumentConflictException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
