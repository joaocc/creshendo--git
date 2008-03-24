using System;
using System.Collections;
using System.Collections.Generic;

namespace Creshendo.Util.Collections
{
    public class GenericHashMap<K, V> : IGenericMap<K, V>
    {
        // must be a power of 2
        private static readonly int DEFAULT_INITIAL_CAPACITY = 101;

        //the maximum capacity, must be a power of 2 <= 1<<30

        // load factor, when none is specified
        private static readonly float DEFAULT_LOAD_FACTOR = 0.75f;
        private static readonly int MAXIMUM_CAPACITY = 1 << 30;

        // length must be a power of 2 always, will be resized as necessary

        //load factor of the table
        private readonly double loadFactor;

        /**
		 * The number of times this HashMap has been structurally modified
		 * Structural modifications are those that change the number of mappings in
		 * the HashMap or otherwise modify its internal structure (e.g.,
		 * rehash).  This field is used to make iterators on Collection-views of
		 * the HashMap fail-fast.  (See ConcurrentModificationException).
		 */
        private int modCount;
        protected int size;
        private IHashMapEntry<K, V>[] table;
        private int threshold;

        #region constructors

        /**
		 * Constructs an empty <tt>HashMap</tt> with the specified initial
		 * capacity and load factor.
		 *
		 * @param  initialCapacity The initial capacity.
		 * @param  loadFactor      The load factor.
		 * @throws IllegalArgumentException if the initial capacity is negative
		 *         or the load factor is nonpositive.
		 */

        /**
		* Value representing null keys inside tables.
		*/
        private static readonly K NULL_KEY = default(K);
        private bool isFixedSize;
        private bool isReadOnly;

        public GenericHashMap(int initialCapacity, float loadFactor)
        {
            if (initialCapacity < 0)
                throw new ArgumentException("Illegal initial capacity: " +
                                            initialCapacity);
            if (initialCapacity > MAXIMUM_CAPACITY)
                initialCapacity = MAXIMUM_CAPACITY;
            if (loadFactor <= 0 || Double.IsNaN(loadFactor))
                throw new ArgumentException("Illegal load factor: " +
                                            loadFactor);

            // Find a power of 2 >= initialCapacity
            int capacity = 1;
            while (capacity < initialCapacity)
                capacity <<= 1;

            this.loadFactor = loadFactor;
            threshold = (int) (capacity*loadFactor);
            table = new HashMapEntry<K, V>[capacity];
            init();
        }

        /**
		 * Returns internal representation for key. Use NULL_KEY if key is null.
		 */

        /**
		* Constructs an empty <tt>HashMap</tt> with the specified initial
		* capacity and the default load factor (0.75).
		*
		* @param  initialCapacity the initial capacity.
		* @throws IllegalArgumentException if the initial capacity is negative.
		*/

        public GenericHashMap(int initialCapacity) : this(initialCapacity, DEFAULT_LOAD_FACTOR)
        {
        }

        /**
		 * Constructs an empty <tt>HashMap</tt> with the default initial capacity
		 * (16) and the default load factor (0.75).
		 */

        public GenericHashMap()
        {
            loadFactor = DEFAULT_LOAD_FACTOR;
            threshold = (int) (DEFAULT_INITIAL_CAPACITY);
            table = new HashMapEntry<K, V>[DEFAULT_INITIAL_CAPACITY];
            init();
        }

        public Object maskNull(Object key)
        {
            return (key == null ? NULL_KEY : key);
        }

        /**
		 * Returns key represented by specified internal representation.
		 */

        public Object unmaskNull(Object key)
        {
            return (key.Equals(NULL_KEY) ? default(K) : key);
        }

        /**
		* Initialization hook for subclasses. This method is called
		* in all constructors and pseudo-constructors (clone, readObject)
		* after HashMap has been initialized but before any entries have
		* been inserted.  (In the absence of this method, readObject would
		* require explicit knowledge of subclasses.)
		*/

        private void init()
        {
        }

        #endregion

        #region properties

        public ICollection<V> Values
        {
            get { return new HashMapValues(this); }
        }

        public bool IsReadOnly
        {
            get { return isReadOnly; }
        }

        public bool IsFixedSize
        {
            get { return isFixedSize; }
        }

