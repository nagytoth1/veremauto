using System;
using System.Runtime.Serialization;

namespace DZKBX0_Beadando
{
    [Serializable]
    internal class RuleNotFoundException : Exception
    {
        public RuleNotFoundException()
        {
        }

        public RuleNotFoundException(string message) : base(message)
        {
        }

        public RuleNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RuleNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}