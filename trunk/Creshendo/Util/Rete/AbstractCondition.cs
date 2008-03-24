using System;
using System.Collections;
using System.Collections.Generic;
using Creshendo.Util.Rete;
using Creshendo.Util.Rete.Compiler;

namespace Creshendo.Util.Rule
{
    public abstract class AbstractCondition : ICondition
    {
        /// <summary> the constraints for the condition element
        /// </summary>
        //UPGRADE_NOTE: The initialization of  'constraints' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
        protected internal IList constraints;

        /// <summary> In the case the object pattern is negated, the boolean
        /// would be set to true.
        /// </summary>
        protected internal bool negated = false;

        /// <summary> a list for the RETE nodes created by RuleCompiler
        /// </summary>
        //UPGRADE_NOTE: The initialization of  'nodes' was moved to method 'InitBlock'. 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="jlca1005"'
        protected internal IList nodes;

        /// <summary> the deftemplate associated with the ObjectCondition
        /// </summary>
        protected internal Deftemplate template = null;

        /// <summary> The string template name from the parser, before we
        /// resolve it to the Template object
        /// </summary>
        protected internal String templateName = null;

        public AbstractCondition()
        {
            InitBlock();
        }

        public virtual BaseNode FirstNode
        {
            get
            {
                if (nodes.Count > 0)
                {
                    return (BaseNode) nodes[0];
                }
                else
                {
                    return null;
                }
            }
        }

        public virtual String TemplateName
        {
            get { return templateName; }

            set { templateName = value; }
        }

        public virtual Deftemplate Deftemplate
        {
            get { return template; }

            set { template = value; }
        }

        /// <summary> by default patterns are not negated. Negated Conditional Elements
        /// (aka object patterns) are expensive, so they should be used with 
        /// care.
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        /// <summary> set whether or not the pattern is negated
        /// </summary>
        /// <param name="">negate
        /// 
        /// </param>
        public virtual bool Negated
        {
            get { return negated; }

            set { negated = value; }
        }

        public virtual IConstraint[] Constraints
        {
            get
            {
                IConstraint[] con = new IConstraint[constraints.Count];
                constraints.CopyTo(con, 0);
                return con;
            }
        }

        #region Condition Members

        /// <summary> returns the bindings, excluding predicateConstraints
        /// </summary>
        public virtual IList BindConstraints
        {
            get
            {
                List<object> binds = new List<Object>();
                IEnumerator itr = constraints.GetEnumerator();
                while (itr.MoveNext())
                {
                    Object c = itr.Current;
                    if (c is BoundConstraint)
                    {
                        BoundConstraint bc = (BoundConstraint) c;
                        if (!bc.firstDeclaration() && !bc.IsObjectBinding)
                        {
                            binds.Add(c);
                        }
                    }
                    else if (c is PredicateConstraint)
                    {
                        PredicateConstraint pc = (PredicateConstraint) c;
                        if (pc.PredicateJoin)
                        {
                            binds.Add(pc);
                        }
                    }
                }
                return binds;
            }
        }

        public virtual BaseNode LastNode
        {
            get
            {
                if (nodes.Count > 0)
                {
                    return (BaseNode) nodes[nodes.Count - 1];
                }
                else
                {
                    return null;
                }
            }
        }

        public virtual IList Nodes
        {
            get { return nodes; }
        }

        public virtual void addNewAlphaNodes(BaseNode node)
        {
            nodes.Add(node);
            for (int i = 0; i < node.SuccessorNodes.Length; i++)
            {
                nodes.Add(node.SuccessorNodes[i]);
                addNewAlphaNodes(((BaseAlpha) node.SuccessorNodes[i]));
            }
        }

        public virtual void addNode(BaseNode node)
        {
            if (!nodes.Contains(node))
            {
                nodes.Add(node);
            }
        }

        public virtual void clear()
        {
            nodes.Clear();
        }

        public abstract IConditionCompiler getCompiler(IRuleCompiler ruleCompiler);

        public abstract bool compare(ICondition cond);

        //    /**
        //     * method returns all bindings include predicateConstraints that
        //     * aren't predicate joins
        //     */
        //    public org.jamocha.rete.util.IList getAllBindings() {
        //        org.jamocha.rete.util.List<Object> binds = new org.jamocha.rete.util.List<Object>();
        //        IEnumerator itr = constraints.GetEnumerator();
        //        while (itr.MoveNext()) {
        //            Object c = itr.Current();
        //            if (c instanceof BoundConstraint) {
        //                BoundConstraint bc = (BoundConstraint)c;
        //                if (!bc.firstDeclaration()) {
        //                    binds.Add(c);
        //                }
        //            } else if (c instanceof PredicateConstraint) {
        //                if (((PredicateConstraint)c).isPredicateJoin()) {
        //                    binds.Add(c);
        //                }
        //            }
        //        }
        //        return binds;
        //    }


        //    public boolean hasVariables() {
        //        IEnumerator itr = constraints.GetEnumerator();
        //        while (itr.MoveNext()) {
        //            Object ob = itr.Current();
        //            if (ob instanceof BoundConstraint) {
        //                return true;
        //            } else if (ob instanceof PredicateConstraint) {
        //                return ((PredicateConstraint)ob).isPredicateJoin();
        //            }
        //        }
        //        return false;
        //    }

        /// <summary> Subclasses must implement this method
        /// </summary>
        public abstract String toPPString();

        #endregion

        private void InitBlock()
        {
            constraints = new List<Object>(8);
            nodes = new List<Object>();
        }


        public virtual void addConstraint(IConstraint con)
        {
            constraints.Add(con);
        }

        public virtual void addConstraint(IConstraint con, int position)
        {
            constraints.Insert(0, con);
        }

        public virtual void removeConstraint(IConstraint con)
        {
            constraints.Remove(con);
        }
    }
}