using System.Collections.Generic;
using System.IO;
using System.Linq;
using CustomTraceAnalyzer.Analyzers.SupportClasses;
using CustomTraceAnalyzer.Extensions;

namespace CustomTraceAnalyzer.Analyzers
{
    class CompareCommands : IAnalyzer
    {
        public string DirectoryName { get; set; }
        private const string SearchPattern = "*.csv";

        public CompareCommands(string directory)
        {
            DirectoryName = directory;
        }

        public void Analyze()
        {
            var bytes = ReadFiles();

            var firstParts = bytes.Select(file => file.FirstPartBytes).ToList();
            var secondParts = bytes.Select(file => file.FirstPartBytes).ToList();

            CompareArrays(firstParts);
        }

        private void CompareArrays(List<byte[]> firstParts)
        {
            var firstArray = firstParts.First();
            var otherArrays = firstParts.Skip(1);

            foreach (var secondArray in otherArrays)
            {
                if (!CompareArray(firstArray, secondArray))
                {
                    
                }
            }
        }

        private bool CompareArray<T>(T[] one, T[] two)
        {
            bool equal = false;

            if (one.Length == two.Length)
            {
                for (int index = 0; index < one.Length; index++)
                {
                    equal = one[index].Equals(two[index]);
                }
            }

            return equal;
        }

        private List<CompareCommandsFile> ReadFiles()
        {
            var bytes = new List<CompareCommandsFile>();
            
            var files = Directory.GetFiles(DirectoryName, SearchPattern);

            foreach (var file in files)
            {
                var lines = CsvReader.ReadFile(file);

                for (int blurb = 0; blurb < 3; blurb++)
                {
                    var bytesBlurbFirst = lines.GetFirstPart(blurb).GenerateBytes();
                    var bytesBlurbSecond = lines.GetSecondPart(blurb).GenerateBytes();

                    var ccf = new CompareCommandsFile
                    {
                        FileName = file,
                        BlurbIndex = blurb,
                        FirstPartBytes = bytesBlurbFirst,
                        SecondPartBytes = bytesBlurbSecond
                    };

                    bytes.Add(ccf);
                }
            }

            return bytes;
        }
    }
}
