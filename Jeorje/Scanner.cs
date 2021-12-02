using System.Collections.Generic;
using System.Linq;

namespace Jeorje
{
    public static class Scanner
    {
        private static readonly List<char> _paddedItems = new List<char>()
        {
            '!',
            '&',
            '|',
            '.',
            ':',
            '{',
            '}',
            '(',
            ')',
            '+',
            '-',
            '*',
            '/',
            ',',
            '#'
        };

        public static List<Line> ScanInput(string[] input)
        {
           return input.ToList().Select(line => ScanLine(line)).ToList();
        }

        private static Line ScanLine(string line)
        {
            var transformedLine = TransformLine(line);
            var splitLine = transformedLine.Split(" ");
            
            return new Line(splitLine.Where(e => !string.IsNullOrEmpty(e)).Select(token => new Token(token)).ToList());
        }

        private static string TransformLine(string line)
        {
            var lineList = line.ToList();

            var i = 0;

            while (i < lineList.Count)
            {
                if (_paddedItems.Contains(lineList[i])) // should pad item
                {
                    lineList.Insert(i+1, ' '); // inserts space after
                    lineList.Insert(i, ' '); // inserts space before
                    i++;
                }

                i++;
            }

            return new string(lineList.ToArray());
        }
        
    }
}