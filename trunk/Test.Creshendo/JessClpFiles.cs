using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Creshendo.Util.Rete;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Creshendo
{
    [TestClass]
    public class JessClpFiles : FileTestBase
    {
        [TestMethod]
        public void RunTests()
        {

            var root = @"C:\Src\Creshendo\Test.Creshendo\ClpFiles";

            foreach (string d in Directory.GetDirectories(root))
            {
                foreach (string f in Directory.GetFiles(d, "*.clp", SearchOption.AllDirectories))
                {
                    RunRules(d, f);
                }
            } 
        }

        private void RunRules(string dir, string clpFile)
        {

            var outFile = Path.Combine(dir, String.Concat(Path.GetFileNameWithoutExtension(clpFile), ".out"));
            var refFile = Path.Combine(dir, String.Concat(Path.GetFileNameWithoutExtension(clpFile), ".ref"));

            using (TextWriter writer = new StreamWriter(outFile))
	        {
                Rete engine = new Rete();
                engine.addPrintWriter("File", writer);
                engine.loadRuleset(clpFile);
                engine.printWorkingMemory(false, false);
                engine.close();
                writer.Flush();
                writer.Close();
            }

            var outTxt = File.ReadAllText(outFile);
            var refTxt = File.ReadAllText(refFile);
        }
    }
}
