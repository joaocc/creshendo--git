using System;
using System.Collections;
using System.IO;
using Creshendo.Functions;
using Creshendo.Util.Parser.Clips2;
using Creshendo.Util.Rete;
using Creshendo.Util.Rule;
using Random=System.Random;

namespace Test.Creshendo
{
    public abstract class FileTestBase
    {
        protected static Random ran = new Random();
        private const string mypath = @"C:\Src\Creshendo\Test.Creshendo\Data";


        protected string getRoot(string fileName)
        {
            FileInfo file = new FileInfo(Path.Combine(mypath, fileName));

            if (file.Exists)
                return file.FullName;

            throw new FileNotFoundException("File not found", file.FullName);
        }

        protected string getRoot(string fileName, bool create)
        {
            FileInfo file = new FileInfo(Path.Combine(mypath, fileName));

            if (file.Exists)
                return file.FullName;
            if (create)
                file.Create();
            return file.FullName;
        }

        protected void parse(Rete engine, CLIPSParser parser, IList factlist)
        {
            Object itm = null;
            try
            {
                while ((itm = parser.basicExpr()) != null)
                {
                    // System.Console.WriteLine("obj is " + itm.getClass().Name);
                    if (itm is Defrule)
                    {
                        Defrule rule = (Defrule) itm;
                        engine.RuleCompiler.addRule(rule);
                    }
                    else if (itm is Deftemplate)
                    {
                        Deftemplate dt = (Deftemplate) itm;
                        Console.WriteLine("template=" + dt.Name);
                        engine.declareTemplate(dt);
                    }
                    else if (itm is FunctionAction)
                    {
                        FunctionAction fa = (FunctionAction) itm;
                    }
                    else if (itm is IFunction)
                    {
                        IReturnVector rv = ((IFunction) itm).executeFunction(engine, null);
                        IEnumerator itr = rv.Iterator;
                        while (itr.MoveNext())
                        {
                            IReturnValue rval = (IReturnValue) itr.Current;
                            Console.WriteLine(rval.StringValue);
                        }
                    }
                }
            }
            catch
            {
                // Console.WriteLine(e.Message);
                parser.ReInit(Console.OpenStandardInput());
            }
        }
    }
}