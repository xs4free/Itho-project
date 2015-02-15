namespace CustomTraceAnalyzer.Analyzers.SupportClasses
{
    class CompareCommandsFile
    {
        /// <summary>
        /// Name of the file the byte arrays are read from
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// The index of the command within the file (0, 1 or 2)
        /// </summary>
        public int BlurbIndex { get; set; }

        /// <summary>
        /// 3 byte arrays for the first part of each repeated command
        /// </summary>
        public byte[] FirstPartBytes { get; set; }

        /// <summary>
        /// 3 byte arrays for the second part of each repeated command
        /// </summary>
        public byte[] SecondPartBytes { get; set; }
    }
}
