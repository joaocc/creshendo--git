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
using Creshendo.Functions;
using Creshendo.Util.Collections;
using Creshendo.Util.Logging;
using Creshendo.Util.Rete.Exception;
using Creshendo.Util.Rule;

namespace Creshendo.Util.Rete
{
    /// <author>  Peter Lin
    /// *
    /// DefaultRuleCompiler is a simple implementation. It's for research purposes
    /// for now. It may become a full blown rule compiler later, but for now,
    /// I just need a simple compiler to load my samples rule to test the JIT
    /// algorithm.
    /// 
    /// 
    /// 
    /// </author>
    public class DefaultRuleCompiler : IRuleCompiler
    {
        public readonly String ASSERT_ON_PROPOGATE = Messages.getString("CompilerProperties_assert_on_Add");
        public readonly String FUNCTION_NOT_FOUND = Messages.getString("CompilerProperties_function_not_found");
        public readonly String INVALID_FUNCTION = Messages.getString("CompilerProperties_invalid_function");
        private IModule currentMod = null;
        private readonly Rete engine = null;
        private readonly IGenericMap<object, object> inputnodes = null;
        private List<object> listener;
        //protected internal ILogger log;
        private IWorkingMemory memory = null;
        protected internal TemplateValidation tval = null;
        protected internal bool validate = true;

        public event CompileEventHandler RuleAdded;
        public event CompileEventHandler RuleRemoved;
        public event CompileEventHandler CompileError;

        /*
        static DefaultRuleCompiler()
        {
            FUNCTION_NOT_FOUND = Messages.getString("CompilerProperties_function_not_found");
            INVALID_FUNCTION = Messages.getString("CompilerProperties_invalid_function");
            ASSERT_ON_PROPOGATE = Messages.getString("CompilerProperties_assert_on_Add");
        }
        */

        public DefaultRuleCompiler(Rete engine, IGenericMap<object, object> inputNodes)
        {
            InitBlock();
            this.engine = engine;
            this.inputnodes = inputNodes;
            tval = new TemplateValidation(engine);
        }

        public virtual Rule.IRule Module
        {
            set
            {
                // we check the name of the rule to see if it is for a specific
                // module. if it is, we have to Add it to that module
                if (value.Name.IndexOf("::") > 0)
                {
                    //$NON-NLS-1$
                    String text = value.Name;
                    String[] sp = text.Split("::".ToCharArray()); //$NON-NLS-1$
                    value.Name = sp[1];
                    String modName = sp[0].ToUpper();
                    currentMod = engine.findModule(modName);
                    if (currentMod == null)
                    {
                        engine.addModule(modName, false);
                        currentMod = engine.findModule(modName);
                    }
                }
                else
                {
                    currentMod = engine.CurrentFocus;
                }
                value.Module = currentMod;
            }
        }

        public virtual Rete Engine
        {
            get { return engine; }
        }

        public virtual IWorkingMemory Memory
        {
            get { return memory; }
        }

        public virtual IGenericMap<object, object> Inputnodes
        {
            get { return inputnodes; }
        }

        #region RuleCompiler Members

        public virtual bool ValidateRule
        {
            get { return validate; }

            set { validate = value; }
        }

        public virtual IWorkingMemory WorkingMemory
        {
            set { memory = value; }
        }


