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

namespace Creshendo.Util
{
    /// <author>  Peter Lin
    /// *
    /// 
    /// </author>
    public abstract class AbstractMap : IMap
    {
        private const int MAX_CAPACITY = 1 << 30;
        private readonly int _capacity;
private EntryIterator _entryIterator;

        protected internal IObjectComparator comparator;
        protected internal float loadFactor;
        protected internal int currentSize;
        protected internal IEntry[] entryArray;
        protected internal int threshold;

        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractMap"/> class.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        /// <param name="factor">The factor.</param>
        public AbstractMap(int capacity, float factor)
        {
            loadFactor = factor;
            threshold = (int) (capacity*loadFactor);
            entryArray = new IEntry[capacity];
            comparator = EqualityEquals.Instance;
            _entryIterator = new EntryIterator(this);
            _capacity = capacity;
        }

        #region Map Members

        /// <summary>
        /// Gets a value indicating whether this <see cref="AbstractMap"/> is empty.
        /// </summary>
        /// <value><c>true</c> if empty; otherwise, <c>false</c>.</value>
        public virtual bool Empty
        {
            get { return currentSize == 0; }
        }

        public abstract object this[object key] { get; set;}

        public abstract System.Collections.IEnumerator GetEnumerator();

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public virtual int Count
        {
            get { return currentSize; }
        }


        /// <summary>
        /// Determines whether the specified key contains key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        /// 	<c>true</c> if the specified key contains key; otherwise, <c>false</c>.
        /// </returns>
        public abstract bool ContainsKey(Object key);

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public abstract Object Get(Object key);

        /// <summary>
        /// Puts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value_Renamed">The value_ renamed.</param>
        /// <returns></returns>
        //public abstract Object PutWithReturn(Object key, Object value_Renamed);

        public abstract void Put(Object key, Object value_Renamed);

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public abstract Object RemoveWithReturn(Object key);


        public abstract void Remove(Object key);

        /// <summary>
        /// Clear aggressively clears the entryArray and nulls the
        /// references.
        /// </summary>
        public virtual void Clear()
        {
            if (entryArray != null)
            {
                for (int idx = 0; idx < entryArray.Length; idx++)
                {
                    if (entryArray[idx] != null)
                    {
                        // we Clear the entryArray
                        IEntry e = entryArray[idx];
                        e.clear();
                    }
                }
            }
            entryArray = new IEntry[_capacity];
            _entryIterator.Reset();
        }

        /// <summary>
        /// Keys the iterator.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator keyIterator()
        {
            if (_entryIterator == null)
            {
                _entryIterator = new EntryIterator(this);
            }
            _entryIterator.Reset();
            return _entryIterator;
        }

        /// <summary>
        /// Entries the set.
        /// </summary>
        /// <returns></returns>
        public virtual IList entrySet()
        {
            ArrayList lst = new ArrayList(Count);
            if (entryArray == null)
                return lst;
            foreach (IEntry entry in entryArray)
            {
                if (entry != null)
                {
                    lst.Add(entry);
                }
            }

            return lst;
        }

        /// <summary>
        /// Keys the set.
        /// </summary>
        /// <returns></returns>
        public virtual IList Keys
        {
            get
            {
                ArrayList lst = new ArrayList(Count);
                if (entryArray == null)
                    return lst;
                foreach (IEntry entry in entryArray)
                {
                    if (entry != null)
                    {
                        lst.Add(entry.Key);
                    }
                }

                return lst;
            }
        }

        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <value>The values.</value>
        public IList Values
        {
            get
            {
                ArrayList lst = new ArrayList(Count);
                if (entryArray == null)
                    return lst;
                foreach (IEntry entry in entryArray)
                {
                    if (entry != null)
                    {
                        lst.Add(entry.Value);
                    }
                }

                return lst;
            }
        }

        /// <summary>
        /// Puts all.
        /// </summary>
        /// <param name="methods">The methods.</param>
        public void putAll(IMap methods)
        {
            throw new NotImplementedException();
        }

        #endregion

        protected internal virtual int indexOf(int hashCode, int dataSize)
        {
            return hashCode & (dataSize - 1);
        }

