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

namespace Creshendo.Util.Rete
{
    /// <author>  Peter Lin
    /// *
    /// Basic interface for BetaMemory. I created the interface after I coded up
    /// the implementation, so the interface has the important methods. Hopefully
    /// I won't need to change this interface much in the future.
    /// 
    /// </author>
    public interface IBetaMemory
    {
        /// <summary> Get the index for the beta memory.
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        Index Index { get; }

        /// <summary> classes implementing the interface should Get the
        /// Fact[] from the index
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        IFact[] LeftFacts { get; }

        /// <summary> Clear the beta memory
        /// </summary>
        void clear();

        /// <summary> Get the facts that match from the right side
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        IEnumerator iterateRightFacts();

        /// <summary> Get the match count
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        int matchCount();

        /// <summary> check if a fact already matched
        /// </summary>
        /// <param name="">rightfact
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        bool matched(IFact rightfact);

        /// <summary> Add a match
        /// </summary>
        /// <param name="">rightfact
        /// 
        /// </param>
        void addMatch(IFact rightfact);

        /// <summary> Remove a matched fact
        /// </summary>
        /// <param name="">rightfact
        /// 
        /// </param>
        void removeMatch(IFact rightfact);

        /// <summary> the implementing class needs to decide to format the
        /// matches in the beta memory
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        String toPPString();
    }
}