/*
* Copyright 2002-2007 Peter Lin
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
using Creshendo.Util.Collections;
using Creshendo.Util.Rete;

namespace Creshendo.Util.Collections
{
    /// <author>  Peter Lin
    /// *
    /// The purpose of this factory is to centralize the creation of Creshendo.rete.util.List,
    /// Creshendo.rete.util.IList, Collection, Set and Creshendo.rete.util.Map data structures. This is done so that
    /// we can easily drop in some other implementation, like Tangosol's
    /// Coherence product, which uses distributed HashMaps.
    /// 
    /// </author>
    public static class CollectionFactory
    {
        //protected internal static CollectionFactory factory = null;

        //protected internal CollectionFactory()
        //{
        //}

        public static IGenericMap<object, object> CustomMap
        {
            get { return new GenericHashMap<Object, Object>(); }
        }

        //public static void init()
        //{
        //    factory = new CollectionFactory();
        //}

        public static IGenericMap<IFact, IFact> newAlphaMemoryMap(string name)
        {
            return new GenericHashMap<IFact, IFact>();
        }

        public static IGenericMap<object, object> newHashedAlphaMemoryMap(string name)
        {
            return new GenericHashMap<Object, Object>();
        }

        public static IGenericMap<object, object> newLinkedHashmap(string name)
        {
            return new GenericHashMap<Object, Object>();
        }

        public static IGenericMap<object, object> newBetaMemoryMap(string name)
        {
            return new GenericHashMap<Object, Object>();
        }

        public static IGenericMap<object, object> newTerminalMap()
        {
            return new GenericHashMap<Object, Object>();
        }

        public static IGenericMap<object, object> newClusterableMap(string name)
        {
            return new GenericHashMap<Object, Object>();
        }

        public static IGenericMap<object, object> newMap()
        {
            return new GenericHashMap<Object, Object>();
        }

        public static IGenericMap<object, object> newHashMap()
        {
            return new GenericHashMap<Object, Object>();
        }

        public static IGenericMap<object, object> newNodeMap(string name)
        {
            return new GenericHashMap<Object, Object>();
        }

        /// <summary> the sole purpose of this method is to return a Creshendo.rete.util.Map that is not
        /// clustered. The other methods will return a map, but depending
        /// on the settings, they may return a Creshendo.rete.util.Map that is hooked into a
        /// JCache compliant product like Tangosol's Coherence.
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public static IGenericMap<object, object> localMap()
        {
            return new GenericHashMap<Object, Object>();
        }

        public static IGenericMap<object, object> javaHashMap()
        {
            return new GenericHashMap<Object, Object>();
        }
    }
}