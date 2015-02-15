using System;

namespace CustomTraceAnalyzer
{
    class CsvLine
    {
        /// <summary>
        /// Time in Femtoseconds (10 ^ -15)
        /// </summary>
        /// <example>
        /// 261833333333
        /// </example>
        public long TimeInFS { get; set; }

        /// <summary>
        /// Time in seconds (based on TimeInFS)
        /// </summary>
        /// <example>
        /// 0.000261833333333
        /// </example>
        public double TimeInSeconds
        {
            get { return TimeInFS*(10*Math.Exp(-16)); }
        }

        /// <summary>
        /// The value 
        /// </summary>
        /// <example>
        /// 0b  1111  1111  1111  1111
        /// </example>
        public string Value { get; set; }

        /// <summary>
        /// The last bit of Value
        /// </summary>
        /// <example>
        /// true if 1 else false
        /// </example>
        public bool ValueBit
        {
            get { return Value.Substring(Value.Length - 1) == "1"; }
        }
    }
}
