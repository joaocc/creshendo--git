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
using Creshendo.Util.Rule;

namespace Creshendo.Util.Rete
{
    public delegate void CompileEventHandler(object sender, CompileMessageEventArgs e);

    /// <author>  Peter Lin
    /// 
    /// </author>
    public class CompileMessageEventArgs : EventArgs
    {
        private String message = "";
        private IRule rule;
        private EventType type;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompileMessageEventArgs"/> class.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="eventType">Type of the event.</param>
        public CompileMessageEventArgs(Object source, EventType eventType) 
        {
            type = eventType;
        }

        public virtual EventType EventType
        {
            get { return type; }
            set { type = value; }
        }

        public virtual String Message
        {
            get
            {
                if (rule != null)
                {
                    return rule.Name + " " + message;
                }
                else
                {
                    return message;
                }
            }

            set { message = value; }
        }

        public virtual IRule Rule
        {
            get { return rule; }
            set { rule = value; }
        }
    }
}