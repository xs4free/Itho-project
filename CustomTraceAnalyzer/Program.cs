using CustomTraceAnalyzer.Analyzers;

namespace CustomTraceAnalyzer
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new ArgumentOptions();
            IAnalyzer analyzer = null;

            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                if (options.ConvertToByteArray)
                {
                    analyzer = new ConvertToByteArray(options.InputFile);
                }
                else if (options.CompareCommands)
                {
                    analyzer = new CompareCommands(options.InputDirectory);
                }
            }

            if (analyzer != null)
            {
                analyzer.Analyze();
            }
        }
    }
}
