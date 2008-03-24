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

namespace Creshendo.Util.Rete
{
    /// <author>  Peter Lin
    /// 
    /// BaseAlpha is the baseAlpha node for all 1-input nodes.
    /// 
    /// </author>
    public abstract class BaseAlpha : BaseNode
    {
        /// <summary> The operator to compare two values
        /// </summary>
        //UPGRADE_NOTE: The initialization of  'operator_Renamed' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
        protected internal int operator_Renamed;


        public BaseAlpha(int id) : base(id)
        {
            InitBlock();
        }

        /// <summary> Abstract implementation returns an int code for the
        /// operator. To Get the string representation, it should
        /// be converted.
        /// </summary>
        public virtual int Operator
        {
            get { return operator_Renamed; }
        }

        private void InitBlock()
        {
            operator_Renamed = Constants.EQUAL;
        }

        /// <summary> Alpha nodes must implement this method
        /// </summary>
        /// <param name="">factInstance
        /// </param>
        /// <param name="">engine
        /// 
        /// </param>
        public abstract void assertFact(IFact factInstance, Rete engine, IWorkingMemory mem);

        /// <summary> Alpha nodes must implement this method. Retract should Remove
        /// a fact from the node and propogate through the RETE network.
        /// </summary>
        /// <param name="">factInstance
        /// </param>
        /// <param name="">engine
        /// 
        /// </param>
        public abstract void retractFact(IFact factInstance, Rete engine, IWorkingMemory mem);

        public virtual int successorCount()
        {
            return successorNodes.Length;
        }

        /// <summary> method for propogating the retract
        /// </summary>
        /// <param name="">fact
        /// </param>
        /// <param name="">engine
        /// 
        /// </param>
        protected internal virtual void propogateRetract(IFact fact, Rete engine, IWorkingMemory mem)
        {
            for (int idx = 0; idx < successorNodes.Length; idx++)
            {
                Object nNode = successorNodes[idx];
                if (nNode is BaseAlpha)
                {
                    BaseAlpha next = (BaseAlpha) nNode;
                    next.retractFact(fact, engine, mem);
                }
                else if (nNode is BaseJoin)
                {
                    BaseJoin next = (BaseJoin) nNode;
                    // AlphaNodes always call retractRight in the
                    // BetaNode
                    next.retractRight(fact, engine, mem);
                }
                else if (nNode is TerminalNode)
                {
                    Index inx = new Index(new IFact[] {fact});
                    ((TerminalNode) nNode).retractFacts(inx, engine, mem);
                }
            }
        }

        /// <summary> Method is used to pass a fact to the successor nodes
        /// </summary>
        /// <param name="">fact
        /// </param>
        /// <param name="">engine
        /// 
        /// </param>
        protected internal virtual void propogateAssert(IFact fact, Rete engine, IWorkingMemory mem)
        {
            for (int idx = 0; idx < successorNodes.Length; idx++)
            {
                Object nNode = successorNodes[idx];
                if (nNode is BaseAlpha)
                {
                    BaseAlpha next = (BaseAlpha) nNode;
                    next.assertFact(fact, engine, mem);
                }
                else if (nNode is BaseJoin)
                {
                    BaseJoin next = (BaseJoin) nNode;
                    next.assertRight(fact, engine, mem);
                }
                else if (nNode is TerminalNode)
                {
                    TerminalNode next = (TerminalNode) nNode;
                    Index inx = new Index(new IFact[] {fact});
                    next.assertFacts(inx, engine, mem);
                }
            }
        }

        /// <summary> Set the Current node in the sequence of 1-input nodes.
        /// The Current node can be an AlphaNode or a LIANode.
        /// </summary>
        /// <param name="">node
        /// 
        /// </param>
        public override void addSuccessorNode(BaseNode node, Rete engine, IWorkingMemory mem)
        {
            if (addNode(node))
            {
                // if there are matches, we propogate the facts to 
                // the new successor only
                IAlphaMemory alpha = (IAlphaMemory) mem.getAlphaMemory(this);
                if (alpha.size() > 0)
                {
                    IEnumerator itr = alpha.GetEnumerator();
                    while (itr.MoveNext())
                    {
                        if (node is BaseAlpha)
                        {
                            BaseAlpha next = (BaseAlpha) node;
                            next.assertFact((IFact) itr.Current, engine, mem);
                        }
                        else if (node is BaseJoin)
                        {
                            BaseJoin next = (BaseJoin) node;
                            next.assertRight((IFact) itr.Current, engine, mem);
                        }
                        else if (node is TerminalNode)
                        {
                            TerminalNode next = (TerminalNode) node;
                            Index inx = new Index(new IFact[] {(IFact) itr.Current});
                            next.assertFacts(inx, engine, mem);
                        }
                    }
                }
            }
        }

        /// <summary> Remove a successor node
        /// </summary>
        /// <param name="">node
        /// </param>
        /// <param name="">engine
        /// </param>
        /// <param name="">mem
        /// @throws AssertException
        /// 
        /// </param>
        public virtual void removeSuccessorNode(BaseNode node, Rete engine, IWorkingMemory mem)
        {
            if (removeNode(node))
            {
                // we retract the memories first, before removing the node
                IAlphaMemory alpha = (IAlphaMemory) mem.getAlphaMemory(this);
                if (alpha.size() > 0)
                {
                    IEnumerator itr = alpha.GetEnumerator();
                    while (itr.MoveNext())
                    {
                        if (node is BaseAlpha)
                        {
                            BaseAlpha next = (BaseAlpha) node;
                            next.retractFact((IFact) itr.Current, engine, mem);
                        }
                        else if (node is BaseJoin)
                        {
                            BaseJoin next = (BaseJoin) node;
                            next.retractRight((IFact) itr.Current, engine, mem);
                        }
                    }
                }
            }
        }

        /// <summary> Get the list of facts that have matched the node
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public virtual IAlphaMemory getMemory(IWorkingMemory mem)
        {
            return (IAlphaMemory) mem.getAlphaMemory(this);
        }

        /// <summary>
        /// implementation simply Clear the List
        /// </summary>
        /// <param name="mem">The mem.</param>
        public override void clear(IWorkingMemory mem)
        {
            getMemory(mem).clear();
        }


        /// <summary> Subclasses need to implement this method. The hash string
        /// should be the slotId + operator + value
        /// </summary>
        public abstract override String hashString();

        /// <summary> subclasses need to implement PrettyPrintString and print
        /// out user friendly representation fo the node
        /// </summary>
        public abstract override String toPPString();

        /// <summary> subclasses need to implement the toString and return a textual
        /// form representation of the node.
        /// </summary>
        public abstract override String ToString();

        /// <summary> Method is used to decompose the network and make sure
        /// the nodes are detached from each other.
        /// </summary>
        public override void removeAllSuccessors()
        {
            for (int idx = 0; idx < successorNodes.Length; idx++)
            {
                BaseNode bn = (BaseNode) successorNodes[idx];
                bn.removeAllSuccessors();
            }
            successorNodes = new BaseNode[0];
            useCount = 0;
        }
    }
}