        public ICollection<K> Keys
        {
            get { return new HashMapKeys(this); }
        }

        public EntrySet TheEntrySet
        {
            get { return new EntrySet(this); }
        }

        /// <summary>
        /// returns the number of key-value mappings
        /// </summary>
        public int Count
        {
            get { return size; }
        }

        /// <summary>
        /// Get the current capacity.  Readonly
        /// </summary>
        public int Capacity
        {
            get { return table.Length; }
        }

        public bool IsSynchronized
        {
            get { return false; }
        }

        public object SyncRoot
        {
            get { return null; }
        }

        /// <summary>
        /// returns true if there are no key-value mappings
        /// </summary>
        public bool IsEmpty
        {
            get { return size == 0; }
        }

        public V this[K key]
        {
            get
            {
                //object k = maskNull(key);
                if (key == null)
                    return default(V);

                int thehash = hash(key);
                int i = indexFor(thehash, table.Length);
                IHashMapEntry<K, V> e = table[i];
                while (true)
                {
                    if (e == null)
                        return default(V);
                    if (e.Hash == thehash && eq(key, e.Key))
                        return e.Value;
                    e = e.Next;
                }
            }
            set { Put(key, value); }
        }

        public V Get(K key)
        {
            return this[key];
        }

        #endregion

        public bool Empty
        {
            get { return size == 0; }
        }

        #region IGenericMap<K,V> Members

        public IEnumerator<IHashMapEntry<K, V>> GetEnumerator()
        {
            return new EntryEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new EntryEnumerator(this);
        }

        #endregion

        public void CopyTo(Array theArr, int length)
        {
            /*unimplemented for now*/
        }

        /*
        /// <summary>
        /// check for equality of non-null references x and possibly-null y
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private bool eq(Object x, Object y)
        {
            return x == y || x.Equals(y);
        }
        */

