using System;
using System.Collections.Generic;
using System.Text;
using CustomTraceAnalyzer.Extensions;

namespace CustomTraceAnalyzer.Analyzers
{
    class ConvertToByteArray : IAnalyzer
    {
        public string FileName { get; set; }

        public ConvertToByteArray(string fileName)
        {
            FileName = fileName;
        }

        void IAnalyzer.Analyze()
        {
            var lines = CsvReader.ReadFile(FileName);

            // output an array for all 3 blurbs (each button press generates 3 commands)
            for (int indexBlurb = 0; indexBlurb < 3; indexBlurb++)
            {
                // output a array for each of the 2 parts (each blurb consists of 2 parts)
                for (int indexPart = 0; indexPart < 2; indexPart++)
                {
                    var partLines = indexPart == 0 ? lines.GetFirstPart(indexBlurb) : lines.GetSecondPart(indexBlurb);
                    string arrayName = String.Format("itho_{0}_{1}", indexPart == 0 ? "RF" : "RFT", indexBlurb);

                    Console.WriteLine(GenerateArray(partLines, arrayName));
                }
            }
        }

        /// <summary>
        /// Generate a C array declaration based on the CSV partLines
        /// </summary>
        /// <param name="partLines"></param>
        /// <param name="arrayName"></param>
        /// <returns>
        /// const byte itho_RF_Full[] = {
        /// 170, 170, 170, 173, 51,  83,  74, 203, 76, 205, 84,  213,  85,  51, 82, 180, 170, 171, 85, 75 
        /// };
        /// </returns>
        private string GenerateArray(IEnumerable<CsvLine> partLines, string arrayName)
        {
            var result = new StringBuilder();

            result.AppendLine(String.Format("const byte {0}[] = {{ ", arrayName));

            byte[] bytes = partLines.GenerateBytes();

            for (int index = 0; index < bytes.Length; index++)
            {
                result.Append(bytes[index]);

                if (index < bytes.Length - 1)
                {
                    result.Append(", ");
                }
            }

            result.AppendLine();
            result.AppendLine("}}");

            return result.ToString();
        }
    }
}
