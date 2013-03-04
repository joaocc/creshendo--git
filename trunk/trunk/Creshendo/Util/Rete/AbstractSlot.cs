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

namespace Creshendo.Util.Rete
{
    ///   Peter Lin
    /// *
    /// AbstractSlot Contains common attributes of Slot, multislot and
    /// binding. Slot classes need to implement the clone method for
    /// cloning the slots. This is necessary because slots are used to
    /// parse CLIPS and for the RETE nodes.
    /// 
    /// 
    /// 
    /// 
    [Serializable]
    public abstract class AbstractSlot : ICloneable
    {
        /// <summary> depth is a place holder for ordered facts, which is a list
        /// of symbols. For the first version, ordered facts are not
        /// implemented. it is also used in the case a condition has
        /// multiple equal/not equal as in (attr2 "me" | "you" | ~"her" | ~"she")
        /// </summary>
        private int depth;

        /// <summary> in some cases, users may want a template to have a default value
        /// </summary>
        private bool hasDefault = false;

        /// <summary> the id of the slot
        /// </summary>
        private int id;

        /// <summary> The name of the slot
        /// </summary>
        private String name;

        /// <summary> node count is used to keep track of how many nodes use the given
        /// slot. This is done for statistical purposes, which serve 3 main
        /// functions.
        /// 1. provide a way to calculate the relative importance of a slot
        /// with regard to the entire RETE network
        /// 2. provide a way to optimize runtime execution
        /// 3. provide valuable information for engine management
        /// </summary>
        private int nodeCount = 1;

        /// <summary> The type of the value
        /// </summary>
        private int type = - 1;

        public AbstractSlot(string nme)
        {
            name = nme;
        }

        public AbstractSlot()
        {
        }

        /// <summary> Get the name of the slot
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        /// <summary> Set the name of the slot
        /// </summary>
        /// <param name="">text
        /// 
        /// </param>
        public virtual String Name
        {
            get { return name; }

            set { name = value; }
        }

        public virtual int ValueType
        {
            get { return type; }

            set { type = value; }
        }

        /// <summary> the id is the column id, this is the sequence java
        /// introspection returns the fields for the object
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        /// <summary> Set the column id for the slot. the id is the position
        /// of the slot in the deftemplate
        /// </summary>
        /// <param name="">id
        /// 
        /// </param>
        public virtual int Id
        {
            get { return id; }

            set { id = value; }
        }

        /// <summary> return the number of nodes the given slot participates
        /// in. It may not be a complete count. In some cases, it
        /// may only count the direct successors of ObjectTypeNode
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public virtual int NodeCount
        {
            get { return nodeCount; }
        }

        #region ICloneable Members

        /// <summary> A convienance method to clone slots. subclasses must implement
        /// this method.
        /// </summary>
        public abstract Object Clone();

        #endregion

        /// <summary> Increment the node count
        /// </summary>
        public virtual void incrementNodeCount()
        {
            nodeCount++;
        }

        /// <summary> decrement the node count
        /// *
        /// </summary>
        public virtual void decrementNodeCount()
        {
            --nodeCount;
        }

    }
}