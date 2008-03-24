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
using System.Collections.Generic;

namespace Creshendo.Util.Rete
{
    /// <author>  Peter Lin
    /// *
    /// Interface defining a module. A module may contain templates and
    /// rules. It is responsible for keeping track of the Activations
    /// and adding the activation to the list.
    /// 
    /// </author>
    public interface IModule
    {
        /// <summary> fireActivations will execute the Activations in the activation
        /// list. Implementing classes should make the method synchronized.
        /// </summary>
        int ActivationCount { get; }

        /// <summary> Return a list of all the activation
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        IActivationList AllActivations { get; }

        /// <summary> Return the name of the module. The interface doesn't provide
        /// any guidelines for the format, but it is a good idea to restrict
        /// names without punction marks.
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        String ModuleName { get; }

        /// <summary> Return the Deftemplates in a collection
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        ICollection<object> Templates { get; }

        /// <summary> return the number of actual deftemplates declared
        /// using deftemplate or objects
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        int TemplateCount { get; }

        /// <summary> Return a list of all the rules in this module
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        ICollection<object> AllRules { get; }

        /// <summary> Return the rule count
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        int RuleCount { get; }

        /// <summary> To use a lazy agenda, call the method with true. To turn off
        /// lazy agenda, call it with false.
        /// </summary>
        /// <param name="">lazy
        /// 
        /// </param>
        bool Lazy { set; }

        IStrategy Strategy { get; set; }

        /// <summary> Add a new activation. Classes implementing the Agenda should
        /// check to make sure the activation is new. If it isn't new,
        /// don't Add it.
        /// </summary>
        /// <param name="">actv
        /// 
        /// </param>
        void addActivation(IActivation actv);

        /// <summary> Add a new rule to the module. Implementing classes may want
        /// to check the status of the rule engine before adding new
        /// rules. In the case where rules are added dynamically at
        /// runtime, it's a good idea to check the rule engine isn't
        /// busy. A important note about this method is the rule
        /// should already be compiled to RETE nodes. If the rule isn't
        /// compiled, it will not Get evaluated.
        /// </summary>
        /// <param name="">rl
        /// 
        /// </param>
        void addRule(Rule.IRule rl);

        /// <summary> Add a new template to the module
        /// </summary>
        /// <param name="">temp
        /// 
        /// </param>
        void addTemplate(ITemplate temp, Rete engine, IWorkingMemory mem);

        /// <summary> Clear will Remove all the rules, activations, and templates
        /// from the module.
        /// </summary>
        void clear();

        /// <summary> Implementing classes need to keep a list of the rules, so
        /// that when new rules are added, the module can check to see
        /// if a rule with the same name already exists.
        /// </summary>
        /// <param name="">rl
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        bool containsRule(Rule.IRule rl);

        /// <summary> Implementing classes need to keep a list of rules, so that
        /// when a new template is declared, the module can check to see
        /// if the module already exists.
        /// </summary>
        /// <param name="">key
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        bool containsTemplate(Object key);

        /// <summary> Look up the template
        /// </summary>
        /// <param name="">key
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        ITemplate getTemplate(Defclass key);

        /// <summary> look up the template using a string template name
        /// </summary>
        /// <param name="">key
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        ITemplate getTemplate(String key);

        /// <summary> Look up the parent template by the template name
        /// </summary>
        /// <param name="">key
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        ITemplate findParentTemplate(String key);

        /// <summary> Remove an activation from the activation list.
        /// </summary>
        /// <param name="">actv
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        IActivation removeActivation(IActivation actv);

        /// <summary> Remove a rule from the module. The method returns void, since
        /// the user should have found the rule they want to Remove first.
        /// </summary>
        /// <param name="">rl
        /// 
        /// </param>
        void removeRule(Rule.IRule rl, Rete engine, IWorkingMemory mem);

        /// <summary> Remove a template from the module. The method returns void,
        /// since the user should have found the template first.
        /// </summary>
        /// <param name="">temp
        /// 
        /// </param>
        void removeTemplate(ITemplate temp, Rete engine, IWorkingMemory mem);

        /// <summary> In the event we need to find the rule, this method will look
        /// up the name of the rule without the module name. Implementing
        /// classes may take the module + rulename for the parameter, it
        /// is up to the developer.
        /// </summary>
        /// <param name="">name
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        Rule.IRule findRule(String name);

        /// <summary> Method will Remove the activation from the module and return it
        /// to the engine. The method should only be called when the RHS
        /// of the rule should be executed.
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        IActivation nextActivation(Rete engine);
    }
}