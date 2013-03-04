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
using Creshendo.Functions;
using Creshendo.Util.Rete;
using Creshendo.Util.Rete.Compiler;

namespace Creshendo.Util.Rule
{
    /// <author>  Peter Lin
    /// *
    /// A TestCondition is a pattern that uses a function. For example,
    /// in CLIPS, (test (> ?var1 ?var2) )
    /// 
    /// </author>
    public class TestCondition : ICondition
    {
        protected internal List<object> binds;
        protected internal IFunction func = null;
        protected internal bool negated = false;
        protected internal TestNode node = null;

        /// <summary> 
        /// </summary>
        public TestCondition() 
        {
            InitBlock();
        }

        public TestCondition(IFunction function)
        {
            InitBlock();
            func = function;
        }

        public virtual IFunction Function
        {
            get { return func; }

            set { func = value; }
        }

        public virtual TestNode TestNode
        {
            get { return node; }

            set { node = value; }
        }

        public virtual bool Negated
        {
            get { return negated; }

            set { negated = value; }
        }

        #region Condition Members

        /// <summary>
        /// the current implementation creates a new Creshendo.rete.util.List, adds the
        /// TestNode to it and returns the list.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// </returns>
        public virtual IList Nodes
        {
            get
            {
                IList n = new List<Object>();
                n.Add(node);
                return n;
            }
        }

        public virtual BaseNode LastNode
        {
            get { return node; }
        }

        /// <summary> return an Creshendo.rete.util.IList of the bindings. in the case of TestCondition, the
        /// bindings are BoundParam
        /// </summary>
        public virtual IList BindConstraints
        {
            get { return binds; }
        }

        public virtual bool compare(ICondition cond)
        {
            return false;
        }


        /// <summary> The current implementation checks to make sure the node is a
        /// TestNode. If it is, it will set the node. If not, it will ignore
        /// it.
        /// </summary>
        public virtual void addNode(BaseNode node)
        {
            if (node is TestNode)
            {
                this.node = (TestNode) node;
            }
        }

        public virtual void addNewAlphaNodes(BaseNode node)
        {
            addNode(node);
        }


        //    /**
        //     * the implementation will look at the parameters for
        //     * the function and see if it takes BoundParam
        //     */
        //    public boolean hasVariables() {
        //        if (this.func.getParameter() != null) {
        //            Class[] pms = func.getParameter();
        //            for (int idx=0; idx < pms.length; idx++) {
        //                if (pms[idx] == BoundParam.class) {
        //                    binds.Add(pms[idx]);
        //                }
        //            }
        //            if (binds.Count() > 0) {
        //                return true;
        //            } else {
        //                return true;
        //            }
        //        } else {
        //            return false;
        //        }
        //    }


        public virtual void clear()
        {
            node = null;
        }

        public virtual String toPPString()
        {
            StringBuilder buf = new StringBuilder();
            String pad = "  ";
            buf.Append(pad + "(" + func.Name);
            if (func is ShellFunction)
            {
                IParameter[] p = ((ShellFunction) func).Parameters;
                for (int idx = 0; idx < p.Length; idx++)
                {
                    if (p[idx] is BoundParam)
                    {
                        buf.Append(" ?" + ((BoundParam) p[idx]).VariableName);
                    }
                    else
                    {
                        buf.Append(" " + ConversionUtils.formatSlot(p[idx].Value));
                    }
                }
            }
            buf.Append(")" + Constants.LINEBREAK);
            return buf.ToString();
        }

        public virtual IConditionCompiler getCompiler(IRuleCompiler ruleCompiler)
        {
            CompilerProvider.getInstance(ruleCompiler);
            return CompilerProvider.testConditionCompiler;
        }

        #endregion

        private void InitBlock()
        {
            binds = new List<Object>();
        }

        public virtual bool executeFunction(Rete.Rete engine, IParameter[] params_Renamed)
        {
            IReturnVector rv = func.executeFunction(engine, params_Renamed);
            // we return the first ReturnValue
            return rv.firstReturnValue().BooleanValue;
        }
    }
}