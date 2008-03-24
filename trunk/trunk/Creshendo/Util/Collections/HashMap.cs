using System;
using System.Collections;
using Creshendo.Util.Collections;

namespace Creshendo.Util.Collections
{
    [Serializable]
    public class HashMap : IMap
    {
        // must be a power of 2
        static readonly int DEFAULT_INITIAL_CAPACITY = 101;

        //the maximum capacity, must be a power of 2 <= 1<<30
        static readonly int MAXIMUM_CAPACITY = 1 << 30;

        // load factor, when none is specified
        static readonly float DEFAULT_LOAD_FACTOR = 0.75f;

        // length must be a power of 2 always, will be resized as necessary
        HashMapEntry[] table;

        // the number of key-value mappings in this mape
        protected int size;
  
        // tells us when we will need to resize next
        int threshold;
  
        //load factor of the table
        readonly double loadFactor;

        /**
		 * The number of times this HashMap has been structurally modified
		 * Structural modifications are those that change the number of mappings in
		 * the HashMap or otherwise modify its internal structure (e.g.,
		 * rehash).  This field is used to make iterators on Collection-views of
		 * the HashMap fail-fast.  (See ConcurrentModificationException).
		 */
        int modCount;

        #region constructors

        //public HashMap(SerializationInfo info, StreamingContext c)
        //{
        //    info.AddValue("table", table);
        //    info.AddValue("size", size);
        //    info.AddValue("loadFactor", loadFactor);
        //    info.AddValue("threshold", threshold);
        //}

        //public void GetObjectData (SerializationInfo info, StreamingContext context)
        //{
        //    table = (HashMapEntry[])info.GetValue("table", typeof(System.Array));
        //    loadFactor = info.GetDouble("loadfactor");
        //    size = info.GetInt32("size");
        //    threshold = info.GetInt32("threshold");

        //}
        /**
		 * Constructs an empty <tt>HashMap</tt> with the specified initial
		 * capacity and load factor.
		 *
		 * @param  initialCapacity The initial capacity.
		 * @param  loadFactor      The load factor.
		 * @throws IllegalArgumentException if the initial capacity is negative
		 *         or the load factor is nonpositive.
		 */
        public HashMap(int initialCapacity, float loadFactor) 
        {
            if (initialCapacity < 0)
                throw new System.ArgumentException("Illegal initial capacity: " +
                                                   initialCapacity);
            if (initialCapacity > MAXIMUM_CAPACITY)
                initialCapacity = MAXIMUM_CAPACITY;
            if (loadFactor <= 0 || Double.IsNaN(loadFactor))
                throw new System.ArgumentException("Illegal load factor: " +
                                                   loadFactor);

            // Find a power of 2 >= initialCapacity
            int capacity = 1;
            while (capacity < initialCapacity) 
                capacity <<= 1;
    
            this.loadFactor = loadFactor;
            threshold = (int)(capacity * loadFactor);
            table = new HashMapEntry[capacity];
            init();
        }

        /**
		* Value representing null keys inside tables.
		*/
        static readonly object NULL_KEY = new object();
        private bool isReadOnly;
        private bool isFixedSize;

        /**
		 * Returns internal representation for key. Use NULL_KEY if key is null.
		 */
        static object maskNull(object key) 
        {
            return (key == null ? NULL_KEY : key);
        }

        /**
		 * Returns key represented by specified internal representation.
		 */
        static object unmaskNull(object key) 
        {
            return (key == NULL_KEY ? null : key);
        }

        /**
		* Constructs an empty <tt>HashMap</tt> with the specified initial
		* capacity and the default load factor (0.75).
		*
		* @param  initialCapacity the initial capacity.
		* @throws IllegalArgumentException if the initial capacity is negative.
		*/
        public HashMap(int initialCapacity) : this(initialCapacity, DEFAULT_LOAD_FACTOR)
        {}

        /**
		 * Constructs an empty <tt>HashMap</tt> with the default initial capacity
		 * (16) and the default load factor (0.75).
		 */
        public HashMap() 
        {
            this.loadFactor = DEFAULT_LOAD_FACTOR;
            threshold = (int)(DEFAULT_INITIAL_CAPACITY);
            table = new HashMapEntry[DEFAULT_INITIAL_CAPACITY];
            init();
        }

