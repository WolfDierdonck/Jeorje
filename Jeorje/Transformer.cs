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

                default:
                    throw new Exception($"check type {checkType.ToString()} not supported yet");
            } 
        }

        private static List<Token> InsertHelperTokens(List<Token> tokens)
        {
            var updatedTokens = tokens;
            var i = 0;

            while (i < tokens.Count)
            {
                if (tokens[i].TokenType == TokenType.Or)
                {
                    updatedTokens.Insert(i, new Token(TokenType.DummyNotOperand, "$"));
                }
                else if (i < tokens.Count - 1 &&
                         tokens[i].TokenType == TokenType.Identifier && tokens[i + 1].TokenType == TokenType.LParen)
                {
                    updatedTokens.Insert(i + 1, new Token(TokenType.FuncSeparator, "@"));
                }

                i++;
            }

            return updatedTokens;
        }

        private static NDFormat GetNDFormat(List<Token> tokens)
        {
            var i = 0;
            var predicates = new List<Line>();
                    
            while (tokens[i].TokenType != TokenType.Entails)
            {
                if (tokens[i].TokenType == TokenType.Comma)
                {
                    predicates.Add(new Line(tokens.GetRange(0, i)));
                    tokens.RemoveRange(0, i+1);
                    i = 0;
                }
                else
                {
                    i++;
                }
            }
            
            predicates.Add(new Line(tokens.GetRange(0, i)));
            tokens.RemoveRange(0, i+1);

            
            i = 0;
            while (tokens[i].TokenType != TokenType.Label)
            {
                i++;
            }

            var goal = new Line(tokens.GetRange(0, i));
            tokens.RemoveRange(0, i+1);


            var proof = new List<Line>();
            i = 1; // first token will be a label, so we must start at 1
            while (i < tokens.Count)
            {
                if (tokens[i].TokenType == TokenType.Label)
                {
                    proof.Add(new Line(tokens.GetRange(0, i)));
                    tokens.RemoveRange(0, i+1);
                    i = 0;
                }
                else
                {
                    i++;
                }
            }
            
            proof.Add(new Line(tokens.GetRange(0, i)));

            return new NDFormat(predicates, goal, proof);
        }
    }
}