using System;
using System.Collections.Generic;

namespace Creshendo.Util
{
    //public class GenericHashmap<K, V> : Dictionary<K, V>, IGenericMap<K, V>
    //{
    //    public GenericHashmap() : base(101)
    //    {
    //    }

    //    #region IGenericMap<K,V> Members

    //    public bool Empty
    //    {
    //        get { return Count == 0; }
    //    }

    //    public V Get(K key)
    //    {
    //        return this[key];
    //    }

    //    public void Put(K key, V val)
    //    {
    //        this[key] = val;
    //    }

    //    public V RemoveWithReturn(K key)
    //    {
    //        V ret = this[key];
    //        Remove(key);
    //        return ret;
    //    }

    //    public void putAll(IGenericMap<K, V> methods)
    //    {
    //        foreach (K key in methods.Keys)
    //        {
    //            Add(key, methods.Get(key));
    //        }
    //    }

    //    public new bool ContainsKey(K key)
    //    {
    //        try
    //        {
    //            return base.ContainsKey(key);
    //        }
    //        catch (ArgumentNullException)
    //        {
    //            return false;
    //        }
    //    }

    //    #endregion
    //}
}