        /**
		* Initialization hook for subclasses. This method is called
		* in all constructors and pseudo-constructors (clone, readObject)
		* after HashMap has been initialized but before any entries have
		* been inserted.  (In the absence of this method, readObject would
		* require explicit knowledge of subclasses.)
		*/
        void init() 
        {
        }
        #endregion

        #region properties

        public ICollection Values
        {
            get{return new HashMapValues(this);}
        }

        #region IDictionary Members

        public bool IsReadOnly
        {
            get { return isReadOnly; }
        }

        public bool IsFixedSize
        {
            get { return isFixedSize; }
        }

        #endregion

        public ICollection Keys
        {
            get { return new HashMapKeys(this); }
        }

        public EntrySet TheEntrySet
        {
            get{return new EntrySet(this);}
        }
        /// <summary>
        /// returns the number of key-value mappings
        /// </summary>
        public int Count 
        {
            get{return size;}
        }

        /// <summary>
        /// Get the current capacity.  Readonly
        /// </summary>
        public int Capacity
        {
            get{return table.Length;}
        }

        public bool IsSynchronized
        {
            get{return false;}
        }

        public object SyncRoot
        {
            get{return null;}
        }

        /// <summary>
        /// returns true if there are no key-value mappings
        /// </summary>
        public bool IsEmpty
        {
            get{return size == 0;}
        }

        public object Get(object key)
        {
            return this[key];
        }

        public object this[object key]
        {
            get
            {
                object k = maskNull(key);
                int thehash = hash(k);
                int i = indexFor(thehash, table.Length);
                HashMapEntry e = table[i]; 
                while (true) 
                {
                    if (e == null)
                        return e;
                    if (e.Hash == thehash && eq(k, e.Key)) 
                        return e.Value;
                    e = e.Next;
                }
            }
            set
            {
                Put(key, value);
            }
        }
        #endregion


        public void CopyTo(System.Array theArr, int length)
        { /*unimplemented for now*/ }

        public IEnumerator GetEnumerator()
        {
            return new EntryEnumerator(this);
        }

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new EntryEnumerator(this);
        }

        #endregion

        /// <summary>
        /// check for equality of non-null references x and possibly-null y
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        static bool eq(Object x, Object y) 
        {
            return x == y || x.Equals(y);
        }

        /// <summary>
        /// returns index for hash code h
        /// </summary>
        /// <param name="h"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        static int indexFor(int h, int length) 
        {
            return h & (length-1);
        }

        #region IMap Members

        public bool Empty
        {
            get { return size == 0; }
        }

        #endregion

        /// <summary>
        /// true if this map contains a mapping for the specified key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(object key) 
        {
            object k = maskNull(key);
            int thehash = hash(k);
            int i = indexFor(thehash, table.Length);
            HashMapEntry e = table[i]; 
            while (e != null) 
            {
                if (e.Hash == thehash && eq(k, e.Key)) 
                    return true;
                e = e.Next;
            }
            return false;
        }

        public bool Contains(object key)
        {
            object k = maskNull(key);
            int thehash = hash(k);
            int i = indexFor(thehash, table.Length);
            HashMapEntry e = table[i];
            while (e != null)
            {
                if (e.Hash == thehash && eq(k, e.Key))
                    return true;
                e = e.Next;
            }
            return false;
        }

        public void Add(object key, object entry)
        {
            Object k = maskNull(key);
            int theHash = hash(k);
            int i = indexFor(theHash, table.Length);

            for (HashMapEntry e = table[i]; e != null; e = e.Next)
            {
                if (e.Hash == theHash && eq(k, e.Key))
                {
                    Object oldValue = e.Value;
                    e.Value = entry;
                    return;
                }
            }

            modCount++;
            AddEntry(theHash, k, entry, i);

        }

        /// <summary>
        /// add a key value pair to this map
        /// </summary>
        /// <param name="key"></param>
        /// <param name="entry"></param>
        public void Put(object key, object entry)
        {
            Object k = maskNull(key);
            int theHash = hash(k);
            int i = indexFor(theHash, table.Length);

            for (HashMapEntry e = table[i]; e != null; e = e.Next) 
            {
                if (e.Hash == theHash && eq(k, e.Key)) 
                {
                    Object oldValue = e.Value;
                    e.Value = entry;
                    return;
                }
            }

            modCount++;
            AddEntry(theHash, k, entry, i);

        }

