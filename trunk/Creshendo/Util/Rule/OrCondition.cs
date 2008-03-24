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
using Creshendo.Util.Rete;
using Creshendo.Util.Rete.Compiler;

namespace Creshendo.Util.Rule
{
    /// <author>  Peter Lin
    /// *
    /// AndCondition is specifically created to handle and conjunctions. AndConditions
    /// are compiled to a BetaNode.
    /// 
    /// </author>
    public class OrCondition : ICondition
    {
        protected internal IList nestedCE;
        protected internal BaseJoin reteNode = null;

        /// <summary> 
        /// </summary>
        public OrCondition() 
        {
            InitBlock();
        }

        public virtual IList NestedConditionalElement
        {
            get { return nestedCE; }
        }

        #region Condition Members

        public virtual IList Nodes
        {
            get { return new List<Object>(); }
        }

        public virtual BaseNode LastNode
        {
            get { return reteNode; }
        }

        public virtual IList BindConstraints
        {
            get
            {
                // TODO Auto-generated method stub
                return null;
            }
        }

        //UPGRADE_NOTE: The initialization of  'nestedCE' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'

        public virtual bool compare(ICondition cond)
        {
            if (!(cond is OrCondition))
            {
                return false;
            }
            OrCondition orc = (OrCondition) cond;
            if (orc.NestedConditionalElement.Count == nestedCE.Count)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary> not implemented yet
        /// </summary>
        public virtual void addNode(BaseNode node)
        {
        }

        /// <summary> not implemented yet
        /// </summary>
        public virtual void addNewAlphaNodes(BaseNode node)
        {
        }


        public virtual void clear()
        {
            reteNode = null;
        }

        public virtual String toPPString()
        {
            return "";
        }

        public virtual IConditionCompiler getCompiler(IRuleCompiler ruleCompiler)
        {
            // TODO Auto-generated method stub
            return null;
        }

        #endregion

        private void InitBlock()
        {
            nestedCE = new List<Object>();
        }

        public virtual void addNestedConditionElement(Object ce)
        {
            nestedCE.Add(ce);
        }
    }
}