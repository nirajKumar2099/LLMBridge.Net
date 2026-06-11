using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLMBridge.Net.Exceptions
{
    public sealed class LLMBridgeException : Exception
    {
        public LLMBridgeException(string message) : base(message)
        {
        }

        public LLMBridgeException(string message,Exception innerException) : base(message, innerException)
        {
        }
    }
}
