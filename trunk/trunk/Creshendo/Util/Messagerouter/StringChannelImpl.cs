using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Creshendo.Util.Parser.Clips2;
using Creshendo.Util.Rete;
//using Wintellect.PowerCollections;
//using ParseException=Creshendo.Util.Parser.Clips.ParseException;
//using TokenMgrError=Creshendo.Util.Parser.Clips.TokenMgrError;

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
    internal class StringChannelImpl : AbstractCommunicationChannel//, IStringChannel
    {
        private System.Collections.IList alreadyReceived;
        private readonly CLIPSParser parser;

        public StringChannelImpl(String channelId, MessageRouter router, InterestType interest) : base(channelId, router, interest)
        {
            InitBlock();
            parser = new CLIPSParser(router.ReteEngine, (StreamReader) null);
        }

        #region StringChannel Members

        public virtual void executeCommand(String commandString)
        {
            executeCommand(commandString, false);
        }

        public virtual void executeCommand(String commandString, bool blocked)
        {
            List<Object> commandMessages = blocked ? new List<Object>() : null;
            parser.ReInit(new MemoryStream(ASCIIEncoding.ASCII.GetBytes(commandString)));
            Object command = null;
            try
            {
                alreadyReceived.Clear();
                while ((command = parser.basicExpr()) != null)
                {
                    OnCommand(new CommandEventArgs(command, ChannelId));
                    if (blocked)
                    {
                        try
                        {
                            while (blocked)
                            {
                                //router.fillMessageList(ChannelId, commandMessages);
                                int count = commandMessages.Count;
                                if (count > 0)
                                {
                                    MessageEventArgs last = (MessageEventArgs) commandMessages[count - 1];
                                    if (last.Type == EventType.RESULT || last.Type == EventType.ERROR)
                                    {
                                        foreach(object message in commandMessages)
                                        {
                                            ((IList)alreadyReceived).Add(message);
                                        }
                                        commandMessages.Clear();
                                        blocked = false;
                                    }
                                }
                                if (blocked)
                                {
                                    Thread.Sleep(new TimeSpan(10000*10));
                                }
                            }
                        }
                        catch (ThreadInterruptedException e)
                        {
                            /* TODO for now we just ignore this case */
                        }
                        blocked = true;
                    }
                }
            }
            catch (ParseException e)
            {
                OnMessage(new MessageEventArgs(EventType.PARSE_ERROR, e, ChannelId));
            }
            catch (TokenMgrError e)
            {
                OnMessage(new MessageEventArgs(EventType.PARSE_ERROR, e, ChannelId));
                parser.ReInit(new MemoryStream(ASCIIEncoding.ASCII.GetBytes(commandString)));
            }
        }

        #endregion

        private void InitBlock()
        {
            alreadyReceived = new List<Object>();
        }
    }
}