        /// <summary> Here is a description of the compilation algorithm.
        /// 1. iterate over the conditional elements
        /// i. generate the alpha nodes
        /// a. literal constraints generate alpha node
        /// b. predicate constaints that compare against a literal generate alpha node
        /// ii. calculate the bindings
        /// a. each binding has a rowId
        /// b. NOT and EXIST CE do not increment the rowId
        /// 2. iterate over the conditional elements
        /// i. generate the beta nodes
        /// ii. attach the Left Input adapater nodes
        /// iii. attach the join nodes to the alpha nodes
        /// 3. create the terminal node and attach to the last
        /// join node.
        /// 
        /// This means the rule compiler takes a 2 pass approach to
        /// compiling rules. At the start of the method, it sets 3
        /// attributes to null: prevCE, prevJoinNode, joinNode.
        /// Those attributes are used by the compile join methods,
        /// so it's important to set it to null at the start. If
        /// we don't the Current rule won't compile correctly.
        /// </summary>
        public virtual bool addRule(Rule.IRule rule)
        {
            rule.resolveTemplates(engine);
            if (!validate || (validate && tval.analyze(rule) == Analysis_Fields.VALIDATION_PASSED))
            {
                // we have to set the attributes to null, before we start compiling a rule.

                // we've set the attributes to null, so we can compile now!!

                if (rule.Conditions != null && rule.Conditions.Length > 0)
                {
                    // we check the name of the rule to see if it is for a specific
                    // module. if it is, we have to Add it to that module
                    Module = rule;
                    try
                    {
                        ICondition[] conds = rule.Conditions;
                        // first we create the constraints, before creating the Conditional
                        // elements which include joins
                        // we use a counter and only increment it to make sure the
                        // row index of the bindings are accurate. this makes it simpler
                        // for the rule compiler and compileJoins is cleaner and does
                        // less work.
                        int counter = 0;
                        for (int idx = 0; idx < conds.Length; idx++)
                        {
                            ICondition con = conds[idx];
                            // compile object conditions
                            //implement in the ObjectConditionCompiler.compile or ExistConditionCompiler.compile
                            con.getCompiler(this).compile(con, counter, rule, rule.RememberMatch);

                            if ((con is ObjectCondition) && (!((ObjectCondition) con).Negated))
                            {
                                counter++;
                            }
                        }
                        // now we compile the joins
                        compileJoins(rule);

                        BaseNode last = rule.LastNode;
                        TerminalNode tnode = createTerminalNode(rule);

                        attachTerminalNode(last, tnode);
                        // compile the actions
                        compileActions(rule, rule.Actions);
                        // now we pass the bindings to the rule, so that actiosn can
                        // resolve the bindings

                        // now we Add the rule to the module
                        currentMod.addRule(rule);
                        CompileMessageEventArgs ce = new CompileMessageEventArgs(rule, EventType.ADD_RULE_EVENT);
                        ce.Rule = rule;
                        notifyListener(ce);
                        return true;
                    }
                    catch (AssertException e)
                    {
                        CompileMessageEventArgs ce = new CompileMessageEventArgs(rule, EventType.INVALID_RULE);
                        ce.Message = Messages.getString("RuleCompiler.assert.error"); //$NON-NLS-1$
                        notifyListener(ce);
                        TraceLogger.Instance.Debug(e);
                        return false;
                    }
                }
                else if (rule.Conditions.Length == 0)
                {
                    Module = rule;
                    // the rule has no LHS, this means it only has actions
                    BaseNode last = (BaseNode) inputnodes.Get(engine.initFact);
                    TerminalNode tnode = createTerminalNode(rule);
                    compileActions(rule, rule.Actions);
                    attachTerminalNode(last, tnode);
                    // now we Add the rule to the module
                    currentMod.addRule(rule);
                    CompileMessageEventArgs ce = new CompileMessageEventArgs(rule, EventType.ADD_RULE_EVENT);
                    ce.Rule = rule;
                    notifyListener(ce);
                    return true;
                }
                return false;
            }
            else
            {
                // we need to print out a message saying the rule was not valid
                ISummary error = tval.Errors;
                engine.writeMessage("Rule " + rule.Name + " was not added. ", Constants.DEFAULT_OUTPUT); //$NON-NLS-1$ //$NON-NLS-2$
                engine.writeMessage(error.Message, Constants.DEFAULT_OUTPUT);
                ISummary warn = tval.Warnings;
                engine.writeMessage(warn.Message, Constants.DEFAULT_OUTPUT);
                return false;
            }
        }


        /// <summary> implementation uses the deftemplate for the HashMap key and the
        /// node for the value. If the node already exists in the HashMap,
        /// or the key already exists, the compiler will not Add it to the
        /// network.
        /// </summary>
        public virtual void addObjectTypeNode(ObjectTypeNode node)
        {
            if (!inputnodes.ContainsKey(node.Deftemplate))
            {
                inputnodes.Put(node.Deftemplate, node);
            }
            // if it is the objectTypeNode for InitialFact, we go ahead and create
            // the Left Input Adapter node for it
            if (node.Deftemplate is InitialFact)
            {
                try
                {
                    IFLIANode lian = new IFLIANode(engine.nextNodeId());
                    node.addSuccessorNode(lian, engine, engine.workingMem);
                }
                catch (AssertException e)
                {
                }
            }
        }

