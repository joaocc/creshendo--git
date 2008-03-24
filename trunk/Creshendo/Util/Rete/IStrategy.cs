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
    /// *
    /// Strategy defines the basic methods needed to implement conflict
    /// resolution strategy. In terms of rule engines, Forgy's definition
    /// of conflict resolution strategy using an agenda is the pre-
    /// dominant approach used by RETE, TREAT, LEAPS rule engines.
    /// 
    /// </author>
    public interface IStrategy
    {
        String Name { get; }

        /// <summary> Strategies that sort activations as they are added to the activation
        /// list should implement this method. Strategies that prioritize in a
        /// lazy fashion should implement this method and nextActivation. Lazy
        /// prioritization will should simply Add the activation to the list.
        /// </summary>
        /// <param name="">theModule
        /// </param>
        /// <param name="">newActivation
        /// 
        /// </param>
        void addActivation(IActivationList thelist, IActivation newActivation);

        /// <summary> Strategies that implement lazy prioritization need to compare the
        /// activations in the list and return the correct activation. Strategies
        /// that aren't lazy should just return the first item in the activation
        /// list.
        /// </summary>
        /// <param name="">theModule
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        IActivation nextActivation(IActivationList thelist);

        /// <summary> Compare 2 activations. The return value is similar to Comparable
        /// interface.
        /// -1 less than
        /// 0 equal to
        /// 1 greater than
        /// </summary>
        /// <param name="">left
        /// </param>
        /// <param name="">right
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        int compare(IActivation left, IActivation right);
    }
}