using BenchmarkDotNet.Running;

namespace Redgum.Logging.Extensions.Justifications
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
        }
    }
}