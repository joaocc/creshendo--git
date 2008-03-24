using Creshendo.Functions;
using Creshendo.Util.Rule;

namespace Creshendo.Util.Rete.Compiler
{
    /// <summary> 
    /// </summary>
    /// <author>  HouZhanbin
    /// Oct 12, 2007 2:18:29 PM
    /// *
    /// 
    /// </author>
    public class TestConditionCompiler : IConditionCompiler
    {
        public DefaultRuleCompiler ruleCompiler;

        public TestConditionCompiler(IRuleCompiler ruleCompiler)
        {
            this.ruleCompiler = (DefaultRuleCompiler) ruleCompiler;
        }

        #region ConditionCompiler Members

        public virtual IRuleCompiler RuleCompiler
        {
            get { return ruleCompiler; }
        }

        public virtual void compile(ICondition condition, int position, Rule.IRule util, bool alphaMemory)
        {
            // TODO Auto-generated method stub
        }

        public virtual void compileFirstJoin(ICondition condition, Rule.IRule rule)
        {
            // TODO Auto-generated method stub
        }


        /// <summary> the method is responsible for compiling a TestCE pattern to a testjoin node.
        /// It uses the globally declared prevCE and prevJoinNode
        /// </summary>
        public virtual BaseJoin compileJoin(ICondition condition, int position, Rule.IRule rule)
        {
            TestCondition tc = (TestCondition) condition;
            ShellFunction fn = (ShellFunction) tc.Function;
            fn.lookUpFunction(ruleCompiler.Engine);
            IParameter[] oldpm = fn.Parameters;
            IParameter[] pms = new IParameter[oldpm.Length];
            for (int ipm = 0; ipm < pms.Length; ipm++)
            {
                if (oldpm[ipm] is ValueParam)
                {
                    pms[ipm] = ((ValueParam) oldpm[ipm]).cloneParameter();
                }
                else if (oldpm[ipm] is BoundParam)
                {
                    BoundParam bpm = (BoundParam) oldpm[ipm];
                    // now we need to resolve and setup the BoundParam
                    Binding b = rule.getBinding(bpm.VariableName);
                    BoundParam newpm = new BoundParam(b.LeftRow, b.LeftIndex, 9, bpm.ObjectBinding);
                    newpm.VariableName = bpm.VariableName;
                    pms[ipm] = newpm;
                }
            }
            BaseJoin joinNode = null;
            if (tc.Negated)
            {
                joinNode = new NTestNode(ruleCompiler.Engine.nextNodeId(), fn.Function, pms);
            }
            else
            {
                joinNode = new TestNode(ruleCompiler.Engine.nextNodeId(), fn.Function, pms);
            }
            ((TestNode) joinNode).lookUpFunction(ruleCompiler.Engine);
            return joinNode;
        }


        public virtual void connectJoinNode(ICondition previousCondition, ICondition condition, BaseJoin previousJoinNode, BaseJoin joinNode)
        {
            if (previousJoinNode != null)
            {
                ruleCompiler.attachJoinNode(previousJoinNode, (BaseJoin) joinNode);
            }
            else
            {
                ruleCompiler.attachJoinNode(previousCondition.LastNode, (BaseJoin) joinNode);
            }
        }

        public virtual void compileSingleCE(Rule.IRule rule)
        {
            // TODO Auto-generated method stub
        }

        #endregion
    }
}