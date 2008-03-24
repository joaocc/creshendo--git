using System;
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
    public class ObjectConditionCompiler : AbstractConditionCompiler
    {
        public ObjectConditionCompiler(IRuleCompiler ruleCompiler)
        {
            this.ruleCompiler = (DefaultRuleCompiler) ruleCompiler;
        }

        /// <summary> Compile a single ObjectCondition and create the alphaNodes and/or Bindings
        /// </summary>
        public override void compile(ICondition condition, int position, Rule.IRule util, bool alphaMemory)
        {
            ObjectCondition cond = (ObjectCondition) condition;
            ObjectTypeNode otn = ruleCompiler.findObjectTypeNode(cond.TemplateName);
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
                        current = ruleCompiler.compileConstraint((LiteralConstraint) cnstr, templ, util);
                    }
                    else if (cnstr is AndLiteralConstraint)
                    {
                        current = ruleCompiler.compileConstraint((AndLiteralConstraint) cnstr, templ, util);
                    }
                    else if (cnstr is OrLiteralConstraint)
                    {
                        current = ruleCompiler.compileConstraint((OrLiteralConstraint) cnstr, templ, util);
                    }
                    else if (cnstr is BoundConstraint)
                    {
                        ruleCompiler.compileConstraint((BoundConstraint) cnstr, templ, util, position);
                    }
                    else if (cnstr is PredicateConstraint)
                    {
                        current = ruleCompiler.compileConstraint((PredicateConstraint) cnstr, templ, util, position);
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
        }

        /// <summary>
        /// For now just attach the node and don't bother with node sharing
        /// </summary>
        /// <param name="existing">- an existing node in the network. it may be
        /// an ObjectTypeNode or AlphaNode</param>
        /// <param name="alpha">The alpha.</param>
        /// <param name="cond">The cond.</param>
        protected internal virtual void attachAlphaNode(BaseAlpha existing, BaseAlpha alpha, ICondition cond)
        {
            if (alpha != null)
            {
                try
                {
                    BaseAlpha share = null;
                    share = shareAlphaNode(existing, alpha);
                    if (share == null)
                    {
                        existing.addSuccessorNode(alpha, ruleCompiler.Engine, ruleCompiler.Memory);
                        // if the node isn't shared, we Add the node to the Condition
                        // object the node belongs to.
                        cond.addNewAlphaNodes(alpha);
                    }
                    else if (existing != alpha)
                    {
                        // the node is shared, so instead of adding the new node,
                        // we Add the existing node
                        share.incrementUseCount();
                        cond.addNode(share);
                        ruleCompiler.Memory.removeAlphaMemory(alpha);
                        if (alpha.successorCount() == 1 && alpha.SuccessorNodes[0] is BaseAlpha)
                        {
                            // Get the Current node from the new AlphaNode
                            BaseAlpha nnext = (BaseAlpha) alpha.SuccessorNodes[0];
                            attachAlphaNode(share, nnext, cond);
                        }
                    }
                }
                catch (AssertException e)
                {
                    // send an event with the correct error
                    CompileMessageEventArgs ce = new CompileMessageEventArgs(this, EventType.ADD_NODE_ERROR);
                    ce.Message = alpha.toPPString();
                    ruleCompiler.notifyListener(ce);
                }
            }
        }

        /// <summary>
        /// Implementation will Get the hashString from each node and compare them
        /// </summary>
        /// <param name="existing">The existing.</param>
        /// <param name="alpha">The alpha.</param>
        /// <returns></returns>
        private BaseAlpha shareAlphaNode(BaseAlpha existing, BaseAlpha alpha)
        {
            Object[] scc = existing.SuccessorNodes;
            for (int idx = 0; idx < scc.Length; idx++)
            {
                Object next = scc[idx];
                if (next is BaseAlpha)
                {
                    BaseAlpha baseAlpha = (BaseAlpha) next;
                    if (baseAlpha.hashString().Equals(alpha.hashString()))
                    {
                        return baseAlpha;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Compiles the first join.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <param name="rule">The rule.</param>
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

        /// <summary> method compiles ObjectConditions, which include NOTCE
        /// </summary>
        public override BaseJoin compileJoin(ICondition condition, int position, Rule.IRule rule)
        {
            Binding[] binds = getBindings(condition, rule, position);
            ObjectCondition oc = (ObjectCondition) condition;
            BaseJoin joinNode = null;
            //deal with the CE which is not NOT CE.
            if (!oc.Negated)
            {
                if (binds.Length > 0 && oc.HasPredicateJoin)
                {
                    joinNode = new PredicateBNode(ruleCompiler.Engine.nextNodeId());
                }
                else if (binds.Length > 0 && oc.HasNotEqual)
                {
                    joinNode = new HashedNotEqBNode(ruleCompiler.Engine.nextNodeId());
                }
                else if (binds.Length > 0)
                {
                    joinNode = new HashedEqBNode(ruleCompiler.Engine.nextNodeId());
                }
                else if (binds.Length == 0)
                {
                    joinNode = new ZJBetaNode(ruleCompiler.Engine.nextNodeId());
                }
            }

            //deal with the CE which is NOT CE.
            if (oc.Negated)
            {
                if (binds.Length > 0 && oc.HasPredicateJoin)
                {
                    joinNode = new NotJoin(ruleCompiler.Engine.nextNodeId());
                }
                else if (oc.HasNotEqual)
                {
                    joinNode = new HashedNotEqNJoin(ruleCompiler.Engine.nextNodeId());
                }
                else
                {
                    joinNode = new HashedEqNJoin(ruleCompiler.Engine.nextNodeId());
                }
            }

            if (joinNode != null) 
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
            ObjectCondition oc = (ObjectCondition) conds[0];
            if (oc.Negated)
            {
                // the ObjectCondition is negated, so we need to
                // handle it appropriate. This means we need to
                // Add a LIANode to _IntialFact and attach a NOTNode
                // to the LIANode.
                ObjectTypeNode otn = (ObjectTypeNode) ruleCompiler.Inputnodes.Get(ruleCompiler.Engine.InitFact);
                LIANode lianode = ruleCompiler.findLIANode(otn);
                NotJoinFrst njoin = new NotJoinFrst(ruleCompiler.Engine.nextNodeId());
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
                //HACK TODO
                if(otn != null)
                {
                    LIANode lianode = new LIANode(ruleCompiler.Engine.nextNodeId());
                    otn.addSuccessorNode(lianode, ruleCompiler.Engine, ruleCompiler.Memory);
                    rule.Conditions[0].addNode(lianode);
                }
            }
        }
    }
}