using System;
using System.Collections.Generic;
using Gentzen.Gentzen.Common;

namespace Gentzen.Gentzen
{
    public static class Cleaner
    {
        public static (CheckType, List<Line>, Line, List<Line>) CleanLines(List<Line> input)
        {
            var checkType = CheckType.ND;
            List<Line> predicates = null;
            Line goal = null;
            List<Line> lines = null;

            var currentLineIndex = 0;

            while (input[currentLineIndex].Tokens[0].TokenType != TokenType.Hashtag)
            {
                currentLineIndex++;

                if (currentLineIndex == input.Count) // no hashtag found in input file
                {
                    throw new Exception("No check type found");
                }
            }

            if (currentLineIndex != input.Count) // if there is a hashtag, removes all lines 
            {
                input.RemoveRange(0, currentLineIndex+1);
            }

            return (checkType, predicates, goal, lines);
        }
    }
}