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
    /// NotJoin is used for Negated Conditional Elements. It is similar to
    /// BetaNode with a few important differences. When facts enter through
    /// the right side, it can only result in retracting facts from
    /// successor nodes and removal of activations from the agenda.
    /// Retracting facts from the right can only result in propogating
    /// facts down the RETE network. The node will only propogate when
    /// the match count goes from 1 to zero. Removing activations only
    /// happens when the match count on the left goes from zero to one. 
    /// 
    /// </author>
    public class NotJoin : BaseJoin
    {
        public NotJoin(int id) : base(id)
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

        /// <summary>
        /// assertLeft takes an array of facts. Since the Current join may be
        /// joining against one or more objects, we need to pass all
        /// previously matched facts.
        /// </summary>
        /// <param name="linx">The linx.</param>
        /// <param name="engine">The engine.</param>
        /// <param name="mem">The mem.</param>
        public override void assertLeft(Index linx, Rete engine, IWorkingMemory mem)
        {
            IGenericMap<Object, Object> leftmem = (IGenericMap<Object, Object>) mem.getBetaLeftMemory(this);
            // we create a new list for storing the matches.
            // any fact that isn't in the list will be evaluated.
            IBetaMemory bmem = new BetaMemoryImpl(linx);
            leftmem.Put(bmem.Index, bmem);
            IGenericMap<IFact, IFact> rightmem = (IGenericMap<IFact, IFact>)mem.getBetaRightMemory(this);
            int prevCount = bmem.matchCount();
            IEnumerator itr = rightmem.Values.GetEnumerator();
            while (itr.MoveNext())
            {
                IFact rfcts = (IFact) itr.Current;
                if (evaluate(linx.Facts, rfcts, engine))
                {
                    // it matched, so we Add it to the beta memory
                    bmem.addMatch(rfcts);
                }
            }
            // since the Fact[] is entering the left for the first time,
            // if there are no matches, we merged the facts propogate. 
            if (bmem.matchCount() == 0)
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
            // we only proceed if the fact hasn't already entered
            // the join node
            IGenericMap<IFact, IFact> rightmem = (IGenericMap<IFact, IFact>)mem.getBetaRightMemory(this);
            rightmem.Put(rfact, rfact);
            // now that we've added the facts to the list, we
            // proceed with evaluating the fact
            IGenericMap<Object, Object> leftmem = (IGenericMap<Object, Object>) mem.getBetaLeftMemory(this);
            IEnumerator itr = leftmem.Values.GetEnumerator();
            while (itr.MoveNext())
            {
                IBetaMemory bmem = (IBetaMemory) itr.Current;
                Index linx = bmem.Index;
                int prevCount = bmem.matchCount();
                if (evaluate(linx.Facts, rfact, engine))
                {
                    bmem.addMatch(rfact);
                }
                // When facts are asserted from the right, it can only
                // increase the match count, so basically it will never
                // need to propogate to successor nodes.
                if (prevCount == 0 && bmem.matchCount() != 0)
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
        public override void retractLeft(Index linx, Rete engine, IWorkingMemory mem)
        {
            IGenericMap<Object, Object> leftmem = (IGenericMap<Object, Object>) mem.getBetaLeftMemory(this);
            // the left memory Contains the fact array, so we 
            // retract it.
            IBetaMemory bmem = (IBetaMemory) leftmem.RemoveWithReturn(linx);
            if (bmem != null)
            {
                // if watch is turned on, we send an event
                propogateRetract(linx, engine, mem);
            }
        }

        /// <summary>
        /// Retract from the right works in the following order.
        /// 1. Remove the fact from the right memory
        /// 2. check which left memory matched
        /// 3. propogate the retract
        /// </summary>
        /// <param name="rfact">The rfact.</param>
        /// <param name="engine">The engine.</param>
        /// <param name="mem">The mem.</param>
        public override void retractRight(IFact rfact, Rete engine, IWorkingMemory mem)
        {
            IGenericMap<IFact, IFact> rightmem = (IGenericMap<IFact, IFact>)mem.getBetaRightMemory(this);
            if (rightmem.RemoveWithReturn(rfact) != null)
            {
                // now we see the left memory matched and Remove it also
                IGenericMap<Object, Object> leftmem = (IGenericMap<Object, Object>) mem.getBetaLeftMemory(this);
                IEnumerator itr = leftmem.Values.GetEnumerator();
                while (itr.MoveNext())
                {
                    IBetaMemory bmem = (IBetaMemory) itr.Current;
                    int prevCount = bmem.matchCount();
                    if (bmem.matched(rfact))
                    {
                        // we Remove the fact from the memory
                        bmem.removeMatch(rfact);
                        // since 1 or more matches prevents propogation
                        // we don't need to propogate retract. if the
                        // match count is now zero, we need to propogate
                        // assert
                        if (prevCount != 0 && bmem.matchCount() == 0)
                        {
                            try
                            {
                                propogateAssert(bmem.Index, engine, mem);
                            }
                            catch (AssertException e)
                            {
                                //UPGRADE_TODO: The equivalent in .NET for method 'java.lang.Throwable.getMessage' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                                throw new RetractException("NotJion - " + e.Message);
                            }
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
        public virtual bool evaluate(IFact[] leftlist, IFact right, Rete engine)
        {
            bool eval = true;
            // we iterate over the binds and evaluate the facts
            for (int idx = 0; idx < binds.Length; idx++)
            {
                Binding bnd = binds[idx];
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

        /// <summary> Method will evaluate a single slot from the left against the right.
        /// </summary>
        /// <param name="">left
        /// </param>
        /// <param name="">leftId
        /// </param>
        /// <param name="">right
        /// </param>
        /// <param name="">rightId
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public virtual bool evaluate(IFact left, int leftId, IFact right, int rightId, int opr)
        {
            if (opr == Constants.NOTEQUAL)
            {
                return Evaluate.evaluateNotEqual(left.getSlotValue(leftId), right.getSlotValue(rightId));
            }
            else
            {
                return Evaluate.evaluateEqual(left.getSlotValue(leftId), right.getSlotValue(rightId));
            }
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
                        // iterate over the matches
                        if (bmem.matchCount() == 0)
                        {
                            node.assertFacts(bmem.Index, engine, mem);
                        }
                    }
                }
            }
        }

        /// <summary> TODO implement this to return the bind info
        /// </summary>
        public override String ToString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append("NOT CE - ");
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
            buf.Append("node-" + nodeID + "> NOT CE - ");
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