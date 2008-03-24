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
using System.Collections.Generic;
using System.Text;
using Creshendo.Util.Collections;
using Creshendo.Util.Rete;

namespace Creshendo.Util.Rule
{
    //using LinkedHashMap = java.util.LinkedHashMap;

    /// <author>  Peter Lin
    /// *
    /// A basic implementation of the Rule interface
    /// 
    /// </author>
    [Serializable]
    public class Defrule : IRule
    {
        protected internal List<Object> actions = null;

        /// <summary> by default a rule is active, unless set to false
        /// </summary>
        private bool active = true;

        protected internal bool auto = false;
        private GenericHashMap<string, Binding> bindings;
        protected internal IGenericMap<string, object> bindValues;
        private String comment = "";
        protected internal IComplexity complex = null;
        protected internal List<Object> conditions = null;

        /// <summary> Be default, the rule is set to forward chaining
        /// </summary>
        protected internal int direction;

        /// <summary> default is set to zero
        /// </summary>
        protected internal long effectiveDate = 0;

        /// <summary> default is set to zero
        /// </summary>
        protected internal long expirationDate = 0;

        protected internal List<Object> joins = null;
        protected internal String _name = null;

        /// <summary> by default noAgenda is false
        /// </summary>
        protected internal bool noAgenda = false;

        protected internal bool rememberMatch = true;
        protected internal int salience = 100;

        /// <summary> default for temporal activation is false
        /// </summary>
        protected internal bool temporalActivation = false;

        protected internal IModule themodule = null;
        protected internal IFact[] triggerFacts = null;
        protected internal String version = "";

        /// <summary> by default watch is off
        /// </summary>
        protected internal bool watch = false;

        /// <summary> 
        /// </summary>
        public Defrule()
        {
            InitBlock();
            conditions = new List<Object>();
            actions = new List<Object>();
            joins = new List<Object>();
        }

        public Defrule(String name) : this()
        {
            InitBlock();
            _name = name;
        }

        public virtual IList RuleProperties
        {
            set
            {
                IEnumerator itr = value.GetEnumerator();
                while (itr.MoveNext())
                {
                    RuleProperty declaration = (RuleProperty) itr.Current;
                    if (declaration.Name.Equals(RuleProperty.AUTO_FOCUS))
                    {
                        AutoFocus = declaration.BooleanValue;
                    }
                    else if (declaration.Name.Equals(RuleProperty.SALIENCE))
                    {
                        Salience = declaration.IntValue;
                    }
                    else if (declaration.Name.Equals(RuleProperty.VERSION))
                    {
                        Version = declaration.Value;
                    }
                    else if (declaration.Name.Equals(RuleProperty.REMEMBER_MATCH))
                    {
                        RememberMatch = declaration.BooleanValue;
                    }
                    else if (declaration.Name.Equals(RuleProperty.NO_AGENDA))
                    {
                        NoAgenda = declaration.BooleanValue;
                    }
                    else if (declaration.Name.Equals(RuleProperty.EFFECTIVE_DATE))
                    {
                        effectiveDate = getDateTime(declaration.Value);
                    }
                    else if (declaration.Name.Equals(RuleProperty.EXPIRATION_DATE))
                    {
                        expirationDate = getDateTime(declaration.Value);
                    }
                    else if (declaration.Name.Equals(RuleProperty.TEMPORAL_ACTIVATION))
                    {
                        temporalActivation = declaration.BooleanValue;
                    }
                }
            }
        }

        #region Rule Members

        public virtual String Name
        {
            /* (non-Javadoc)
			* @see woolfel.engine.rule.Rule#getName()
			*/

            get { return _name; }
            /* (non-Javadoc)
			* @see woolfel.engine.rule.Rule#setName()
			*/

            set { _name = value; }
        }

        /// <summary> by default a rule should return true
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public virtual bool Active
        {
            get { return active; }

            set { active = value; }
        }

        public virtual bool Watch
        {
            /* (non-Javadoc)
			* @see woolfel.engine.rule.Rule#getWatch()
			*/

            get { return watch; }
            /* (non-Javadoc)
			* @see woolfel.engine.rule.Rule#setWatch(boolean)
			*/

            set { watch = value; }
        }

        public virtual bool AutoFocus
        {
            get { return auto; }

            set { auto = value; }
        }

        public virtual int Salience
        {
            get { return salience; }

            set { salience = value; }
        }

        public virtual String Comment
        {
            get { return comment; }

            set { comment = value.Substring(1, (value.Length - 1) - (1)); }
        }

        public virtual IComplexity Complexity
        {
            get { return complex; }

            set { complex = value; }
        }

        public virtual long EffectiveDate
        {
            get { return effectiveDate; }

            set { effectiveDate = value; }
        }

        public virtual long ExpirationDate
        {
            get { return expirationDate; }

            set { expirationDate = value; }
        }

