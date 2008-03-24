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
    /// HashedAlphaMemory2 is different in that it has 2 levels of
    /// indexing. The first handles equal to comparisons. The second
    /// level handles not equal to.
    /// 
    /// </author>
    public class HashedNeqAlphaMemory : HashedAlphaMemoryImpl
    {
        /// <summary> 
        /// </summary>
        public HashedNeqAlphaMemory(String name) : base(name)
        {
        }

        /// <summary> addPartialMatch stores the fact with the factId as the
        /// key.
        /// </summary>
        public virtual int addPartialMatch(NotEqHashIndex index, IFact fact)
        {
            IGenericMap<Object, Object> matches = (IGenericMap<Object, Object>) memory.Get(index);
            int count = 0;
            if (matches == null)
            {
                count = addNewPartialMatch(index, fact);
            }
            else
            {
                IGenericMap<object, object> submatch = (IGenericMap<object, object>)matches.Get(index.SubIndex);
                if (submatch == null)
                {
                    submatch = CollectionFactory.newHashMap();
                    submatch.Put(fact, fact);
                    matches.Put(index.SubIndex, submatch);
                    count = matches.Count;
                }
                else
                {
                    submatch.Put(fact, fact);
                    count = submatch.Count;
                }
            }
            counter++;
            return count;
        }

        public virtual int addNewPartialMatch(NotEqHashIndex index, IFact fact)
        {
            IGenericMap<object, object> matches = CollectionFactory.newHashMap();
            IGenericMap<object, object> submatch = CollectionFactory.newHashMap();
            submatch.Put(fact, fact);
            matches.Put(index.SubIndex, submatch);
            memory.Put(index, matches);
            return 1;
        }

        /// <summary> Clear the memory.
        /// </summary>
        public override void clear()
        {
            IEnumerator itr = memory.Keys.GetEnumerator();
            while (itr.MoveNext())
            {
                object key = itr.Current;
                IGenericMap<Object, Object> matches = (IGenericMap<Object, Object>) memory.Get(key);
                IEnumerator itr2 = matches.Keys.GetEnumerator();
                while (itr2.MoveNext())
                {
                    Object subkey = itr2.Current;
                    IGenericMap<Object, Object> submatch = (IGenericMap<Object, Object>) matches.Get(subkey);
                    submatch.Clear();
                }
                matches.Clear();
            }
            memory.Clear();
        }

        public virtual bool isPartialMatch(NotEqHashIndex index, IFact fact)
        {
            IGenericMap<Object, Object> match = (IGenericMap<Object, Object>) memory.Get(index);
            if (match != null)
            {
                IGenericMap<Object, Object> submatch = (IGenericMap<Object, Object>) match.Get(index.SubIndex);
                if (submatch != null)
                {
                    return submatch.ContainsKey(fact);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary> Remove a partial match from the memory
        /// </summary>
        public virtual int removePartialMatch(NotEqHashIndex index, IFact fact)
        {
            IGenericMap<Object, Object> match = (IGenericMap<Object, Object>) memory.Get(index);
            if (match != null)
            {
                IGenericMap<Object, Object> submatch = (IGenericMap<Object, Object>) match.Get(index.SubIndex);
                submatch.Remove(fact);
                if (submatch.Count == 0)
                {
                    match.Remove(index.SubIndex);
                }
                counter--;
                return submatch.Count;
            }
            return - 1;
        }

        /// <summary> Return the number of memories of all hash buckets
        /// </summary>
        public override int size()
        {
            IEnumerator itr = memory.Keys.GetEnumerator();
            int count = 0;
            while (itr.MoveNext())
            {
                IGenericMap<Object, Object> matches = (IGenericMap<Object, Object>) memory.Get(itr.Current);
                IEnumerator itr2 = matches.Keys.GetEnumerator();
                while (itr2.MoveNext())
                {
                    EqHashIndex ehi = (EqHashIndex) itr2.Current;
                    IGenericMap<Object, Object> submatch = (IGenericMap<Object, Object>) matches.Get(ehi);
                    count += submatch.Count;
                }
            }
            return count;
        }

        public override int bucketCount()
        {
            return memory.Count;
        }

        /// <summary> Return an GetEnumerator of the values
        /// </summary>
        public virtual Object[] iterator(NotEqHashIndex index)
        {
            IGenericMap<Object, Object> matches = (IGenericMap<Object, Object>) memory.Get(index);
            Object[] list = new Object[counter];
            Object[] trim = null;
            int idz = 0;
            if (matches != null)
            {
                IEnumerator itr = matches.Keys.GetEnumerator();
                while (itr.MoveNext())
                {
                    Object key = itr.Current;
                    // if the key doesn't match the subindex, we
                    // Add it to the list. If it matches, we exclude
                    // it.
                    if (!index.SubIndex.Equals(key))
                    {
                        IGenericMap<Object, Object> submatch = (IGenericMap<Object, Object>) matches.Get(key);
                        IEnumerator itr2 = submatch.Keys.GetEnumerator();
                        while (itr2.MoveNext())
                        {
                            list[idz] = itr2.Current;
                            idz++;
                        }
                    }
                    trim = new Object[idz];
                    Array.Copy(list, 0, trim, 0, idz);
                }
                list = null;
                return trim;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// if there are zero matches for the NotEqHashIndex2, the method
        /// return true. If there are matches, the method returns false.
        /// False means there's 1 or more matches
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public virtual bool zeroMatch(NotEqHashIndex index)
        {
            IGenericMap<Object, Object> matches = (IGenericMap<Object, Object>) memory.Get(index);
            int idz = 0;
            if (matches != null)
            {
                IEnumerator itr = matches.Keys.GetEnumerator();
                while (itr.MoveNext())
                {
                    Object key = itr.Current;
                    // if the key doesn't match the subindex, Add it to the
                    // counter.
                    if (!index.SubIndex.Equals(key))
                    {
                        IGenericMap<Object, Object> submatch = (IGenericMap<Object, Object>) matches.Get(key);
                        idz += submatch.Count;
                    }
                    if (idz > 0)
                    {
                        break;
                    }
                }
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary> return an List with all the facts
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public override Object[] iterateAll()
        {
            Object[] facts = new Object[counter];
            IEnumerator itr = memory.Keys.GetEnumerator();
            int idx = 0;
            while (itr.MoveNext())
            {
                IGenericMap<Object, Object> matches = (IGenericMap<Object, Object>) memory.Get(itr.Current);
                IEnumerator itr2 = matches.Keys.GetEnumerator();
                while (itr2.MoveNext())
                {
                    IGenericMap<Object, Object> submatch = (IGenericMap<Object, Object>) matches.Get(itr2.Current);
                    IEnumerator itr3 = submatch.Values.GetEnumerator();
                    while (itr3.MoveNext())
                    {
                        facts[idx] = itr3.Current;
                        idx++;
                    }
                }
            }
            Object[] trim = new Object[idx];
            Array.Copy(facts, 0, trim, 0, idx);
            facts = null;
            return trim;
        }
    }
}