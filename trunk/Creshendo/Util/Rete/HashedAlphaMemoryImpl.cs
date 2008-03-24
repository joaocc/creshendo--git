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
    /// Basic implementation of Alpha memory. It uses HashMap for storing
    /// the indexes.
    /// 
    /// </author>
    [Serializable]
    public class HashedAlphaMemoryImpl
    {
        protected internal int counter = 0;
        protected internal IGenericMap<object, object> memory = null;

        /// <summary> 
        /// </summary>
        public HashedAlphaMemoryImpl(String name) 
        {
            memory = CollectionFactory.newHashedAlphaMemoryMap(name);
        }

        /// <summary> addPartialMatch stores the fact with the factId as the
        /// key.
        /// </summary>
        public virtual int addPartialMatch(IHashIndex index, IFact fact)
        {
            IGenericMap<Object, Object> matches = (IGenericMap<Object, Object>) memory.Get(index);
            int count = 0;
            if (matches == null)
            {
                count = addNewPartialMatch(index, fact);
            }
            else
            {
                matches.Put(fact, fact);
                count = matches.Count;
            }
            counter++;
            return count;
        }

        public virtual int addNewPartialMatch(IHashIndex index, IFact fact)
        {
            IGenericMap<object, object> matches = CollectionFactory.newMap();
            matches.Put(fact, fact);
            memory.Put(index, matches);
            return 1;
        }

        /// <summary> Clear the memory.
        /// </summary>
        public virtual void clear()
        {
            IEnumerator itr = memory.Values.GetEnumerator();
            while (itr.MoveNext())
            {
                ((IGenericMap<Object, Object>) itr.Current).Clear();
            }
            memory.Clear();
        }

        public virtual bool isPartialMatch(IHashIndex index, IFact fact)
        {
            IGenericMap<Object, Object> list = (IGenericMap<Object, Object>) memory.Get(index);
            if (list != null)
            {
                return list.ContainsKey(fact);
            }
            else
            {
                return false;
            }
        }

        /// <summary> Remove a partial match from the memory
        /// </summary>
        public virtual int removePartialMatch(IHashIndex index, IFact fact)
        {
            IGenericMap<Object, Object> list = (IGenericMap<Object, Object>) memory.Get(index);
            if (list != null)
            {
                list.Remove(fact);
                if (list.Count == 0)
                {
                    memory.Remove(index);
                }
                counter--;
                return list.Count;
            }
            else
            {
                return 0;
            }
        }

        /// <summary> Return the number of memories of all hash buckets
        /// </summary>
        public virtual int size()
        {
            IEnumerator itr = memory.Keys.GetEnumerator();
            int count = 0;
            while (itr.MoveNext())
            {
                IGenericMap<Object, Object> matches = (IGenericMap<Object, Object>) memory.Get(itr.Current);
                count += matches.Count;
            }
            return count;
        }

        public virtual int bucketCount()
        {
            return counter;
        }

        /// <summary> Return an GetEnumerator of the values
        /// </summary>
        public virtual IEnumerator iterator(IHashIndex index)
        {
            IGenericMap<Object, Object> list = (IGenericMap<Object, Object>) memory.Get(index);
            if (list != null)
            {
                return list.Values.GetEnumerator();
            }
            else
            {
                return null;
            }
        }

        public virtual int count(IHashIndex index)
        {
            IGenericMap<Object, Object> list = (IGenericMap<Object, Object>) memory.Get(index);
            if (list != null)
            {
                return list.Count;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// return an List with all the facts
        /// </summary>
        /// <returns></returns>
        public virtual Object[] iterateAll()
        {
            Object[] all = new Object[counter];
            IEnumerator itr = memory.Keys.GetEnumerator();
            int idx = 0;
            while (itr.MoveNext())
            {
                IGenericMap<Object, Object> f = (IGenericMap<Object, Object>) memory.Get(itr.Current);
                IEnumerator itr2 = f.Values.GetEnumerator();
                while (itr2.MoveNext())
                {
                    all[idx] = itr2.Current;
                    idx++;
                }
            }
            return all;
        }
    }
}