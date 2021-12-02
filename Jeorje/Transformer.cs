using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Jeorje
{
    public static class Transformer
    {
        public static (CheckType, List<Line>, Line, List<Line>) TransformLines(List<Line> lines)
        {
            CollapseEntails(lines); // modifies lines
            RemoveEmptyLines(lines); // modifies lines
            
            var checkType = FindCheckType(lines); // modifies lines
            (List<Line> predicates, Line goal) = FindPremisesAndGoal(lines); // modifies lines
            List<Line> proof = lines;

            return (checkType, predicates, goal, proof);
        }

        private static void CollapseEntails(List<Line> lines)
        {
            foreach (var line in lines)
            {
                var i = 0;
                while (i < line.Tokens.Count-1)
                {
                    if (line.Tokens[i].TokenType == TokenType.Or && line.Tokens[i + 1].Lexeme == "-")
                    {
                        var temp = line.Tokens.GetRange(0, i);
                        temp.Add(new Token(TokenType.Entails, "|-"));
                        if (i + 2 != line.Tokens.Count)
                        {
                            temp.AddRange(line.Tokens.GetRange(i+2, line.Tokens.Count-i-2));
                        }
                        
                        line.Tokens = temp;
                    }
                }
            }
            
        }

        private static void RemoveEmptyLines(List<Line> lines)
        {
            lines.RemoveAll(line => line.Tokens.Count == 0);
        }
        
        private static CheckType FindCheckType(List<Line> lines)
        {
            int checkIndex = 0;
            
            while (lines[checkIndex].Tokens[0].TokenType != TokenType.Hashtag)
            { 
                if (lines[checkIndex].Tokens[0].TokenType == TokenType.Hashtag)
                {
                    break;
                }

                if (checkIndex + 1 == lines.Count)
                {
                    throw new Exception(
                        "No check type found (you probably need to include #check ND at the top of the file)");
                }

                checkIndex++;
            }

            if (lines[checkIndex].Tokens[1].Lexeme != "check" || lines[checkIndex].Tokens.Count != 3)
            {
                throw new Exception($"Line {checkIndex+1} must be in the format: #check \"checkType\"");
            }
            
            return (CheckType) Enum.Parse(typeof(CheckType), lines[checkIndex].Tokens[2].Lexeme);

        }

        private static (List<Line>, Line) FindPremisesAndGoal(List<Line> lines)
        {
            var currentLineIndex = 0;
            while (lines[currentLineIndex].Tokens[0].TokenType != TokenType.Entails)
            {
                currentLineIndex++;
            }

            var premises = lines.GetRange(0, currentLineIndex);
            var goal = lines[currentLineIndex + 1];
            lines.RemoveRange(0, currentLineIndex+2);

            return (premises, goal);
        }
    }
}