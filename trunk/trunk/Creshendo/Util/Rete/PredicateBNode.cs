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
    /// HashedBetaNode indexes the right input to improve cross product performance.
    /// 
    /// </author>
    public class PredicateBNode : BaseJoin
    {
        public PredicateBNode(int id) : base(id)
        {
        }

        /// <summary> Set the bindings for this join
        /// 
        /// </summary>
        /// <param name="">binds
        /// 
        /// </param>
        public override Binding[] Bindings
        {
            set { binds = value; }
        }


        /// <summary> Clear will Clear the lists
        /// </summary>
        public override void clear(IWorkingMemory mem)
        {
            IGenericMap<Object, Object> leftmem = (IGenericMap<Object, Object>) mem.getBetaLeftMemory(this);
            HashedAlphaMemoryImpl rightmem = (HashedAlphaMemoryImpl) mem.getBetaRightMemory(this);
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
            // TODO Clear the right memory
            rightmem.clear();
        }

        /// <summary> assertLeft takes an array of facts. Since the Current join may be joining
        /// against one or more objects, we need to pass all previously matched
        /// facts.
        /// 
        /// </summary>
        /// <param name="">factInstance
        /// </param>
        /// <param name="">engine
        /// 
        /// </param>
        public override void assertLeft(Index linx, Rete engine, IWorkingMemory mem)
        {
            IGenericMap<Object, Object> leftmem = (IGenericMap<Object, Object>) mem.getBetaLeftMemory(this);
            IBetaMemory bmem = new BetaMemoryImpl(linx);
            leftmem.Put(linx, bmem);
            IGenericMap<Object, Object> rightmem = (IGenericMap<Object, Object>) mem.getBetaRightMemory(this);
            IEnumerator itr = rightmem.Keys.GetEnumerator();
            if (itr != null)
            {
                while (itr.MoveNext())
                {
                    IFact vl = (IFact) itr.Current;
                    // we have to evaluate the function
                    if (vl != null && evaluate(linx.Facts, vl, engine))
                    {
                        bmem.addMatch(vl);
                        propogateAssert(linx.add(vl), engine, mem);
                    }
                }
            }
        }

        /// <summary> Assert from the right side is always going to be from an Alpha node.
        /// 
        /// </summary>
        /// <param name="">factInstance
        /// </param>
        /// <param name="">engine
        /// 
        /// </param>
        public override void assertRight(IFact rfact, Rete engine, IWorkingMemory mem)
        {
            IGenericMap<Object, Object> rightmem = (IGenericMap<Object, Object>) mem.getBetaRightMemory(this);
            rightmem.Put(rfact, rfact);
            IGenericMap<Object, Object> leftmem = (IGenericMap<Object, Object>) mem.getBetaLeftMemory(this);
            IEnumerator itr = leftmem.Values.GetEnumerator();
            while (itr.MoveNext())
            {
                IBetaMemory bmem = (IBetaMemory) itr.Current;
                if (evaluate(bmem.LeftFacts, rfact, engine))
                {
                    // now we propogate
                    bmem.addMatch(rfact);
                    propogateAssert(bmem.Index.add(rfact), engine, mem);
                }
            }
        }

        /// <summary> Retracting from the left requires that we propogate the
        /// 
        /// </summary>
        /// <param name="">factInstance
        /// </param>
        /// <param name="">engine
        /// 
        /// </param>
        public override void retractLeft(Index linx, Rete engine, IWorkingMemory mem)
        {
            IGenericMap<Object, Object> leftmem = (IGenericMap<Object, Object>) mem.getBetaLeftMemory(this);
            leftmem.Remove(linx);
            IGenericMap<Object, Object> rightmem = (IGenericMap<Object, Object>) mem.getBetaRightMemory(this);
            IEnumerator itr = rightmem.Keys.GetEnumerator();
            if (itr != null)
            {
                while (itr.MoveNext())
                {
                    propogateRetract(linx.add((IFact) itr.Current), engine, mem);
                }
            }
        }

        /// <summary> Retract from the right works in the following order. 1. Remove the fact
        /// from the right memory 2. check which left memory matched 3. propogate the
        /// retract
        /// 
        /// </summary>
        /// <param name="">factInstance
        /// </param>
        /// <param name="">engine
        /// 
        /// </param>
        public override void retractRight(IFact rfact, Rete engine, IWorkingMemory mem)
        {
            IGenericMap<Object, Object> rightmem = (IGenericMap<Object, Object>) mem.getBetaRightMemory(this);
            rightmem.Remove(rfact);
            IGenericMap<Object, Object> leftmem = (IGenericMap<Object, Object>) mem.getBetaLeftMemory(this);
            IEnumerator itr = leftmem.Values.GetEnumerator();
            while (itr.MoveNext())
            {
                IBetaMemory bmem = (IBetaMemory) itr.Current;
                if (evaluate(bmem.LeftFacts, rfact, engine))
                {
                    bmem.removeMatch(rfact);
                    propogateRetract(bmem.Index, engine, mem);
                }
            }
        }

        /// <summary> Method will use the right binding to perform the evaluation of the join.
        /// Since we are building joins similar to how CLIPS and other rule engines
        /// handle it, it means 95% of the time the right fact list only has 1 fact.
        /// 
        /// </summary>
        /// <param name="">leftlist
        /// </param>
        /// <param name="">right
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public virtual bool evaluate(IFact[] leftlist, IFact right, Rete engine)
        {
            bool eval = true;
            // we iterate over the binds and evaluate the facts
            for (int idx = 0; idx < binds.Length; idx++)
            {
                Binding bnd = (Binding) binds[idx];
                if (bnd is Binding2)
                {
                    eval = ((Binding2) bnd).evaluate(leftlist, right, engine);
                }
                else
                {
                    eval = bnd.evaluate(leftlist, right);
                }
                if (!eval)
                {
                    break;
                }
            }
            return eval;
        }

        /// <summary> Basic implementation will return string format of the betaNode
        /// </summary>
        public override String ToString()
        {
            StringBuilder buf = new StringBuilder();
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

        /// <summary> returns the node named + node id and the bindings in a string format
        /// </summary>
        public override String toPPString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append("PredicateBNode-" + nodeID + "> ");
            for (int idx = 0; idx < binds.Length; idx++)
            {
                if (idx > 0)
                {
                    buf.Append(" && ");
                }
                if (binds[idx] != null)
                {
                    buf.Append(binds[idx].toPPString());
                }
            }
            return buf.ToString();
        }
    }
}