        /// <summary>
        /// check for equality of non-null references x and possibly-null y
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private bool eq(K x, K y)
        {
            if (x != null)
            {
                return ((y != null) && x.Equals(y));
            }
            if (y != null)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// returns index for hash code h
        /// </summary>
        /// <param name="h"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private static int indexFor(int h, int length)
        {
            return h & (length - 1);
        }

        /// <summary>
        /// true if this map contains a mapping for the specified key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(K key)
        {
            //object k = maskNull(key);

            if (key == null)
                return false;

            int thehash = hash(key);
            int i = indexFor(thehash, table.Length);
            IHashMapEntry<K, V> e = table[i];
            while (e != null)
            {
                if (e.Hash == thehash && eq(key, e.Key))
                    return true;
                e = e.Next;
            }
            return false;
        }

        public bool Contains(K key)
        {
            //object k = maskNull(key);

            if (key == null)
                return false;

            int thehash = hash(key);
            int i = indexFor(thehash, table.Length);
            IHashMapEntry<K, V> e = table[i];
            while (e != null)
            {
                if (e.Hash == thehash && eq(key, e.Key))
                    return true;
                e = e.Next;
            }
            return false;
        }

        public void Add(K key, V entry)
        {
            if (key == null)
                throw new NullReferenceException("Key cannot be null");
            //K k = maskNull(key);
            int theHash = hash(key);
            int i = indexFor(theHash, table.Length);

            for (IHashMapEntry<K, V> e = table[i]; e != null; e = e.Next)
            {
                if (e.Hash == theHash && eq(key, e.Key))
                {
                    Object oldValue = e.Value;
                    e.Value = entry;
                    return;
                }
            }

            modCount++;
            AddEntry(theHash, key, entry, i);
        }

        /// <summary>
        /// add a key value pair to this map
        /// </summary>
        /// <param name="key"></param>
        /// <param name="entry"></param>
        public void Put(K key, V entry)
        {
            if (key == null)
                throw new NullReferenceException("Key cannot be null");
            int theHash = hash(key);
            int i = indexFor(theHash, table.Length);

            for (IHashMapEntry<K, V> e = table[i]; e != null; e = e.Next)
            {
                if (e.Hash == theHash && eq(key, e.Key))
                {
                    Object oldValue = e.Value;
                    e.Value = entry;
                    return;
                }
            }

            modCount++;
            AddEntry(theHash, key, entry, i);
        }

        /// <summary>
        /// removes an entry for the specified key
        /// returns the removed entry
        /// </summary>
        private IHashMapEntry<K, V> RemoveEntryForKey(K key)
        {
            if (key == null)
                return null;
            int thehash = hash(key);
            int i = indexFor(thehash, table.Length);
            IHashMapEntry<K, V> prev = table[i];
            IHashMapEntry<K, V> e = prev;

            while (e != null)
            {
                IHashMapEntry<K, V> next = e.Next;
                if (e.Hash == thehash && eq(key, e.Key))
                {
                    modCount++;
                    size--;
                    if (prev == e)
                        table[i] = next;
                    else
                        prev.Next = next;
                    return e;
                }
                prev = e;
                e = next;
            }

            return e;
        }

        /// <summary>
        /// remove the mapping for a specified entry
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public IHashMapEntry<K, V> RemoveMapping(object o)
        {
            if (!(o is HashMapEntry<K, V>))
                return null;

            HashMapEntry<K, V> entry = (HashMapEntry<K, V>) o;
            //object k = maskNull(entry.Key);
            int thehash = hash(entry.Key);
            int i = indexFor(thehash, table.Length);
            IHashMapEntry<K, V> prev = table[i];
            IHashMapEntry<K, V> e = prev;

            while (e != null)
            {
                IHashMapEntry<K, V> next = e.Next;
                if (e.Hash == thehash && e.Equals(entry))
                {
                    modCount++;
                    size--;
                    if (prev == e)
                        table[i] = next;
                    else
                        prev.Next = next;
                    return e;
                }
                prev = e;
                e = next;
            }

            return e;
        }


        //clear the table
        public void Clear()
        {
            modCount++;
            IHashMapEntry<K, V>[] tab = table;
            for (int i = 0; i < tab.Length; i++)
                tab[i] = null;
            size = 0;
        }

        //IDictionaryEnumerator IDictionary.GetEnumerator()
        //{
        //    throw new NotSupportedException(); 
        //}


        /// <summary>
        /// generate a hash code for the specified key
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        private static int hash(object x)
        {
            int h = x.GetHashCode();

            h += ~(h << 9);
            h ^= (h >> 14);
            h += (h << 4);
            h ^= h >> 10;
            return h;
        }

        /// <summary>
        /// used instead of Add by Constructors and Clones, does not resize the table
        /// calls CreateEntry instead of AddEntry
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private void PutForCreate(K key, V value)
        {
            //K k = maskNull(key);
            int thehash = hash(key);
            int i = indexFor(thehash, table.Length);

            /**
			 * Look for preexisting entry for key.  This will never happen for
			 * clone or deserialize.  It will only happen for construction if the
			 * input Map is a sorted map whose ordering is inconsistent w/ equals.
			 */
            for (IHashMapEntry<K, V> e = table[i]; e != null; e = e.Next)
            {
                if (e.Hash == thehash && eq(key, e.Key))
                {
                    e.Value = value;
                    return;
                }
            }

            CreateEntry(thehash, key, value, i);
        }

        private void PutAllForCreate(GenericHashMap<K, V> m)
        {
            for (IEnumerator<IHashMapEntry<K, V>> i = m.GetEnumerator(); i.MoveNext(); )
            {
                IHashMapEntry<K, V> e = i.Current;
                PutForCreate(e.Key, e.Value);
            }
        }

        /// <summary>
        /// add a new entry with the key, value and hashcode, to the specified bucket
        /// if needed this method will resize the table
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="bucketIndex"></param>
        private void AddEntry(int hash, K key, V value, int bucketIndex)
        {
            table[bucketIndex] = new HashMapEntry<K, V>(hash, key, value, table[bucketIndex]);
            if (size++ >= threshold)
                Resize(2*table.Length);
        }

        /// <summary>
        /// this is used when creating entries as part of a constructor or Clone call
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="bucketIndex"></param>
        private void CreateEntry(int hash, K key, V value, int bucketIndex)
        {
            table[bucketIndex] = new HashMapEntry<K, V>(hash, key, value, table[bucketIndex]);
            size++;
        }

        /// <summary>
        /// rehashes the the contents of this map into a new HashMap with a larger capacity
        /// called automatically when the number of keys exceedts the capacity and load factor
        /// 
        /// </summary>
        /// <param name="newCapacity">new capacity, must be a power of 2</param>
        private void Resize(int newCapacity)
        {
            IHashMapEntry<K, V>[] oldTable = table;
            int oldCapacity = oldTable.Length;

            // check if needed
            if (size < threshold || oldCapacity > newCapacity)
                return;

            HashMapEntry<K, V>[] newTable = new HashMapEntry<K, V>[newCapacity];
            Transfer(newTable);
            table = newTable;
            threshold = (int) (newCapacity*loadFactor);
        }

        /// <summary>
        /// transfer all entries from current table into the newTable
        /// </summary>
        /// <param name="newTable"></param>
        private void Transfer(IHashMapEntry<K, V>[] newTable)
        {
            IHashMapEntry<K, V>[] src = table;
            int newCapacity = newTable.Length;
            for (int j = 0; j < src.Length; j++)
            {
                IHashMapEntry<K, V> e = src[j];
                if (e != null)
                {
                    src[j] = null;
                    do
                    {
                        IHashMapEntry<K, V> next = e.Next;
                        int i = indexFor(e.Hash, newCapacity);
                        e.Next = newTable[i];
                        newTable[i] = e;
                        e = next;
                    } while (e != null);
                }
            }
        }

        /// <summary>
        /// copies all mappings from the specified map to this map
        /// will replace any mappings that had matching keys
        /// </summary>
        /// <param name="t"></param>
        public void putAll(IGenericMap<K, V> t)
        {
            // Expand enough to hold t's elements without resizing.
            int n = t.Count;
            if (n == 0)
                return;
            if (n >= threshold)
            {
                n = (int) (n/loadFactor + 1);
                if (n > MAXIMUM_CAPACITY)
                    n = MAXIMUM_CAPACITY;
                int capacity = table.Length;
                while (capacity < n)
                    capacity <<= 1;
                Resize(capacity);
            }

            foreach (IHashMapEntry<K, V> entry in t)
            {
                Put(entry.Key, entry.Value);
            }
        }


        /// <summary>
        /// removes the mapping for this key from this map
        /// </summary>
        /// <param name="key"></param>
        /// <returns>previous value for the key, or null if no mapping</returns>
        public void Remove(K key)
        {
            RemoveEntryForKey(key);
        }

        public V RemoveWithReturn(K key)
        {
            IHashMapEntry<K, V> e = RemoveEntryForKey(key);
            return (e == null ? default(V) : e.Value);
        }

        /// <summary>
        /// returns true if this Map has a mapping for the object
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool ContainsValue(V value)
        {
            if (value.Equals(default(V)))
                return ContainsNullValue();

            IHashMapEntry<K, V>[] tab = table;
            for (int i = 0; i < tab.Length; i++)
                for (IHashMapEntry<K, V> e = tab[i]; e != null; e = e.Next)
                    if (value.Equals(e.Value))
                        return true;
            return false;
        }

        /// <summary>
        /// special case code for containsValue with null argument
        /// </summary>
        /// <returns></returns>
        private bool ContainsNullValue()
        {
            IHashMapEntry<K, V>[] tab = table;
            for (int i = 0; i < tab.Length; i++)
                for (IHashMapEntry<K, V> e = tab[i]; e != null; e = e.Next)
                    if (e.Value.Equals(default(V)))
                        return true;
            return false;
        }

        /// <summary>
        /// return a shallow copy of the HashMap instance
        /// the keys and values are not cloned
        /// </summary>
        /// <returns></returns>
        public GenericHashMap<K, V> Clone()
        {
            GenericHashMap<K, V> result = new GenericHashMap<K, V>();

            result.table = new HashMapEntry<K, V>[table.Length];
            result.modCount = 0;
            result.size = 0;
            result.init();
            result.PutAllForCreate(this);

            return result;
        }

        /// <summary>
        /// reutrns the entry associated with the key
        /// </summary>
        private IHashMapEntry<K, V> GetEntry(K key)
        {
            //K k = maskNull(key);
            int theHash = hash(key);
            int i = indexFor(theHash, table.Length);
            IHashMapEntry<K, V> e = table[i];
            while (e != null && !(e.Hash == theHash && eq(key, e.Key)))
                e = e.Next;
            return e;
        }

        #region internal classes

        // Subclass overrides these to alter behavior of views' iterator() method
        private IEnumerator<K> newKeyEnumerator()
        {
            return new KeyEnumerator(this);
        }

        private IEnumerator<V> newValueEnumerator()
        {
            return new ValueEnumerator(this);
        }

        private IEnumerator<IHashMapEntry<K,V>> newEntryEnumerator()
        {
            return new EntryEnumerator(this);
        }

        #region Nested type: EntryEnumerator

        private class EntryEnumerator : HashEnumerator, IEnumerator<IHashMapEntry<K, V>>
        {
            public EntryEnumerator(GenericHashMap<K, V> h) : base(h)
            {
            }

            #region IEnumerator<IHashMapEntry<K,V>> Members

            public IHashMapEntry<K, V> Current
            {
                get { return base.current; }
            }

            object IEnumerator.Current
            {
                get { return base.current; }
            }

            #endregion
        }

        #endregion

        #region Nested type: EntrySet

        [Serializable]
        public class EntrySet
        {
            private readonly GenericHashMap<K, V> _map;

            public EntrySet(GenericHashMap<K, V> theMap)
            {
                _map = theMap;
            }

            public int Count
            {
                get { return _map.Count; }
            }

            public IEnumerator GetEnumerator()
            {
                return _map.newEntryEnumerator();
            }

            public bool Contains(Object o)
            {
                if (!(o is HashMapEntry<K, V>))
                    return false;
                HashMapEntry<K, V> e = (HashMapEntry<K, V>) o;
                IHashMapEntry<K, V> candidate = _map.GetEntry(e.Key);
                return candidate != null && candidate.Equals(e);
            }
        }

        #endregion

        #region Nested type: HashEnumerator

        private abstract class HashEnumerator
        {
            private readonly GenericHashMap<K, V> _map;
            private IHashMapEntry<K, V> _current; // current entry
            private int _expectedModCount; // For fast-fail 
            private int _index; // current slot 
            private IHashMapEntry<K, V> _next; // next entry to return

            internal HashEnumerator(GenericHashMap<K, V> theMap)
            {
                _map = theMap;
                _expectedModCount = _map.modCount;
                IHashMapEntry<K, V>[] t = theMap.table;
                int i = t.Length;
                IHashMapEntry<K, V> n = null;
                if (theMap.size != 0)
                {
                    // advance to first entry
                    while (i > 0 && (n = t[--i]) == null)
                        ;
                }
                _next = n;
                _index = i;
            }

            protected virtual IHashMapEntry<K, V> current
            {
                get
                {
                    IHashMapEntry<K, V> e = _next;
                    if (e == null)
                        throw new ApplicationException("no such element");

                    IHashMapEntry<K, V> n = e.Next;
                    IHashMapEntry<K, V>[] t = _map.table;
                    int i = _index;
                    while (n == null && i > 0)
                        n = t[--i];
                    _index = i;
                    _next = n;
                    return _current = e;
                }
            }

            public bool MoveNext()
            {
                return _next != null;
            }

            public void Reset()
            {
            }

            public void Dispose()
            {
                _current = null;
                _next = null;
            }
        }

        #endregion

        #region Nested type: HashMapEntry

        /// <summary>
        /// represents an entry in the HashMap
        /// </summary>
        /// 
        [Serializable]
        public class HashMapEntry<K, V> : IHashMapEntry<K, V>
        {
            private readonly int _hash;
            private readonly K _key;
            private IHashMapEntry<K, V> _next;
            private V _value;

            public HashMapEntry(int h, K key, V val, IHashMapEntry<K, V> n)
            {
                _key = key;
                _value = val;
                _hash = h;
                _next = n;
            }

            #region IHashMapEntry<K,V> Members

            public int Hash
            {
                get { return _hash; }
            }

            public IHashMapEntry<K, V> Next
            {
                get { return _next; }
                set { _next = value; }
            }

            public K Key
            {
                get { return _key; }
            }

            public V Value
            {
                get { return _value; }
                set
                {
                    object oldValue = _value;
                    _value = value;
                }
            }

            #endregion

            public override bool Equals(object o)
            {
                if (!(o is HashMapEntry<K, V>))
                    return false;
                HashMapEntry<K, V> e = (HashMapEntry<K, V>) o;
                object k1 = Key;
                object k2 = e.Key;
                if (k1 == k2 || (k1 != null && k1.Equals(k2)))
                {
                    object v1 = Value;
                    object v2 = e.Value;
                    if (v1 == v2 || (v1 != null && v1.Equals(v2)))
                        return true;
                }
                return false;
            }

            public override int GetHashCode()
            {
                return (_key.Equals(NULL_KEY) ? 0 : _key.GetHashCode()) ^
                       (_value.Equals(default(V)) ? 0 : _value.GetHashCode());
            }
        }

        #endregion

        #region Nested type: HashMapKeys

        public class HashMapKeys : ICollection<K>, IEnumerable<K>, ICollection, IEnumerable
        {
            private readonly GenericHashMap<K, V> map;

            public HashMapKeys(GenericHashMap<K, V> map)
            {
                this.map = map;
            }

            #region ICollection<K> Members

            public void Add(K item)
            {
                throw new NotSupportedException();
            }

            public void Clear()
            {
                throw new NotSupportedException();
            }

            public bool Contains(K item)
            {
                return map.ContainsKey(item);
            }

            public void CopyTo(K[] array, int arrayIndex)
            {
                if (array == null)
                {
                    throw new ArgumentNullException("array");
                }
                if ((arrayIndex < 0) || (arrayIndex > array.Length))
                {
                    throw new ArgumentOutOfRangeException("arrayIndex");
                }
                if ((array.Length - arrayIndex) < this.map.Count)
                {
                    throw new ArgumentException("The array + offset is too small.");
                }
                int count = this.map.Count;
                IHashMapEntry<K, V>[] entries = this.map.table;
                for (int i = 0; i < count; i++)
                {
                    if (entries[i].Hash >= 0)
                    {
                        array[arrayIndex++] = entries[i].Key;
                    }
                }
            }

            public bool Remove(K item)
            {
                throw new NotSupportedException();
            }

            #region ICollection Members

            public void CopyTo(Array array, int index)
            {
                if (array == null)
                {
                    throw new ArgumentNullException("array");
                }
                if (array.Rank != 1)
                {
                    throw new ArgumentException("Multi Dim Not Supported");
                }
                if (array.GetLowerBound(0) != 0)
                {
                    throw new ArgumentException("Non Zero Lower Bound");
                }
                if ((index < 0) || (index > array.Length))
                {
                    throw new ArgumentOutOfRangeException("index");
                }
                if ((array.Length - index) < this.map.Count)
                {
                    throw new ArgumentException("Array Plus Offset Too Small");
                }
                V[] localArray = array as V[];
                if (localArray != null)
                {
                    this.CopyTo(localArray, index);
                }
                else
                {
                    object[] objArray = array as object[];
                    if (objArray == null)
                    {
                        throw new ArgumentException("InvalidArrayType");
                    }
                    int count = this.map.Count;
                    IHashMapEntry<K, V>[] entries = this.map.table;
                    try
                    {
                        for (int i = 0; i < count; i++)
                        {
                            if (entries[i].Hash >= 0)
                            {
                                objArray[index++] = entries[i].Key;
                            }
                        }
                    }
                    catch (ArrayTypeMismatchException)
                    {
                        throw new ArgumentException("Invalid Array Type");
                    }
                }
            }

            public int Count
            {
                get { return map.Count; }
            }

            int ICollection<K>.Count
            {
                get { return map.Count; }
            }

            public bool IsReadOnly
            {
                get { return true; }
            }

            #endregion

            public object SyncRoot
            {
                get { throw new NotSupportedException(); }
            }

            public bool IsSynchronized
            {
                get { return false; }
            }

            #endregion

            public IEnumerator GetEnumerator()
            {
                return map.newKeyEnumerator();
            }

            IEnumerator<K> IEnumerable<K>.GetEnumerator()
            {
                return map.newKeyEnumerator();
            }

        }

        #endregion

        #region Nested type: HashMapValues

        public class HashMapValues : ICollection<V>, IEnumerable<V>, ICollection, IEnumerable
        {
            private readonly GenericHashMap<K, V> map;

            public HashMapValues(GenericHashMap<K, V> map)
            {
                this.map = map;
            }

            #region ICollection Members

            public void CopyTo(Array array, int index)
            {
                if (array == null)
                {
                    throw new ArgumentNullException("array");
                }
                if (array.Rank != 1)
                {
                    throw new ArgumentException("Multi Dim Not Supported");
                }
                if (array.GetLowerBound(0) != 0)
                {
                    throw new ArgumentException("Non Zero Lower Bound");
                }
                if ((index < 0) || (index > array.Length))
                {
                    throw new ArgumentOutOfRangeException("index");
                }
                if ((array.Length - index) < this.map.Count)
                {
                    throw new ArgumentException("Array Plus Offset Too Small");
                }
                V[] localArray = array as V[];
                if (localArray != null)
                {
                    this.CopyTo(localArray, index);
                }
                else
                {
                    object[] objArray = array as object[];
                    if (objArray == null)
                    {
                        throw new ArgumentException("InvalidArrayType");
                    }
                    int count = this.map.Count;
                    IHashMapEntry<K, V>[] entries = this.map.table;
                    try
                    {
                        for (int i = 0; i < count; i++)
                        {
                            if (entries[i].Hash >= 0)
                            {
                                objArray[index++] = entries[i].Value;
                            }
                        }
                    }
                    catch (ArrayTypeMismatchException)
                    {
                        throw new ArgumentException("Invalid Array Type");
                    }
                }

            }

            #region ICollection<V> Members

            public void Add(V item)
            {
                throw new NotSupportedException();
            }

            public void Clear()
            {
                throw new NotSupportedException();
            }

            public bool Contains(V item)
            {
                return map.ContainsValue(item);
            }

            public void CopyTo(V[] array, int arrayIndex)
            {
                if (array == null)
                {
                    throw new ArgumentNullException("array");
                }
                if ((arrayIndex < 0) || (arrayIndex > array.Length))
                {
                    throw new ArgumentOutOfRangeException("arrayIndex");
                }
                if ((array.Length - arrayIndex) < this.map.Count)
                {
                    throw new ArgumentException("The array + offset is too small.");
                }
                int count = this.map.Count;
                IHashMapEntry<K, V>[] entries = this.map.table;
                for (int i = 0; i < count; i++)
                {
                    if (entries[i].Hash >= 0)
                    {
                        array[arrayIndex++] = entries[i].Value;
                    }
                }

            }

            public bool Remove(V item)
            {
                throw new NotSupportedException();
            }

            public int Count
            {
                get { return map.size; }
            }

            public bool IsReadOnly
            {
                get { return true; }
            }

            #endregion

            public object SyncRoot
            {
                get { throw new NotSupportedException(); }
            }

            public bool IsSynchronized
            {
                get { return false; }
            }

            #endregion

            public IEnumerator GetEnumerator()
            {
                return map.newValueEnumerator();
            }

            IEnumerator<V> IEnumerable<V>.GetEnumerator()
            {
                return map.newValueEnumerator();
            }
        }

        #endregion

        #region Nested type: KeyEnumerator

        private class KeyEnumerator : HashEnumerator, IEnumerator<K>
        {
            public KeyEnumerator(GenericHashMap<K, V> h) : base(h)
            {
            }

            #region IEnumerator<K> Members

            public K Current
            {
                get { return base.current.Key; }
            }

            object IEnumerator.Current
            {
                get { return base.current.Key; }
            }

            #endregion

        }

        #endregion

        #region Nested type: ValueEnumerator

        private class ValueEnumerator : HashEnumerator, IEnumerator<V>
        {
            public ValueEnumerator(GenericHashMap<K, V> h) : base(h)
            {
            }

            #region IEnumerator<V> Members

            public V Current
            {
                get { return base.current.Value; }
            }

            object IEnumerator.Current
            {
                get { return base.current.Value; }
            }

            #endregion
        }

        #endregion

        #endregion

        #region IPrettyPrint Members

        public string toPPString()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}