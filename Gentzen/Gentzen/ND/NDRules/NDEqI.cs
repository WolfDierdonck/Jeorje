using System;
using System.Collections.Generic;
using Gentzen.Gentzen.Common;

namespace Gentzen.Gentzen.ND.NDRules
{
    public class NDEqI : NDRule
    {
        public static string _name = "eq_i";
        public override string Name => _name;

        public sealed override string Label { get; set; }
        public sealed override AST Predicate { get; set; }
        public sealed override List<string> Requirements { get; set; }

        public NDEqI(string label, AST predicate, List<string> requirements)
        {
            Label = label;
            Requirements = requirements;
            Predicate = predicate;
        }
        public override bool CheckRule(SymbolTable symbolTable, List<AST> premises)
        {
            if (Requirements != null && Requirements.Count != 0)
            {
                throw new Exception($"Error on line with label {Label}: eq_i takes no arguments");
            }
            
            if (Predicate.Token.TokenType != TokenType.Equal)
            {
                throw new Exception($"Error on line with label {Label}: eq_i missing = operator");
            }

            if (Predicate.Children.Count != 2)
            {
                throw new Exception($"Error on line with label {Label}: eq_i must have 2 children");
            }
            
            var negSide = Predicate.Children[0];
            var otherSide = Predicate.Children[1];

            if (negSide == null || otherSide == null)
            {
                throw new Exception($"Error on line with label {Label}: eq_i not used properly!");
            }
            
            if (negSide != otherSide)
            {
                throw new Exception($"Error on line with label {Label}: right operator of eq_i isn't the same as the left");
            }
            return true;

        }
    }
}