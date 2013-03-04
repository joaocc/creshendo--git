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
using Creshendo.Util.Collections;
using Creshendo.Util.Rule;

namespace Creshendo.Util.Rete
{
    /// <author>  Peter Lin
    /// *
    /// A module represents a set of rulesets. The concept is from CLIPS and provides
    /// a way to isolate the rule activation and pattern matching.
    /// 
    /// </author>
    [Serializable]
    public class Defmodule : IModule
    {
        /// <summary> A simple Creshendo.rete.util.IList of the activations for the given module
        /// </summary>
        protected internal IActivationList activations = null;

        /// <summary> The key is either the template name if it was created
        /// from the shell, or the defclass if it was created from
        /// an Object.
        /// </summary>
        protected internal IGenericMap<object, object> deftemplates;

        protected internal int id;

        /// <summary> The name of the module. A rule engine may have one or
        /// more modules with rules loaded
        /// </summary>
        protected internal String name = null;

        /// <summary> A simple list of the rules in this module. Before an
        /// activation is added to the module, the class should
        /// check to see if the rule is in the module first.
        /// </summary>
        protected internal IGenericMap<object, object> rules;

        private int templateCount = 0;

        /// <summary> 
        /// </summary>
        public Defmodule(String name) 
        {
            InitBlock();
            this.name = name;
            // activations = new ArrayActivationList(strat);
            activations = new LinkedActivationList();
        }

        #region Module Members

        /// <summary> Return all the activations within the module
        /// </summary>
        public virtual IActivationList AllActivations
        {
            get
            {
                if (activations.size() == 0)
                    return activations;
                return activations.cloneActivationList();
            }
        }

        /// <summary> When the focus is changed, fireActivations should be
        /// called to make sure any activations in the module are
        /// processed.
        /// </summary>
        public virtual int ActivationCount
        {
            get
            {
                lock (this)
                {
                    return activations.size();
                }
            }
        }

        /// <summary> Return the name of the module
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public virtual String ModuleName
        {
            get { return name; }
        }

        /// <summary> implementation returns the Values of the HashMap
        /// </summary>
        public virtual System.Collections.Generic.ICollection<object> AllRules
        {
            get { return rules.Values; }
        }

        /// <summary> implementation returns HashMap.Count()
        /// </summary>
        public virtual int RuleCount
        {
            get { return rules.Count; }
        }

        /// <summary> Method returns the entrySet of the HashMap containing the
        /// Deftemplates. Because of how we map the deftemplates, the
        /// number of entries will not correspond to the number of
        /// actual deftemplates
        /// </summary>
        public virtual System.Collections.Generic.ICollection<object> Templates
        {
            get { return deftemplates.Values; }
        }

        public virtual int TemplateCount
        {
            get { return templateCount; }
        }

        /// <summary> Call the method with true to turn on lazy agenda. Call with
        /// false to turn it off.
        /// </summary>
        public virtual bool Lazy
        {
            set { activations.Lazy = value; }
        }

        public virtual IStrategy Strategy
        {
            get { return activations.Strategy; }

            set { activations.Strategy = value; }
        }


        /// <summary> The method should Get the agenda and use it to Add the new
        /// activation to the agenda
        /// </summary>
        /// <param name="">actv
        /// 
        /// </param>
        public virtual void addActivation(IActivation actv)
        {
            activations.addActivation(actv);
        }

        /// <summary> Remove an activation from the list
        /// </summary>
        /// <param name="">actv
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        public virtual IActivation removeActivation(IActivation actv)
        {
            return (IActivation) activations.removeActivation(actv);
        }

        /// <summary> The current implementation will Remove the first activation
        /// and return it. If there's no more activations, the method
        /// return null;
        /// </summary>
        public virtual IActivation nextActivation(Rete engine)
        {
            IActivation act = activations.nextActivation();
            if (act is LinkedActivation)
            {
                ((LinkedActivation) act).remove(engine);
            }
            return act;
        }


        /// <summary> When Clear is called, the module needs to Clear all the internal lists
        /// for rules and activations. The handle to Rete should not be nulled.
        /// </summary>
        public virtual void clear()
        {
            activations.clear();
            IEnumerator itr = rules.Values.GetEnumerator();
            while (itr.MoveNext())
            {
                Defrule rl = (Defrule) itr.Current;
                rl.clear();
            }
            rules.Clear();
            deftemplates.Clear();
        }

        /// <summary> Add a compiled rule to the module
        /// </summary>
        public virtual void addRule(Rule.IRule rl)
        {
            rules.Put(rl.Name, rl);
        }

