using System;
using System.Collections;

namespace Creshendo.Util
{
    public abstract class AbstractLinkedDictionary : IDictionary
    {
        public readonly IDictionary Backend;
        protected Link first;
        protected Link last;
        protected volatile uint updates;

        public AbstractLinkedDictionary(IDictionary backend)
        {
            Backend = backend;
        }

        #region IDictionary Members

        public bool IsReadOnly
        {
            get { return Backend.IsReadOnly; }
        }

        public virtual IDictionaryEnumerator GetEnumerator()
        {
            return GetEnumeratorForward();
        }

        public object this[object key]
        {
            get { return getItem(key).Value; }
            set
            {
                updates++;
                setItem(key, value);
            }
        }

        public void Remove(object key)
        {
            updates++;
            removeItem(key);
        }

        public bool Contains(object key)
        {
            return Backend.Contains(key);
        }

        public void Clear()
        {
            updates++;
            Backend.Clear();
            first = null;
            last = null;
        }

        public virtual ICollection Values
        {
            get { return ValuesForward; }
        }

        public void Add(object key, object value)
        {
            updates++;
            addItem(key, value);
        }

        public virtual ICollection Keys
        {
            get { return KeysForward; }
        }

        public bool IsFixedSize
        {
            get { return Backend.IsFixedSize; }
        }

        public bool IsSynchronized
        {
            get { return Backend.IsSynchronized; }
        }

        public int Count
        {
            get { return Backend.Count; }
        }

        public void CopyTo(Array array, int index)
        {
            foreach (DictionaryEntry e in this)
                array.SetValue(e, index++);
        }

        public object SyncRoot
        {
            get { return Backend.SyncRoot; }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #region Debug Helpers

        private static object[] Array(ICollection c)
        {
            object[] a = new object[c.Count];
            c.CopyTo(a, 0);
            return a;
        }

        #endregion

        #endregion

        #region Nested type: DictionaryEnumerator_

        private class DictionaryEnumerator_ : IDictionaryEnumerator
        {
            private readonly bool Forward;
            private readonly AbstractLinkedDictionary Parent;
            private readonly uint updates;
            private Link current;

            public DictionaryEnumerator_(AbstractLinkedDictionary parent, bool forward)
            {
                Parent = parent;
                Forward = forward;
                current = null;
                updates = parent.updates;
            }

            #region IDictionaryEnumerator Members

            public void Reset()
            {
                current = null;
            }

            public object Current
            {
                get { return Entry; }
            }

            public bool MoveNext()
            {
                if (Parent.updates != updates)
                    throw new InvalidOperationException("Collection was modified after the enumerator was created");
                if (current == null)
                    current = Forward ? Parent.first : Parent.last;
                else
                    current = Forward ? current.Next : current.Previous;
                return current != null;
            }

            public object Key
            {
                get
                {
                    if (current == null)
                        throw new IndexOutOfRangeException();
                    else
                        return current.Key;
                }
            }

            public object Value
            {
                get
                {
                    if (current == null)
                        throw new IndexOutOfRangeException();
                    else
                        return current.Value;
                }
            }

            public DictionaryEntry Entry
            {
                get
                {
                    if (current == null)
                        throw new IndexOutOfRangeException();
                    else
                        return new DictionaryEntry(current.Key, current.Value);
                }
            }

            #endregion
        }

        #endregion

        #region Nested type: Keys_

        public struct Keys_ : ICollection
        {
            private readonly bool Forward;
            private readonly AbstractLinkedDictionary Parent;

            public Keys_(AbstractLinkedDictionary parent, bool forward)
            {
                Parent = parent;
                Forward = forward;
            }

            #region ICollection Members

            public bool IsSynchronized
            {
                get { return Parent.IsSynchronized; }
            }

            public int Count
            {
                get { return Parent.Backend.Count; }
            }

            public void CopyTo(Array array, int index)
            {
                foreach (object o in this)
                    array.SetValue(o, index++);
            }

            public object SyncRoot
            {
                get { return Parent.SyncRoot; }
            }

            public IEnumerator GetEnumerator()
            {
                return new Enumerator_(new DictionaryEnumerator_(Parent, Forward));
            }

            #endregion

            #region Nested type: Enumerator_

            private struct Enumerator_ : IEnumerator
            {
                private readonly DictionaryEnumerator_ Enumerator;

                public Enumerator_(DictionaryEnumerator_ enumerator)
                {
                    Enumerator = enumerator;
                }

                #region IEnumerator Members

                public void Reset()
                {
                    Enumerator.Reset();
                }

                public object Current
                {
                    get { return Enumerator.Key; }
                }

                public bool MoveNext()
                {
                    return Enumerator.MoveNext();
                }

                #endregion
            }

            #endregion
        }

        #endregion

        #region Nested type: Link

        protected class Link
        {
            public readonly object Key;
            public readonly object Value;
            public Link Next;
            public Link Previous;

