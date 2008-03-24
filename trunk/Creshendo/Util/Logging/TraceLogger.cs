using System;
using System.Diagnostics;

namespace Creshendo.Util.Logging
{
    public sealed class TraceLogger : ILogger
    {
        private static readonly TraceLogger instance = new TraceLogger();

        static TraceLogger()
        {
        }

        private TraceLogger()
        {
        }

        public static TraceLogger Instance
        {
            get { return instance; }
        }

        #region ILogger Members

        public void Debug(string msg)
        {
            Trace.TraceInformation(msg);
        }

        public void Debug(Exception exc)
        {
            Trace.TraceInformation(exc.Message);
        }

        public void Fatal(string msg)
        {
            Trace.TraceError(msg);
        }

        public void Fatal(Exception exc)
        {
            Trace.TraceError(exc.Message);
        }

        public void Info(string msg)
        {
            Trace.TraceInformation(msg);
        }

        public void Info(Exception exc)
        {
            Trace.TraceInformation(exc.Message);
        }

        public void Warn(string msg)
        {
            Trace.TraceWarning(msg);
        }

        public void Warn(Exception msg)
        {
            Trace.TraceWarning(msg.Message);
        }

        #endregion
    }
}