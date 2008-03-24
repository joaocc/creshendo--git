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
using Creshendo.Util.Collections;

namespace Creshendo.Util.Rete
{
    /// <author>  Peter Lin
    /// *
    /// NoAgendaTNode is different than TerminalNode2 in that it doesn't
    /// Get added to the agenda. Instead, it fires immediately.
    /// 
    /// </author>
    public class NoAgendaTNode : TerminalNode2
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NoAgendaTNode"/> class.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rl"></param>
        public NoAgendaTNode(int id, Rule.IRule rl) : base(id, rl)
        {
            theRule = rl;
        }

        /// <summary>
        /// Return the Rule object associated with this terminal node
        /// </summary>
        /// <value></value>
        /// <returns>
        /// </returns>
        public override Rule.IRule Rule
        {
            get { return theRule; }
        }

        /// <summary>
        /// method does not apply for no agenda terminal node
        /// </summary>
        /// <param name="mem"></param>
        public override void clear(IWorkingMemory mem)
        {
            IGenericMap<Object, Object> tmem = (IGenericMap<Object, Object>) mem.getTerminalMemory(this);
            if (tmem != null)
            {
                tmem.Clear();
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="inx"></param>
        /// <param name="engine"></param>
        /// <param name="mem"></param>
        public override void assertFacts(Index inx, Rete engine, IWorkingMemory mem)
        {
            LinkedActivation act = new LinkedActivation(theRule, inx);
            act.TerminalNode = this;
            // fire the activation immediately
            engine.fireActivation(act);
        }

        /// <summary>
        /// method does not apply, since the activation fires immediately,
        /// there's nothing to Remove from the agenda
        /// </summary>
        /// <param name="facts">The facts.</param>
        /// <param name="engine">The engine.</param>
        /// <param name="mem">The mem.</param>
        public virtual void retractFacts(IFact[] facts, Rete engine, IWorkingMemory mem)
        {
        }


        /// <summary>
        /// method doesn't apply for no agenda terminal node
        /// </summary>
        /// <param name="mem"></param>
        /// <param name="activation"></param>
        public override void removeActivation(IWorkingMemory mem, LinkedActivation activation)
        {
        }

        /// <summary>
        /// return the name of the rule
        /// </summary>
        /// <returns></returns>
        public override String ToString()
        {
            return theRule.Name;
        }

        /// <summary>
        /// return the name of the rule
        /// </summary>
        /// <returns></returns>
        public override String toPPString()
        {
            return theRule.Name;
        }
    }
}