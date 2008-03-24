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
    /// ExistJoin is implemented differently than how CLIPS does it. According
    /// to CLIPS beginners guide, Exist is convert to (not (not (blah) ) ).
    /// Rather than do that, I'm experimenting with a specialized Existjoin
    /// node instead. The benefit is reduce memory and fewer nodes in the 
    /// network. 
    /// 
    /// </author>
    public class ExistJoin : BaseJoin
    {
        public ExistJoin(int id) : base(id)
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

        /// <summary> assertLeft takes an array of facts. Since the Current join may be
        /// joining against one or more objects, we need to pass all
        /// previously matched facts.
        /// </summary>
        /// <param name="">factInstance
        /// </param>
        /// <param name="">engine
        /// 
        /// </param>
        public override void assertLeft(Index linx, Rete engine, IWorkingMemory mem)
        {
            IGenericMap<Object, Object> leftmem = (IGenericMap<Object, Object>) mem.getBetaLeftMemory(this);
            leftmem.Put(linx, linx);
            EqHashIndex inx = new EqHashIndex(NodeUtils.getLeftValues(binds, linx.Facts));
            HashedAlphaMemoryImpl rightmem = (HashedAlphaMemoryImpl) mem.getBetaRightMemory(this);
            if (rightmem.count(inx) > 0)
            {
                propogateAssert(linx, engine, mem);
            }
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
            HashedAlphaMemoryImpl rightmem = (HashedAlphaMemoryImpl) mem.getBetaRightMemory(this);
            EqHashIndex inx = new EqHashIndex(NodeUtils.getRightValues(binds, rfact));
            int after = rightmem.addPartialMatch(inx, rfact);
            IGenericMap<Object, Object> leftmem = (IGenericMap<Object, Object>) mem.getBetaLeftMemory(this);
            IEnumerator itr = leftmem.Values.GetEnumerator();
            while (itr.MoveNext())
            {
                Index linx = (Index) itr.Current;
                if (evaluate(linx.Facts, rfact))
                {
                    if (after == 1)
                    {
                        propogateAssert(linx, engine, mem);
                    }
                }
            }
        }

        /// <summary> Retracting from the left is different than retractRight for couple
        /// of reasons.
        /// <ul>
        /// <li> NotJoin will only propogate the facts from the left</li>
        /// <li> NotJoin never needs to merge the left and right</li>
        /// </ul>
        /// </summary>
        /// <param name="">factInstance
        /// </param>
        /// <param name="">engine
        /// 
        /// </param>
        public override void retractLeft(Index inx, Rete engine, IWorkingMemory mem)
        {
            IGenericMap<Object, Object> leftmem = (IGenericMap<Object, Object>) mem.getBetaLeftMemory(this);
            leftmem.Remove(inx);
            propogateRetract(inx, engine, mem);
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
            HashedAlphaMemoryImpl rightmem = (HashedAlphaMemoryImpl) mem.getBetaRightMemory(this);
            EqHashIndex inx = new EqHashIndex(NodeUtils.getRightValues(binds, rfact));
            // Remove the fact from the right
            int after = rightmem.removePartialMatch(inx, rfact);
            if (after == 0)
            {
                // now we see the left memory matched and Remove it also
                IGenericMap<Object, Object> leftmem = (IGenericMap<Object, Object>) mem.getBetaLeftMemory(this);
                IEnumerator itr = leftmem.Values.GetEnumerator();
                while (itr.MoveNext())
                {
                    Index linx = (Index) itr.Current;
                    if (evaluate(linx.Facts, rfact))
                    {
                        propogateRetract(linx, engine, mem);
                    }
                }
                inx = null;
            }
        }

        /// <summary> Method will use the right binding to perform the evaluation
        /// of the join. Since we are building joins similar to how
        /// CLIPS and other rule engines handle it, it means 95% of the
        /// time the right fact list only has 1 fact.
        /// </summary>
        /// <param name="">leftlist
        /// </param>
        /// <param name="">right
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public virtual bool evaluate(IFact[] leftlist, IFact right)
        {
            bool eval = true;
            // we iterate over the binds and evaluate the facts
            for (int idx = 0; idx < binds.Length; idx++)
            {
                Binding bnd = binds[idx];
                eval = bnd.evaluate(leftlist, right);
                if (!eval)
                {
                    break;
                }
            }
            return eval;
        }

        /// <summary> simple implementation for toString. may need to change the format
        /// later so it looks nicer.
        /// </summary>
        public override String ToString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append("Exist - ");
            for (int idx = 0; idx < binds.Length; idx++)
            {
                if (idx > 0)
                {
                    buf.Append(" && ");
                }
                buf.Append(binds[idx].toBindString());
            }
            return buf.ToString();
        }

        /// <summary> The current implementation is similar to BetaNode
        /// </summary>
        public override String toPPString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append("<node-" + nodeID + "> Exist - ");
            if (binds != null && binds.Length > 0)
            {
                for (int idx = 0; idx < binds.Length; idx++)
                {
                    if (idx > 0)
                    {
                        buf.Append(" && ");
                    }
                    buf.Append(binds[idx].toPPString());
                }
            }
            else
            {
                buf.Append(" no joins ");
            }
            return buf.ToString();
        }
    }
}