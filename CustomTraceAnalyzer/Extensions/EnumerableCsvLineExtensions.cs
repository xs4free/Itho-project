using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomTraceAnalyzer.Extensions
{
    static class EnumerableCsvLineExtensions
    {
        /// <summary>
        /// Return the relevant lines for the first parallel transmission
        /// </summary>
        public static IEnumerable<CsvLine> GetFirstPart(this IEnumerable<CsvLine> lines, int index)
        {
            int skip = GetFirstLineIndex(index) + 118;

            return lines.Skip(skip).Take(160);
        }

        public static IEnumerable<CsvLine> GetSecondPart(this IEnumerable<CsvLine> lines, int index)
        {
            int skip = GetFirstLineIndex(index) + 406;

            return lines.Skip(406).Take(403);
        }

        /// <summary>
        /// Get the first line index for a button message (each trace-file contains 3 messages for each button-press)
        /// </summary>
        /// <param name="blurbIndex"></param>
        /// <returns></returns>
        private static int GetFirstLineIndex(int blurbIndex)
        {
            int line;

            switch (blurbIndex)
            {
                default:
                case 0:
                    line = 2; //MOSI 0x30
                    break;
                case 1:
                    line = 822;
                    break;
                case 2:
                    line = 1642;
                    break;

            }

            return line - 2; // correct the number because the first line in the CSV is the header and an index should be 0-based while the line-numbers are 1 based.
        }


        /// <summary>
        /// Generate a byte array from the supplied CsvLines
        /// </summary>
        public static byte[] GenerateBytes(this IEnumerable<CsvLine> partLines)
        {
            var bytes = new List<byte>();

            int partLinesCount = partLines.Count();

            for (int index = 0; index < partLinesCount; index += 8)
            {
                var byteLines = partLines.Skip(index).Take(8);
                byte value = GenerateByte(byteLines);

                bytes.Add(value);
            }

            return bytes.ToArray();
        }

        /// <summary>
        /// Convert 8 CsvLines to a byte
        /// </summary>
        private static byte GenerateByte(IEnumerable<CsvLine> byteLines)
        {
            var bits = byteLines.Select(line => line.ValueBit ? "1" : "0").ToArray();

            StringBuilder bitString = new StringBuilder();
            foreach (var bit in bits)
            {
                bitString.Append(bit);
            }

            byte result = Convert.ToByte(bitString.ToString(), 2);
            return result;
        }

    }
}