        /// <summary> Remove a rule from this module
        /// </summary>
        public virtual void removeRule(Rule.IRule rl, Rete engine, IWorkingMemory mem)
        {
            rules.Remove(rl.Name);
            // we should iterate over the nodes of the rule and Remove
            // them if they are not shared
            ICondition[] cnds = rl.Conditions;
            // first Remove the alpha nodes
            for (int idx = 0; idx < cnds.Length; idx++)
            {
                ICondition cnd = cnds[idx];
                if (cnd is ObjectCondition)
                {
                    ObjectCondition oc = (ObjectCondition) cnd;
                    String templ = oc.TemplateName;
                    Deftemplate temp = (Deftemplate) deftemplates.Get(templ);
                    ObjectTypeNode otn = mem.RuleCompiler.getObjectTypeNode(temp);
                    removeAlphaNodes(oc.Nodes, otn);
                }
            }
            // now Remove the betaNodes, since the engine currently
            // doesn't share the betaNodes, we can just Remove it
            IList bjl = rl.Joins;

            for (int idx = 0; idx < bjl.Count; idx++)
            {
                BaseJoin bjoin = (BaseJoin) bjl[idx];
                ICondition cnd = cnds[idx + 1];
                if (cnd is ObjectCondition)
                {
                    ObjectCondition oc = (ObjectCondition) cnd;
                    String templ = oc.TemplateName;
                    Deftemplate temp = (Deftemplate) deftemplates.Get(templ);
                    ObjectTypeNode otn = mem.RuleCompiler.getObjectTypeNode(temp);
                    otn.removeNode(bjoin);
                }
            }
        }

        /// <summary> If the module already Contains the rule, it will return true.
        /// The lookup uses the rule name, so rule names are distinct
        /// within a single module. The same rule name may be used in
        /// multiple modules.
        /// </summary>
        public virtual bool containsRule(Rule.IRule rl)
        {
            return rules.ContainsKey(rl.Name);
        }


        /// <summary> The key is either the Defclass or a string name
        /// </summary>
        public virtual bool containsTemplate(Object key)
        {
            if (key is Defclass)
            {
                Defclass dc = (Defclass) key;
                return deftemplates.ContainsKey(dc.ClassObject.FullName);
            }
            else
            {
                return deftemplates.ContainsKey(key);
            }
        }

        /// <summary> implementation looks up the template and assumes the key
        /// is the classname or the user define name.
        /// </summary>
        public virtual ITemplate getTemplate(Defclass key)
        {
            return (Deftemplate) deftemplates.Get(key.ClassObject.FullName);
        }

        public virtual ITemplate getTemplate(String key)
        {
            return (Deftemplate) deftemplates.Get(key);
        }

        /// <summary> find a parent template using the string template name
        /// </summary>
        public virtual ITemplate findParentTemplate(String key)
        {
            Deftemplate tmpl = null;
            IEnumerator itr = deftemplates.Keys.GetEnumerator();
            while (itr.MoveNext())
            {
                Object keyval = itr.Current;
                Deftemplate entry = (Deftemplate) deftemplates.Get(keyval);
                if (entry.Name.Equals(key))
                {
                    tmpl = entry;
                    break;
                }
            }
            return tmpl;
        }

        /// <summary> The implementation will use either the defclass or the
        /// template name for the key. The templates are stored in
        /// a HashMap.
        /// </summary>
        public virtual void addTemplate(ITemplate temp, Rete engine, IWorkingMemory mem)
        {
            if (!deftemplates.ContainsKey(temp.Name))
            {
                // we have to set the template's module
                if (temp.ClassName != null)
                {
                    deftemplates.Put(temp.Name, temp);
                    deftemplates.Put(temp.ClassName, temp);
                    templateCount++;
                }
                else
                {
                    deftemplates.Put(temp.Name, temp);
                    templateCount++;
                }
                ObjectTypeNode otn = new ObjectTypeNode(engine.nextNodeId(), temp);
                mem.RuleCompiler.addObjectTypeNode(otn);
            }
        }

        /// <summary> implementation will Remove the template from the HashMap
        /// and it will Remove the ObjectTypeNode from the network.
        /// </summary>
        public virtual void removeTemplate(ITemplate temp, Rete engine, IWorkingMemory mem)
        {
            deftemplates.Remove(temp.Name);
            if (temp.ClassName != null)
            {
                deftemplates.Remove(temp.ClassName);
            }
            ObjectTypeNode otn = mem.RuleCompiler.getObjectTypeNode(temp);
            mem.RuleCompiler.removeObjectTypeNode(otn);
        }


        /// <summary> implementation looks up the rule in the HashMap
        /// </summary>
        public virtual Rule.IRule findRule(String name)
        {
            return (Rule.IRule) rules.Get(name);
        }

        #endregion

        private void InitBlock()
        {
            rules = CollectionFactory.localMap();
            deftemplates = CollectionFactory.localMap();
        }

        protected internal virtual void removeAlphaNodes(IList nodes, ObjectTypeNode otn)
        {
            BaseNode prev = otn;
            for (int idx = 0; idx < nodes.Count; idx++)
            {
                BaseNode node = (BaseNode) nodes[idx];
                if (node.useCount > 1)
                {
                    node.decrementUseCount();
                }
                else
                {
                    prev.removeNode(node);
                }
                prev = node;
            }
        }
    }
}