/*
* Copyright 2002-2007 Peter Lin
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

namespace Creshendo.Util.Rete
{
    public enum EventType
    {
        ADD_RULE_EVENT = 0,
        REMOVE_RULE_EVENT = 1,
        PARSE_ERROR = 2,
        INVALID_RULE = 3,
        RULE_EXISTS = 4,
        TEMPLATE_NOTFOUND = 5,
        CLIPSPARSER_ERROR = 6,
        CLIPSPARSER_WARNING = 7,
        CLIPSPARSER_REINIT = 8,
        FUNCTION_NOT_FOUND = 9,
        FUNCTION_INVALID = 10,
        ADD_NODE_ERROR = 11,
        ERROR = 100,
        COMMAND = 101,
        RESULT = 102,
        ENGINE = 103,
    }

    /// <author>  Peter Lin
    /// 
    /// 
    /// </author>
    public abstract class AbstractMessageEventArgs : EventArgs
    {
        private String _channelId;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractMessageEventArgs"/> class.
        /// </summary>
        /// <param name="channelId">The channel id.</param>
        public AbstractMessageEventArgs(String channelId)
        {
            _channelId = channelId;
        }

        /// <summary> Returns the id of the sender of the message.
        /// 
        /// </summary>
        /// <returns> The sender-id
        /// 
        /// </returns>
        public virtual String ChannelId
        {
            get { return _channelId; }
            set { _channelId = value; }
        }
    }
}