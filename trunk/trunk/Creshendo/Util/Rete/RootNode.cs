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
    /// 
    /// RootNode does not extend BaseNode like all other RETE nodes. This is
    /// done for a couple of reasons.<br/>
    /// <ul>
    /// <li> RootNode doesn't need to have a memory </li>
    /// <li> RootNode only has ObjectTypeNode for successors</li>
    /// <li> RootNode doesn't need the toPPString and other string methods</li>
    /// </ul>
    /// In the future, the design may change. For now, I've decided to keep
    /// it as simple as necessary.
    /// 
    /// </author>
    [Serializable]
    public class RootNode
    {
        protected internal IGenericMap<object, object> inputNodes;

        /// <summary> 
        /// </summary>
        public RootNode() 
        {
            InitBlock();
        }

        /// <summary> Return the HashMap with all the ObjectTypeNodes
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public virtual IGenericMap<object, object> ObjectTypeNodes
        {
            get { return inputNodes; }
        }

        private void InitBlock()
        {
            inputNodes = CollectionFactory.newHashMap();
        }

        /// <summary>
        /// Add a new ObjectTypeNode. The implementation will check to see
        /// if the node already exists. It will only Add the node if it
        /// doesn't already exist in the network.
        /// </summary>
        /// <param name="node">The node.</param>
        public virtual void addObjectTypeNode(ObjectTypeNode node)
        {
            if (!inputNodes.ContainsKey(node.Deftemplate))
            {
                inputNodes.Put(node.Deftemplate, node);
            }
        }

        /// <summary> The current implementation just removes the ObjectTypeNode
        /// and doesn't prevent the removal. The method should be called
        /// with care, since removing the ObjectTypeNode can have serious
        /// negative effects. This would generally occur when an undeftemplate
        /// occurs.
        /// </summary>
        public virtual void removeObjectTypeNode(ObjectTypeNode node)
        {
            inputNodes.Remove(node.Deftemplate);
        }


         /// <summary>
        /// assertObject begins the pattern matching
        /// </summary>
        /// <param name="fact">The fact.</param>
        /// <param name="engine">The engine.</param>
        /// <param name="mem">The mem.</param>
        public virtual void assertObject(IFact fact, Rete engine, IWorkingMemory mem)
        {
            lock (this)
            {
                // we assume Rete has already checked to see if the object
                // has been added to the working memory, so we just assert.
                // we need to lookup the defclass and deftemplate to assert
                // the object to the network
                ObjectTypeNode otn = (ObjectTypeNode) inputNodes.Get(fact.Deftemplate);
                if (otn != null)
                {
                    otn.assertFact(fact, engine, mem);
                }
                if (fact.Deftemplate.Parent != null)
                {
                    assertObjectParent(fact, fact.Deftemplate.Parent, engine, mem);
                }
            }
        }

         /// <summary>
        /// Method will Get the deftemplate's parent and do a lookup
        /// </summary>
        /// <param name="fact">The fact.</param>
        /// <param name="template">The template.</param>
        /// <param name="engine">The engine.</param>
        /// <param name="mem">The mem.</param>
        public virtual void assertObjectParent(IFact fact, ITemplate template, Rete engine, IWorkingMemory mem)
        {
            lock (this)
            {
                ObjectTypeNode otn = (ObjectTypeNode) inputNodes.Get(template);
                if (otn != null)
                {
                    otn.assertFact(fact, engine, mem);
                }
                if (template.Parent != null)
                {
                    assertObjectParent(fact, template.Parent, engine, mem);
                }
            }
        }

        /// <summary>
        /// Retract an object from the Working memory
        /// </summary>
        /// <param name="fact">The fact.</param>
        /// <param name="engine">The engine.</param>
        /// <param name="mem">The mem.</param>
        public virtual void retractObject(IFact fact, Rete engine, IWorkingMemory mem)
        {
            lock (this)
            {
                ObjectTypeNode otn = (ObjectTypeNode) inputNodes.Get(fact.Deftemplate);
                if (otn != null)
                {
                    otn.retractFact(fact, engine, mem);
                }
                if (fact.Deftemplate.Parent != null)
                {
                    retractObjectParent(fact, fact.Deftemplate.Parent, engine, mem);
                }
            }
        }

        /// <summary>
        /// Method will Get the deftemplate's parent and do a lookup
        /// </summary>
        /// <param name="fact">The fact.</param>
        /// <param name="template">The template.</param>
        /// <param name="engine">The engine.</param>
        /// <param name="mem">The mem.</param>
        public virtual void retractObjectParent(IFact fact, ITemplate template, Rete engine, IWorkingMemory mem)
        {
            lock (this)
            {
                ObjectTypeNode otn = (ObjectTypeNode) inputNodes.Get(template);
                if (otn != null)
                {
                    otn.retractFact(fact, engine, mem);
                }
                if (template.Parent != null)
                {
                    retractObjectParent(fact, template.Parent, engine, mem);
                }
            }
        }

        public virtual void clear()
        {
            lock (this)
            {
                IEnumerator itr = inputNodes.Values.GetEnumerator();
                while (itr.MoveNext())
                {
                    ObjectTypeNode otn = (ObjectTypeNode) itr.Current;
                    otn.clearSuccessors();
                }
                inputNodes.Clear();
            }
        }
    }
}