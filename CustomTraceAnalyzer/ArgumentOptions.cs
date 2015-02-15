using CommandLine;

namespace CustomTraceAnalyzer
{
    class ArgumentOptions
    {
        [Option('a', "ConvertToByteArray", DefaultValue = false, HelpText = "Convert a parallel CSV trace file into 2 C arrays so they can be included in Arduino code.")]
        public bool ConvertToByteArray { get; set; }

        [Option('i', "input", HelpText = "The file to read")]
        public string InputFile { get; set; }

        [Option('c', "CompareCommands", DefaultValue = false, HelpText = "Compare the commands of both parts in all CSV trace files in the given directory")]
        public bool CompareCommands { get; set; }

        [Option('d', "InputDirectory", HelpText = "The directory to read all CSV-files from")]
        public string InputDirectory { get; set; }
    }
}
