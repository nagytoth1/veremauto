using System;
using System.Runtime.Serialization;

namespace DZKBX0_Beadando
{
    [Serializable]
    internal class DefaultStateEmptyException : Exception
    {
        public DefaultStateEmptyException()
        {
        }

        public DefaultStateEmptyException(string message) : base(message)
        {
        }

        public DefaultStateEmptyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DefaultStateEmptyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}