        protected internal virtual void resize(int newCapacity)
        {
            IEntry[] oldTable = entryArray;
            int oldCapacity = oldTable.Length;
            if (oldCapacity == MAX_CAPACITY)
            {
                threshold = Int32.MaxValue;
                return;
            }

            IEntry[] newTable = new IEntry[newCapacity];

            for (int i = 0; i < entryArray.Length; i++)
            {
                IEntry entry = entryArray[i];
                if (entry == null)
                {
                    continue;
                }
                entryArray[i] = null;
                IEntry next = null;
                while (entry != null)
                {
                    next = entry.Next;

                    int index = indexOf(entry.GetHashCode(), newTable.Length);
                    entry.Next = newTable[index];
                    newTable[index] = entry;

                    entry = next;
                }
            }

            entryArray = newTable;
            threshold = (int) (newCapacity*loadFactor);
        }

        #region Nested type: EntryIterator

        public class EntryIterator : IEnumerator
        {
            private readonly AbstractMap _hashMap;
            private IEntry _thisEntry;
            private int _length;
            private IEntry _nextEntry;
            private int _currentRow;
            private IEntry[] table;

            public EntryIterator(AbstractMap map)
            {
                _hashMap = map;
            }

            #region IEnumerator Members

            public System.Collections.IEnumerator GetEnumerator()
            {
                return this;
            }

            public virtual object Current
            {
                get
                {
                    if (_thisEntry == null)
                    {
                        // keep skipping rows until we come to the end, or find one that is populated
                        while (_thisEntry == null)
                        {
                            _currentRow++;
                            if (_currentRow == _length)
                            {
                                return null;
                            }
                            _thisEntry = table[_currentRow];
                        }
                    }
                    else
                    {
                        _thisEntry = _thisEntry.Next;
                        if (_thisEntry == null)
                        {
                            _thisEntry = (IEntry) Current;
                        }
                    }

                    return _thisEntry;
                }
            }

            public bool MoveNext()
            {
                bool bln = true;
                this._currentRow++;
                if (this._currentRow > this.table.Length - 1)
                    bln = false;
                return bln;

            }

            public virtual void Reset()
            {
                if (_hashMap.entryArray != null)
                {
                    table = _hashMap.entryArray;
                    _length = table.Length;
                }
                else
                {
                    _length = 0;
                }
                _currentRow = - 1;
                _thisEntry = null;
                _nextEntry = null;
            }

            #endregion

            public virtual void remove()
            {
                _hashMap.Remove(_thisEntry);
            }
        }

        #endregion

        #region Nested type: EqualityEquals

        public class EqualityEquals : IObjectComparator
        {
            public static IObjectComparator INSTANCE;

            static EqualityEquals()
            {
                INSTANCE = new EqualityEquals();
            }

            public static IObjectComparator Instance
            {
                get { return INSTANCE; }
            }

            #region ObjectComparator Members

            public virtual int hashCodeOf(Object key)
            {
                return rehash(key.GetHashCode());
            }

            public virtual int rehash(int h)
            {
                h += ~ (h << 9);
                h ^= (SupportClass.URShift(h, 14));
                h += (h << 4);
                h ^= (SupportClass.URShift(h, 10));
                return h;
            }

            public virtual bool equal(Object object1, Object object2)
            {
                return object1.Equals(object2);
            }

            #endregion
        }

        #endregion

        #region Nested type: InstanceEquals

        public class InstanceEquals : IObjectComparator
        {
            public static IObjectComparator INSTANCE;

            static InstanceEquals()
            {
                INSTANCE = new InstanceEquals();
            }

            public static IObjectComparator Instance
            {
                get { return INSTANCE; }
            }

            #region ObjectComparator Members

            public virtual int hashCodeOf(Object key)
            {
                return rehash(key.GetHashCode());
            }

            public virtual int rehash(int h)
            {
                h += ~ (h << 9);
                h ^= (SupportClass.URShift(h, 14));
                h += (h << 4);
                h ^= (SupportClass.URShift(h, 10));
                return h;
            }

            public virtual bool equal(Object object1, Object object2)
            {
                return object1 == object2;
            }

            #endregion
        }

        #endregion

        #region Nested type: ObjectComparator

        /// <summary> Internal interface for comparing objects
        /// </summary>
        /// <author>  pete
        /// *
        /// 
        /// </author>
        public interface IObjectComparator
        {
            int hashCodeOf(Object object_Renamed);
            int rehash(int hashCode);
            bool equal(Object object1, Object object2);
        }

        #endregion
    }
}