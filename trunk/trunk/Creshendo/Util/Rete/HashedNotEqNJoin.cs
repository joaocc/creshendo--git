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
using Creshendo.Util.Rete.Exception;

namespace Creshendo.Util.Rete
{
    /// <author>  Peter Lin
    /// 
    /// HashedNotEqBNode2 indexes the right input for joins that use
    /// not equal to. It uses 2 levels of indexing. The first is the bindings
    /// for equal to, the second is not equal to.
    /// 
    /// </author>
    public class HashedNotEqNJoin : BaseJoin
    {
        public HashedNotEqNJoin(int id) : base(id)
        {
        }

        /// <summary> Clear will Clear the lists
        /// </summary>
        public override void clear(IWorkingMemory mem)
        {
            IGenericMap<Object, Object> leftmem = (IGenericMap<Object, Object>) mem.getBetaLeftMemory(this);
            HashedNeqAlphaMemory rightmem = (HashedNeqAlphaMemory) mem.getBetaRightMemory(this);
            IEnumerator itr = leftmem.Keys.GetEnumerator();
            // first we iterate over the list for each fact
            // and Clear it.
            while (itr.MoveNext())
            {
                IBetaMemory bmem = (IBetaMemory) leftmem.Get(itr.Current);
                bmem.clear();
            }
            // now that we've cleared the list for each fact, we
            // can Clear the Creshendo.rete.util.Map.
            leftmem.Clear();
            rightmem.clear();
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
            // need to think the getLeftValues through better to
            // account for cases when a join has no bindings
            NotEqHashIndex inx = new NotEqHashIndex(NodeUtils.getLeftBindValues(binds, linx.Facts));
            HashedNeqAlphaMemory rightmem = (HashedNeqAlphaMemory) mem.getBetaRightMemory(this);
            if (rightmem.zeroMatch(inx))
            {
                propogateAssert(linx, engine, mem);
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
            // Get the memory for the node
            HashedNeqAlphaMemory rightmem = (HashedNeqAlphaMemory) mem.getBetaRightMemory(this);
            NotEqHashIndex inx = new NotEqHashIndex(NodeUtils.getRightBindValues(binds, rfact));

            rightmem.addPartialMatch(inx, rfact);
            bool zm = rightmem.zeroMatch(inx);
            IGenericMap<Object, Object> leftmem = (IGenericMap<Object, Object>) mem.getBetaLeftMemory(this);
            IEnumerator itr = leftmem.Values.GetEnumerator();
            while (itr.MoveNext())
            {
                Index linx = (Index) itr.Current;
                if (evaluate(linx.Facts, rfact))
                {
                    if (!zm)
                    {
                        try
                        {
                            propogateRetract(linx, engine, mem);
                        }
                        catch (RetractException e)
                        {
                            throw new AssertException("NotJion - " + e.Message);
                        }
                    }
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
            propogateRetract(linx, engine, mem);
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
            NotEqHashIndex inx = new NotEqHashIndex(NodeUtils.getRightBindValues(binds, rfact));
            HashedNeqAlphaMemory rightmem = (HashedNeqAlphaMemory) mem.getBetaRightMemory(this);
            // first we Remove the fact from the right
            rightmem.removePartialMatch(inx, rfact);
            bool zm = rightmem.zeroMatch(inx);
            // now we see the left memory matched and Remove it also
            IGenericMap<Object, Object> leftmem = (IGenericMap<Object, Object>) mem.getBetaLeftMemory(this);
            IEnumerator itr = leftmem.Values.GetEnumerator();
            while (itr.MoveNext())
            {
                Index linx = (Index) itr.Current;
                if (evaluate(linx.Facts, rfact))
                {
                    if (zm)
                    {
                        try
                        {
                            propogateAssert(linx, engine, mem);
                        }
                        catch (AssertException e)
                        {
                            throw new RetractException("NotJion - " + e.Message);
                        }
                    }
                }
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

        /// <summary> returs the node name + id and bindings
        /// </summary>
        public override String toPPString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append("HashedNotEqNJoin-" + nodeID + "> ");
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