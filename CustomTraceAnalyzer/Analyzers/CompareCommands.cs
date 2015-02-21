using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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
            var outputFileName1 = Path.Combine(DirectoryName, "CompareResults-Part1.csv");
            var outputFileName2 = Path.Combine(DirectoryName, "CompareResults-Part2.csv");

            CheckArraysWithinFile(bytes);
            WriteCsv(outputFileName1, true, bytes);
            WriteCsv(outputFileName2, false, bytes);
        }

        private void CheckArraysWithinFile(IEnumerable<CompareCommandsFile> bytes)
        {
            var filesGrouped = bytes.GroupBy(file => file.FileName);
            foreach (var group in filesGrouped)
            {
                var firstBytes = @group.Select(g => g.FirstPartBytes);
                if (!CompareArrays(firstBytes))
                {
                    Console.WriteLine("First bytes differ in file: " + group.Key);
                }

                var secondBytes = @group.Select(g => g.SecondPartBytes);
                if (!CompareArrays(secondBytes))
                {
                    Console.WriteLine("Second bytes differ in file: " + group.Key);
                }
            }
        }


        private void WriteCsv(string outputFileName, bool firstParts, List<CompareCommandsFile> compareCommandsFiles)
        {
            using (var file = File.CreateText(outputFileName))
            {
                var headerFileNames = compareCommandsFiles.Where(ccf => ccf.BlurbIndex == 0).Select(ccf => Path.GetFileName(ccf.FileName)).ToArray();
                string header = String.Join(",", headerFileNames);
                file.WriteLine(header);

                var arrays = compareCommandsFiles
                    .Where(ccf => ccf.BlurbIndex == 0) // only output the first blurb of each part, since we compared the other blurbs and they are the same
                    .Select(ccf => firstParts ? ccf.FirstPartBytes : ccf.SecondPartBytes);

                var lines = new string[arrays.First().Length];

                foreach (var array in arrays)
                {
                    for (int index = 0; index < array.Length; index++)
                    {
                        lines[index] += "," + array[index];
                    }
                }

                foreach (var line in lines)
                {
                    file.WriteLine(line.Trim(new [] {','}));
                }
            }
        }

        private bool CompareArrays(IEnumerable<byte[]> firstParts)
        {
            bool result = true;

            var firstArray = firstParts.First();
            var otherArrays = firstParts.Skip(1);

            foreach (var secondArray in otherArrays)
            {
                if (!CompareArray(firstArray, secondArray))
                {
                    result = false;
                    break;
                }
            }

            return result;
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
