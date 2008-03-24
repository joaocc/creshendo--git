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
using Creshendo.Util.Collections;

namespace Creshendo.Util.Rete
{
    /// <author>  Peter Lin
    /// *
    /// BaseNode is meant to define common logic that all nodes must have
    /// and implement common logic.
    /// 
    /// </author>
    [Serializable]
    public abstract class BaseNode : IPrettyPrint
    {
        protected internal int nodeID;

        /// <summary> We use an object Array to keep things efficient
        /// </summary>
        protected internal BaseNode[] successorNodes;

        /// <summary> The useCount is used to keep track of how many times
        /// an Alpha node is shared. This is needed so that we
        /// can dynamically Remove a rule at run time and Remove
        /// the node from the network. If we didn't keep count,
        /// it would be harder to figure out if we can Remove the node.
        /// </summary>
        protected internal int useCount = 0;

        /// <summary>
        /// BaseNode has only one constructor which takes an unique
        /// node id. All subclasses need to call the constructor.
        /// </summary>
        /// <param name="id">The id.</param>
        public BaseNode(int id)
        {
            InitBlock();
            nodeID = id;
        }

        /// <summary>
        /// Returns the successor nodes
        /// </summary>
        /// <value>The successor nodes.</value>
        public virtual Object[] SuccessorNodes
        {
            get { return successorNodes; }
        }

        /// <summary>
        /// Return the node id
        /// </summary>
        /// <value>The node id.</value>
        /// <returns>
        /// </returns>
        public virtual int NodeId
        {
            get { return nodeID; }
        }

        #region Print Members

        /// <summary>
        /// toPPString should return a string format, but formatted
        /// nicely so it's easier for humans to read. Chances are
        /// this method will be used in debugging mode, so the more
        /// descriptive the string is, the easier it is to figure out
        /// what the node does.
        /// </summary>
        /// <returns></returns>
        public abstract String toPPString();

        #endregion

        private void InitBlock()
        {
            successorNodes = new BaseNode[0];
        }


        /// <summary>
        /// Determines whether the specified list contains node.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="node">The node.</param>
        /// <returns>
        /// 	<c>true</c> if the specified list contains node; otherwise, <c>false</c>.
        /// </returns>
        protected internal virtual bool containsNode(Object[] list, Object node)
        {
            bool cn = false;
            for (int idx = 0; idx < list.Length; idx++)
            {
                if (list[idx] == node)
                {
                    cn = true;
                    break;
                }
            }
            return cn;
        }

        /// <summary>
        /// Add the node to the list of successors
        /// </summary>
        /// <param name="n">The n.</param>
        /// <returns></returns>
        protected internal virtual bool addNode(BaseNode n)
        {
            bool add = false;
            if (!containsNode(successorNodes, n))
            {
                successorNodes = ConversionUtils.add(successorNodes, n);
                add = true;
            }
            return add;
        }

        /// <summary>
        /// Remove the node from the succesors
        /// </summary>
        /// <param name="n">The n.</param>
        /// <returns></returns>
        public virtual bool removeNode(BaseNode n)
        {
            bool rem = false;
            if (containsNode(successorNodes, n))
            {
                successorNodes = ConversionUtils.remove(successorNodes, n);
                rem = true;
            }
            return rem;
        }

        /// <summary>
        /// Adds the successor node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="engine">The engine.</param>
        /// <param name="mem">The mem.</param>
        public abstract void addSuccessorNode(BaseNode node, Rete engine, IWorkingMemory mem);


        /// <summary>
        /// Method is used to decompose the network and make sure
        /// the nodes are detached from each other
        /// </summary>
        public abstract void removeAllSuccessors();

        /// <summary>
        /// Subclasses need to implement Clear and make sure all
        /// memories are cleared properly.
        /// </summary>
        /// <param name="mem">The mem.</param>
        public virtual void clear(IWorkingMemory mem)
        {
            IGenericMap<Object, Object> leftmem = (IGenericMap<Object, Object>) mem.getBetaLeftMemory(this);
            leftmem.Clear();
            HashedAlphaMemoryImpl rightmem = (HashedAlphaMemoryImpl) mem.getBetaRightMemory(this);
            rightmem.clear();
        }


        /// <summary>
        /// every time the node is shared, the method
        /// needs to be called so we keep an accurate count.
        /// </summary>
        public virtual void incrementUseCount()
        {
            useCount++;
        }

        /// <summary>
        /// every time a rule is removed from the network
        /// we need to decrement the count. Once the count
        /// reaches zero, we can Remove the node by calling
        /// it's finalize.
        /// </summary>
        public virtual void decrementUseCount()
        {
            useCount--;
        }

        /// <summary>
        /// toString should return a string format of the node and
        /// the pattern it matches.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public abstract override String ToString();

        /// <summary>
        /// hashString should return a string which can be used as
        /// a key for HashMap or HashTable
        /// </summary>
        /// <returns></returns>
        public virtual String hashString()
        {
            return "";
        }
    }
}