        /// <summary> Method will Remove the ObjectTypeNode and call Clear on it.
        /// </summary>
        public virtual void removeObjectTypeNode(ObjectTypeNode node)
        {
            inputnodes.Remove(node.Deftemplate);
            node.clear(memory);
            node.clearSuccessors();
        }

        /// <summary> if the ObjectTypeNode does not exist, the method will return null.
        /// </summary>
        public virtual ObjectTypeNode getObjectTypeNode(ITemplate template)
        {
            return (ObjectTypeNode) inputnodes.Get(template);
        }
        /*
        /// <summary> Implementation will check to see if the 
        /// </summary>
        public virtual void addListener(ICompilerListener listener)
        {
            if (!this.listener.Contains(listener))
            {
                this.listener.Add(listener);
            }
        }

       
        public virtual void removeListener(ICompilerListener listener)
        {
            this.listener.Remove(listener);
        }
        */
        #endregion

        private void InitBlock()
        {
            listener = new List<Object>();
            //log = new Log4netLogger(typeof (DefaultRuleCompiler));
        }

        /// <summary>
        /// The method is responsible for creating the right terminal node based on the
        /// settings for the rule.
        /// </summary>
        /// <param name="rl">The rl.</param>
        /// <returns></returns>
        protected internal virtual TerminalNode createTerminalNode(Rule.IRule rl)
        {
            if (rl.NoAgenda && rl.ExpirationDate == 0)
            {
                return new NoAgendaTNode(engine.nextNodeId(), rl);
            }
            else if (rl.NoAgenda && rl.ExpirationDate > 0)
            {
                return new NoAgendaTNode2(engine.nextNodeId(), rl);
            }
            else if (rl.ExpirationDate > 0)
            {
                return new TerminalNode3(engine.nextNodeId(), rl);
            }
            else
            {
                return new TerminalNode2(engine.nextNodeId(), rl);
            }
        }

        /// <summary>
        /// Finds the object type node.
        /// </summary>
        /// <param name="templateName">Name of the template.</param>
        /// <returns></returns>
        public virtual ObjectTypeNode findObjectTypeNode(String templateName)
        {
            foreach (ITemplate tmpl in inputnodes.Keys)
            {
                //ITemplate tmpl = (ITemplate) entry.Key;
                if (tmpl.Name.Equals(templateName))
                {
                    return (ObjectTypeNode)inputnodes.Get(tmpl);
                }
            }
            return null;
        }

        /*
        public virtual ObjectTypeNode findObjectTypeNode(String templateName)
        {
            IEnumerator itr = inputnodes.keySet().GetEnumerator();
            Template tmpl = null;
            while (itr.MoveNext())
            {
                tmpl = (Template)itr.Current;
                if (tmpl.Name.Equals(templateName))
                {
                    break;
                }
            }
            if (tmpl != null)
            {
                return (ObjectTypeNode)inputnodes.Get(tmpl);
            }
            else
            {
                log.Debug(Messages.getString("RuleCompiler.deftemplate.error")); //$NON-NLS-1$
                return null;
            }
        }
        */

        /// <summary>
        /// method compiles a literalConstraint
        /// </summary>
        /// <param name="cnstr">The CNSTR.</param>
        /// <param name="templ">The templ.</param>
        /// <param name="rule">The rule.</param>
        /// <returns></returns>
        public virtual BaseAlpha2 compileConstraint(LiteralConstraint cnstr, ITemplate templ, Rule.IRule rule)
        {
            BaseAlpha2 current = null;
            if (templ.getSlot(cnstr.Name) != null)
            {
                Slot sl = (Slot) templ.getSlot(cnstr.Name).Clone();
                Object sval = ConversionUtils.convert(sl.ValueType, cnstr.Value);
                sl.Value = sval;
                if (rule.RememberMatch)
                {
                    current = new AlphaNode(engine.nextNodeId());
                }
                else
                {
                    current = new NoMemANode(engine.nextNodeId());
                }
                current.Slot = sl;
                current.Operator = Constants.EQUAL;
                current.incrementUseCount();
                // we increment the node use count when when create a new
                // AlphaNode for the LiteralConstraint
                templ.getSlot(sl.Id).incrementNodeCount();
            }
            return current;
        }

