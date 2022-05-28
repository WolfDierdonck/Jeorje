using System;
using System.Collections.Generic;
using Gentzen.Gentzen.Common;

namespace Gentzen.Gentzen.ND.NDRules
{
    public class NDIffMP : NDRule
    {
        public static string _name = "iff_mp";
        public override string Name => _name;

        public sealed override string Label { get; set; }
        public sealed override AST Predicate { get; set; }
        public sealed override List<string> Requirements { get; set; }

        public NDIffMP(string label, AST predicate, List<string> requirements)
        {
            Label = label;
            Requirements = requirements;
            Predicate = predicate;
        }
        
        public override bool CheckRule(SymbolTable symbolTable, List<AST> premises)
        {
            if (Requirements.Count != 2)
            {
                throw new Exception($"Error on line with label {Label}: {_name} expecting 2 operands but received {Requirements.Count}");
            }
            
            if (Predicate.Token.TokenType != TokenType.Identifier)
            {
                throw new Exception($"Error on line with label {Label}: outermost token must be an identifier");
            }
            
            var req1 = symbolTable.Statements[Requirements[0]];
            var req2 = symbolTable.Statements[Requirements[1]];

            if (req1.Token.TokenType == TokenType.Iff && req2.Token.TokenType == TokenType.Identifier) // ? <=> ?, ?
            {
                if (req1.Children[0] == req2) // P <=> ?, P |- ?
                {
                    if (req1.Children[1] == Predicate) // P <=> Q, P |- Q
                    {
                        return true;
                    }
                }

                if (req1.Children[1] == req2) // ? <=> Q, Q |- ?
                {
                    if (req1.Children[0] == Predicate) // P <=> Q, Q |- P
                    {
                        return true;
                    }
                }
            }
            else if (req1.Token.TokenType == TokenType.Identifier &&
                     req2.Token.TokenType == TokenType.Iff) // ?, ? <=> ?
            {
                if (req2.Children[0] == req1) // P, P <=> ? |- ?
                {
                    if (req2.Children[1] == Predicate) // P, P <=> Q |- Q
                    {
                        return true;
                    }
                }

                if (req2.Children[1] == req1) // Q, ? <=> Q |- ?
                {
                    if (req2.Children[0] == Predicate) // Q, P <=> Q |- P
                    {
                        return true;
                    }
                }
            }
            
            throw new Exception($"Error on line with label {Label}: Could not match any subrule of iff_e");
        }
    }
}