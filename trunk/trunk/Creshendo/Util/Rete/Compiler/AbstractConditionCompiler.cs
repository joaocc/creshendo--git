using System;
using System.Collections;
using Creshendo.Util.Rule;

namespace Creshendo.Util.Rete.Compiler
{
    /// <summary> 
    /// </summary>
    /// <author>  HouZhanbin
    /// Oct 16, 2007 2:22:07 PM
    /// *
    /// 
    /// </author>
    public abstract class AbstractConditionCompiler : IConditionCompiler
    {
        internal DefaultRuleCompiler ruleCompiler;

        #region ConditionCompiler Members

        public virtual IRuleCompiler RuleCompiler
        {
            get { return ruleCompiler; }
        }

        public abstract void compile(ICondition condition, int position, Rule.IRule util, bool alphaMemory);

        public abstract void compileFirstJoin(ICondition condition, Rule.IRule rule);

        public abstract BaseJoin compileJoin(ICondition condition, int position, Rule.IRule rule);


        /// <summary> The first step is to connect the exist join to the parent on the left side. 
        /// The second step is to connect it to the parent on the right. For the right 
        /// side, if the objectCondition doesn't have any nodes, we attach it to the 
        /// objectType node.
        /// </summary>
        public void connectJoinNode(ICondition previousCondition, ICondition condition, BaseJoin previousJoinNode, BaseJoin joinNode)
        {
            if (previousJoinNode != null)
            {
                ruleCompiler.attachJoinNode(previousJoinNode, (BaseJoin) joinNode);
            }
            else
            {
                ruleCompiler.attachJoinNode(previousCondition.LastNode, (BaseJoin) joinNode);
            }
            // Current we have to Add the ExistJoin for the right side, which should be either
            // an alphaNode or the objectTypeNode
            ObjectCondition oc = getObjectCondition(condition);
            ObjectTypeNode otn = ruleCompiler.findObjectTypeNode(oc.TemplateName);

            if (oc.Nodes.Count > 0)
            {
                ruleCompiler.attachJoinNode(oc.LastNode, (BaseJoin) joinNode);
            }
            else
            {
                otn.addSuccessorNode(joinNode, ruleCompiler.Engine, ruleCompiler.Engine.WorkingMemory);
            }
        }

        public abstract void compileSingleCE(Rule.IRule rule);

        #endregion

        internal abstract ObjectCondition getObjectCondition(ICondition condition);

        /// <summary> the paramList should be clean and 
        /// other codes surrounding this method in subclass may be removed into this method.
        /// Houzhanbin 10/16/2007
        /// </summary>
        /// <param name="">condition
        /// </param>
        /// <param name="">rule
        /// </param>
        /// <param name="">Constraints
        /// </param>
        /// <param name="">position
        /// </param>
        /// <param name="">hasNotEqual
        /// </param>
        /// <param name="">hasPredicateJoin
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        internal Binding[] getBindings(ICondition condition, Rule.IRule rule, int position)
        {
            ObjectCondition oc = getObjectCondition(condition);
            IList Constraints = oc.BindConstraints;
            Deftemplate tmpl = oc.Deftemplate;
            Binding[] binds = new Binding[Constraints.Count];
            for (int idz = 0; idz < Constraints.Count; idz++)
            {
                Object cst = Constraints[idz];
                if (cst is BoundConstraint)
                {
                    BoundConstraint bc = (BoundConstraint) cst;
                    Binding cpy = rule.copyBinding(bc.VariableName);
                    if (cpy.LeftRow >= position)
                    {
                        binds = new Binding[0];
                        break;
                    }
                    else
                    {
                        binds[idz] = cpy;
                        int rinx = tmpl.getColumnIndex(bc.Name);
                        // we increment the count to make sure the
                        // template isn't removed if it is being used
                        tmpl.incrementColumnUseCount(bc.Name);
                        binds[idz].RightIndex = rinx;
                        binds[idz].Negated = bc.Negated;
                        if (bc.Negated)
                        {
                            oc.HasNotEqual = true;
                        }
                    }
                }
                else if (cst is PredicateConstraint)
                {
                    PredicateConstraint pc = (PredicateConstraint) cst;
                    if (pc.Value is BoundParam)
                    {
                        oc.HasPredicateJoin = true;
                        BoundParam bpm = (BoundParam) pc.Value;
                        String var = bpm.VariableName;
                        int op = ConversionUtils.getOperatorCode(pc.FunctionName);
                        // check and make sure the function isn't user defined
                        if (op != Constants.USERDEFINED)
                        {
                            // if the first binding in the function is from the object type
                            // we reverse the operator
                            if (pc.Parameters[0] != bpm)
                                op = ConversionUtils.getOppositeOperatorCode(op);
                            binds[idz] = rule.copyPredicateBinding(var, op);
                            ((Binding2) binds[idz]).RightVariable = pc.VariableName;
                        }
                        else
                        {
                            binds[idz] = rule.copyPredicateBinding(var, op);
                        }
                        binds[idz].PredJoin = true;
                        int rinx = tmpl.getColumnIndex(pc.Name);
                        // we increment the count to make sure the
                        // template isn't removed if it is being used
                        tmpl.incrementColumnUseCount(pc.Name);
                        binds[idz].RightIndex = rinx;
                    }
                    else if (pc.Value is FunctionParam)
                    {
                        // this means there is a nested function
                    }
                }
            }
            return binds;
        }
    }
}