        /// <summary>
        /// removes an entry for the specified key
        /// returns the removed entry
        /// </summary>
        HashMapEntry RemoveEntryForKey(object key) 
        {
            object k = maskNull(key);
            int thehash = hash(k);
            int i = indexFor(thehash, table.Length);
            HashMapEntry prev = table[i];
            HashMapEntry e = prev;

            while (e != null) 
            {
                HashMapEntry next = e.Next;
                if (e.Hash == thehash && eq(k, e.Key)) 
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
        public HashMapEntry RemoveMapping(object o)
        {
            if (!(o is HashMapEntry))
                return null;

            HashMapEntry entry = (HashMapEntry)o;
            object k = maskNull(entry.Key);
            int thehash = hash(k);
            int i = indexFor(thehash, table.Length);
            HashMapEntry prev = table[i];
            HashMapEntry e = prev;

            while (e != null) 
            {
                HashMapEntry next = e.Next;
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
            HashMapEntry[] tab = table;
            for (int i = 0; i < tab.Length; i++) 
                tab[i] = null;
            size = 0;
        }

        //IDictionaryEnumerator IDictionary.GetEnumerator()
        //{
        //    throw new NotImplementedException(); 
        //}


        /// <summary>
        /// generate a hash code for the specified key
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        static int hash(object x) 
        {
            int h = x.GetHashCode();

            h += ~(h << 9);
            h ^=  (h >> 14);
            h +=  (h << 4);
            h ^=  h >> 10;
            return h;
        }

        /// <summary>
        /// used instead of Add by Constructors and Clones, does not resize the table
        /// calls CreateEntry instead of AddEntry
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private void PutForCreate(Object key, object value) 
        {
            object k = maskNull(key);
            int thehash = hash(k);
            int i = indexFor(thehash, table.Length);

            /**
			 * Look for preexisting entry for key.  This will never happen for
			 * clone or deserialize.  It will only happen for construction if the
			 * input Map is a sorted map whose ordering is inconsistent w/ equals.
			 */
            for (HashMapEntry e = table[i]; e != null; e = e.Next) 
            {
                if (e.Hash == thehash && eq(k, e.Key)) 
                {
                    e.Value = value;
                    return;
                }
            }

            CreateEntry(thehash, k, value, i);
        }

        void PutAllForCreate(HashMap m) 
        {
            for (IEnumerator i = m.TheEntrySet.GetEnumerator(); i.MoveNext(); ) 
            {
                HashMapEntry e = (HashMapEntry) i.Current;
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
        void AddEntry(int hash, object key, object value, int bucketIndex) 
        {
            table[bucketIndex] = new HashMapEntry(hash, key, value, table[bucketIndex]);
            if (size++ >= threshold) 
                Resize(2 * table.Length);
        }

        /// <summary>
        /// this is used when creating entries as part of a constructor or Clone call
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="bucketIndex"></param>
        void CreateEntry(int hash, object key, object value, int bucketIndex) 
        {
            table[bucketIndex] = new HashMapEntry(hash, key, value, table[bucketIndex]);
            size++;
        }

        /// <summary>
        /// rehashes the the contents of this map into a new HashMap with a larger capacity
        /// called automatically when the number of keys exceedts the capacity and load factor
        /// 
        /// </summary>
        /// <param name="newCapacity">new capacity, must be a power of 2</param>
        void Resize(int newCapacity) 
        {
            HashMapEntry[] oldTable = table;
            int oldCapacity = oldTable.Length;
    
            // check if needed
            if (size < threshold || oldCapacity > newCapacity) 
                return;
    
            HashMapEntry[] newTable = new HashMapEntry[newCapacity];
            Transfer(newTable);
            table = newTable;
            threshold = (int)(newCapacity * loadFactor);
        }

        /// <summary>
        /// transfer all entries from current table into the newTable
        /// </summary>
        /// <param name="newTable"></param>
        void Transfer(HashMapEntry[] newTable) 
        {
            HashMapEntry[] src = table;
            int newCapacity = newTable.Length;
            for (int j = 0; j < src.Length; j++) 
            {
                HashMapEntry e = src[j];
                if (e != null) 
                {
                    src[j] = null;
                    do 
                    {
                        HashMapEntry next = e.Next;
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
        public void putAll(IMap t) 
        {
            // Expand enough to hold t's elements without resizing.
            int n = t.Count;
            if (n == 0)
                return;
            if (n >= threshold) 
            {
                n = (int)(n / loadFactor + 1);
                if (n > MAXIMUM_CAPACITY)
                    n = MAXIMUM_CAPACITY;
                int capacity = table.Length;
                while (capacity < n) 
                    capacity <<= 1;
                Resize(capacity);
            }

            foreach(HashMapEntry entry in ((HashMap)t).TheEntrySet)
            {
                Put(entry.Key, entry.Value);
            }

            //for (IEnumerator i = t.GetEnumerator(); i.MoveNext(); ) 
            //{
            //    HashMapEntry e = (HashMapEntry) i.Current;
            //    Put(e.Key, e.Value);
            //}
        }
  

        /// <summary>
        /// removes the mapping for this key from this map
        /// </summary>
        /// <param name="key"></param>
        /// <returns>previous value for the key, or null if no mapping</returns>
        public void Remove(object key) 
        {
            RemoveEntryForKey(key);
        }

        public object RemoveWithReturn(object key)
        {
            HashMapEntry e = RemoveEntryForKey(key);
            return (e == null ? e : e.Value);
        }

        /// <summary>
        /// returns true if this Map has a mapping for the object
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool ContainsValue(Object value) 
        {
            if (value == null) 
                return ContainsNullValue();

            HashMapEntry[] tab = table;
            for (int i = 0; i < tab.Length ; i++)
                for (HashMapEntry e = tab[i] ; e != null ; e = e.Next)
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
            HashMapEntry[] tab = table;
            for (int i = 0; i < tab.Length ; i++)
                for (HashMapEntry e = tab[i] ; e != null ; e = e.Next)
                    if (e.Value == null)
                        return true;
            return false;
        }

        /// <summary>
        /// return a shallow copy of the HashMap instance
        /// the keys and values are not cloned
        /// </summary>
        /// <returns></returns>
        public object Clone() 
        {
            HashMap result = new HashMap();
		
            result.table = new HashMapEntry[table.Length];
            result.modCount = 0;
            result.size = 0;
            result.init();
            result.PutAllForCreate(this);

            return result;
        }

        /// <summary>
        /// reutrns the entry associated with the key
        /// </summary>
        HashMapEntry GetEntry(object key) 
        {
            object k = maskNull(key);
            int theHash = hash(k);
            int i = indexFor(theHash, table.Length);
            HashMapEntry e = table[i]; 
            while (e != null && !(e.Hash == theHash && eq(k, e.Key)))
                e = e.Next;
            return e;
        }

        #region internal classes
        private abstract class HashEnumerator : IEnumerator 
        {
            HashMapEntry next;                  // next entry to return
            int expectedModCount;        // For fast-fail 
            int index;                   // current slot 
            HashMapEntry current;               // current entry
            readonly HashMap map;

            internal HashEnumerator(HashMap theMap) 
            {
                map = theMap;
                expectedModCount = map.modCount;
                HashMapEntry[] t = theMap.table;
                int i = t.Length;
                HashMapEntry n = null;
                if (theMap.size != 0) 
                { // advance to first entry
                    while (i > 0 && (n = t[--i]) == null)
                        ;
                }
                next = n;
                index = i;
            }

            public bool MoveNext() 
            {
                return next != null;
            }
			
            public void Reset()
            {}

            public virtual object Current 
            { 
                get
                {
                    HashMapEntry e = next;
                    if (e == null) 
                        throw new ApplicationException("no such element");
                
                    HashMapEntry n = e.Next;
                    HashMapEntry[] t = map.table;
                    int i = index;
                    while (n == null && i > 0)
                        n = t[--i];
                    index = i;
                    next = n;
                    return current = e;
                }
            }
        }

        private class ValueEnumerator : HashEnumerator 
        {
            public ValueEnumerator(HashMap h):base(h){}
            public override object Current
            {
                get{return ((HashMapEntry)base.Current).Value;}
            }
        }

        private class KeyEnumerator : HashEnumerator 
        {
            public KeyEnumerator(HashMap h):base(h){}
            public override object Current 
            {
                get{return ((HashMapEntry)base.Current).Key;}
            }
        }

        private class EntryEnumerator : HashEnumerator 
        {
            public EntryEnumerator(HashMap h):base(h){}
			
            //public override object Current
            //{
            //    get{return base.Current;}
            //}
        }

        // Subclass overrides these to alter behavior of views' iterator() method
        IEnumerator newKeyEnumerator()   
        {
            return new KeyEnumerator(this);
        }
        IEnumerator newValueEnumerator()   
        {
            return new ValueEnumerator(this);
        }
        IEnumerator newEntryEnumerator()   
        {
            return new EntryEnumerator(this);
        }
        /// <summary>
        /// represents an entry in the HashMap
        /// </summary>
        /// 
        [Serializable]
        public class HashMapEntry
        {
            private readonly object _key;
            private object _value;
            private readonly int _hash;
            HashMapEntry _next;

            public HashMapEntry(int h, object key, object val, HashMapEntry n) 
            {
                _key = key;
                _value = val;
                _hash = h;
                _next = n;

            }

            public int Hash
            {
                get{return _hash;}
            }

            public HashMapEntry Next
            {
                get{return _next;}
                set{_next = value;}
            }

            public object Key
            {
                get{return HashMap.unmaskNull(_key);}
            }

            public object Value
            {
                get{return _value;}
                set
                {
                    object oldValue = _value;
                    _value = value;
                }
            }

            public override bool Equals(object o)
            {
                if (!(o is HashMapEntry))
                    return false;
                HashMapEntry e = (HashMapEntry)o;
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
                return (_key==NULL_KEY ? 0 : _key.GetHashCode()) ^
                       (_value==null   ? 0 : _value.GetHashCode());
            }
        }


        [Serializable]
        public class EntrySet 
        {
            readonly HashMap _map;
            public EntrySet(HashMap theMap)
            {
                _map = theMap;
            }

            public IEnumerator GetEnumerator() 
            {
                return null;
            }

            public bool Contains(Object o) 
            {
                if (!(o is HashMapEntry))
                    return false;
                HashMapEntry e = (HashMapEntry)o;
                HashMapEntry candidate = _map.GetEntry(e.Key);
                return candidate != null && candidate.Equals(e);
            }

            public int Count
            {
                get{return _map.Count;}
            }
        }

        public class HashMapValues : ICollection
        {
            readonly HashMap map;
            private object syncRoot;
            private bool isSynchronized;

            public HashMapValues(HashMap map)
            {
                this.map = map;
            }

            #region ICollection Members

            public void CopyTo(Array array, int index)
            {
                throw new NotImplementedException();
            }

            public int Count
            {
                get { return map.size;  }
            }

            public object SyncRoot
            {
                get { return syncRoot; }
            }

            public bool IsSynchronized
            {
                get { return isSynchronized; }
            }

            #endregion

            public IEnumerator GetEnumerator() 
            {
                return map.newValueEnumerator();
            }
            public int size()
            {
                return map.size;
            }
            public bool contains(Object o) 
            {
                return map.ContainsValue(o);
            }
            public void Clear() 
            {
                map.Clear();
            }
        }

        public class HashMapKeys : ICollection
        {
            readonly HashMap map;
            private object syncRoot;
            private bool isSynchronized;

            public HashMapKeys(HashMap map)
            {
                this.map = map;
            }

            #region ICollection Members

            public void CopyTo(Array array, int index)
            {
                throw new NotImplementedException();
            }

            public int Count
            {
                get { return map.size; }
            }

            public object SyncRoot
            {
                get { return syncRoot; }
            }

            public bool IsSynchronized
            {
                get { return isSynchronized; }
            }

            #endregion

            public IEnumerator GetEnumerator()
            {
                return map.newKeyEnumerator();
            }
            public int size()
            {
                return map.size;
            }
            public bool contains(Object o)
            {
                return map.ContainsKey(o);
            }
            public void Clear()
            {
                map.Clear();
            }
        }

        #endregion

        
    }
}