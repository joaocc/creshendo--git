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
    /// Index is used for the Alpha and BetaMemories for the index. Instead of the
    /// original quick and dirty String index implementation, this implementation
    /// takes an Array of Fact[] objects and calculates the hash.
    /// The class overrides GetHashCode and Equals(Object) so that it works correctly
    /// as the key for HashMaps. There is an unit test called IndexTest in the
    /// test directory.
    /// 
    /// The implementation for now is very simple. Later on, we may need to update
    /// it and make sure it works for memory snapshots and other features.
    /// 
    /// </author>
    [Serializable]
    public sealed class Index : IHashIndex
    {
        private IFact[] _facts;

        private int _hash;

        /// <summary>
        /// Initializes a new instance of the <see cref="Index"/> class.
        /// </summary>
        /// <param name="facts">The facts.</param>
        public Index(IFact[] facts) 
        {
            this._facts = facts;
            calculateHash();
        }

        /// <summary>
        /// constructor takes facts and a new hashcode. it expects the
        /// hashcode to be correct.
        /// </summary>
        /// <param name="facts">The facts.</param>
        /// <param name="hashCode">The hash code.</param>
        private Index(IFact[] facts, int hashCode) 
        {
            this._facts = facts;
            _hash = hashCode;
        }

        /// <summary>
        /// this method should be refactored,so that we couldn't change the value of the memeber vairable facts
        /// Houzhanbin 10/25/2007
        /// </summary>
        /// <value>The facts.</value>
        /// <returns>
        /// </returns>
        protected internal IFact[] Facts
        {
            get { return _facts; }
        }

        #region HashIndex Members

        /// <summary>
        /// The implementation is very close to Drools FactHandleList implemented
        /// by simon. The main difference is that Drools uses interfaces and
        /// Sumatra doesn't. I don't see a need to abstract this out to an
        /// interface, since no one other than an experience rule engine
        /// developer would be writing a new Index class. And even then, it only
        /// makes sense to replace the implementation. Having multiple index
        /// implementations doesn't really make sense.
        /// </summary>
        /// <param name="val">The val.</param>
        /// <returns></returns>
        public override bool Equals(Object val)
        {
            if (this == val)
            {
                return true;
            }
            // the class will only be an Index instance,
            // so we don't have to check the class type
            if (val == null)
            {
                return false;
            }

            Index other = val as Index;

            if (other == null)
                return false;

            if (_facts.Length != other._facts.Length)
                return false;

            for (int i = 0; i < _facts.Length; i++)
            {
                if (_facts[i] != other._facts[i])
                {
                    return false;
                }
            }
            return true;
            //return Arrays.equals(_facts, ((Index) val)._facts);
        }

        /// <summary>
        /// Method simply returns the cached GetHashCode.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override int GetHashCode()
        {
            return _hash;
        }

        /// <summary>
        /// Return a pretty print formatted String.
        /// </summary>
        /// <returns></returns>
        public String toPPString()
        {
            StringBuilder buf = new StringBuilder();
            for (int idx = 0; idx < _facts.Length; idx++)
            {
                if (idx > 0)
                {
                    buf.Append(",");
                }
                buf.Append(_facts[idx].FactId);
            }
            return buf.ToString();
        }

        #endregion

        /// <summary>
        /// This is a very simple implementation that basically adds the hashCodes
        /// of the Facts in the array.
        /// </summary>
        private void calculateHash()
        {
            int hash = 0;
            for (int idx = 0; idx < _facts.Length; idx++)
            {
                hash += _facts[idx].GetHashCode();
            }
            _hash = hash;
        }

        /// <summary>
        /// Clear the index
        /// </summary>
        public void clear()
        {
            _facts = null;
            _hash = 0;
        }

        /// <summary>
        /// Adds the specified fact.
        /// </summary>
        /// <param name="fact">The fact.</param>
        /// <returns></returns>
        public Index add(IFact fact)
        {
            IFact[] facts1 = new IFact[this._facts.Length + 1];
            Array.Copy(this._facts, 0, facts1, 0, this._facts.Length);
            facts1[this._facts.Length] = fact;
            return new Index(facts1, _hash + fact.GetHashCode());
        }

        ///// <summary>
        ///// Adds the specified fact.
        ///// </summary>
        ///// <param name="fact">The fact.</param>
        ///// <returns></returns>
        //public Index add(IFact fact)
        //{
        //    int factsLength = this._facts.Length;
        //    Array.Resize<IFact>(ref this._facts, factsLength + 1);
        //    this._facts[factsLength] = fact;
        //    this._hash = _hash + fact.GetHashCode();
        //    return this;
        //}

        /// <summary>
        /// Adds all.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public Index addAll(Index index)
        {
            IFact[] facts1 = new IFact[this._facts.Length + index._facts.Length];
            Array.Copy(this._facts, 0, facts1, 0, this._facts.Length);
            Array.Copy(index._facts, 0, facts1, this._facts.Length, index._facts.Length);
            return new Index(facts1, _hash + index._hash);
        }
    }
}