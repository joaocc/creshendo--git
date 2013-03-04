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
    /// TemporalHashedAlphaMem is the hashed alpha memory for Temporal nodes.
    /// We use a linkedHashMap so that we can easily Remove the expired facts.
    /// This means at the top of the list are older facts and the bottom has
    /// the newer facts. We only need to Remove facts that are older than the
    /// current timestamp - time window. The equal would be this.
    /// 
    /// if ( (currentTime - time window) > fact.timestamp )
    /// 
    /// Rather than keep a timestamp of when the fact entered the join node,
    /// we assume the elapsed time between the time the fact entered the
    /// engine and when it activated the node is less than 1 second. Keeping
    /// a timestamp of when the fact activated the node is too costly and
    /// isn't practical. This means for each fact, there would be n timestamps,
    /// where n is the number of temporal nodes for the given object type.
    /// 
    /// If we look at the number of temporal node timestamps the engine
    /// would need to maintain would be this.
    /// 
    /// f * n = number of temporal timestamps
    /// 
    /// f = number of facts
    /// n = number of temporal nodes
    /// 
    /// If we have 100,000 facts and 100 temporal nodes, the engine would
    /// maintain 10,000,000 timestamps. clearly that isn't scalable and
    /// would have a significant impact.
    /// 
    /// </author>
    [Serializable]
    public class TemporalHashedAlphaMem
    {
        protected internal int counter = 0;
        protected internal IGenericMap<object, object> memory = null;

        /// <summary> 
        /// </summary>
        public TemporalHashedAlphaMem(String name) 
        {
            memory = CollectionFactory.newLinkedHashmap(name);
        }

        /// <summary> addPartialMatch stores the fact with the factId as the
        /// key.
        /// </summary>
        public virtual void addPartialMatch(IHashIndex index, IFact fact)
        {
            IGenericMap<Object, Object> matches = (IGenericMap<Object, Object>) memory[index];
            if (matches == null)
            {
                addNewPartialMatch(index, fact);
            }
            else
            {
                matches.Put(fact, fact);
            }
            counter++;
        }

        public virtual void addNewPartialMatch(IHashIndex index, IFact fact)
        {
            IGenericMap<object, object> matches = CollectionFactory.newMap();
            matches.Put(fact, fact);
            memory.Put(index, matches);
        }

        /// <summary> Clear the memory.
        /// </summary>
        public virtual void clear()
        {
            foreach(IGenericMap<Object, Object> val in memory.Values)
            {
                val.Clear();
            }
            memory.Clear();

            //IEnumerator itr = memory.Values.GetEnumerator();
            //while (itr.MoveNext())
            //{
            //    ((Map)itr.Current()).Clear();
            //}
            //memory.Clear();
        }

        public virtual bool isPartialMatch(IHashIndex index, IFact fact)
        {
            IGenericMap<Object, Object> list = (IGenericMap<Object, Object>) memory[index];
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
            IGenericMap<Object, Object> list = (IGenericMap<Object, Object>) memory[index];
            list.Remove(fact);
            if (list.Count == 0)
            {
                memory.Remove(index);
            }
            counter--;
            return list.Count;
        }

        /// <summary> Return the number of memories of all hash buckets
        /// </summary>
        public virtual int size()
        {
            int count = 0;
            
            foreach(Object key in memory.Keys)
            {
                IGenericMap<Object, Object> matches = (IGenericMap<Object, Object>)memory[key];
                count += matches.Count;
            }

            return count;

            //IEnumerator itr = memory.keySet().GetEnumerator();
            //int count = 0;
            //while (itr.MoveNext())
            //{
            //    Map matches = (Map) memory[itr.Current()];
            //    count += matches.Count;
            //}
            //return count;
        }

        public virtual int bucketCount()
        {
            return counter;
        }

        /// <summary> Return an GetEnumerator of the values
        /// </summary>
        public virtual IEnumerator iterator(IHashIndex index)
        {
            IGenericMap<Object, Object> list = (IGenericMap<Object, Object>) memory[index];
            if (list != null)
            {
                // we have to create a new Creshendo.rete.util.List<Object> with the values
                // so the GetEnumerator will work correctly. if we didn't
                // do this, we might Get a NullPointerException or a
                // possibly a concurrent modification exception, since
                // the node could be still iterating over the facts
                // as stale facts are removed.
                System.Collections.Generic.List<Object> rlist = new System.Collections.Generic.List<Object>(list.Values);
                return rlist.GetEnumerator();
            }
            else
            {
                return null;
            }
        }

        public virtual int count(IHashIndex index)
        {
            IGenericMap<Object, Object> list = (IGenericMap<Object, Object>) memory[index];
            if (list != null)
            {
                return list.Count;
            }
            else
            {
                return 0;
            }
        }

        /// <summary> return an List with all the facts
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public virtual Object[] iterateAll()
        {
            Object[] all = new Object[counter];
            int idx = 0;

            foreach(object key in memory.Keys)
            {
                IGenericMap<Object, Object> f = (IGenericMap<Object, Object>)memory[key];
                foreach(Object val in f.Values)
                {
                    all[idx] = val;
                    idx++;
                }
            }
            return all;

            //Object[] all = new Object[counter];
            //IEnumerator itr = memory.keySet().GetEnumerator();
            //int idx = 0;
            //while (itr.MoveNext())
            //{
            //    Map f = (Map) memory[itr.Current()];
            //    IEnumerator itr2 = f.Values.GetEnumerator();
            //    while (itr2.MoveNext())
            //    {
            //        all[idx] = itr2.Current();
            //        idx++;
            //    }
            //}
            //return all;
        }
    }
}