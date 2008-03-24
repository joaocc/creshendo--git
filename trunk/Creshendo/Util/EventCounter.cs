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
using System.Collections;
using System.Collections.Generic;
using Creshendo.Util.Collections;
using Creshendo.Util.Rete;

namespace Creshendo.Util
{
    /// <author>  Peter Lin
    /// *
    /// EventCounter is a simple utility class for counting and keeping track 
    /// of events. It can be used for various purposes like keeping track of
    /// statistics or unit tests.
    /// 
    /// </author>
    public class EventCounter : EngineEventListener
    {
        //UPGRADE_NOTE: The initialization of  'asserts' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
        private List<Object> asserts;
        //UPGRADE_NOTE: The initialization of  'retracts' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
        //UPGRADE_NOTE: The initialization of  'nodeFilter' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
        private IGenericMap<Object, Object> nodeFilter;
        private List<Object> profiles;
        private List<Object> retracts;

        /// <summary> 
        /// </summary>
        public EventCounter() 
        {
            InitBlock();
        }

        public virtual int AssertCount
        {
            get { return asserts.Count; }
        }

        public virtual int ProfileCount
        {
            get { return profiles.Count; }
        }

        public virtual int RetractCount
        {
            get { return retracts.Count; }
        }

        /* (non-Javadoc)
		* @see woolfel.engine.rete.EngineEventListener#eventOccurred(woolfel.engine.rete.EngineEvent)
		*/

        #region EngineEventListener Members

        public virtual void eventOccurred(EngineEvent event_Renamed)
        {
            if (event_Renamed.EventType == EngineEvent.ASSERT_EVENT)
            {
                asserts.Add(event_Renamed);
            }
            else if (event_Renamed.EventType == EngineEvent.ASSERT_PROFILE_EVENT)
            {
                asserts.Add(event_Renamed);
                profiles.Add(event_Renamed);
            }
            else if (event_Renamed.EventType == EngineEvent.ASSERT_RETRACT_EVENT)
            {
                asserts.Add(event_Renamed);
                retracts.Add(event_Renamed);
            }
            else if (event_Renamed.EventType == EngineEvent.ASSERT_RETRACT_PROFILE_EVENT)
            {
                asserts.Add(event_Renamed);
                profiles.Add(event_Renamed);
                retracts.Add(event_Renamed);
            }
            else if (event_Renamed.EventType == EngineEvent.PROFILE_EVENT)
            {
                profiles.Add(event_Renamed);
            }
            else if (event_Renamed.EventType == EngineEvent.RETRACT_EVENT)
            {
                retracts.Add(event_Renamed);
            }
            Object val = nodeFilter.Get(event_Renamed.SourceNode);
            if (val != null)
            {
                ((List<Object>) val).Add(event_Renamed);
            }
        }

        #endregion

        private void InitBlock()
        {
            asserts = new List<Object>();
            retracts = new List<Object>();
            profiles = new List<Object>();
            nodeFilter = new GenericHashMap<object, object>();
        }


        /// <summary> To listen to a specific node, Add the node to the filter
        /// </summary>
        /// <param name="">node
        /// 
        /// </param>
        public virtual void addNodeFilter(BaseNode node)
        {
            nodeFilter.Put(node, new List<Object>());
        }

        public virtual IList getNodeEvents(BaseNode node)
        {
            return (IList) nodeFilter.Get(node);
        }
    }
}