            public Link(object key, object value, Link prev, Link next)
            {
                Key = key;
                Value = value;
                Previous = prev;
                Next = next;
            }
        }

        #endregion

        #region Nested type: Values_

        public struct Values_ : ICollection
        {
            private readonly bool Forward;
            private readonly AbstractLinkedDictionary Parent;

            public Values_(AbstractLinkedDictionary parent, bool forward)
            {
                Parent = parent;
                Forward = forward;
            }

            #region ICollection Members

            public bool IsSynchronized
            {
                get { return Parent.IsSynchronized; }
            }

            public int Count
            {
                get { return Parent.Count; }
            }

            public void CopyTo(Array array, int index)
            {
                foreach (object o in this)
                    array.SetValue(o, index++);
            }

            public object SyncRoot
            {
                get { return Parent.SyncRoot; }
            }

            public IEnumerator GetEnumerator()
            {
                return new Enumerator_(new DictionaryEnumerator_(Parent, Forward));
            }

            #endregion

            #region Nested type: Enumerator_

            private struct Enumerator_ : IEnumerator
            {
                private readonly DictionaryEnumerator_ Enumerator;

                public Enumerator_(DictionaryEnumerator_ enumerator)
                {
                    Enumerator = enumerator;
                }

                #region IEnumerator Members

                public void Reset()
                {
                    Enumerator.Reset();
                }

                public object Current
                {
                    get { return Enumerator.Value; }
                }

                public bool MoveNext()
                {
                    return Enumerator.MoveNext();
                }

                #endregion
            }

            #endregion
        }

        #region Abstracts

        protected abstract void removeItem(object key);
        protected abstract void addItem(object key, object value);
        protected abstract void setItem(object key, object value);
        protected abstract Link getItem(object key);

        #endregion

        #region Explicit directional iteration

        public ICollection ValuesForward
        {
            get { return new Values_(this, true); }
        }

        public ICollection ValuesBackward
        {
            get { return new Values_(this, false); }
        }

        public ICollection KeysForward
        {
            get { return new Keys_(this, true); }
        }

        public ICollection KeysBackward
        {
            get { return new Keys_(this, false); }
        }

        public IDictionaryEnumerator GetEnumeratorForward()
        {
            return new DictionaryEnumerator_(this, true);
        }

        public IDictionaryEnumerator GetEnumeratorBackward()
        {
            return new DictionaryEnumerator_(this, false);
        }

        #endregion

        #endregion
    }

    /// <summary>
    /// Provides an IDictionary which is iterated in the inverse order of update
    /// </summary>
    public class UpdateLinkedDictionary : AbstractLinkedDictionary
    {
        public UpdateLinkedDictionary(IDictionary backend) : base(backend)
        {
        }

        protected override void addItem(object key, object value)
        {
            Link l = (Link) Backend[key];
            if (l != null)
                throw new ArgumentException(String.Format("Key \"{0}\" already present in dictionary", key));
            l = new Link(key, value, last, null);
            if (last != null)
                last.Next = l;
            last = l;
            if (first == null)
                first = l;
            Backend.Add(key, l);
        }

        protected override Link getItem(object key)
        {
            return (Link) Backend[key];
        }

        protected override void removeItem(object key)
        {
            Link l = (Link) Backend[key];
            if (l != null)
            {
                Link pre = l.Previous;
                Link nxt = l.Next;

                if (pre != null)
                    pre.Next = nxt;
                else
                    first = nxt;
                if (nxt != null)
                    nxt.Previous = pre;
                else
                    last = pre;
            }
            Backend.Remove(key);
        }

        protected override void setItem(object key, object value)
        {
            Link l = getItem(key);
            if (l != null)
                removeItem(key);
            addItem(key, value);
        }
    }

    public class LRUDictionary : UpdateLinkedDictionary
    {
        public LRUDictionary(IDictionary backend) : base(backend)
        {
        }

        protected override Link getItem(object key)
        {
            Link l = (Link) Backend[key];
            if (l == null)
                return null;
            Link nxt = l.Next;
            if (nxt != null) // last => no-change
            {
                updates++; // looking is updating
                // note, atleast 2 items in chain now, since l != last
                Link pre = l.Previous;
                if (pre == null)
                    first = nxt;
                else
                    pre.Next = l.Next;
                nxt.Previous = pre; // nxt != null since l != last
                last.Next = l;
                l.Next = null;
                l.Previous = last;
                last = l;
            }
            return l;
        }
    }

    public class MRUDictionary : LRUDictionary
    {
        public MRUDictionary(IDictionary backend) : base(backend)
        {
        }

        public override ICollection Keys
        {
            get { return KeysBackward; }
        }

        public override ICollection Values
        {
            get { return ValuesBackward; }
        }

        public override IDictionaryEnumerator GetEnumerator()
        {
            return GetEnumeratorForward();
        }
    }
}