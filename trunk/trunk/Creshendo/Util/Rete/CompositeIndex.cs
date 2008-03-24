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

namespace Creshendo.Util.Rete
{
    /// <author>  Peter Lin
    /// 
    /// CompositeIndex is used by ObjectTypeNodes to hash AlphaNodes and stick them
    /// in a HashTable. This should improve the performance over the proof-of-concept
    /// implementation using Strings.
    /// 
    /// </author>
    [Serializable]
    public class CompositeIndex
    {
        private int hashCode_Renamed_Field;
        private String name = null;

        // by default, we set it to Equals
        //UPGRADE_NOTE: The initialization of  'operator_Renamed' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
        private int operator_Renamed;

        private Object value_Renamed = null;

        /// <summary> 
        /// </summary>
        public CompositeIndex(String name, int operator_Renamed, Object value_Renamed) 
        {
            InitBlock();
            this.name = name;
            this.operator_Renamed = operator_Renamed;
            this.value_Renamed = value_Renamed;
            calculateHash();
        }

        private void InitBlock()
        {
            operator_Renamed = Constants.EQUAL;
        }

        protected internal virtual void calculateHash()
        {
            hashCode_Renamed_Field = name.GetHashCode() + operator_Renamed + value_Renamed.GetHashCode();
        }

        public override bool Equals(Object val)
        {
            if (this == val)
            {
                return true;
            }
            if (val == null || GetType() != val.GetType())
            {
                return false;
            }

            CompositeIndex ci = (CompositeIndex) val;
            return ci.name.Equals(name) && ci.operator_Renamed == operator_Renamed && ci.value_Renamed.Equals(value_Renamed);
        }

        public override int GetHashCode()
        {
            return hashCode_Renamed_Field;
        }

        public virtual String toPPString()
        {
            return name + ":" + operator_Renamed + ":" + value_Renamed.ToString();
        }
    }
}