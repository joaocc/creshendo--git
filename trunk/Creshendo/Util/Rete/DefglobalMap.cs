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
using Creshendo.Util.Collections;

namespace Creshendo.Util.Rete
{
    /// <summary> 
    /// </summary>
    /// <author>  Peter Lin
    /// 
    /// The purpose of DefglobalMap is to centralize the handling of defglobals
    /// in a convienant class that can be serialized easily from one engine
    /// to another.
    /// 
    /// </author>
    [Serializable]
    public class DefglobalMap
    {
        /// <summary> later on we should replace this and have it
        /// </summary>
        //UPGRADE_NOTE: The initialization of  'variables' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
        private IGenericMap<object, object> variables;

        public DefglobalMap() 
        {
            InitBlock();
        }

        private void InitBlock()
        {
            variables = CollectionFactory.newHashMap();
        }

        /// <summary> The current implementation doesn't check and simply puts the
        /// new defglobal into the underlying HashMap
        /// </summary>
        /// <param name="">name
        /// </param>
        /// <param name="">value
        /// 
        /// </param>
        public virtual void declareDefglobal(String name, Object value_Renamed)
        {
            variables.Put(name, value_Renamed);
        }

        /// <summary> The current implementation calls HashMap.Get(key). if the key
        /// and value aren't in the HashMap, it returns null.
        /// </summary>
        /// <param name="">name
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public virtual Object getValue(String name)
        {
            return variables.Get(name);
        }

        /// <summary> Convienance method for iterating over the entries in the HashMap
        /// and printing it out. The implementation prints the String key and
        /// calls Object.toString() for the value.
        /// </summary>
        /// <param name="">engine
        /// 
        /// </param>
        public virtual void printDefglobals(Rete engine)
        {
            IEnumerator itr = variables.Keys.GetEnumerator();
            while (itr.MoveNext())
            {
                String key = (String) itr.Current;
                Object val = variables.Get(key);
                //UPGRADE_TODO: The equivalent in .NET for method 'java.Object.toString' may return a different value. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1043"'
                engine.writeMessage(key + "=" + val.ToString());
            }
        }
    }
}