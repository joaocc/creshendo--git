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

using System.Collections;
using Creshendo.Util.Rete;
using Creshendo.Util.Rete.Compiler;

namespace Creshendo.Util.Rule
{
    /// <author>  Peter Lin
    /// *
    /// Conditions are patterns. It may be a simple fact pattern, test function,
    /// or an object pattern.
    /// 
    /// </author>
    public interface ICondition : IPrettyPrint
    {
        /// <summary> Get the nodes associated with the condition. In the case of
        /// TestConditions, it should only be 1 node.
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        IList Nodes { get; }

        /// <summary> Get the last node in the Condition
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        BaseNode LastNode { get; }

        /// <summary> Get the bind Constraint org.jamocha.rete.util.IList including BoundConstraint (isObjectBinding==false) 
        /// and PredicateConstraint (isPredicateJoin==true)
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        IList BindConstraints { get; }

        /// <summary> Method is used to compare the pattern to another pattern and
        /// determine if they are equal.
        /// </summary>
        /// <param name="">cond
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        bool compare(ICondition cond);

        /// <summary> When the rule is compiled, we Add the node to the condition,
        /// so that we can print out the matches for a given rule.
        /// </summary>
        /// <param name="">node
        /// 
        /// </param>
        void addNode(BaseNode node);

        /// <summary> if the rule's alpha nodes aren't shared, this method is
        /// used to Add the alphaNodes to the condition
        /// </summary>
        /// <param name="">node
        /// 
        /// </param>
        void addNewAlphaNodes(BaseNode node);

        /// <summary> Clear the condition
        /// </summary>
        void clear();

        /// <summary> obtain the compiler compile this condition
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        IConditionCompiler getCompiler(IRuleCompiler ruleCompiler);
    }
}