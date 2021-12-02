using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace Jeorje
{
    public static class Transformer
    {
        public static ProofFormat TransformLines(List<Line> lines)
        {
            CollapseEntails(lines); // modifies lines
            RemoveEmptyLines(lines); // modifies lines
            InsertHelperTokens(lines); // modifies lines
            
            var checkType = FindCheckType(lines); // modifies lines

            switch (checkType)
            {
                case CheckType.ND:
                    var t1 = FindPremisesAndGoal(lines); // modifies lines
                    return new NDFormat(t1.Item1, t1.Item2, lines);
                
                case CheckType.ST:
                    var t2 = FindPremisesAndGoal(lines); // modifies lines
                    return new STFormat(t2.Item1, t2.Item2, lines);
                
                case CheckType.PC:
                    return new PCFormat(lines);
                
                default:
                    throw new Exception($"check type {checkType.ToString()} not supported yet");
            } 
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

                    i++;
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
            
            var checkType = (CheckType) Enum.Parse(typeof(CheckType), lines[checkIndex].Tokens[2].Lexeme);
            lines.RemoveRange(0, checkIndex+1);

            return checkType;

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

        /// <summary>
        /// inserts tokens between function name and bracket and to the left of NOT characters
        /// </summary>
        /// <param name="lines"></param>
        private static void InsertHelperTokens(List<Line> lines)
        {
            foreach (var line in lines)
            {
                var tokens = line.Tokens;
                var i = 0;

                while (i < tokens.Count)
                {
                    if (tokens[i].TokenType == TokenType.Or)
                    {
                        tokens.Insert(i, new Token(TokenType.DummyNotOperand, "$"));
                    }
                    else if (i < tokens.Count - 1 &&
                             tokens[i].TokenType == TokenType.Identifier && tokens[i + 1].TokenType == TokenType.LParen)
                    {
                        tokens.Insert(i+1, new Token(TokenType.FuncSeparator, "@"));
                    }

                    i++;
                }
            }
        }
    }
}