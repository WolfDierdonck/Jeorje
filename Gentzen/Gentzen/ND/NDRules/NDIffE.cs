using System;
using System.Collections.Generic;
using Gentzen.Gentzen.Common;

namespace Gentzen.Gentzen.ND.NDRules
{
    public class NDIffE : NDRule
    {
        public static string _name = "iff_e";
        public override string Name => _name;

        public sealed override string Label { get; set; }
        public sealed override AST Predicate { get; set; }
        public sealed override List<string> Requirements { get; set; }

        public NDIffE(string label, AST predicate, List<string> requirements)
        {
            Label = label;
            Requirements = requirements;
            Predicate = predicate;
        }
        
        public override bool CheckRule(SymbolTable symbolTable, List<AST> premises)
        {
            if (Requirements.Count != 1)
            {
                throw new Exception($"Error on line with label {Label}: {_name} expecting 1 operand but received {Requirements.Count}");
            }
            
            if (Predicate.Token.TokenType != TokenType.Implies)
            {
                throw new Exception($"Error on line with label {Label}: outermost operand must be =>");
            }
            
            var iff = symbolTable.Statements[Requirements[0]];

            if (iff.Token.TokenType != TokenType.Iff)
            {
                throw new Exception($"Error on line with label {Label}: Could not match any subrule of iff_e");
            }

            if (iff.Children[0] == Predicate.Children[0] && iff.Children[1] == Predicate.Children[1]) // P <=> Q |- P => Q
            {
                return true;
            }

            if (iff.Children[0] == Predicate.Children[1] && iff.Children[1] == Predicate.Children[0]) // P <=> Q |- Q => P
            {
                return true;
            }
            
            throw new Exception($"Error on line with label {Label}: Could not match any subrule of iff_e");
        }
    }
}