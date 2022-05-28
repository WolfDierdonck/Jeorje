using System;
using System.Collections.Generic;
using Gentzen.Gentzen.Common;

namespace Gentzen.Gentzen.ND.NDRules
{
    public class NDLem : NDRule
    {
        public static string _name = "lem";
        public override string Name => _name;

        public sealed override string Label { get; set; }
        public sealed override AST Predicate { get; set; }
        public sealed override List<string> Requirements { get; set; }

        public NDLem(string label, AST predicate, List<string> requirements)
        {
            Label = label;
            Requirements = requirements;
            Predicate = predicate;
        }
        public override bool CheckRule(SymbolTable symbolTable, List<AST> premises)
        {
            if (Predicate.Token.TokenType != TokenType.Or)
            {
                throw new Exception($"Error on line with label {Label}: lem does not have | operator");
            }

            if (Predicate.Children.Count != 2)
            {
                throw new Exception($"Error on line with label {Label}: lem must have 2 children");
            }
            
            var negSide = Predicate.Children.Find(x => x.Token.TokenType == TokenType.Not);
            var otherSide = Predicate.Children.Find(x => x.Token.TokenType != TokenType.Not);

            if (negSide == null || otherSide == null)
            {
                throw new Exception($"Error on line with label {Label}: lem not used properly!");
            }
            
            if (negSide.Children[1] != otherSide)
            {
                throw new Exception($"Error on line with label {Label}: right operator of lem isn't the negation of the left");
            }
            return true;

        }
    }
}