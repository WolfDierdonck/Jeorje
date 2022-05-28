using System;
using System.Collections.Generic;
using Gentzen.Gentzen.Common;

namespace Gentzen.Gentzen.ND.NDRules
{
    public class NDNotE : NDRule
    {
        public static string _name = "not_e";
        public override string Name => _name;

        public sealed override string Label { get; set; }
        public sealed override AST Predicate { get; set; }
        public sealed override List<string> Requirements { get; set; }

        public NDNotE(string label, AST predicate, List<string> requirements)
        {
            Label = label;
            Requirements = requirements;
            Predicate = predicate;
        }
        
        public override bool CheckRule(SymbolTable symbolTable, List<AST> premises)
        {
            if (Requirements.Count != 2)
            {
                throw new Exception($"{_name} must be performed on two lines");
            }

            var firstReq = Requirements[0];
            var secondReq = Requirements[1];

            var firstAST = symbolTable.Statements[firstReq];
            var secondAST = symbolTable.Statements[secondReq];

            if (firstAST.Token.TokenType != TokenType.Not)
            {
                if (secondAST.Token.TokenType != TokenType.Not)
                {
                    throw new Exception($"Error on line with label {Label}: unable to match requirements for not_e");
                } 
                else 
                {
                    var otherSide = secondAST.Children.Find(x => x.Token.TokenType != TokenType.DummyNotOperand);
                    if (otherSide == null || otherSide != firstAST) 
                    {
                        throw new Exception($"Error on line with label {Label}: unable to match requirements for not_e");
                    }
                }
            }
            else 
            {
                var otherSide = firstAST.Children.Find(x => x.Token.TokenType != TokenType.DummyNotOperand);
                if (otherSide == null || otherSide != secondAST) 
                {
                     throw new Exception($"Error on line with label {Label}: unable to match requirements for not_e");
                }
            }

            return true;
        }
    }
}