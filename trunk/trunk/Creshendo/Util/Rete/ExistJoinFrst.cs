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
using System.Text;
using Creshendo.Util.Collections;

namespace Creshendo.Util.Rete
{
    /// <author>  Peter Lin
    /// 
    /// ExistJoinFrst is a special implementation for situations
    /// when the first Conditional Element is an Exists. The main
    /// difference is the left input is a dummy and doesn't do
    /// anything. This gets around needing an InitialFact when the
    /// first CE is Exists. 
    /// 
    /// </author>
    public class ExistJoinFrst : BaseJoin
    {
        public ExistJoinFrst(int id) : base(id)
        {
        }

        /// <summary> Clear will Clear the lists
        /// </summary>
        public override void clear(IWorkingMemory mem)
        {
            IGenericMap<Object, Object> rightmem = (IGenericMap<Object, Object>) mem.getBetaRightMemory(this);
            IGenericMap<Object, Object> leftmem = (IGenericMap<Object, Object>) mem.getBetaRightMemory(this);
            IEnumerator itr = leftmem.Keys.GetEnumerator();
            // first we iterate over the list for each fact
            // and Clear it.
            while (itr.MoveNext())
            {
                IBetaMemory bmem = (IBetaMemory) leftmem.Get(itr.Current);
                bmem.clear();
            }
            // now that we've cleared the list for each fact, we
            // can Clear the org.jamocha.rete.util.Map.
            leftmem.Clear();
            rightmem.Clear();
        }

        /// <summary> assertLeft is a dummy, since we don't need an initial
        /// fact or LeftInputAdapater.
        /// </summary>
        /// <param name="">factInstance
        /// </param>
        /// <param name="">engine
        /// 
        /// </param>
        public override void assertLeft(Index linx, Rete engine, IWorkingMemory mem)
        {
        }

        /// <summary> Assert from the right side is always going to be from an
        /// Alpha node.
        /// </summary>
        /// <param name="">factInstance
        /// </param>
        /// <param name="">engine
        /// 
        /// </param>
        public override void assertRight(IFact rfact, Rete engine, IWorkingMemory mem)
        {
            // we only proceed if the fact hasn't already entered
            // the join node
            Index inx = new Index(new IFact[] {rfact});
            IGenericMap<Object, Object> rightmem = (IGenericMap<Object, Object>) mem.getBetaRightMemory(this);
            if (!rightmem.ContainsKey(inx))
            {
                int count = rightmem.Count;
                rightmem.Put(inx, rfact);
                // now that we've added the facts to the list, we
                // proceed with evaluating the fact
                if (count == 0 && rightmem.Count == 1)
                {
                    propogateAssert(inx, engine, mem);
                }
            }
        }

        /// <summary> retractLeft is a dummy and doesn't do anything
        /// </summary>
        /// <param name="">factInstance
        /// </param>
        /// <param name="">engine
        /// 
        /// </param>
        public override void retractLeft(Index linx, Rete engine, IWorkingMemory mem)
        {
        }

        /// <summary> Retract from the right works in the following order.
        /// 1. Remove the fact from the right memory
        /// 2. check which left memory matched
        /// 3. propogate the retract
        /// </summary>
        /// <param name="">factInstance
        /// </param>
        /// <param name="">engine
        /// 
        /// </param>
        public override void retractRight(IFact rfact, Rete engine, IWorkingMemory mem)
        {
            Index inx = new Index(new IFact[] {rfact});
            IGenericMap<Object, Object> rightmem = (IGenericMap<Object, Object>) mem.getBetaRightMemory(this);
            if (rightmem.ContainsKey(inx))
            {
                int count = rightmem.Count;
                rightmem.Remove(inx);
                if (count == 1 && rightmem.Count == 0)
                {
                    propogateRetract(inx, engine, mem);
                }
            }
        }

        /// <summary> method returns string format for the node
        /// </summary>
        public override String ToString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append("Exist - (no bindings)");
            return buf.ToString();
        }

        /// <summary> The current implementation is similar to BetaNode
        /// </summary>
        public override String toPPString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append("<node-" + nodeID + "> Exist - ");
            buf.Append(" (no bindings) ");
            return buf.ToString();
        }
    }
}