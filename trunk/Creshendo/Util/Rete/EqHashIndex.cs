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
    /// <author>  Peter Lin<p/>
    /// *
    /// EqHashIndex is used by the BetaNode for indexing the facts that
    /// enter from the right.
    /// 
    /// </author>
    [Serializable]
    public class EqHashIndex : IHashIndex
    {
        private int hashCode_Renamed_Field;
        private Object[] values = null;

        /// <summary> 
        /// </summary>
        public EqHashIndex(Object[] thevalues) 
        {
            values = thevalues;
            calculateHash();
        }

        #region HashIndex Members

        /// <summary> The implementation is similar to the index class.
        /// </summary>
        public override bool Equals(Object val)
        {
            if (this == val)
            {
                return true;
            }
            if (val == null || !(val is EqHashIndex))
            {
                return false;
            }
            EqHashIndex eval = (EqHashIndex) val;
            bool eq = true;
            for (int idx = 0; idx < values.Length; idx++)
            {
                if (!eval.values[idx].Equals(values[idx]))
                {
                    eq = false;
                    break;
                }
            }
            return eq;
        }

        /// <summary> Method simply returns the cached GetHashCode.
        /// </summary>
        public override int GetHashCode()
        {
            return hashCode_Renamed_Field;
        }

        public virtual String toPPString()
        {
            // TODO Auto-generated method stub
            return null;
        }

        #endregion

        /// <summary> This is a very simple implementation that gets the slot hash from
        /// the deffact.
        /// </summary>
        private void calculateHash()
        {
            if (values != null && values.Length > 0)
            {
                for (int idx = 0; idx < values.Length; idx++)
                {
                    if (values[idx] != null)
                    {
                        hashCode_Renamed_Field += values[idx].GetHashCode();
                    }
                }
            }
        }

        public virtual void clear()
        {
            values = null;
        }
    }
}