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
    /// HashEqNJoin stands for Hashed Equal NotJoin. It is different from
    /// NotJoin. The right facts are hashed to improve performance. This means
    /// the node performs index joins to see if there's a matching facts on
    /// the right side. If there is, the node will propogate without
    /// performing evaluation. We can do this because only facts that match
    /// would have the same index.
    /// 
    /// </author>
    public class HashedEqNJoin : BaseJoin
    {
        public HashedEqNJoin(int id) : base(id)
        {
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
            // we don't bother adding the right fact to the left, since
            // the right side is already Hashed
            if (rightmem.count(inx) == 0)
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
            HashedAlphaMemoryImpl rightmem = (HashedAlphaMemoryImpl) mem.getBetaRightMemory(this);
            EqHashIndex inx = new EqHashIndex(NodeUtils.getRightValues(binds, rfact));
            rightmem.addPartialMatch(inx, rfact);
            int after = rightmem.count(inx);
            IGenericMap<Object, Object> leftmem = (IGenericMap<Object, Object>) mem.getBetaLeftMemory(this);
            IEnumerator itr = leftmem.Values.GetEnumerator();
            while (itr.MoveNext())
            {
                Index linx = (Index) itr.Current;
                if (evaluate(linx.Facts, rfact))
                {
                    if (after == 1)
                    {
                        // we have to retract
                        try
                        {
                            propogateRetract(linx, engine, mem);
                        }
                        catch (RetractException e)
                        {
                            //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                            throw new AssertException("NotJion - " + e.Message);
                        }
                    }
                }
            }
        }

        /// <summary> Retracting from the left is different than retractRight for couple of
        /// reasons.
        /// <ul>
        /// <li> NotJoin will only propogate the facts from the left</li>
        /// <li> NotJoin never needs to merge the left and right</li>
        /// </ul>
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
                        try
                        {
                            propogateAssert(linx, engine, mem);
                        }
                        catch (AssertException e)
                        {
                            //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                            throw new RetractException("NotJion - " + e.Message);
                        }
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

        /// <summary> NotJoin has to have a special addSuccessorNode since it needs
        /// to just propogate the left facts if it has zero matches.
        /// </summary>
        public override void addSuccessorNode(TerminalNode node, Rete engine, IWorkingMemory mem)
        {
            if (addNode(node))
            {
                // first, we Get the memory for this node
                IGenericMap<Object, Object> leftmem = (IGenericMap<Object, Object>) mem.getBetaLeftMemory(this);
                // now we iterate over the entry set
                IEnumerator itr = leftmem.Values.GetEnumerator();
                while (itr.MoveNext())
                {
                    Object omem = itr.Current;
                    if (omem is IBetaMemory)
                    {
                        IBetaMemory bmem = (IBetaMemory) omem;
                        EqHashIndex inx = new EqHashIndex(NodeUtils.getLeftValues(binds, bmem.LeftFacts));
                        HashedAlphaMemoryImpl rightmem = (HashedAlphaMemoryImpl) mem.getBetaRightMemory(this);
                        // we don't bother adding the right fact to the left, since
                        // the right side is already Hashed
                        if (rightmem.count(inx) == 0)
                        {
                            node.assertFacts(bmem.Index, engine, mem);
                        }
                    }
                }
            }
        }

        /// <summary> method returns a simple format for the node
        /// </summary>
        public override String ToString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append("HashedEqNJoin- ");
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
            buf.Append("HashedEqNJoin-" + nodeID + "> ");
            for (int idx = 0; idx < binds.Length; idx++)
            {
                if (idx > 0)
                {
                    buf.Append(" && ");
                }
                buf.Append(binds[idx].toPPString());
            }
            return buf.ToString();
        }
    }
}