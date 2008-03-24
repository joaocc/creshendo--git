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
using Creshendo.Util.Rete.Exception;

namespace Creshendo.Util.Rete
{
    /// <author>  woolfel
    /// 
    /// TemporalTNode is the temporal version of TerminalNode. The main difference
    /// is the node will check the facts. If all facts have not expired, the activation
    /// gets added to the agenda. If it's expired, the expired facts Get retracted and
    /// no activation is created.
    /// 
    /// </author>
    public class TemporalTNode : TerminalNode2
    {
        private bool temporal = false;

        /// <param name="">id
        /// </param>
        /// <param name="">rl
        /// 
        /// </param>
        public TemporalTNode(int id, Rule.IRule rl)
            : base(id, rl)
        {
        }

        public virtual bool Temporal
        {
            get { return temporal; }

            set { temporal = value; }
        }


        /// <summary> Method will call checkFacts() first to make sure none of the facts have
        /// expired. An activation is only created if the facts are valid.
        /// </summary>
        /// <param name="">facts
        /// </param>
        /// <param name="">engine
        /// 
        /// </param>
        public override void assertFacts(Index inx, Rete engine, IWorkingMemory mem)
        {
            // first check the facts and make sure they didn't expire
            if (checkFacts(inx, engine, mem))
            {
                LinkedActivation act = new LinkedActivation(theRule, inx);
                act.TerminalNode = this;
                if (temporal)
                {
                    engine.fireActivation(act);
                }
                else
                {
                    IGenericMap<Object, Object> tmem = (IGenericMap<Object, Object>) mem.getTerminalMemory(this);
                    tmem.Put(inx, act);
                    // Add the activation to the current module's activation list.
                    engine.Agenda.addActivation(act);
                }
            }
        }

        /// <summary> if all the facts have not expired, the method returns true. If a fact has
        /// expired, the method will retract the fact.
        /// </summary>
        /// <param name="">inx
        /// </param>
        /// <param name="">engine
        /// </param>
        /// <param name="">mem
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        protected internal virtual bool checkFacts(Index inx, Rete engine, IWorkingMemory mem)
        {
            IFact[] facts = inx.Facts;
            bool fresh = true;
            long current = (DateTime.Now.Ticks - 621355968000000000)/10000;
            for (int idx = 0; idx < facts.Length; idx++)
            {
                if (facts[idx] is ITemporalFact)
                {
                    TemporalDeffact tf = (TemporalDeffact) facts[idx];
                    if (tf.ExpirationTime < current)
                    {
                        // the fact has expired
                        fresh = false;
                        try
                        {
                            engine.retractFact(tf);
                        }
                        catch (RetractException e)
                        {
                            // we do nothing
                        }
                    }
                }
            }
            return fresh;
        }
    }
}