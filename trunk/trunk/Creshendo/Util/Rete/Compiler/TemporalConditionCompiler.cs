using Creshendo.Util.Rete.Exception;
using Creshendo.Util.Rule;

namespace Creshendo.Util.Rete.Compiler
{
    /// <summary> 
    /// </summary>
    /// <author>  HouZhanbin
    /// Oct 12, 2007 9:42:15 AM
    /// *
    /// 
    /// </author>
    public class TemporalConditionCompiler : ObjectConditionCompiler
    {
        public TemporalConditionCompiler(IRuleCompiler ruleCompiler) : base(ruleCompiler)
        {
        }

        /// <summary> Compile a single ObjectCondition and create the alphaNodes and/or Bindings
        /// </summary>
        public override void compile(ICondition condition, int position, Rule.IRule rule, bool alphaMemory)
        {
            TemporalCondition cond = (TemporalCondition) condition;
            ObjectTypeNode otn = ruleCompiler.findObjectTypeNode(cond.TemplateName);
            // we set remember match to false, since the rule is temporal
            bool switchMatch = false;
            if (rule.RememberMatch)
            {
                rule.RememberMatch = false;
                switchMatch = true;
            }
            if (otn != null)
            {
                BaseAlpha2 first = null;
                BaseAlpha2 previous = null;
                BaseAlpha2 current = null;
                ITemplate templ = cond.Deftemplate;

                IConstraint[] constrs = cond.Constraints;
                for (int idx = 0; idx < constrs.Length; idx++)
                {
                    IConstraint cnstr = constrs[idx];
                    if (cnstr is LiteralConstraint)
                    {
                        current = ruleCompiler.compileConstraint((LiteralConstraint) cnstr, templ, rule);
                    }
                    else if (cnstr is AndLiteralConstraint)
                    {
                        current = ruleCompiler.compileConstraint((AndLiteralConstraint) cnstr, templ, rule);
                    }
                    else if (cnstr is OrLiteralConstraint)
                    {
                        current = ruleCompiler.compileConstraint((OrLiteralConstraint) cnstr, templ, rule);
                    }
                    else if (cnstr is BoundConstraint)
                    {
                        ruleCompiler.compileConstraint((BoundConstraint) cnstr, templ, rule, position);
                    }
                    else if (cnstr is PredicateConstraint)
                    {
                        current = ruleCompiler.compileConstraint((PredicateConstraint) cnstr, templ, rule, position);
                    }
                    // we Add the node to the previous
                    if (first == null)
                    {
                        first = current;
                        previous = current;
                    }
                    else if (current != previous)
                    {
                        try
                        {
                            previous.addSuccessorNode(current, ruleCompiler.Engine, ruleCompiler.Memory);
                            // now set the previous to current
                            previous = current;
                        }
                        catch (AssertException e)
                        {
                            // send an event
                        }
                    }
                }
                if (first != null)
                {
                    attachAlphaNode(otn, first, cond);
                }
            }

            if (!cond.Negated)
            {
                position++;
            }
            if (switchMatch)
            {
                rule.RememberMatch = true;
            }
        }

