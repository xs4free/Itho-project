using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CustomTraceAnalyzer
{
    static class CsvReader
    {
        public static string Seperator { get; set; }

        static CsvReader()
        {
            Seperator = ",";
        }


        public static IEnumerable<CsvLine> ReadFile(string fileName)
        {
            var lines = File.ReadAllLines(fileName);

            return lines
                .Skip(1) //skip the header line
                .Select(ConvertLine)
                .ToList();
        }

        private static CsvLine ConvertLine(string input)
        {
            var split = input.Split(new[] {Seperator}, StringSplitOptions.None);

            CsvLine result = null;

            if (split.Length == 2)
            {
                long timeInFs = long.Parse(split[0].Replace(".", ""));
                
                result = new CsvLine
                {
                    TimeInFS = timeInFs, 
                    Value = split[1]
                };
            }

            return result;
        }
    }
}
