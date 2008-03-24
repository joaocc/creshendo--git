using System;
using System.Collections.Generic;
using System.Text;

namespace Creshendo.Util.Collections
{
    public interface IHashMapEntry<K,V>
    {
        int Hash{ get;}
        IHashMapEntry<K,V> Next{ get; set;}
        K Key { get;}
        V Value{ get; set;}
    }
}
