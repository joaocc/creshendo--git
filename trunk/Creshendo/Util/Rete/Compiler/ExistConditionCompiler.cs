using Creshendo.Util.Rule;

namespace Creshendo.Util.Rete.Compiler
{
    /// <summary> 
    /// </summary>
    /// <author>  HouZhanbin
    /// Oct 12, 2007 9:50:17 AM
    /// *
    /// 
    /// </author>
    public class ExistConditionCompiler : AbstractConditionCompiler
    {
        private IConditionCompiler conditionCompiler;


        public ExistConditionCompiler(IConditionCompiler conditionCompiler)
        {
            this.conditionCompiler = conditionCompiler;
            ruleCompiler = (DefaultRuleCompiler) conditionCompiler.RuleCompiler;
        }

        /// <summary> Method will compile exists quantifier
        /// </summary>
        public override void compile(ICondition condition, int position, Rule.IRule util, bool alphaMemory)
        {
            ExistCondition cond = (ExistCondition) condition;
            ObjectCondition oc = (ObjectCondition) cond;
            conditionCompiler.compile(oc, position, util, alphaMemory);
        }

        /// <summary> TODO - note the logic feels a bit messy. Need to rethink it and make it
        /// simpler. When the first conditional element is Exist, it can only have
        /// literal constraints, so we shouldn't need to check if the last node
        /// is a join. That doesn't make any sense. Need to rethink this and clean
        /// it up. Peter Lin 10/14/2007
        /// </summary>
        public override void compileFirstJoin(ICondition condition, Rule.IRule rule)
        {
            BaseJoin bjoin = new ExistJoinFrst(ruleCompiler.Engine.nextNodeId());
            ExistCondition cond = (ExistCondition) condition;
            BaseNode base_Renamed = cond.LastNode;
            if (base_Renamed != null)
            {
                if (base_Renamed is BaseAlpha)
                {
                    ((BaseAlpha) base_Renamed).addSuccessorNode(bjoin, ruleCompiler.Engine, ruleCompiler.Memory);
                }
                else if (base_Renamed is BaseJoin)
                {
                    ((BaseJoin) base_Renamed).addSuccessorNode(bjoin, ruleCompiler.Engine, ruleCompiler.Memory);
                }
            }
            else
            {
                // the rule doesn't have a literal constraint so we need to Add
                // ExistJoinFrst as a child
                ObjectTypeNode otn = ruleCompiler.findObjectTypeNode(cond.TemplateName);
                otn.addSuccessorNode(bjoin, ruleCompiler.Engine, ruleCompiler.Memory);
            }
            // important, do not call this before ExistJoinFrst is added
            // if it's called first, the List<Object> will return index
            // out of bound, since there's nothing in the list
            cond.addNode(bjoin);
        }

        /// <summary> method compiles ExistCE to an exist node. It does not include rules that
        /// start with Exist for the first CE.
        /// </summary>
        public override BaseJoin compileJoin(ICondition condition, int position, Rule.IRule rule)
        {
            ExistCondition exc = (ExistCondition) condition;
            Binding[] binds = getBindings(exc, rule, position);
            BaseJoin joinNode = null;
            if (exc.HasPredicateJoin)
            {
                joinNode = new ExistPredJoin(ruleCompiler.Engine.nextNodeId());
            }
            else if (exc.HasNotEqual)
            {
                joinNode = new ExistNeqJoin(ruleCompiler.Engine.nextNodeId());
            }
            else
            {
                joinNode = new ExistJoin(ruleCompiler.Engine.nextNodeId());
            }
            joinNode.Bindings = binds;
            return joinNode;
        }

        internal override ObjectCondition getObjectCondition(ICondition condition)
        {
            return (ObjectCondition) condition;
        }

        public override void compileSingleCE(Rule.IRule rule)
        {
            ICondition[] conds = rule.Conditions;
            ICondition condition = conds[0];
            ExistCondition cond = (ExistCondition) condition;
            BaseNode base_Renamed = cond.LastNode;
            BaseJoin bjoin = new ExistJoinFrst(ruleCompiler.Engine.nextNodeId());
            if (base_Renamed != null)
            {
                if (base_Renamed is BaseAlpha)
                {
                    ((BaseAlpha) base_Renamed).addSuccessorNode(bjoin, ruleCompiler.Engine, ruleCompiler.Memory);
                }
                else if (base_Renamed is BaseJoin)
                {
                    ((BaseJoin) base_Renamed).addSuccessorNode(bjoin, ruleCompiler.Engine, ruleCompiler.Memory);
                }
            }
            else
            {
                // the rule doesn't have a literal constraint so we need to Add
                // ExistJoinFrst as a child
                ObjectTypeNode otn = ruleCompiler.findObjectTypeNode(cond.TemplateName);
                otn.addSuccessorNode(bjoin, ruleCompiler.Engine, ruleCompiler.Memory);
            }
            // important, do not call this before ExistJoinFrst is added
            // if it's called first, the List<Object> will return index
            // out of bound, since there's nothing in the list
            cond.addNode(bjoin);
            rule.addJoinNode(bjoin);
        }
    }
}