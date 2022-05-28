using System;
using System.Collections.Generic;
using System.Linq;
using Gentzen.Gentzen.Common;

namespace Gentzen.Gentzen.ND.NDRules
{
    public class NDTrue : NDRule
    {
        public static string _name = "true";
        public override string Name => _name;

        public sealed override string Label { get; set; }
        public sealed override AST Predicate { get; set; }
        public sealed override List<string> Requirements { get; set; }

        public NDTrue(string label, AST predicate, List<string> requirements)
        {
            Label = label;
            Requirements = requirements;
            Predicate = predicate;
        }
        //0) true by true
        public override bool CheckRule(SymbolTable symbolTable, List<AST> premises)
        {
            if (Requirements != null)
            {
                throw new Exception($"{_name} must be performed on 0 lines");
            }

            if (!(Predicate.Token.TokenType == TokenType.True && !Predicate.Children.Any()))
            {
                throw new Exception($"Error on line with label {Label}: true used incorrectly");
            }

            return true;
        }
    }
}