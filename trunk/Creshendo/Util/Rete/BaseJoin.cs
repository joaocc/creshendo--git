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
using Creshendo.Util.Collections;

namespace Creshendo.Util.Rete
{
    /// <author>  Peter Lin
    /// *
    /// BaseJoin is the abstract base for all join node classes.
    /// 
    /// </author>
    public abstract class BaseJoin : BaseNode
    {
        /// <summary> binding for the join
        /// </summary>
        protected internal Binding[] binds = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseJoin"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        public BaseJoin(int id) : base(id)
        {
        }

        /// <summary>
        /// Set the bindings for this join
        /// </summary>
        /// <value>The bindings.</value>
        public virtual Binding[] Bindings
        {
            set { binds = value; }
        }

        /// <summary>
        /// Subclasses must implement this method. assertLeft takes
        /// inputs from left input adapter nodes and join nodes.
        /// </summary>
        /// <param name="lfacts">The lfacts.</param>
        /// <param name="engine">The engine.</param>
        /// <param name="mem">The mem.</param>
        public abstract void assertLeft(Index lfacts, Rete engine, IWorkingMemory mem);

        /// <summary>
        /// Subclasses must implement this method. assertRight takes
        /// input from alpha nodes.
        /// </summary>
        /// <param name="rfact">The rfact.</param>
        /// <param name="engine">The engine.</param>
        /// <param name="mem">The mem.</param>
        public abstract void assertRight(IFact rfact, Rete engine, IWorkingMemory mem);

        /// <summary>
        /// Subclasses must implement this method. retractLeft takes
        /// input from left input adapter nodes and join nodes.
        /// </summary>
        /// <param name="lfacts">The lfacts.</param>
        /// <param name="engine">The engine.</param>
        /// <param name="mem">The mem.</param>
        public abstract void retractLeft(Index lfacts, Rete engine, IWorkingMemory mem);

        /// <summary>
        /// Subclasses must implement this method. retractRight takes
        /// input from alpha nodes.
        /// </summary>
        /// <param name="rfact">The rfact.</param>
        /// <param name="engine">The engine.</param>
        /// <param name="mem">The mem.</param>
        public abstract void retractRight(IFact rfact, Rete engine, IWorkingMemory mem);


        /// <summary>
        /// Adds the successor node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="engine">The engine.</param>
        /// <param name="mem">The mem.</param>
        public override void addSuccessorNode(BaseNode node, Rete engine, IWorkingMemory mem)
        {
            if (node is BaseJoin)
            {
                addSuccessorNode((BaseJoin) node, engine, mem);
            }
            else
            {
                addSuccessorNode((TerminalNode) node, engine, mem);
            }
        }

        /// <summary>
        /// When new Successor nodes are added, we propogate the facts that matched to
        /// the new join node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="engine">The engine.</param>
        /// <param name="mem">The mem.</param>
        public virtual void addSuccessorNode(BaseJoin node, Rete engine, IWorkingMemory mem)
        {
            if (addNode(node))
            {
                // first, we Get the memory for this node
                IGenericMap<Object, Object> leftmem = (IGenericMap<Object, Object>) mem.getBetaLeftMemory(this);
                // now we iterate over the entry set
                IEnumerator itr = leftmem.GetEnumerator();
                while (itr.MoveNext())
                {
                    IBetaMemory bmem = (IBetaMemory) itr.Current;
                    Index left = bmem.Index;
                    // iterate over the matches
                    IGenericMap<Object, Object> rightmem = (IGenericMap<Object, Object>) mem.getBetaRightMemory(this);
                    IEnumerator ritr = rightmem.Keys.GetEnumerator();
                    while (ritr.MoveNext())
                    {
                        IFact rfcts = (IFact) ritr.Current;
                        // now assert in the new join node
                        node.assertLeft(left.add(rfcts), engine, mem);
                    }
                }
            }
        }

        /// <summary>
        /// it's unlikely 2 rules are identical, except for the name. The implementation
        /// gets the current memory and propogates, but I wonder how much sense this
        /// makes in a real production environment. An user really shouldn't be deploying
        /// identical rules with different rule name.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="engine">The engine.</param>
        /// <param name="mem">The mem.</param>
        public virtual void addSuccessorNode(TerminalNode node, Rete engine, IWorkingMemory mem)
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
                        Index left = bmem.Index;
                        // iterate over the matches
                        IGenericMap<Object, Object> rightmem = (IGenericMap<Object, Object>) mem.getBetaRightMemory(this);
                        IEnumerator ritr = rightmem.Keys.GetEnumerator();
                        while (ritr.MoveNext())
                        {
                            IFact rfcts = (IFact) ritr.Current;
                            // merge the left and right fact into a new Array
                            node.assertFacts(left.add(rfcts), engine, mem);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Method is used to decompose the network and make sure
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

        /// <summary>
        /// Method is used to pass a fact to the successor nodes
        /// </summary>
        /// <param name="inx">The inx.</param>
        /// <param name="engine">The engine.</param>
        /// <param name="mem">The mem.</param>
        protected internal virtual void propogateAssert(Index inx, Rete engine, IWorkingMemory mem)
        {
            for (int idx = 0; idx < successorNodes.Length; idx++)
            {
                BaseJoin baseJoin = successorNodes[idx] as BaseJoin;
                if (baseJoin != null)
                {
                    baseJoin.assertLeft(inx, engine, mem);
                    return;
                }

                TerminalNode terminalNode = successorNodes[idx] as TerminalNode;
                if (terminalNode != null)
                {
                    terminalNode.assertFacts(inx, engine, mem);
                    return;
                }

                //BaseNode node = successorNodes[idx];
                //if (node is BaseJoin)
                //{
                //    ((BaseJoin) node).assertLeft(inx, engine, mem);
                //}
                //else if (node is TerminalNode)
                //{
                //    ((TerminalNode) node).assertFacts(inx, engine, mem);
                //}
            }
        }

        /// <summary>
        /// method for propogating the retract
        /// </summary>
        /// <param name="inx">The inx.</param>
        /// <param name="engine">The engine.</param>
        /// <param name="mem">The mem.</param>
        protected internal virtual void propogateRetract(Index inx, Rete engine, IWorkingMemory mem)
        {
            for (int idx = 0; idx < successorNodes.Length; idx++)
            {

                BaseJoin node = successorNodes[idx] as BaseJoin;
                if (node != null)
                {
                    node.retractLeft(inx, engine, mem);
                }
                else
                {
                    TerminalNode tnode = successorNodes[idx] as TerminalNode;
                    if (tnode != null)
                    {
                        tnode.retractFacts(inx, engine, mem);
                    }
                }


                //BaseNode node = successorNodes[idx];
                //if (node is BaseJoin)
                //{
                //    ((BaseJoin) node).retractLeft(inx, engine, mem);
                //}
                //else if (node is TerminalNode)
                //{
                //    ((TerminalNode) node).retractFacts(inx, engine, mem);
                //}
            }
        }
    }
}