        /// <summary>
        /// method compiles AndLiteralConstraint into alpha nodes
        /// </summary>
        /// <param name="cnstr">The CNSTR.</param>
        /// <param name="templ">The templ.</param>
        /// <param name="rule">The rule.</param>
        /// <returns></returns>
        public virtual BaseAlpha2 compileConstraint(AndLiteralConstraint cnstr, ITemplate templ, Rule.IRule rule)
        {
            BaseAlpha2 current = null;
            if (templ.getSlot(cnstr.Name) != null)
            {
                Slot2 sl = new Slot2(cnstr.Name);
                sl.Id = templ.getColumnIndex(cnstr.Name);
                Object sval = cnstr.Value;
                sl.Value = sval;
                if (rule.RememberMatch)
                {
                    current = new AlphaNodeAnd(engine.nextNodeId());
                }
                else
                {
                    current = new NoMemAnd(engine.nextNodeId());
                }
                current.Slot = sl;
                current.incrementUseCount();
                // we increment the node use count when when create a new
                // AlphaNode for the LiteralConstraint
                templ.getSlot(sl.Id).incrementNodeCount();
            }
            return current;
        }

        /// <summary>
        /// method compiles OrLiteralConstraint into alpha nodes
        /// </summary>
        /// <param name="cnstr">The CNSTR.</param>
        /// <param name="templ">The templ.</param>
        /// <param name="rule">The rule.</param>
        /// <returns></returns>
        public virtual BaseAlpha2 compileConstraint(OrLiteralConstraint cnstr, ITemplate templ, Rule.IRule rule)
        {
            BaseAlpha2 current = null;
            if (templ.getSlot(cnstr.Name) != null)
            {
                Slot2 sl = new Slot2(cnstr.Name);
                sl.Id = templ.getColumnIndex(cnstr.Name);
                Object sval = cnstr.Value;
                sl.Value = sval;
                if (rule.RememberMatch)
                {
                    current = new AlphaNodeOr(engine.nextNodeId());
                }
                else
                {
                    current = new NoMemOr(engine.nextNodeId());
                }
                current.Slot = sl;
                current.incrementUseCount();
                // we increment the node use count when when create a new
                // AlphaNode for the LiteralConstraint
                templ.getSlot(sl.Id).incrementNodeCount();
            }
            return current;
        }

        /// <summary>
        /// method creates Bindings from the bound constraint and adds them to
        /// the Rule.
        /// </summary>
        /// <param name="cnstr">The CNSTR.</param>
        /// <param name="templ">The templ.</param>
        /// <param name="rule">The rule.</param>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        public virtual BaseAlpha2 compileConstraint(BoundConstraint cnstr, ITemplate templ, Rule.IRule rule, int position)
        {
            BaseAlpha2 current = null;
            if (rule.getBinding(cnstr.VariableName) == null)
            {
                // if the HashMap doesn't already contain the binding, we create
                // a new one
                if (cnstr.IsObjectBinding)
                {
                    Binding bind = new Binding();
                    bind.VarName = cnstr.VariableName;
                    bind.LeftRow = position;
                    bind.LeftIndex = - 1;
                    bind.IsObjectVar = true;
                    rule.addBinding(cnstr.VariableName, bind);
                }
                else
                {
                    Binding bind = new Binding();
                    bind.VarName = cnstr.VariableName;
                    bind.LeftRow = position;
                    bind.LeftIndex = templ.getSlot(cnstr.Name).Id;
                    bind.RowDeclared = position;
                    cnstr.FirstDeclaration = true;
                    rule.addBinding(cnstr.VariableName, bind);
                }
            }
            return current;
        }

