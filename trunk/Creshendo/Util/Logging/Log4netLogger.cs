/*
* Copyright 2002-2006 Peter Lin
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*   http://ruleml-dev.sourceforge.net/
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
* 
*/
using System;
using log4net;

namespace Creshendo.Util.Logging
{
    /// <author>  Peter Lin
    /// 
    /// A quick and simple logger. To make it easier for other classes to
    /// log and not have to import log4j stuff. 
    /// 
    /// </author>
    [Serializable]
    public sealed class Log4netLogger : ILogger
    {
        private static readonly Log4netLogger instance = new Log4netLogger();
        private readonly ILog log;

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Log4netLogger()
        {
        }

        private Log4netLogger()
        {
            log = LogManager.GetLogger("Creshendo");
        }

        public static Log4netLogger Instance
        {
            get { return instance; }
        }

        #region ILogger Members

        public void Debug(String msg)
        {
            log.Debug(msg);
        }

        public void Debug(Exception exc)
        {
            log.Debug(exc);
        }

        public void Fatal(String msg)
        {
            log.Fatal(msg);
        }

        public void Fatal(Exception exc)
        {
            log.Fatal(exc);
        }

        public void Info(String msg)
        {
            log.Info(msg);
        }

        public void Info(Exception exc)
        {
            log.Info(exc);
        }

        public void Warn(String msg)
        {
            log.Warn(msg);
        }

        public void Warn(Exception msg)
        {
            log.Warn(msg);
        }

        #endregion
    }
}