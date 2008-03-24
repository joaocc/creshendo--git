using System;

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
    internal abstract class AbstractCommunicationChannel : ICommunicationChannel
    {
        private readonly String _channelId;
        private readonly InterestType _interest;
        private readonly MessageRouter _router;

        protected internal AbstractCommunicationChannel(String channelId, MessageRouter router, InterestType interest)
        {
            _channelId = channelId;
            _router = router;
            _interest = interest;
        }

        #region ICommunicationChannel Members

        public event CommandHandler Command;
        public event MessageHandler Message;

        public String ChannelId
        {
            get { return _channelId; }
        }

        public InterestType Interest
        {
            get { return _interest; }
        }

        public MessageRouter Router
        {
            get { return _router; }
        }

        #endregion

        protected void OnCommand(CommandEventArgs e)
        {
            if (Command != null)
            {
                Command(this, e);
            }
        }

        protected void OnMessage(MessageEventArgs e)
        {
            if (Message != null)
            {
                Message(this, e);
            }
        }
    }
}