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

namespace Creshendo.Util.Rete
{
    /// <author>  Peter Lin
    /// *
    /// LIANode stands for Left Input Adapter Node. Left input adapter node
    /// can only only have 1 alphaNode above it. Left input adapater nodes are
    /// not shared by multiple branches of the network, so it doesn't have any
    /// memory.
    /// 
    /// </author>
    public class LIANode : BaseAlpha
    {
        public LIANode(int id) : base(id)
        {
        }

        /// <summary> the implementation just propogates the assert down the network
        /// </summary>
        public override void assertFact(IFact fact, Rete engine, IWorkingMemory mem)
        {
            propogateAssert(fact, engine, mem);
        }

        /// <summary> Propogate the assert to the successor nodes
        /// </summary>
        /// <param name="">fact
        /// </param>
        /// <param name="">engine
        /// 
        /// </param>
        protected internal override void propogateAssert(IFact fact, Rete engine, IWorkingMemory mem)
        {
            for (int idx = 0; idx < successorNodes.Length; idx++)
            {
                BaseNode nNode = successorNodes[idx];
                if (nNode is BaseJoin)
                {
                    BaseJoin next = (BaseJoin) nNode;
                    IFact[] newf = new IFact[] {fact};
                    next.assertLeft(new Index(newf), engine, mem);
                }
                else if (nNode is TerminalNode)
                {
                    IFact[] newf = new IFact[] {fact};
                    TerminalNode tn = (TerminalNode) nNode;
                    tn.assertFacts(new Index(newf), engine, mem);
                }
            }
        }

        /// <summary> Retract simply propogates it down the network
        /// </summary>
        public override void retractFact(IFact fact, Rete engine, IWorkingMemory mem)
        {
            propogateRetract(fact, engine, mem);
        }

        /// <summary> propogate the retract
        /// 
        /// </summary>
        /// <param name="">fact
        /// </param>
        /// <param name="">engine
        /// 
        /// </param>
        protected internal override void propogateRetract(IFact fact, Rete engine, IWorkingMemory mem)
        {
            for (int idx = 0; idx < successorNodes.Length; idx++)
            {
                BaseNode nNode = successorNodes[idx];
                if (nNode is BaseJoin)
                {
                    BaseJoin next = (BaseJoin) nNode;
                    IFact[] newf = new IFact[] {fact};
                    next.retractLeft(new Index(newf), engine, mem);
                }
                else if (nNode is TerminalNode)
                {
                    TerminalNode next = (TerminalNode) nNode;
                    IFact[] newf = new IFact[] {fact};
                    next.retractFacts(new Index(newf), engine, mem);
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
                            Index inx = new Index(new IFact[] {(IFact) itr.Current});
                            next.assertLeft(inx, engine, mem);
                        }
                    }
                }
            }
        }

        public override String hashString()
        {
            //UPGRADE_TODO: The equivalent in .NET for method 'java.Object.toString' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
            return ToString();
        }

        /// <summary> the Left Input Adapter Node returns zero length string
        /// </summary>
        public override String ToString()
        {
            return "";
        }

        /// <summary> the Left input Adapter Node returns zero length string
        /// </summary>
        public override String toPPString()
        {
            return "";
        }
    }
}