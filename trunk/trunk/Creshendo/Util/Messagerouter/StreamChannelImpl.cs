using System;
using System.IO;
using System.Threading;
using Creshendo.Util.Logging;
using Creshendo.Util.Parser.Clips2;
using Creshendo.Util.Rete;

/// <summary> Copyright 2006 Alexander Wilden, Christoph Emonds, Sebastian Reinartz
/// *
/// Licensed under the Apache License, Version 2.0 (the "License");
/// you may not use this file except in compliance with the License.
/// You may obtain a copy of the License at
/// *
/// http://ruleml-dev.sourceforge.net/
/// *
/// Unless required by applicable law or agreed to in writing, software
/// distributed under the License is distributed on an "AS IS" BASIS,
/// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
/// See the License for the specific language governing permissions and
/// limitations under the License.
/// 
/// </summary>
namespace Creshendo.Util.Messagerouter
{
    internal class StreamChannelImpl : AbstractCommunicationChannel, IStreamChannel
    {
        private readonly CLIPSParser parser;
        private Thread _worker;
        private TextReader reader;
        private bool stopped = false;


        public StreamChannelImpl(String channelId, MessageRouter router, InterestType interest) : base(channelId, router, interest)
        {
            parser = new CLIPSParser(router.ReteEngine, (StreamReader) null);
        }

        public bool Stopped
        {
            get { return stopped; }
            set { stopped = value; }
        }

        #region IStreamChannel Members

        public bool Available
        {
            get { return false; }
        }

        public void init(TextReader inputStream)
        {
            reader = inputStream;
            parser.ReInit(reader);
            _worker = new Thread(Run);
            _worker.Start();
        }

        public void init(Stream reader)
        {
            this.reader = new StreamReader(reader);
            parser.ReInit(reader);
            _worker = new Thread(Run);
            _worker.Start();
        }

        #endregion

        internal void close()
        {
            stopped = true;

            if (_worker != null)
            {
                _worker.Abort();
                while (_worker.ThreadState != ThreadState.Stopped)
                {
                    _worker.Abort();
                }
            }

            try
            {
                reader.Close();
                reader.Dispose();
            }
            catch (Exception e)
            {
                TraceLogger.Instance.Fatal(e);
            }
        }


        public void executeCommand(String commandString)
        {
            Object command = null;
            try
            {
                while ((command = parser.basicExpr()) != null)
                {
                    OnCommand(new CommandEventArgs(command, ChannelId));
                }
            }
            catch (ParseException e)
            {
                OnMessage(new MessageEventArgs(EventType.PARSE_ERROR, e, ChannelId));
            }
        }


        public void Run()
        {
            while (!stopped)
            {
                Object command = null;
                try
                {
                    while (!stopped && (command = parser.basicExpr()) != null)
                    {
                        OnCommand(new CommandEventArgs(command, ChannelId));
                    }
                }
                catch (ParseException e)
                {
                    OnMessage(new MessageEventArgs(EventType.PARSE_ERROR, e, ChannelId));
                    parser.ReInit(reader);
                }
                catch (TokenMgrError e)
                {
                    OnMessage(new MessageEventArgs(EventType.PARSE_ERROR, e, ChannelId));
                    parser.ReInit(reader);
                }
                catch (Exception e)
                {
                    OnMessage(new MessageEventArgs(EventType.PARSE_ERROR, e, ChannelId));
                    parser.ReInit(reader);
                }
            }
        }
    }
}