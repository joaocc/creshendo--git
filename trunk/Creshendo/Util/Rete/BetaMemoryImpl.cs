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
using System.Text;
using Creshendo.Util.Collections;

namespace Creshendo.Util.Rete
{
    /// <author>  Peter Lin
    /// *
    /// BetaMemory stores the matches
    /// 
    /// </author>
    public class BetaMemoryImpl : IBetaMemory
    {
        protected internal Index index = null;

        //UPGRADE_NOTE: The initialization of  'matches' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
        protected internal IGenericMap<object, object> matches;

        /// <summary> 
        /// </summary>
        public BetaMemoryImpl(Index index) 
        {
            InitBlock();
            this.index = index;
        }

        #region BetaMemory Members

        /// <summary> Return the index of the beta memory
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public virtual Index Index
        {
            get { return index; }
        }

        /// <summary> Get the array of facts
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public virtual IFact[] LeftFacts
        {
            get { return index.Facts; }
        }


        /// <summary> Return the array containing the facts entering
        /// the right input that matched
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public virtual IEnumerator iterateRightFacts()
        {
            return matches.Keys.GetEnumerator();
        }

        /// <summary> The method will check to see if the fact has
        /// previously matched
        /// </summary>
        /// <param name="">rightfacts
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public virtual bool matched(IFact rightfact)
        {
            return matches.ContainsKey(rightfact);
        }

        /// <summary> Add a match to the list
        /// </summary>
        /// <param name="">rightfacts
        /// 
        /// </param>
        public virtual void addMatch(IFact rightfact)
        {
            matches.Put(rightfact, null);
        }

        public virtual void removeMatch(IFact rightfact)
        {
            matches.Remove(rightfact);
        }

        /// <summary> Clear will Clear the memory
        /// </summary>
        public virtual void clear()
        {
            matches.Clear();
            index = null;
        }

        /// <summary> method simply returns the Count
        /// </summary>
        public virtual int matchCount()
        {
            return matches.Count;
        }

        /// <summary> The implementation will append the facts for the left followed
        /// by double colon "::" and then the matches from the right
        /// </summary>
        public virtual String toPPString()
        {
            StringBuilder buf = new StringBuilder();
            for (int idx = 0; idx < index.Facts.Length; idx++)
            {
                if (idx > 0)
                {
                    buf.Append(", ");
                }
                buf.Append(index.Facts[idx].FactId);
            }
            buf.Append(": ");
            IEnumerator itr = matches.Keys.GetEnumerator();
            while (itr.MoveNext())
            {
                IFact f = (IFact) itr.Current;
                buf.Append(f.FactId + ", ");
            }
            return buf.ToString();
        }

        #endregion

        private void InitBlock()
        {
            matches = CollectionFactory.newMap();
        }
    }
}