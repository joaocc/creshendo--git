using Creshendo.Util.Rule;

namespace Creshendo.Util.Rete.Compiler
{
    /// <summary> 
    /// </summary>
    /// <author>  HouZhanbin
    /// Oct 12, 2007 9:32:31 AM
    /// *
    /// 
    /// </author>
    public interface IConditionCompiler
    {
        /// <summary> 
        /// </summary>
        /// <returns>
        /// 
        /// </returns>
        IRuleCompiler RuleCompiler { get; }

        /// <summary> 
        /// </summary>
        /// <param name="">condition
        /// </param>
        /// <param name="position">
        /// </param>
        /// <param name="">util
        /// </param>
        /// <param name="alphaMemory">
        /// 
        /// </param>
        void compile(ICondition condition, int position, Rule.IRule util, bool alphaMemory);

        /// <summary> 
        /// </summary>
        /// <param name="">condition
        /// </param>
        /// <param name="">rule
        /// 
        /// </param>
        void compileFirstJoin(ICondition condition, Rule.IRule rule);

        /// <summary> 
        /// </summary>
        /// <param name="">condition
        /// </param>
        /// <param name="">position
        /// </param>
        /// <param name="">rule
        /// </param>
        /// <returns>
        /// 
        /// </returns>
        BaseJoin compileJoin(ICondition condition, int position, Rule.IRule rule);

        /// <summary> 
        /// </summary>
        /// <param name="">condition
        /// </param>
        /// <param name="">joinNode
        /// 
        /// </param>
        void connectJoinNode(ICondition previousCondition, ICondition condition, BaseJoin previousJoinNode, BaseJoin joinNode);

        /// <summary> compile the only CE in the rule which has only one CE.
        /// </summary>
        /// <param name="">rule
        /// 
        /// </param>
        void compileSingleCE(Rule.IRule rule);
    }
}