using System.Collections.Generic;
using Creshendo.Util.Rete;

namespace Creshendo.Util.Collections
{
    public interface IGenericMap<K, V> : IEnumerable<IHashMapEntry<K, V>>, IHashIndex
    {
        V this[K key] { get; set; }
        bool Empty { get; }
        int Count { get; }
        ICollection<K> Keys { get; }
        ICollection<V> Values { get; }
        bool ContainsKey(K key);
        V Get(K key);
        void Put(K arg0, V arg1);
        V RemoveWithReturn(K key);
        void putAll(IGenericMap<K, V> methods);
        void Clear();
        void Remove(K key);
    }
}