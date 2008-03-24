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
    /// TerminalNode2 is different than TerminalNode in that it uses
    /// a different Activation implementation. Rather than use BasicActivation,
    /// it uses LinkedActivation.
    /// 
    /// </author>
    public class TerminalNode2 : TerminalNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TerminalNode2"/> class.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="rl"></param>
        public TerminalNode2(int id, Rule.IRule rl)
            : base(id, rl)
        {
            theRule = rl;
        }

        /// <summary>
        /// Return the Rule object associated with this terminal node
        /// </summary>
        /// <value>The rule.</value>
        /// <returns>
        /// </returns>
        public override Rule.IRule Rule
        {
            get { return theRule; }
        }

        /// <summary>
        /// The terminal nodes doesn't have a memory, so the method
        /// does nothing.
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
        /// Asserts the facts.
        /// </summary>
        /// <param name="inx">The inx.</param>
        /// <param name="engine">The engine.</param>
        /// <param name="mem">The mem.</param>
        public override void assertFacts(Index inx, Rete engine, IWorkingMemory mem)
        {
            LinkedActivation act = new LinkedActivation(theRule, inx);
            act.TerminalNode = this;
            IGenericMap<Object, Object> tmem = (IGenericMap<Object, Object>) mem.getTerminalMemory(this);
            tmem.Put(inx, act);
            // Add the activation to the current module's activation list.
            engine.Agenda.addActivation(act);
        }

        /// <summary>
        /// Retracts the facts.
        /// </summary>
        /// <param name="inx">The inx.</param>
        /// <param name="engine">The engine.</param>
        /// <param name="mem">The mem.</param>
        public override void retractFacts(Index inx, Rete engine, IWorkingMemory mem)
        {
            IGenericMap<Object, Object> tmem = (IGenericMap<Object, Object>) mem.getTerminalMemory(this);
            LinkedActivation act = (LinkedActivation) tmem.RemoveWithReturn(inx);
            if (act != null)
            {
                engine.Agenda.removeActivation(act);
            }
        }


        /// <summary>
        /// Remove the LinkedActivation from TerminalNode2. This is necessary
        /// when the activation is fired and the actions executed.
        /// </summary>
        /// <param name="mem">The mem.</param>
        /// <param name="activation">The activation.</param>
        public virtual void removeActivation(IWorkingMemory mem, LinkedActivation activation)
        {
            IGenericMap<Object, Object> tmem = (IGenericMap<Object, Object>) mem.getTerminalMemory(this);
            tmem.Remove(activation.Index);
        }

        /// <summary>
        /// method does not apply to termial nodes. therefore it's not implemented
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="engine">The engine.</param>
        /// <param name="mem">The mem.</param>
        public override void addSuccessorNode(BaseNode node, Rete engine, IWorkingMemory mem)
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