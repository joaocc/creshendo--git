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
    /// 
    /// A basic HashMap implementation inspired by Mark's HashTable for
    /// Drools3. The main difference is this HashMap tries to be compatable
    /// with java.util.HashMap. This means it's not as stripped down as
    /// the super optimized HashTable mark wrote for drools. Hopefully the
    /// extra stuff doesn't make too big of a difference.
    /// 
    /// </author>
    public class HashMap : Hashtable, IMap
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HashMap"/> class.
        /// </summary>
        public HashMap() : base(101, 0.75f)
        {
        }

        #region IMap Members

        public bool Empty
        {
            get { return Count == 0; }
        }

        public bool ContainsKey(Object key)
        {
            if (key == null)
                return false;

            return Contains(key);
        }

        public Object Get(Object key)
        {
            return this[key];
        }

        public void Put(Object key, Object val)
        {
            this[key] = val;
        }

        public Object RemoveWithReturn(Object key)
        {
            Object ret = this[key];
            Remove(key);
            return ret;
        }


        public void putAll(IMap methods)
        {
            foreach (Object key in methods.Keys)
            {
                Add(key, methods.Get(key));
            }
        }

        public IEnumerator GetEnumerator()
        {
            return base.GetEnumerator();
        }

        #endregion
    }

    #region EqualityEquals

    public class EqualityEquals : IEqualityComparer
    {
        #region IEqualityComparer Members

        public bool Equals(object x, object y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(object obj)
        {
            return (obj.GetHashCode());
            //return rehash(obj.GetHashCode());
        }

        #endregion

        private int rehash(int h)
        {
            h += ~(h << 9);
            h ^= (URShift(h, 14));
            h += (h << 4);
            h ^= (URShift(h, 10));
            return h;
        }

        private int URShift(int number, int bits)
        {
            if (number >= 0)
                return number >> bits;
            else
                return (number >> bits) + (2 << ~bits);
        }
    }

    #endregion
}