        public virtual bool NoAgenda
        {
            get { return noAgenda; }

            set { noAgenda = value; }
        }

        public virtual bool RememberMatch
        {
            get { return rememberMatch; }

            set { rememberMatch = value; }
        }

        public virtual String Version
        {
            get { return version; }

            set
            {
                if (value != null)
                {
                    version = value;
                }
            }
        }

        public virtual ICondition[] Conditions
        {
            /* (non-Javadoc)
			* @see woolfel.engine.rule.Rule#getConditions()
			*/

            get
            {
                ICondition[] cond = new ICondition[conditions.Count];
                conditions.CopyTo(cond,0);
                return cond;
            }
        }

        public virtual IAction[] Actions
        {
            /* (non-Javadoc)
			* @see woolfel.engine.rule.Rule#getActions()
			*/

            get
            {
                IAction[] acts = new IAction[actions.Count];
                actions.CopyTo(acts,0);
                return acts;
            }
        }

        public virtual IModule Module
        {
            get { return themodule; }

            set { themodule = value; }
        }

        /// <summary> Get the array of join nodes
        /// </summary>
        public virtual IList Joins
        {
            get { return joins; }
        }

        public virtual BaseNode LastNode
        {
            get
            {
                if (joins.Count > 0)
                {
                    return (BaseNode) joins[joins.Count - 1];
                }
                else if (conditions.Count > 0)
                {
                    // this means there's only 1 ConditionalElement, so the conditions
                    // only has 1 element. in all other cases, there will be atleast
                    // 1 join node
                    ICondition c = (ICondition) conditions[0];
                    if (c is ObjectCondition)
                    {
                        return ((ObjectCondition) c).LastNode;
                    }
                    else if (c is TestCondition)
                    {
                        return ((TestCondition) c).TestNode;
                    }
                    return null;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary> This should be called when the action is being fired. after the rule
        /// actions are executed, the trigger facts should be reset. The primary
        /// downside of this design decision is it won't work well with multi-
        /// threaded parallel execution. Since Sumatra has no plans for implementing
        /// parallel execution using multi-threading, the design is not an issue.
        /// Implementing multi-threaded parallel execution isn't desirable and
        /// has been proven to be too costly. A better approach is to queue assert
        /// retract and process them in sequence.
        /// </summary>
        public virtual IFact[] TriggerFacts
        {
            get { return triggerFacts; }

            set { triggerFacts = value; }
        }

        /// <summary> The method will return the Bindings in the order they
        /// were added to the utility.
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public virtual System.Collections.IEnumerator BindingIterator
        {
            get { return bindings.Values.GetEnumerator(); }
        }

        /// <summary> Returns the number of unique bindings. If a binding is
        /// used multiple times to join several facts, it is only
        /// counted once.
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        public virtual int BindingCount
        {
            get { return bindings.Count; }
        }

        public virtual bool TemporalActivation
        {
            get { return temporalActivation; }

            set { temporalActivation = value; }
        }


        /* (non-Javadoc)
		* @see woolfel.engine.rule.Rule#addCondition(woolfel.engine.rule.Condition)
		*/

        public virtual void addCondition(ICondition cond)
        {
            conditions.Add(cond);
        }

        /* (non-Javadoc)
		* @see woolfel.engine.rule.Rule#addAction(woolfel.engine.rule.Action)
		*/

        public virtual void addAction(IAction act)
        {
            actions.Add(act);
        }


        /// <summary> Add join nodes to the rule
        /// </summary>
        public virtual void addJoinNode(BaseJoin node)
        {
            joins.Add(node);
        }


        /// <summary> the current implementation simply replaces the existing
        /// value if one already exists.
        /// </summary>
        public virtual void setBindingValue(Object key, Object value_Renamed)
        {
            bindValues.Put(key.ToString(), value_Renamed);
        }

        /// <summary> return the value associated with the binding
        /// </summary>
        public virtual Object getBindingValue(string key)
        {
            Object val = bindValues.Get(key);
            if (val == null)
            {
                Binding bd = (Binding) bindings[key];
                if (bd != null)
                {
                    IFact left = triggerFacts[bd.LeftRow];
                    if (bd.IsObjectVar)
                    {
                        val = left;
                    }
                    else
                    {
                        val = left.getSlotValue(bd.LeftIndex);
                    }
                }
            }
            return val;
        }

        public virtual void setBindingValue(String name, Object value_Renamed)
        {
            bindValues.Put(name, value_Renamed);
        }


        /// <summary> reset the trigger facts after all the actions have executed.
        /// </summary>
        public virtual void resetTriggerFacts()
        {
            triggerFacts = null;
        }


        /// <summary>
        /// Method will only Add the binding if it doesn't already
        /// exist.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="bind"></param>
        public virtual void addBinding(String key, Binding bind)
        {
            if (!bindings.Contains(key))
            {
                bindings.Add(key, bind);
            }
        }

        /// <summary>
        /// Return the Binding matching the variable name
        /// </summary>
        /// <param name="varName"></param>
        /// <returns></returns>
        public virtual Binding getBinding(String varName)
        {
            return (Binding) bindings[varName];
        }

        /// <summary>
        /// Get a copy of the Binding using the variable name
        /// </summary>
        /// <param name="varName"></param>
        /// <returns></returns>
        public virtual Binding copyBinding(String varName)
        {
            Binding b = getBinding(varName);
            if (b != null)
            {
                Binding b2 = (Binding) b.Clone();
                return b2;
            }
            else
            {
                return null;
            }
        }

        public virtual Binding copyPredicateBinding(String varName, int operator_Renamed)
        {
            Binding b = getBinding(varName);
            if (b != null)
            {
                Binding2 b2 = new Binding2(operator_Renamed);
                b2.LeftRow = b.LeftRow;
                b2.LeftIndex = b.LeftIndex;
                b2.VarName = b.VarName;
                return b2;
            }
            else
            {
                return null;
            }
        }


        public virtual void resolveTemplates(Rete.Rete engine)
        {
            ICondition[] cnds = Conditions;
            for (int idx = 0; idx < cnds.Length; idx++)
            {
                ICondition cnd = cnds[idx];
                if (cnd is ObjectCondition)
                {
                    ObjectCondition oc = (ObjectCondition) cnd;
                    Deftemplate dft = (Deftemplate) engine.findTemplate(oc.TemplateName);
                    if (dft != null)
                    {
                        oc.Deftemplate = dft;
                    }
                }
                else if (cnd is ExistCondition)
                {
                    ExistCondition exc = (ExistCondition) cnd;
                    Deftemplate dft = (Deftemplate) engine.findTemplate(exc.TemplateName);
                    if (dft != null)
                    {
                        exc.Deftemplate = dft;
                    }
                }
                else if (cnd is TemporalCondition)
                {
                    TemporalCondition tempc = (TemporalCondition) cnd;
                    Deftemplate dft = (Deftemplate) engine.findTemplate(tempc.TemplateName);
                    if (dft != null)
                    {
                        tempc.Deftemplate = dft;
                    }
                }
            }
        }


        public virtual String toPPString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append("(defrule " + _name + Constants.LINEBREAK);
            // now print out the rule properties
            buf.Append("  (declare (salience " + salience + ") (rule-version " + version + ") (remember-match " + rememberMatch + ") (effective-date " + effectiveDate + ") (expiration-date " + expirationDate + ") (temporal-activation " + temporalActivation + ") (chaining-direction " + direction + ") )" + Constants.LINEBREAK);
            for (int idx = 0; idx < conditions.Count; idx++)
            {
                ICondition c = (ICondition) conditions[idx];
                buf.Append(c.toPPString());
            }
            buf.Append("=>" + Constants.LINEBREAK);
            // now append the actions
            for (int idx = 0; idx < actions.Count; idx++)
            {
                IAction ac = (IAction) actions[idx];
                buf.Append(ac.toPPString());
            }
            buf.Append(")" + Constants.LINEBREAK);
            return buf.ToString();
        }

        #endregion

        private void InitBlock()
        {
            bindValues = new GenericHashMap<string, Object>();

            bindings = new GenericHashMap<string, Binding>();

            direction = Constants.FORWARD_CHAINING;
        }

        public static long getDateTime(String date)
        {
            if (date != null && date.Length > 0)
            {
                try
                {
                    //UPGRADE_ISSUE: Class 'java.text.SimpleDateFormat' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javatextSimpleDateFormat"'
                    //UPGRADE_ISSUE: Constructor 'java.text.SimpleDateFormat.SimpleDateFormat' was not converted. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1000_javatextSimpleDateFormat"'
                    //java.text.SimpleDateFormat df = new java.text.SimpleDateFormat("mm/dd/yyyy HH:mm");
                    return ((Convert.ToDateTime(date).Ticks - 621355968000000000)/10000) - (long) TimeZone.CurrentTimeZone.GetUtcOffset(Convert.ToDateTime(date)).TotalMilliseconds;
                }
                catch (Exception e)
                {
                    System.Diagnostics.Trace.WriteLine(e.Message);
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }

        public virtual void clear()
        {
            IEnumerator itr = conditions.GetEnumerator();
            while (itr.MoveNext())
            {
                ICondition cond = (ICondition) itr.Current;
                cond.clear();
            }
            joins.Clear();
        }

        //UPGRADE_TODO: The equivalent of method 'java.lang.Object.clone' is not an override method. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1143"'
        /// <summary> TODO need to finish implementing the clone method
        /// </summary>
        public Object Clone()
        {
            Defrule cl = new Defrule(_name);
            return cl;
        }


        public void setRuleProperties(IList dec)
        {
            this.RuleProperties = dec;
        }
    }
}