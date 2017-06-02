using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace MarvinEde
{
    namespace LineCount
    {
        /// <summary>
        /// Counts lines in a directory
        /// </summary>
        public class LineCount
        {
            private static readonly Regex EmptyLineRegex = new Regex("^(\\s*)(//.*)?$");

            public static int CountLines(string path)
            {
                if (Directory.Exists(path))
                    return CountLinesFromDirectory(0, path);
                else
                    return CountLinesFromFile(0, path);
            }

            private static int CountLinesFromDirectory(int count, string directory)
            {
                count += Directory.EnumerateFiles(directory).Aggregate(0, CountLinesFromFile);
                count += Directory.EnumerateDirectories(directory).Aggregate(0, CountLinesFromDirectory);
                return count;
            }

            private static int CountLinesFromFile(int count, string file)
            {
                return count + File.ReadLines(file).Count(IsCodeLine);
            }

            private static bool IsCodeLine(string line)
            {
                return !EmptyLineRegex.IsMatch(line);
            }
        }
    }
}
