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
using System;

namespace Creshendo.Util.Rete
{
    /// <author>  Peter Lin
    /// *
    /// Basic interface for activation. There may be more than one implementation
    /// of Activation. The important thing about the activation is it knows which
    /// facts trigger a single rule.
    /// 
    /// </author>
    public interface IActivation
    {
        /// <summary> The aggregate time is the sum of the Fact timestamps
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        long AggregateTime { get; }

        /// <summary> Get the Facts that triggered the rule
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        IFact[] Facts { get; }

        /// <summary> Get the Index for the Facts
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        Index Index { get; }

        /// <summary> Get the rule that should fire
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        Rule.IRule Rule { get; }

        /// <summary>
        /// If the activation passed in the parameter has the same rule
        /// and facts, the method should return true
        /// </summary>
        /// <param name="act">The act.</param>
        /// <returns></returns>
        bool compare(IActivation act);

        /// <summary>
        /// Execute the right-hand side (aka actions) of the rule.
        /// </summary>
        /// <param name="engine">The engine.</param>
        void executeActivation(Rete engine);

        /// <summary> When watch activation is turned on, we use the method to print out
        /// the activation.
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        String toPPString();

        /// <summary> after the activation is executed, Clear has to be called.
        /// </summary>
        void clear();
    }
}