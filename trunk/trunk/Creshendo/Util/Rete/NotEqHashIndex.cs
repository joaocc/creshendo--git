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
using System.Text;

namespace Creshendo.Util.Rete
{
    /// <author>  Peter Lin<p/>
    /// *
    /// NotEqHashIndex is different from EqHashIndex is 2 ways. The first is
    /// it only uses the values of equality comparison and ignores the not
    /// equal values. The second is it takes BindValue[] instead of just
    /// Object[].
    /// 
    /// </author>
    [Serializable]
    public class NotEqHashIndex : IHashIndex
    {
        private int eqhashCode;
        private EqHashIndex negindex;
        private BindValue[] values = null;

        /// <summary> 
        /// </summary>
        public NotEqHashIndex(BindValue[] thevalues) 
        {
            values = thevalues;
            calculateHash();
        }

        /// <summary> return the subindex
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public virtual EqHashIndex SubIndex
        {
            get { return negindex; }
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
            if (val == null || !(val is NotEqHashIndex))
            {
                return false;
            }
            NotEqHashIndex eval = (NotEqHashIndex) val;
            bool eq = true;
            for (int idx = 0; idx < values.Length; idx++)
            {
                if (!values[idx].negated() && !eval.values[idx].Value.Equals(values[idx].Value))
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
            return eqhashCode;
        }

        public virtual String toPPString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append("NotEqHashIndex2: ");
            buf.Append(negindex.toPPString());
            return buf.ToString();
        }

        #endregion

        /// <summary> The implementation is different than EqHashIndex. It ignores
        /// any Bindings that are negated
        /// </summary>
        private void calculateHash()
        {
            Object[] neg = new Object[values.Length];
            int z = 0;
            if (values != null && values.Length > 0)
            {
                for (int idx = 0; idx < values.Length; idx++)
                {
                    if (values[idx] != null && !values[idx].negated())
                    {
                        eqhashCode += values[idx].Value.GetHashCode();
                    }
                    else
                    {
                        neg[z] = values[idx].Value;
                        z++;
                    }
                }
            }
            Object[] neg2 = new Object[z];
            Array.Copy(neg, 0, neg2, 0, z);
            negindex = new EqHashIndex(neg2);
            neg = null;
            neg2 = null;
        }

        public virtual void clear()
        {
            negindex.clear();
            values = null;
        }
    }
}