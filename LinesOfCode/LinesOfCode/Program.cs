using System;

namespace MarvinEde
{
    namespace LinesOfCode
    {
        class Program
        {
            static void Main(string[] args)
            {
                var path = args[0];
                Console.WriteLine($"Directory or file {path} has {LineCount.LineCount.CountLines(path)} lines of code.");
            }
        }
    }
}
