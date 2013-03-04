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
using Creshendo.Functions;
using Creshendo.Util.Rete;
using Creshendo.Util.Rule.Util;

namespace Creshendo.Util.Rule
{
    /// <summary> Template validation will check the templates of the rule and make
    /// sure they are valid. If it isn't, validate(Rule) will return false
    /// and provide details.
    /// 
    /// </summary>
    /// <author>  Peter Lin
    /// *
    /// 
    /// </author>
    public class TemplateValidation : IAnalysis
    {
        public static readonly String INVALID_FUNCTION;
        public static readonly String INVALID_SLOT;
        public static readonly String INVALID_TEMPLATE;
        public static readonly String NO_FUNCTION;
        public static readonly String NO_MODULE;
        private Rete.Rete engine = null;
        private ErrorSummary error = null;
        private WarningSummary warning = null;

        static TemplateValidation()
        {
            INVALID_SLOT = Messages.getString("CompilerProperties.invalid.slot");
            INVALID_TEMPLATE = Messages.getString("CompilerProperties.invalid.template");
            INVALID_FUNCTION = Messages.getString("CompilerProperties.invalid.function");
            NO_FUNCTION = Messages.getString("CompilerProperties.no.function");
            NO_MODULE = Messages.getString("CompilerProperties.no.module");
        }

        //$NON-NLS-1$

        /// <summary> 
        /// </summary>
        public TemplateValidation(Rete.Rete engine)
        {
            this.engine = engine;
        }

        #region Analysis Members

        public virtual ISummary Errors
        {
            get { return error; }
        }

        public virtual ISummary Warnings
        {
            get { return warning; }
        }


        public virtual void reset()
        {
            error = null;
            warning = null;
        }


        public virtual int analyze(IRule rule)
        {
            int result = Analysis_Fields.VALIDATION_PASSED;
            error = new ErrorSummary();
            warning = new WarningSummary();
            checkForModule(rule);
            ICondition[] cnds = rule.Conditions;
            for (int idx = 0; idx < cnds.Length; idx++)
            {
                ICondition cnd = cnds[idx];
                if (cnd is ObjectCondition)
                {
                    ObjectCondition oc = (ObjectCondition) cnd;
                    Deftemplate dft = oc.Deftemplate;
                    if (dft != null)
                    {
                        IConstraint[] cntrs = oc.Constraints;
                        for (int idy = 0; idy < cntrs.Length; idy++)
                        {
                            IConstraint cons = cntrs[idy];
                            if (cons is LiteralConstraint)
                            {
                                Slot sl = dft.getSlot(cons.Name);
                                if (sl == null)
                                {
                                    error.addMessage(INVALID_SLOT + " " + cons.Name + " slot does not exist.");
                                    result = Analysis_Fields.VALIDATION_FAILED;
                                }
                            }
                            else if (cons is BoundConstraint)
                            {
                                BoundConstraint bc = (BoundConstraint) cons;
                                if (!bc.isObjectBinding)
                                {
                                    Slot sl = dft.getSlot(bc.Name);
                                    if (sl == null)
                                    {
                                        error.addMessage(INVALID_SLOT + " " + cons.Name + " slot does not exist.");
                                        result = Analysis_Fields.VALIDATION_FAILED;
                                    }
                                }
                            }
                            else if (cons is PredicateConstraint)
                            {
                                PredicateConstraint pc = (PredicateConstraint) cons;
                                IFunction f = engine.findFunction(pc.FunctionName);
                                if (f == null)
                                {
                                    addInvalidFunctionError(pc.FunctionName);
                                }
                            }
                        }
                    }
                    else
                    {
                        error.addMessage(INVALID_TEMPLATE + " " + oc.TemplateName + " template does not exist.");
                        result = Analysis_Fields.VALIDATION_FAILED;
                    }
                }
                else if (cnd is TestCondition)
                {
                    TestCondition tc = (TestCondition) cnd;
                    if (tc.Function == null)
                    {
                        error.addMessage(NO_FUNCTION);
                        result = Analysis_Fields.VALIDATION_FAILED;
                    }
                    else
                    {
                        IFunction f = tc.Function;
                        if (engine.findFunction(f.Name) == null)
                        {
                            addInvalidFunctionError(f.Name);
                            result = Analysis_Fields.VALIDATION_FAILED;
                        }
                    }
                }
                else if (cnd is ExistCondition)
                {
                }
            }
            // now we check the Right-hand side
            IAction[] acts = rule.Actions;
            for (int idx = 0; idx < acts.Length; idx++)
            {
                IAction act = acts[idx];
                if (act is FunctionAction)
                {
                    FunctionAction fa = (FunctionAction) act;
                    if (engine.findFunction(fa.FunctionName) == null)
                    {
                        addInvalidFunctionError(fa.FunctionName);
                        result = Analysis_Fields.VALIDATION_FAILED;
                    }
                }
            }
            return result;
        }

        #endregion

        protected internal virtual void checkForModule(IRule rule)
        {
            if (rule.Name.IndexOf("::") > 0)
            {
                String modname = GenerateFacts.parseModuleName(rule, engine);
                if (engine.findModule(modname) == null)
                {
                    // Add an error
                    error.addMessage(NO_MODULE);
                }
            }
        }

        public virtual void addInvalidFunctionError(String name)
        {
            error.addMessage(INVALID_FUNCTION + " " + name + " does not exist.");
        }
    }
}