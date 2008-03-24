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

namespace Creshendo.Util.Rete.Strategies
{
    /// <author>  Peter Lin
    /// *
    /// 
    /// 
    /// </author>
    [Serializable]
    public class RecencyStrategy : IStrategy
    {
        /// <summary> 
        /// </summary>
        public RecencyStrategy() 
        {
        }

        #region Strategy Members

        public virtual String Name
        {
            get { return "recency"; }
        }


        /// <summary> Current implementation will check which order the list is and call
        /// the appropriate method
        /// </summary>
        public virtual void addActivation(IActivationList thelist, IActivation newActivation)
        {
            thelist.addActivation(newActivation);
        }

        /// <summary> Current implementation will check which order the list is and call
        /// the appropriate method
        /// </summary>
        public virtual IActivation nextActivation(IActivationList thelist)
        {
            return thelist.nextActivation();
        }

        /// <summary> The method first compares the salience. If the salience is equal,
        /// we then compare the aggregate time.
        /// </summary>
        /// <param name="">left
        /// </param>
        /// <param name="">right
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public virtual int compare(IActivation left, IActivation right)
        {
            if (right != null)
            {
                if (left.Rule.Salience == right.Rule.Salience)
                {
                    // we compare the facts based on how recent it is
                    return compareRecency(left, right);
                }
                else
                {
                    if (left.Rule.Salience > right.Rule.Salience)
                    {
                        return 1;
                    }
                    else
                    {
                        return - 1;
                    }
                }
            }
            else
            {
                return 1;
            }
        }

        #endregion

        /// <summary> compare will look to see which activation has more facts.
        /// it will first compare the timestamp of the facts. If the facts
        /// are equal, it will return the activation with more facts.
        /// </summary>
        /// <param name="">left
        /// </param>
        /// <param name="">right
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        protected internal virtual int compareRecency(IActivation left, IActivation right)
        {
            IFact[] lfacts = left.Facts;
            IFact[] rfacts = right.Facts;
            int len = lfacts.Length;
            int compare = 0;
            if (rfacts.Length < len)
            {
                len = rfacts.Length;
            }
            // first we compare the time stamp
            for (int idx = 0; idx < len; idx++)
            {
                if (lfacts[idx].timeStamp() > rfacts[idx].timeStamp())
                {
                    return 1;
                }
                else if (lfacts[idx].timeStamp() < rfacts[idx].timeStamp())
                {
                    return - 1;
                }
            }
            // the activation with more facts has a higher priority
            if (lfacts.Length > rfacts.Length)
            {
                return 1;
            }
            else if (lfacts.Length < rfacts.Length)
            {
                return - 1;
            }
            // Current we compare the fact id
            for (int idx = 0; idx < len; idx++)
            {
                if (lfacts[idx].FactId > rfacts[idx].FactId)
                {
                    return 1;
                }
                else if (lfacts[idx].FactId < rfacts[idx].FactId)
                {
                    return - 1;
                }
            }
            return 0;
        }
    }
}