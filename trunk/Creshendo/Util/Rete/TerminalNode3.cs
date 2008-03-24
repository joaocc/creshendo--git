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
    /// TerminalNode3 is for rules that have an effective and expiration date.
    /// When rules do not have it set, we don't bother checking. If it does, we
    /// make sure a new activation is added only if the rule is within the
    /// the two dates.
    /// 
    /// </author>
    public class TerminalNode3 : TerminalNode2
    {
        /// <param name="">id
        /// 
        /// </param>
        public TerminalNode3(int id, Rule.IRule rl) : base(id, rl)
        {
            theRule = rl;
        }

        /// <summary> The implementation checks to see if the rule is active before it tries to
        /// assert the fact. It checks in the following order.
        /// 1. is the expiration date greater than zero
        /// 2. is the current time > the effective date
        /// 3. is the current time the expiration date
        /// </summary>
        /// <param name="">facts
        /// </param>
        /// <param name="">engine
        /// 
        /// </param>
        public override void assertFacts(Index inx, Rete engine, IWorkingMemory mem)
        {
            long time = (DateTime.Now.Ticks - 621355968000000000)/10000;
            if (theRule.ExpirationDate > 0 && time > theRule.EffectiveDate && time < theRule.ExpirationDate)
            {
                LinkedActivation act = new LinkedActivation(theRule, inx);
                act.TerminalNode = this;
                IGenericMap<Object, Object> tmem = (IGenericMap<Object, Object>) mem.getTerminalMemory(this);
                tmem.Put(act.Index, act);
                // Add the activation to the current module's activation list.
                engine.Agenda.addActivation(act);
            }
        }

        /// <summary> The implementation checks to see if the rule is active before it tries to
        /// retract the fact. It checks in the following order.
        /// 1. is the expiration date greater than zero
        /// 2. is the current time > the effective date
        /// 3. is the current time the expiration date
        /// </summary>
        /// <param name="">facts
        /// </param>
        /// <param name="">engine
        /// 
        /// </param>
        public override void retractFacts(Index inx, Rete engine, IWorkingMemory mem)
        {
            long time = (DateTime.Now.Ticks - 621355968000000000)/10000;
            if (theRule.ExpirationDate > 0 && time > theRule.EffectiveDate && time < theRule.ExpirationDate)
            {
                IGenericMap<Object, Object> tmem = (IGenericMap<Object, Object>) mem.getTerminalMemory(this);
                LinkedActivation act = (LinkedActivation) tmem.RemoveWithReturn(inx);
                if (act != null)
                {
                    engine.Agenda.removeActivation(act);
                }
            }
        }
    }
}