        /// <summary>
        /// Compiles the constraint.
        /// </summary>
        /// <param name="cnstr">The CNSTR.</param>
        /// <param name="templ">The templ.</param>
        /// <param name="rule">The rule.</param>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        public virtual BaseAlpha2 compileConstraint(PredicateConstraint cnstr, ITemplate templ, Rule.IRule rule, int position)
        {
            BaseAlpha2 current = null;
            // for now we expect the user to write the predicate in this
            // way (> ?bind value), where the binding is first. this
            // needs to be updated so that we look at the order of the
            // parameters and set the node appropriately

            // we only create an AlphaNode if the predicate isn't
            // joining 2 bindings.
            if (!cnstr.PredicateJoin)
            {
                if (ConversionUtils.isPredicateOperatorCode(cnstr.FunctionName))
                {
                    int oprCode = ConversionUtils.getOperatorCode(cnstr.FunctionName);
                    Slot sl = (Slot) templ.getSlot(cnstr.Name).Clone();
                    Object sval = ConversionUtils.convert(sl.ValueType, cnstr.Value);
                    sl.Value = sval;
                    // create the alphaNode
                    if (rule.RememberMatch)
                    {
                        current = new AlphaNode(engine.nextNodeId());
                    }
                    else
                    {
                        current = new NoMemANode(engine.nextNodeId());
                    }
                    current.Slot = sl;
                    current.Operator = oprCode;
                    current.incrementUseCount();
                    // we increment the node use count when when create a new
                    // AlphaNode for the LiteralConstraint
                    templ.getSlot(sl.Id).incrementNodeCount();
                }
                else
                {
                    // the function isn't a built in predicate function that
                    // returns boolean true/false. We look up the function
                    IFunction f = engine.findFunction(cnstr.FunctionName);
                    if (f != null)
                    {
                        // we create the alphaNode if a function is found and
                        // the return type is either boolean primitive or object
                        if (f.ReturnType == Constants.BOOLEAN_PRIM_TYPE || f.ReturnType != Constants.BOOLEAN_OBJECT)
                        {
                            // TODO - need to implement it
                        }
                        else
                        {
                            // the function doesn't return boolean, so we have to notify
                            // the listeners the condition is not valid
                            CompileMessageEventArgs ce = new CompileMessageEventArgs(this, EventType.FUNCTION_INVALID);
                            ce.Message = INVALID_FUNCTION + " " + f.ReturnType; //$NON-NLS-1$
                            notifyListener(ce);
                        }
                    }
                    else
                    {
                        // we need to notify listeners the function wasn't found
                        CompileMessageEventArgs ce = new CompileMessageEventArgs(this, EventType.FUNCTION_NOT_FOUND);
                        ce.Message = FUNCTION_NOT_FOUND + " " + cnstr.FunctionName; 
                        notifyListener(ce);
                    }
                }
            }
            Binding bind = new Binding();
            bind.VarName = cnstr.VariableName;
            bind.LeftRow = position;
            bind.LeftIndex = templ.getSlot(cnstr.Name).Id;
            bind.RowDeclared = position;
            // we only Add the binding to the map if it doesn't already exist
            if (rule.getBinding(cnstr.VariableName) == null)
            {
                rule.addBinding(cnstr.VariableName, bind);
            }
            return current;
        }

        /// <summary>
        /// Compiles the joins.
        /// </summary>
        /// <param name="rule">The rule.</param>
        public virtual void compileJoins(Rule.IRule rule)
        {
            ICondition[] conds = rule.Conditions;
            BaseJoin prevJoinNode = null;
            BaseJoin joinNode = null;
            ICondition prevCE = null;
            // only if there's more than 1 condition do we attempt to 
            // create the join nodes. A rule with just 1 condition has
            // no joins
            if (conds.Length > 1)
            {
                // previous Condition
                prevCE = conds[0];
                //this.compileFirstJoin(engine, memory); moved to the ConditionCompiler.compileFirstJoin method
                prevCE.getCompiler(this).compileFirstJoin(prevCE, rule);


                // now compile the remaining conditions
                for (int idx = 1; idx < conds.Length; idx++)
                {
                    ICondition cdt = conds[idx];

                    joinNode = cdt.getCompiler(this).compileJoin(cdt, idx, rule);
                    cdt.getCompiler(this).connectJoinNode(prevCE, cdt, prevJoinNode, joinNode);

                    // now we set the previous node to current
                    prevCE = cdt;
                    prevJoinNode = joinNode;
                    rule.addJoinNode(joinNode);
                }
            }
            else if (conds.Length == 1)
            {
                conds[0].getCompiler(this).compileSingleCE(rule);
            }
        }


