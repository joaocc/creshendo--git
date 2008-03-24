using System;

namespace Creshendo.Util
{
    public class RuntimeException : Exception
    {
        public RuntimeException(String message) : base(message)
        {
        }

        public RuntimeException(String message, Exception e) : base(message, e)
        {
        }

        public RuntimeException(Exception e) : base(e.Message)
        {
        }
    }
}