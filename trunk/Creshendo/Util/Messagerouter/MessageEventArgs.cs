using System;
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
    public delegate void MessageEventHandler(object sender, MessageEventArgs e);

    /// <summary> The Class for MessageEvents.
    /// 
    /// </summary>
    /// <author>  Alexander Wilden, Christoph Emonds, Sebastian Reinartz
    /// 
    /// 
    /// </author>
    public class MessageEventArgs : AbstractMessageEventArgs
    {

        /// <summary> The message that was send.
        /// </summary>
        private readonly Object message;

        private readonly EventType type;

        /// <summary>
        /// The constructor for a new MessageEvent. Uses CLIPS as standard-language.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="message">The message.</param>
        /// <param name="channelId">The channel id.</param>
        public MessageEventArgs(EventType type, Object message, String channelId) : base(channelId)
        {
            this.type = type;
            this.message = message;
        }

        /// <summary> Returns the message of this event
        /// 
        /// </summary>
        /// <returns> The message
        /// 
        /// </returns>
        public virtual Object Message
        {
            get { return message; }
        }

        public virtual EventType Type
        {
            get { return type; }
        }

        public virtual bool Error
        {
            get { return type < 0; }
        }
    }
}