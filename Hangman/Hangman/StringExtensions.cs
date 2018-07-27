using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Hangman
{
    public static class StringExtensions
    {
        public static string Times(this string str, int times)
        {
            var result = new StringBuilder("");
            for (int i = 0; i < times; i++)
            {
                result.Append(str);
            }
            return result.ToString();
        }

        public static int[] IndexOfAll(this string str, string sub)
        {
            var letterAsChar = Convert.ToChar(sub);
            return str.EachWithIndex().Where(pair => pair.Item2 == letterAsChar).Select(pair => pair.Item1).ToArray();
        }

        public static string ReplaceAt(this string str, string newLetter, params int[] indices)
        {
            var result = str;
            foreach (var index in indices)
            {
                result = result.ReplaceAt(newLetter, index);
            }
            return result;
        }

        public static string ReplaceAt(this string str, string newLetter, int index)
        {
            return str.Remove(index, 1).Insert(index, newLetter);
        }
    }
}
