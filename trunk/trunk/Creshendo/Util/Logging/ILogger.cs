using System;

namespace Creshendo.Util.Logging
{
    public interface ILogger
    {
        void Debug(String msg);
        void Debug(Exception exc);
        void Fatal(String msg);
        void Fatal(Exception exc);
        void Info(String msg);
        void Info(Exception exc);
        void Warn(String msg);
        void Warn(Exception msg);
    }
}