namespace Creshendo.Util.Rete.Compiler
{
    /// <summary> 
    /// </summary>
    /// <author>  HouZhanbin
    /// Oct 12, 2007 10:24:19 AM
    /// *
    /// 
    /// </author>
    public class CompilerProvider
    {
        private static CompilerProvider compilerProvider;

        public static IConditionCompiler existConditionCompiler;
        public static IConditionCompiler objectConditionCompiler;

        public static IConditionCompiler temporalConditionCompiler;

        public static IConditionCompiler testConditionCompiler;

        public static CompilerProvider getInstance(IRuleCompiler ruleCompiler)
        {
            if (compilerProvider == null)
            {
                objectConditionCompiler = new ObjectConditionCompiler(ruleCompiler);
                existConditionCompiler = new ExistConditionCompiler(objectConditionCompiler);
                temporalConditionCompiler = new TemporalConditionCompiler(ruleCompiler);
                testConditionCompiler = new TestConditionCompiler(ruleCompiler);
                compilerProvider = new CompilerProvider();
            }
            return compilerProvider;
        }

        public static void reset()
        {
            objectConditionCompiler = null;
            existConditionCompiler = null;
            temporalConditionCompiler = null;
            testConditionCompiler = null;
            compilerProvider = null;
        }
    }
}