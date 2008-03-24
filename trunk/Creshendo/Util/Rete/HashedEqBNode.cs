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
    public class HashedEqBNode : BaseJoin
    {
        public HashedEqBNode(int id) : base(id)
        {
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
            leftmem.Put(linx, linx);
            EqHashIndex inx = new EqHashIndex(NodeUtils.getLeftValues(binds, linx.Facts));
            HashedAlphaMemoryImpl rightmem = (HashedAlphaMemoryImpl) mem.getBetaRightMemory(this);
            IEnumerator itr = rightmem.iterator(inx);
            if (itr != null)
            {
                while (itr.MoveNext())
                {
                    IFact vl = (IFact) itr.Current;
                    if (vl != null)
                    {
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
            HashedAlphaMemoryImpl rightmem = (HashedAlphaMemoryImpl) mem.getBetaRightMemory(this);
            EqHashIndex inx = new EqHashIndex(NodeUtils.getRightValues(binds, rfact));

            rightmem.addPartialMatch(inx, rfact);
            // now that we've added the facts to the list, we
            // proceed with evaluating the fact
            IGenericMap<Object, Object> leftmem = (IGenericMap<Object, Object>) mem.getBetaLeftMemory(this);
            // since there may be key collisions, we iterate over the
            // values of the HashMap. If we used keySet to iterate,
            // we could encounter a ClassCastException in the case of
            // key collision.
            IEnumerator itr = leftmem.Values.GetEnumerator();
            while (itr.MoveNext())
            {
                Index linx = (Index) itr.Current;
                if (evaluate(linx.Facts, rfact))
                {
                    // now we propogate
                    propogateAssert(linx.add(rfact), engine, mem);
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
            EqHashIndex eqinx = new EqHashIndex(NodeUtils.getLeftValues(binds, linx.Facts));
            HashedAlphaMemoryImpl rightmem = (HashedAlphaMemoryImpl) mem.getBetaRightMemory(this);

            // now we propogate the retract. To do that, we have
            // merge each item in the list with the Fact array
            // and call retract in the successor nodes
            IEnumerator itr = rightmem.iterator(eqinx);
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
            EqHashIndex inx = new EqHashIndex(NodeUtils.getRightValues(binds, rfact));
            HashedAlphaMemoryImpl rightmem = (HashedAlphaMemoryImpl) mem.getBetaRightMemory(this);
            // first we Remove the fact from the right
            rightmem.removePartialMatch(inx, rfact);
            // now we see the left memory matched and Remove it also
            IGenericMap<Object, Object> leftmem = (IGenericMap<Object, Object>) mem.getBetaLeftMemory(this);
            IEnumerator itr = leftmem.Values.GetEnumerator();
            while (itr.MoveNext())
            {
                Index linx = (Index) itr.Current;
                if (evaluate(linx.Facts, rfact))
                {
                    propogateRetract(linx.add(rfact), engine, mem);
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
        public virtual bool evaluate(IFact[] leftlist, IFact right)
        {
            bool eval = true;
            // we iterate over the binds and evaluate the facts
            for (int idx = 0; idx < binds.Length; idx++)
            {
                // we got the binding
                Binding bnd = binds[idx];
                eval = bnd.evaluate(leftlist, right);
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
            buf.Append("HashedEqBNode-" + nodeID + "> ");
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