using System.Collections.Generic;
using System.IO;

namespace AICore.Utils
{
    public static class FileReaderUtils
    {
        public static List<string> ReadFile(string filename)
        {
            return ReadFile(new StreamReader(filename));
        }

        public static List<string> ReadFile(StreamReader file)
        {
            var result = new List<string>();
            string line;
            while ((line = file.ReadLine()) != null)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    result.Add(line);
                }
            }
            return result;
        }
    }
}