        public override void compileFirstJoin(ICondition condition, Rule.IRule rule)
        {
            ObjectCondition cond = (ObjectCondition) condition;
            ObjectTypeNode otn = ruleCompiler.findObjectTypeNode(cond.TemplateName);
            // the LeftInputAdapterNode is the first node to propogate to
            // the first joinNode of the rule
            LIANode node = new LIANode(ruleCompiler.Engine.nextNodeId());
            // if the condition doesn't have any nodes, we want to Add it to
            // the objectType node if one doesn't already exist.
            // otherwise we Add it to the last AlphaNode
            if (cond.Nodes.Count == 0)
            {
                // try to find the existing LIANode for the given ObjectTypeNode
                // if we don't do this, we end up with multiple LIANodes
                // descending directly from the ObjectTypeNode
                LIANode existingLIANode = ruleCompiler.findLIANode(otn);
                if (existingLIANode == null)
                {
                    otn.addSuccessorNode(node, ruleCompiler.Engine, ruleCompiler.Memory);
                    cond.addNode(node);
                }
                else
                {
                    existingLIANode.incrementUseCount();
                    cond.addNode(existingLIANode);
                }
            }
            else
            {
                // Add the LeftInputAdapterNode to the last alphaNode
                // In the case of node sharing, the LIANode could be the last
                // alphaNode, so we have to check and only Add the node to 
                // the condition if it isn't a LIANode
                BaseAlpha old = (BaseAlpha) cond.LastNode;
                //if the last node of condition has a LIANode successor,
                //the LIANode should be shared with the new CE followed by another CE.
                // Houzhanbin,10/16/2007
                BaseNode[] successors = (BaseNode[]) old.SuccessorNodes;
                for (int i = 0; i < successors.Length; i++)
                {
                    if (successors[i] is LIANode)
                    {
                        cond.addNode(successors[i]);
                        return;
                    }
                }

                if (!(old is LIANode))
                {
                    old.addSuccessorNode(node, ruleCompiler.Engine, ruleCompiler.Memory);
                    cond.addNode(node);
                }
            }
        }

        public override BaseJoin compileJoin(ICondition condition, int position, Rule.IRule rule)
        {
            Binding[] binds = getBindings(condition, rule, position);
            TemporalCondition tc = (TemporalCondition) condition;
            TemporalEqNode joinNode = null;
            //deal with the CE which is not NOT CE.
            if (!tc.Negated)
            {
                if (binds.Length > 0 && tc.HasPredicateJoin)
                {
                    joinNode = new TemporalEqNode(ruleCompiler.Engine.nextNodeId());
                }
                else if (binds.Length > 0 && tc.HasNotEqual)
                {
                    joinNode = new TemporalEqNode(ruleCompiler.Engine.nextNodeId());
                }
                else if (binds.Length > 0)
                {
                    joinNode = new TemporalEqNode(ruleCompiler.Engine.nextNodeId());
                }
            }

            //deal with the CE which is NOT CE.
            if (tc.Negated)
            {
                if (binds.Length > 0 && tc.HasPredicateJoin)
                {
                    joinNode = new TemporalEqNode(ruleCompiler.Engine.nextNodeId());
                }
                else if (tc.HasNotEqual)
                {
                    joinNode = new TemporalEqNode(ruleCompiler.Engine.nextNodeId());
                }
                else
                {
                    joinNode = new TemporalEqNode(ruleCompiler.Engine.nextNodeId());
                }
            }
            joinNode.Bindings = binds;
            joinNode.RightElapsedTime = tc.RelativeTime*1000;
            return joinNode;
        }

        public override void compileSingleCE(Rule.IRule rule)
        {
            ICondition[] conds = rule.Conditions;
            ObjectCondition oc = (ObjectCondition) conds[0];
            if (oc.Negated)
            {
                // the ObjectCondition is negated, so we need to
                // handle it appropriate. This means we need to
                // Add a LIANode to _IntialFact and attach a NOTNode
                // to the LIANode.
                ObjectTypeNode otn = (ObjectTypeNode) ruleCompiler.Inputnodes.Get(ruleCompiler.Engine.InitFact);
                LIANode lianode = ruleCompiler.findLIANode(otn);
                NotJoin njoin = new NotJoin(ruleCompiler.Engine.nextNodeId());
                njoin.Bindings = new Binding[0];
                lianode.addSuccessorNode(njoin, ruleCompiler.Engine, ruleCompiler.Memory);
                // Add the join to the rule object
                rule.addJoinNode(njoin);
                oc.LastNode.addSuccessorNode(njoin, ruleCompiler.Engine, ruleCompiler.Memory);
            }
            else if (oc.Nodes.Count == 0)
            {
                // this means the rule has a binding, but no conditions
                ObjectTypeNode otn = ruleCompiler.findObjectTypeNode(oc.TemplateName);
                LIANode lianode = new LIANode(ruleCompiler.Engine.nextNodeId());
                otn.addSuccessorNode(lianode, ruleCompiler.Engine, ruleCompiler.Memory);
                rule.Conditions[0].addNode(lianode);
            }
        }
    }
}