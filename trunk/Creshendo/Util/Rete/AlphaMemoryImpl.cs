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
using System.Collections.Generic;
using Creshendo.Util.Collections;

namespace Creshendo.Util.Rete
{
    /// <author>  Peter Lin
    /// *
    /// Basic implementation of Alpha memory. It uses HashMap for storing
    /// the indexes.
    /// 
    /// </author>
    public class AlphaMemoryImpl : IAlphaMemory
    {
        private readonly IGenericMap<IFact, IFact> memory = null;

        /// <summary> 
        /// </summary>
        public AlphaMemoryImpl(String name) 
        {
            memory = CollectionFactory.newAlphaMemoryMap(name);
        }

        #region AlphaMemory Members

        /// <summary> addPartialMatch stores the fact with the factId as the
        /// key.
        /// </summary>
        public virtual void addPartialMatch(IFact fact)
        {
            memory.Put(fact, fact);
        }

        /// <summary> Clear the memory.
        /// </summary>
        public virtual void clear()
        {
            memory.Clear();
        }

        /// <summary> Remove a partial match from the memory
        /// </summary>
        public virtual IFact removePartialMatch(IFact fact)
        {
            return memory.RemoveWithReturn(fact);
        }

        /// <summary> Return the Count of the memory
        /// </summary>
        public virtual int size()
        {
            return memory.Count;
        }

        /// <summary> Return an GetEnumerator of the values
        /// </summary>
        public virtual IEnumerator<IFact> GetEnumerator()
        {
            return memory.Keys.GetEnumerator();
        }

        #endregion
    }
}