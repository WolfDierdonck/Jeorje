using System;
using System.Collections.Generic;

namespace Jeorje
{
    public class STRulifier
    {
        public static STBranch RulifyLines(List<Line> lines)
        {

            STBranch branch = new STBranch();
            int i = 0;
            while (i < lines.Count)
            {
                Line currentLine = lines[i];
                if (currentLine.Tokens[0].TokenType == TokenType.LBrace)
                {
                    // Branch
                    
                } else if (currentLine.Tokens[0].TokenType == TokenType.Identifier && currentLine.Tokens[0].Lexeme == "by")
                {
                    // Rule
                    
                } else if (currentLine.Tokens[0].TokenType == TokenType.Label &&
                           currentLine.Tokens[1].TokenType == TokenType.LParen)
                {
                    // STLine
                    
                } else if (currentLine.Tokens[0].TokenType == TokenType.Identifier &&
                           currentLine.Tokens[0].Lexeme == "closed")
                {
                    // Closed
                    
                }
            }
            return branch;
        }
    }
}