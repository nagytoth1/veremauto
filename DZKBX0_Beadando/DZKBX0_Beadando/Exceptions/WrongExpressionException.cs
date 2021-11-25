using System;
using System.Runtime.Serialization;

namespace DZKBX0_Beadando
{
    [Serializable]
    internal class WrongExpressionException : Exception
    {
        public WrongExpressionException()
        {
        }

        public WrongExpressionException(string message) : base(message)
        {
        }

        public WrongExpressionException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected WrongExpressionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}