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

namespace Creshendo.Util.Rete
{
    /// <author>  Peter Lin
    /// *
    /// EngineEvent is a generic event class. Rather than have a bunch of
    /// event subclasses, the current design uses event type code.
    /// 
    /// </author>
    //UPGRADE_ISSUE: Class 'java.util.EventObject' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javautilEventObject"'
    public class EngineEvent //:EventObject
    {
        public const int ASSERT_EVENT = 0;
        public const int ASSERT_PROFILE_EVENT = 5;
        public const int ASSERT_RETRACT_EVENT = 3;
        public const int ASSERT_RETRACT_PROFILE_EVENT = 4;
        public const int PROFILE_EVENT = 2;
        public const int RETRACT_EVENT = 1;
        private IFact[] facts = null;
        private BaseNode sourceNode = null;

        /// <summary> the default value is assert event
        /// </summary>
        //UPGRADE_NOTE: The initialization of  'typeCode' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
        private int typeCode;

        /// <summary> 
        /// </summary>
        /// <param name="source">- the source should be either the workingMemory or Rete
        /// </param>
        /// <param name="typeCode">- event type
        /// </param>
        /// <param name="sourceNode">- the node which initiated the event
        /// 
        /// </param>
        public EngineEvent(Object source, int typeCode, BaseNode sourceNode, IFact[] facts)
        {
            InitBlock();
            //UPGRADE_ISSUE: Constructor 'java.util.EventObject.EventObject' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javautilEventObject"'
            this.typeCode = typeCode;
            this.sourceNode = sourceNode;
            this.facts = facts;
        }

        public virtual int EventType
        {
            get { return typeCode; }

            set { typeCode = value; }
        }

        public virtual BaseNode SourceNode
        {
            get { return sourceNode; }

            set { sourceNode = value; }
        }

        public virtual IFact[] Facts
        {
            get { return facts; }

            set { facts = value; }
        }

        private void InitBlock()
        {
            typeCode = ASSERT_EVENT;
        }
    }
}