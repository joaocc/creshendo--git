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
using System.Collections.Generic;

namespace Creshendo.Util.Rete
{
    /// <author>  Peter Lin
    /// *
    /// Interface for alpha memory. Alpha memories are used to remember
    /// which facts entered and match for alpha nodes.
    /// 
    /// </author>
    public interface IAlphaMemory
    {
        /// <summary>
        /// Add a partial match to the memory
        /// </summary>
        /// <param name="fact">The fact.</param>
        void addPartialMatch(IFact fact);

        /// <summary>
        /// Clear the alpha memory for the node
        /// </summary>
        void clear();

        /// <summary>
        /// Remove a partial match from the memory
        /// </summary>
        /// <param name="fact">The fact.</param>
        /// <returns></returns>
        IFact removePartialMatch(IFact fact);

        /// <summary>
        /// Count returns the number of matches
        /// </summary>
        /// <returns></returns>
        int size();

        /// <summary>
        /// Return an GetEnumerator to iterate over the matches.
        /// </summary>
        /// <returns></returns>
        IEnumerator<IFact> GetEnumerator();
    }
}