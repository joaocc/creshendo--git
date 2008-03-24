/*
* Copyright 2002-2007 Peter Lin
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
using System.Collections.Generic;
using Creshendo.Util.Collections;

namespace Creshendo.Util.Rete
{
    /// <author>  Peter Lin
    /// *
    /// ObjectTypeNode is the input node for a specific type. The node
    /// is created with the appropriate Class. A couple of important notes
    /// about the implementation of ObjectTypeNode.
    /// 
    /// 
    /// the assertFact method does not check the deftemplate matches
    /// the fact. this is because of inheritance.
    /// WorkingMemoryImpl checks to see if the fact's deftemplate
    /// has parents. If it does, it will keep checking to see if there is
    /// an ObjectTypeNode for the parent.
    /// if the template has a parent, it will assert it. this means
    ///  any patterns for parent templates will attempt to pattern
    /// match
    /// 
    /// 
    /// </author>
    [Serializable]
    public class ObjectTypeNode : BaseAlpha
    {
        /// <summary> before the operators included not nill, but for it to work properly
        /// we have to first test the slot is not nill, and then do the hash
        /// lookup. not sure that it's worth it, so removed it instead.
        /// </summary>
        //UPGRADE_NOTE: Final was removed from the declaration of 'operators '. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1003"'
        public static readonly int[] operators = new int[] {Constants.EQUAL, Constants.NILL};

        /// <summary> The Class that defines object type
        /// </summary>
        private ITemplate deftemplate = null;

        /// <summary> HashMap entries for unique AlphaNodes
        /// </summary>
        //UPGRADE_NOTE: The initialization of  'entries' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
        private IGenericMap<object, object> entries;

        /// <summary> If we can gaurantee Uniqueness of the AlphaNodes, set it to true
        /// </summary>
        private bool gauranteeUnique = true;

        /// <summary> Second org.jamocha.rete.util.List for all nodes that do not use ==, null operators
        /// </summary>
        //UPGRADE_NOTE: The initialization of  'successor2' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
        protected internal List<object> successor2;

        /// <summary> 
        /// </summary>
        public ObjectTypeNode(int id, ITemplate deftemp) : base(id)
        {
            InitBlock();
            deftemplate = deftemp;
        }

        public virtual ITemplate Deftemplate
        {
            get { return deftemplate; }
        }

        /// <summary> return the number of successor nodes
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public virtual int SuccessorCount
        {
            get { return successorNodes.Length + successor2.Count; }
        }

        public override Object[] SuccessorNodes
        {
            get
            {
                List<object> successors = new List<Object>();
                successors.AddRange(successor2);
                for (int idx = 0; idx < successorNodes.Length; idx++)
                {
                    successors.Add(successorNodes[idx]);
                }

                Object[] ary = new object[successors.Count];
                successors.CopyTo(ary, 0);
                return ary;
            }
        }

        private void InitBlock()
        {
            entries = CollectionFactory.localMap();
            successor2 = new List<Object>();
        }


        /// <summary> Clear the memory. for now the method does not
        /// Remove all the successor nodes. need to think it over a bit.
        /// </summary>
        public override void clear(IWorkingMemory mem)
        {
            IAlphaMemory am = (IAlphaMemory) mem.getAlphaMemory(this);
            am.clear();
        }

        /// <summary> method to Clear the successors. method doesn't iterate over
        /// the succesors and Clear them individually.
        /// </summary>
        public virtual void clearSuccessors()
        {
            IEnumerator itr = successor2.GetEnumerator();
            while (itr.MoveNext())
            {
                BaseNode n = (BaseNode) itr.Current;
                n.removeAllSuccessors();
            }
            itr = entries.Values.GetEnumerator();
            while (itr.MoveNext())
            {
                BaseNode n = (BaseNode) itr.Current;
                n.removeAllSuccessors();
            }
            for (int idx = 0; idx < successorNodes.Length; idx++)
            {
                successorNodes[idx].removeAllSuccessors();
            }
            successor2.Clear();
            successorNodes = new BaseNode[0];
            entries.Clear();
        }

        /// <summary> assert the fact and propogate. ObjectTypeNode does not call
        /// assertEvent, since it's not that important and doesn't really
        /// help debugging.
        /// </summary>
        /// <param name="">fact
        /// </param>
        /// <param name="">engine
        /// 
        /// </param>
        public override void assertFact(IFact fact, Rete engine, IWorkingMemory mem)
        {
            // ObjectTypeNode doesn't bother checking the deftemplate.
            ((IAlphaMemory) mem.getAlphaMemory(this)).addPartialMatch(fact);
            // if the number of succesor nodes is less than (slot count * opCount)
            if (gauranteeUnique && fact.Deftemplate.AllSlots.Length > 0 && successorNodes.Length > (fact.Deftemplate.AllSlots.Length*operators.Length))
            {
                assertFactWithMap(fact, engine, mem);
            }
            else
            {
                assertAllSuccessors(fact, engine, mem);
            }
        }

        /// <summary> assert using HashMap approach
        /// 
        /// </summary>
        /// <param name="">fact
        /// </param>
        /// <param name="">engine
        /// </param>
        /// <param name="">mem
        /// 
        /// </param>
        public virtual void assertFactWithMap(IFact fact, Rete engine, IWorkingMemory mem)
        {
            Slot[] slots = fact.Deftemplate.AllSlots;
            // iterate over the slots
            for (int idx = 0; idx < slots.Length; idx++)
            {
                // only if the slot's node count is greater than zero 
                // do we go ahead and lookup in the HashMap
                if (slots[idx].NodeCount > 0)
                {
                    // iterate over the operators
                    for (int ops = 0; ops < operators.Length; ops++)
                    {
                        CompositeIndex comIndex = new CompositeIndex(slots[idx].Name, operators[ops], fact.getSlotValue(idx));

                        Object node = entries.Get(comIndex);
                        if (node != null)
                        {
                            if (node is BaseAlpha)
                            {
                                ((BaseAlpha) node).assertFact(fact, engine, mem);
                            }
                            else if (node is BaseJoin)
                            {
                                ((BaseJoin) node).assertRight(fact, engine, mem);
                            }
                            else if (node is TerminalNode)
                            {
                                Index inx = new Index(new IFact[] {fact});
                                ((TerminalNode) node).assertFacts(inx, engine, mem);
                            }
                        }
                    }
                }
            }
            assertSecondSuccessors(fact, engine, mem);
        }

        /// <summary> Propogate the fact using the normal way of iterating over the
        /// successors and calling assert on AlphaNodes and assertRight on
        /// BetaNodes.
        /// </summary>
        /// <param name="">fact
        /// </param>
        /// <param name="">engine
        /// </param>
        /// <param name="">mem
        /// @throws AssertException
        /// 
        /// </param>
        public virtual void assertAllSuccessors(IFact fact, Rete engine, IWorkingMemory mem)
        {
            for (int idx = 0; idx < successorNodes.Length; idx++)
            {
                Object node = successorNodes[idx];
                if (node is BaseAlpha)
                {
                    ((BaseAlpha) node).assertFact(fact, engine, mem);
                }
                else if (node is BaseJoin)
                {
                    ((BaseJoin) node).assertRight(fact, engine, mem);
                }
                else if (node is TerminalNode)
                {
                    Index inx = new Index(new IFact[] {fact});
                    ((TerminalNode) node).assertFacts(inx, engine, mem);
                }
            }
            assertSecondSuccessors(fact, engine, mem);
        }

        public virtual void assertSecondSuccessors(IFact fact, Rete engine, IWorkingMemory mem)
        {
            IEnumerator itr = successor2.GetEnumerator();
            while (itr.MoveNext())
            {
                BaseNode node = (BaseNode) itr.Current;
                if (node is BaseAlpha)
                {
                    ((BaseAlpha) node).assertFact(fact, engine, mem);
                }
                else if (node is BaseJoin)
                {
                    ((BaseJoin) node).assertRight(fact, engine, mem);
                }
                else if (node is TerminalNode)
                {
                    Index inx = new Index(new IFact[] {fact});
                    ((TerminalNode) node).assertFacts(inx, engine, mem);
                }
            }
        }

        /// <summary> Retract the fact to the succeeding nodes. ObjectTypeNode does not call
        /// assertEvent, since it's not that important and doesn't really
        /// help debugging.
        /// </summary>
        /// <param name="">fact
        /// </param>
        /// <param name="">engine
        /// 
        /// </param>
        public override void retractFact(IFact fact, Rete engine, IWorkingMemory mem)
        {
            if (fact.Deftemplate == deftemplate)
            {
                ((IAlphaMemory) mem.getAlphaMemory(this)).removePartialMatch(fact);
                for (int idx = 0; idx < successorNodes.Length; idx++)
                {
                    Object node = successorNodes[idx];
                    if (node is BaseAlpha)
                    {
                        ((BaseAlpha) node).retractFact(fact, engine, mem);
                    }
                    else if (node is BaseJoin)
                    {
                        ((BaseJoin) node).retractRight(fact, engine, mem);
                    }
                }
                IEnumerator itr2 = successor2.GetEnumerator();
                while (itr2.MoveNext())
                {
                    BaseNode node = (BaseNode) itr2.Current;
                    if (node is BaseAlpha)
                    {
                        ((BaseAlpha) node).retractFact(fact, engine, mem);
                    }
                    else if (node is BaseJoin)
                    {
                        ((BaseJoin) node).retractRight(fact, engine, mem);
                    }
                    else if (node is TerminalNode)
                    {
                        Index inx = new Index(new IFact[] {fact});
                        ((TerminalNode) node).retractFacts(inx, engine, mem);
                    }
                }
            }
        }


        /// <summary> Add a successor node
        /// </summary>
        public override void addSuccessorNode(BaseNode node, Rete engine, IWorkingMemory mem)
        {
            if (!containsNode(successorNodes, node) && !successor2.Contains(node))
            {
                if (node is BaseJoin || node is TerminalNode)
                {
                    successor2.Add(node);
                }
                else
                {
                    // we test to see if the operator is ==, nil, not nil
                    // if the node isn't BaseJoin, it should be BaseAlpha
                    BaseAlpha ba = (BaseAlpha) node;
                    if (ba.Operator == Constants.LESS || ba.Operator == Constants.GREATER || ba.Operator == Constants.LESSEQUAL || ba.Operator == Constants.GREATEREQUAL || ba.Operator == Constants.NOTEQUAL || ba.Operator == Constants.NOTNILL)
                    {
                        successor2.Add(node);
                    }
                    else
                    {
                        addNode(node);
                    }
                }
                if (gauranteeUnique && node is AlphaNode)
                {
                    // now we use CompositeIndex instead of HashString
                    AlphaNode anode = (AlphaNode) node;
                    entries.Put(anode.HashIndex, node);
                    // we increment the node count for the slot
                    deftemplate.getSlot(anode.slot.Id).incrementNodeCount();
                }
                // if there are matches, we propogate the facts to 
                // the new successor only
                IAlphaMemory alpha = (IAlphaMemory) mem.getAlphaMemory(this);
                if (alpha.size() > 0)
                {
                    IEnumerator itr = alpha.GetEnumerator();
                    while (itr.MoveNext())
                    {
                        IFact f = (IFact) itr.Current;
                        if (node is BaseAlpha)
                        {
                            BaseAlpha next = (BaseAlpha) node;
                            next.assertFact(f, engine, mem);
                        }
                        else if (node is BaseJoin)
                        {
                            BaseJoin next = (BaseJoin) node;
                            next.assertRight(f, engine, mem);
                        }
                        else if (node is TerminalNode)
                        {
                            TerminalNode t = (TerminalNode) node;
                            Index inx = new Index(new IFact[] {f});
                            t.assertFacts(inx, engine, mem);
                        }
                    }
                }
            }
        }

        public override bool removeNode(BaseNode n)
        {
            bool rem = base.removeNode(n);
            successor2.Remove(n);
            if (n is AlphaNode)
            {
                entries.Remove(((AlphaNode)n).HashIndex);
            }
            return rem;
        }

        /// <summary> For the ObjectTypeNode, the method just returns toString
        /// </summary>
        public override String hashString()
        {
            //UPGRADE_TODO: The equivalent in .NET for method 'java.Object.toString' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
            return ToString();
        }

        /// <summary> this returns name of the deftemplate
        /// </summary>
        public override String ToString()
        {
            return "ObjectTypeNode( " + deftemplate.Name + " ) -";
        }

        /// <summary> this returns name of the deftemplate
        /// </summary>
        public override String toPPString()
        {
            return " Template( " + deftemplate.Name + " )";
        }
    }
}