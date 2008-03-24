using Creshendo.Util.Rete;

namespace Creshendo.Shell
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Rete engine = new Rete();
            Util.Rete.Shell shell = new Util.Rete.Shell(engine);
            shell.Run();
        }
    }
}