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
using Creshendo.Util.Rete;

namespace Creshendo.Util.Rule
{
    /// <author>  Peter Lin
    /// *
    /// The rule interface design is influenced by RuleML 0.8. It is also
    /// influenced by CLIPS, but with some important differences. The 
    /// interface assumes it acts as a bridge between a Rule Parser, which 
    /// parses some text and produces the necessary artifacts and a rule
    /// compiler which generates RETE nodes.
    /// For that reason, the interface defines methods for adding Join
    /// nodes and retrieving the last node in the rule. These convienance
    /// method are present to make it easier to write rule parsers and
    /// compilers.
    /// 
    /// </author>
    public interface IRule : IScope
    {
        /// <summary> if the rule is set to autofocus, it returns true
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        /// <summary> if the rule should fire even when the module is not in focus,
        /// call the method with true
        /// </summary>
        /// <param name="">auto
        /// 
        /// </param>
        bool AutoFocus { get; set; }

        /// <summary> if users want to give a rule a comment, the method will return it.
        /// otherwise it should return zero length string
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        /// <summary> set the comment of the rule. it should be a descriptive comment
        /// about what the rule does.
        /// </summary>
        /// <param name="">text
        /// 
        /// </param>
        String Comment { get; set; }

        /// <summary> Get the complexity of the rule, which measure how many conditions
        /// a rule has
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        /// <summary> set the complexity of the rule. this should be calculated
        /// by the rule engine or rule compiler
        /// </summary>
        /// <param name="">complexity
        /// 
        /// </param>
        IComplexity Complexity { get; set; }

        /// <summary> return the effective date in milliseconds
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        /// <summary> Be default classes implementing the interface should set
        /// the effective date to zero. only when the user sets the
        /// date should it have a non-zero positive long value.
        /// </summary>
        /// <param name="">mstime
        /// 
        /// </param>
        long EffectiveDate { get; set; }

        /// <summary> return the expiration date in milliseconds
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        /// <summary> by default classes implementing the interface should set
        /// the expiration date to zero. only when the user sets the
        /// date should it have a non-zero positive value greater
        /// than the effective date.
        /// </summary>
        /// <param name="">mstime
        /// 
        /// </param>
        long ExpirationDate { get; set; }

        /// <summary> Add a conditional element to the rule
        /// </summary>
        /// <param name="">cond
        /// 
        /// </param>
        /// <summary> Get the name of the rule
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        /// <summary> set the name of the rule
        /// </summary>
        /// <param name="">name
        /// 
        /// </param>
        String Name { get; set; }

        /// <summary> if the rule should skip the agenda and fire immediately,
        /// the method returns true. By default it should be false
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        /// <summary> if a rule should skip the agenda, set it to true
        /// </summary>
        /// <param name="">agenda
        /// 
        /// </param>
        bool NoAgenda { get; set; }

        /// <summary> classes implementing the interface can choose to ignore
        /// this rule property. Sumatra currently provides the ability
        /// to turn off AlphaMemory. By default, it is set to true.
        /// If a user wants to turn off AlphaMemory for a given rule,
        /// set it to false.
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        /// <summary> to turn of alpha memory, set it to false
        /// </summary>
        /// <param name="">match
        /// 
        /// </param>
        bool RememberMatch { get; set; }

        /// <summary> Get the salience of the rule
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        /// <summary> to lower the priority of a rule, set the value lower
        /// </summary>
        /// <param name="">sal
        /// 
        /// </param>
        int Salience { get; set; }

        /// <summary> the version of the rule
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        /// <summary> set the version of the rule
        /// </summary>
        /// <param name="">ver
        /// 
        /// </param>
        String Version { get; set; }

        /// <summary> watch is used for debugging
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        /// <summary> to debug a rule, set the watch to true
        /// </summary>
        /// <param name="">watch
        /// 
        /// </param>
        bool Watch { get; set; }

        /// <summary> by default a rule should return true
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        /// <summary> to turn off a rule, call the method with false
        /// </summary>
        /// <param name="">active
        /// 
        /// </param>
        bool Active { get; set; }

        /// <summary> if the rule uses temporal facts, the method should return
        /// true. the defrule must declare temporal-activation true
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        /// <summary> set the rule to use temporal activation
        /// </summary>
        /// <param name="">temporalActivation
        /// 
        /// </param>
        bool TemporalActivation { get; set; }

        ICondition[] Conditions { get; }
        IAction[] Actions { get; }
        IList Joins { get; }

        /// <summary> The method should return the last node in the rule, not counting
        /// the terminal node.
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        BaseNode LastNode { get; }

        /// <summary> Return the module the rule belongs to. A rule can only belong to a
        /// single module.
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        /// <summary> When the rule is compiled, the rule compiler needs to set the module
        /// so that the terminalNode can Add the activation to the correct
        /// activationList.
        /// </summary>
        /// <param name="">mod
        /// 
        /// </param>
        IModule Module { get; set; }

        /// <summary> Get a GetEnumerator to the Binding objects
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        System.Collections.IEnumerator BindingIterator { get; }

        /// <summary> Get a count of the Binding
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        int BindingCount { get; }

        /// <summary> In case an user wants to Get the trigger facts in the right hand
        /// side of the rule.
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        /// <summary> We need to set the trigger facts, so the rule action can look up
        /// values easily.
        /// </summary>
        /// <param name="">facts
        /// 
        /// </param>
        IFact[] TriggerFacts { get; set; }

        void addCondition(ICondition cond);
        void addAction(IAction act);
        void addJoinNode(BaseJoin node);

        /// <summary> A rule action can create local bindings, so a rule needs to provide
        /// a way to store and retrieve bindings.
        /// </summary>
        /// <param name="">key
        /// </param>
        /// <param name="">value
        /// 
        /// </param>
        void setBindingValue(Object key, Object value_Renamed);

        /// <summary> Return the value of the for the binding
        /// </summary>
        /// <param name="key">is the name of the variable
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        new Object getBindingValue(String key);

        /// <summary> Add a new binding to the rule with the variable as the key
        /// </summary>
        /// <param name="">key
        /// </param>
        /// <param name="">bind
        /// 
        /// </param>
        void addBinding(String key, Binding bind);

        /// <summary> Get the Binding object for the given key
        /// </summary>
        /// <param name="">varName
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        Binding getBinding(String varName);

        /// <summary> utility method for copying bindings
        /// </summary>
        /// <param name="">varName
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        Binding copyBinding(String varName);

        /// <summary> utility method for copying predicate bindings
        /// </summary>
        /// <param name="">varName
        /// </param>
        /// <param name="">operator
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        Binding copyPredicateBinding(String varName, int operator_Renamed);

        /// <summary> After the actions of a rule are executed, reset should be called
        /// to make sure the rule doesn't hold on to the facts.
        /// </summary>
        void resetTriggerFacts();

        /// <summary> this method needs to be called before rule compilation begins. It
        /// avoids doing multiple lookups for the corresponding template.
        /// </summary>
        /// <param name="">engine
        /// 
        /// </param>
        void resolveTemplates(Rete.Rete engine);

        /// <summary> Return a pretty print formatted string for the rule.
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        String toPPString();
    }
}