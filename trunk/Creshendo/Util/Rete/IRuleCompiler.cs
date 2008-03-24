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
namespace Creshendo.Util.Rete
{
    /// <author>  Peter Lin
    /// *
    /// The purpose of a RuleCompiler is to convert a Rule object into
    /// the appropriate RETE network. We have a generic interface, so
    /// that others can implement their own RuleCompiler.
    /// 
    /// </author>
    public interface IRuleCompiler
    {
        IWorkingMemory WorkingMemory { set; }

        /// <summary>
        /// return whether the rule compiler is set to validate the rule
        /// before compiling it.
        /// </summary>
        /// <value><c>true</c> if [validate rule]; otherwise, <c>false</c>.</value>
        /// <returns>
        /// </returns>
        bool ValidateRule { get; set; }

        /// <summary>
        /// A rule can be added dynamically at runtime to an existing
        /// engine. If the engine wasn't able to Add the rule, it
        /// will throw an exception.
        /// </summary>
        /// <param name="rule">The rule.</param>
        /// <returns></returns>
        bool addRule(Rule.IRule rule);

        /// <summary>
        /// Add a new ObjectTypeNode to the network
        /// </summary>
        /// <param name="node">The node.</param>
        void addObjectTypeNode(ObjectTypeNode node);

        /// <summary>
        /// Remove an ObjectTypeNode from the network. This should be
        /// when the rule engine isn't running. When an ObjectTypeNode
        /// is removed, all nodes and rules using the ObjectTypeNode
        /// need to be removed.
        /// </summary>
        /// <param name="node">The node.</param>
        void removeObjectTypeNode(ObjectTypeNode node);

        /// <summary>
        /// Look up the ObjectTypeNode using the Template
        /// </summary>
        /// <param name="template">The template.</param>
        /// <returns></returns>
        ObjectTypeNode getObjectTypeNode(ITemplate template);

        event CompileEventHandler RuleAdded;
        event CompileEventHandler RuleRemoved;
        event CompileEventHandler CompileError;
    }
}