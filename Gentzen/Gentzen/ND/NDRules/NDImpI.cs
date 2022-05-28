using System;
using System.Collections.Generic;
using Gentzen.Gentzen.Common;

namespace Gentzen.Gentzen.ND.NDRules
{
    public class NDImpI : NDRule
    {
        public static string _name = "imp_i";
        public override string Name => _name;

        public sealed override string Label { get; set; }
        public sealed override AST Predicate { get; set; }
        public sealed override List<string> Requirements { get; set; }

        public NDImpI(string label, AST predicate, List<string> requirements)
        {
            Label = label;
            Requirements = requirements;
            Predicate = predicate;
        }
        
        public override bool CheckRule(SymbolTable symbolTable, List<AST> premises)
        {
            if (Requirements.Count != 1)
            {
                throw new Exception($"Expecting 1 premise, got {Requirements.Count}");
            }

            var requirements = Requirements[0].Split('-');

            if (requirements.Length != 2)
            {
                throw new Exception($"Expecting range of rules, got {Requirements[0]}");
            }

            if (Predicate.Token.TokenType != TokenType.Implies)
            {
                throw new Exception($"Operand for imp_i must be implication");
            }

            var impStartLabel = requirements[0];
            var impEndLabel = requirements[1];

            if (symbolTable.Statements[impStartLabel] != Predicate.Children[0])
            {
                throw new Exception($"Hypotheses for imp_i don't match");
            }
            if (symbolTable.Statements[impEndLabel] != Predicate.Children[1])
            {
                throw new Exception($"Conclusions for imp_i don't match");
            }

            return true;
        }
    }
}