        /// <summary>
        /// The method is responsible for compiling the string form of the
        /// actions to the equivalent functions.
        /// </summary>
        /// <param name="rule">The rule.</param>
        /// <param name="acts">The acts.</param>
        protected internal virtual void compileActions(Rule.IRule rule, IAction[] acts)
        {
            for (int idx = 0; idx < acts.Length; idx++)
            {
                IAction atn = acts[idx];
                atn.configure(engine, rule);
            }
        }

        protected internal virtual void attachTerminalNode(BaseNode last, TerminalNode terminal)
        {
            if (last != null && terminal != null)
            {
                try
                {
                    if (last is BaseJoin)
                    {
                        ((BaseJoin) last).addSuccessorNode(terminal, engine, memory);
                    }
                    else if (last is BaseAlpha)
                    {
                        ((BaseAlpha) last).addSuccessorNode(terminal, engine, memory);
                    }
                }
                catch (AssertException e)
                {
                }
            }
        }

        /// <summary>
        /// Method will attach a new JoinNode to an ancestor node. An ancestor
        /// could be LIANode, AlphaNode or BetaNode.
        /// </summary>
        /// <param name="last">The last.</param>
        /// <param name="join">The join.</param>
        public virtual void attachJoinNode(BaseNode last, BaseJoin join)
        {
            if (last is BaseAlpha)
            {
                ((BaseAlpha) last).addSuccessorNode(join, engine, memory);
            }
            else if (last is BaseJoin)
            {
                ((BaseJoin) last).addSuccessorNode(join, engine, memory);
            }
        }

        /// <summary>
        /// method will find the first LeftInputAdapter node for the
        /// ObjectTypeNode. There should only be one that is a direct
        /// successor.
        /// </summary>
        /// <param name="otn">The otn.</param>
        /// <returns></returns>
        public virtual LIANode findLIANode(ObjectTypeNode otn)
        {
            LIANode node = null;
            if (otn.SuccessorNodes != null && otn.SuccessorNodes.Length > 0)
            {
                Object[] nodes = (Object[]) otn.SuccessorNodes;
                for (int idx = 0; idx < nodes.Length; idx++)
                {
                    if (nodes[idx] is LIANode)
                    {
                        node = (LIANode) nodes[idx];
                        break;
                    }
                }
            }
            return node;
        }


        //public virtual void fireErrorEvent(Object reason)
        //{
        //}

        /// <summary>
        /// basic method iterates over the listeners and passes the event, checking
        /// what kind of event it is and calling the appropriate method.
        /// </summary>
        /// <param name="eventArgs">The event_ renamed.</param>
        public virtual void notifyListener(CompileMessageEventArgs eventArgs)
        {

            switch(eventArgs.EventType)
            {
                case EventType.ADD_RULE_EVENT:
                    if(RuleAdded != null)
                    {
                        RuleAdded(this, eventArgs);
                    }
                    break;
                case EventType.REMOVE_RULE_EVENT:
                    if (RuleRemoved != null)
                    {
                        RuleRemoved(this, eventArgs);
                    }
                    break;    
                default:
                    if (CompileError != null)
                    {
                        CompileError(this, eventArgs);
                    }
                    break;
            }

            //IEnumerator itr = listener.GetEnumerator();
            ////engine.writeMessage(event.getMessage());
            //while (itr.MoveNext())
            //{
            //    ICompilerListener listen = (ICompilerListener) itr.Current;
            //    EventType etype = messageEventArgsRenamed.EventType;
            //    if (etype == EventType.ADD_RULE_EVENT)
            //    {
            //        listen.ruleAdded(messageEventArgsRenamed);
            //    }
            //    else if (etype == EventType.REMOVE_RULE_EVENT)
            //    {
            //        listen.ruleRemoved(messageEventArgsRenamed);
            //    }
            //    else
            //    {
            //        listen.compileError(messageEventArgsRenamed);
            //    }
            //}
        }
    }
}