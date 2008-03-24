using System;

namespace Creshendo.Util
{
    /// <author>  Peter Lin
    /// 
    /// A basic HashMap implementation inspired by Mark's HashTable for
    /// Drools3. The main difference is this HashMap tries to be compatable
    /// with java.util.HashMap. This means it's not as stripped down as
    /// the super optimized HashTable mark wrote for drools. Hopefully the
    /// extra stuff doesn't make too big of a difference.
    /// 
    /// </author>
    public class HashMap2 : AbstractMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HashMap2"/> class.
        /// </summary>
        public HashMap2() : base(101, 0.75f)
        {
        }

        public override bool ContainsKey(Object key)
        {
            return Get(key) != null;
        }

        public override object this[object key]
        {
            get { return Get(key); }
            set { Put_(key, value,false); }
        }

        public override System.Collections.IEnumerator GetEnumerator()
        {
            return entryArray.GetEnumerator();
        }

        public override Object Get(Object key)
        {
            int hashCode = comparator.hashCodeOf(key);
            int index = indexOf(hashCode, entryArray.Length);

            ObjectEntry current = (ObjectEntry) entryArray[index];
            while (current != null)
            {
                if (hashCode == current.GetHashCode() && comparator.equal(key, current.Key))
                {
                    return current.Value;
                }
                current = (ObjectEntry) current.Next;
            }
            return null;
        }

        ///// <summary> 
        ///// </summary>
        //public override Object PutWithReturn(Object key, Object value_Renamed)
        //{
        //    return Put(key, value_Renamed, false);
        //}

        public override void Put(Object key, Object value_Renamed)
        {
            Put_(key, value_Renamed, false);
        }

        public virtual Object Put_(Object key, Object value_Renamed, bool checkExists)
        {
            int hashCode = comparator.hashCodeOf(key);
            int index = indexOf(hashCode, entryArray.Length);

            // scan the linked entries to see if it exists
            if (checkExists)
            {
                IEntry current = entryArray[index];
                while (current != null)
                {
                    if (hashCode == current.GetHashCode() && key.Equals(current.Key))
                    {
                        Object oldValue = current.Value;
                        current.Value = value_Renamed;
                        return oldValue;
                    }
                    current = (ObjectEntry) current.Next;
                }
            }

            // create a new ObjectEntry
            ObjectEntry entry = new ObjectEntry(key, value_Renamed, hashCode);
            // in case there is already an entry with the same hashcode,
            // set it as the Current entry for the new one. this means the older
            // entries are pushed down the bucket
            entry.Next = entryArray[index];
            entryArray[index] = entry;

            if (currentSize++ >= threshold)
            {
                resize(2*entryArray.Length);
            }
            return null;
        }

        public override Object RemoveWithReturn(Object key)
        {
            int hashCode = comparator.hashCodeOf(key);
            int index = indexOf(hashCode, entryArray.Length);

            ObjectEntry previous = (ObjectEntry) entryArray[index];
            ObjectEntry current = previous;
            while (current != null)
            {
                ObjectEntry next = (ObjectEntry) current.Next;
                if (hashCode == current.GetHashCode() && comparator.equal(key, current.Key))
                {
                    if (previous == current)
                    {
                        entryArray[index] = next;
                    }
                    else
                    {
                        previous.Next = next;
                    }
                    current.Next = null;
                    currentSize--;
                    return current.Value;
                }
                previous = current;
                current = next;
            }
            return null;
        }


        public override void Remove(Object key)
        {
            int hashCode = comparator.hashCodeOf(key);
            int index = indexOf(hashCode, entryArray.Length);

            ObjectEntry previous = (ObjectEntry)entryArray[index];
            ObjectEntry current = previous;
            while (current != null)
            {
                ObjectEntry next = (ObjectEntry)current.Next;
                if (hashCode == current.GetHashCode() && comparator.equal(key, current.Key))
                {
                    if (previous == current)
                    {
                        entryArray[index] = next;
                    }
                    else
                    {
                        previous.Next = next;
                    }
                    current.Next = null;
                    currentSize--;
                }
                previous = current;
                current = next;
            }
        }

        //public override Set<object> entrySet()
        //{
        //    throw new NotImplementedException();
        //}

        #region Nested type: ObjectEntry

        public class ObjectEntry : IEntry
        {
            private readonly int _hash;
            private Object _key;
            private IEntry _nextEntry;
            private Object _value;

            public ObjectEntry(Object key, Object value_Renamed, int hashCode)
            {
                _key = key;
                _value = value_Renamed;
                _hash = hashCode;
            }

            #region Entry Members

            public virtual Object Value
            {
                get { return _value; }

                set { _value = value; }
            }

            public virtual Object Key
            {
                get { return _key; }
            }

            public virtual IEntry Next
            {
                get { return _nextEntry; }

                set { _nextEntry = value; }
            }


            public virtual void clear()
            {
                _key = null;
                _value = null;
                if (_nextEntry != null)
                    _nextEntry.clear();
            }

            #endregion

            public override int GetHashCode()
            {
                return _hash;
            }

            public override bool Equals(Object obj)
            {
                if (obj == this)
                {
                    return true;
                }
                ObjectEntry other = (ObjectEntry) obj;
                return _key.Equals(other.Key) && _value.Equals(other.Value);
            }
        }

        #endregion
    }
}