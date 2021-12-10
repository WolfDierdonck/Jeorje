using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;

namespace Jeorje
{
    public static class Transformer
    {
        public static ProofFormat TransformTokens(List<Token> tokens)
        {
            var modifiedTokens = InsertHelperTokens(tokens).Where(token => 
                token.TokenType != TokenType.Comment && token.TokenType != TokenType.Whitespace
                ).ToList();

            var hashtagIndex = modifiedTokens.FindIndex(token => token.TokenType == TokenType.Hashtag);
            var checkType = (CheckType) Enum.Parse(typeof(CheckType), modifiedTokens[hashtagIndex+2].Lexeme);
            modifiedTokens.RemoveRange(0, hashtagIndex+3);
            
            switch (checkType)
            {
                case CheckType.ND:
                    return GetNDFormat(modifiedTokens);
                case CheckType.ST:
                    return GetSTFormat(modifiedTokens);
                default:
                    throw new Exception($"Check type {checkType.ToString()} not supported yet");
            } 
        }

        private static List<Token> InsertHelperTokens(List<Token> tokens)
        {
            var updatedTokens = tokens;
            var i = 0;

            while (i < tokens.Count)
            {
                if (tokens[i].TokenType == TokenType.Not)
                {
                    updatedTokens.Insert(i, new Token(TokenType.DummyNotOperand, "$"));
                    i++;
                }
                else if (tokens[i].TokenType is TokenType.Forall or TokenType.Exists)
                {
                    updatedTokens.Insert(i, new Token(TokenType.DummyQuantifierOperand, "`"));
                    i++;
                }
                else if (i < tokens.Count - 1 &&
                         tokens[i].TokenType == TokenType.Identifier && tokens[i + 1].TokenType == TokenType.LParen)
                {
                    updatedTokens.Insert(i + 1, new Token(TokenType.FuncSeparator, "@"));
                    i++;
                }

                i++;
            }

            return updatedTokens;
        }

        private static STFormat GetSTFormat(List<Token> tokens)
        {
            // This method transforms a list of tokens into an ST format
            // Includes turning tokens into lines
            
            // First must check for premises.
            var i = 0;
            var premises = new List<Line>();

            var lparenCount = 0;
            var quantifierCount = 0;
                    
            while (tokens[i].TokenType != TokenType.Entails)
            {
                if (tokens[i].TokenType == TokenType.Comma && lparenCount == 0 && quantifierCount == 0)
                {
                    premises.Add(new Line(tokens.GetRange(0, i)));
                    tokens.RemoveRange(0, i+1);
                    i = 0;
                }
                else
                {
                    if (tokens[i].TokenType == TokenType.LParen)
                    {
                        lparenCount++;
                    }
                    if (tokens[i].TokenType == TokenType.RParen) 
                    {
                        lparenCount--;
                    }
                    if (tokens[i].TokenType is TokenType.Forall or TokenType.Exists)
                    {
                        quantifierCount++;
                    }
                    if (tokens[i].TokenType == TokenType.Dot)
                    {
                        quantifierCount--;
                    }
                    
                    i++;
                }
            }
            
            premises.Add(new Line(tokens.GetRange(0, i)));
            tokens.RemoveRange(0, i+1);
            
            // Now must check for goal
            i = 0;
            while (tokens[i].TokenType != TokenType.Label)
            {
                i++;
            }

            var goal = new Line(tokens.GetRange(0, i));
            tokens.RemoveRange(0, i);
            
            // Now must check for proof (LMAO)
            var proof = new List<Line>();
            
            i = 1; // first token will be a label i guess but this freaking sucks wfuewhfuewhfeuhfeufhewufew
            while (i < tokens.Count)
            {
                if (i < tokens.Count - 1 && tokens[i].TokenType == TokenType.Label &&
                     tokens[i + 1].TokenType == TokenType.RParen) 
                {
                    proof.Add(new Line(tokens.GetRange(0, i)));
                    tokens.RemoveRange(0, i);
                    i = 1;
                } else if (tokens[i].TokenType == TokenType.LBrace
                    || tokens[i].TokenType == TokenType.RBrace)
                {
                    proof.Add(new Line(tokens.GetRange(0, i-1)));
                    tokens.RemoveRange(0, i-1);
                    proof.Add(new Line(tokens.GetRange(0, 1)));
                    tokens.RemoveRange(0, 1);
                } else if (tokens[i].TokenType == TokenType.Identifier && tokens[i].Lexeme == "by")
                {
                    proof.Add(new Line(tokens.GetRange(0, i)));
                    tokens.RemoveRange(0, i);
                }
                else
                {
                    i++;
                }
            }
            proof.Add(new Line(tokens.GetRange(0, i)));
            return null;
        }
        
        private static NDFormat GetNDFormat(List<Token> tokens)
        {
            var i = 0;
            var premises = new List<Line>();

            var lparenCount = 0;
            var quantifierCount = 0;
                    
            while (tokens[i].TokenType != TokenType.Entails)
            {
                if (tokens[i].TokenType == TokenType.Comma && lparenCount == 0 && quantifierCount == 0)
                {
                    premises.Add(new Line(tokens.GetRange(0, i)));
                    tokens.RemoveRange(0, i+1);
                    i = 0;
                }
                else
                {
                    if (tokens[i].TokenType == TokenType.LParen)
                    {
                        lparenCount++;
                    }
                    if (tokens[i].TokenType == TokenType.RParen) 
                    {
                        lparenCount--;
                    }
                    if (tokens[i].TokenType is TokenType.Forall or TokenType.Exists)
                    {
                        quantifierCount++;
                    }
                    if (tokens[i].TokenType == TokenType.Dot)
                    {
                        quantifierCount--;
                    }
                    
                    i++;
                }
            }
            
            premises.Add(new Line(tokens.GetRange(0, i)));
            tokens.RemoveRange(0, i+1);

            
            i = 0;
            while (tokens[i].TokenType != TokenType.Label)
            {
                i++;
            }

            var goal = new Line(tokens.GetRange(0, i));
            tokens.RemoveRange(0, i);


            var proof = new List<Line>();
            i = 1; // first token will be a label, so we must start at 1
            while (i < tokens.Count)
            {
                if ((i < tokens.Count - 1 && tokens[i].TokenType == TokenType.Label &&
                     tokens[i + 1].TokenType == TokenType.RParen) 
                    || tokens[i].TokenType == TokenType.LBrace
                    || tokens[i].TokenType == TokenType.RBrace)
                {
                    proof.Add(new Line(tokens.GetRange(0, i)));
                    tokens.RemoveRange(0, i);
                    i = 1;
                }
                else
                {
                    i++;
                }
            }

            proof.Add(new Line(tokens.GetRange(0, i)));

            return new NDFormat(premises, goal, proof);
        }
    }
}