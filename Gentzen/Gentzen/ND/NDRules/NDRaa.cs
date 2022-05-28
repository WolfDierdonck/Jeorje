using System;
using System.Collections.Generic;
using Gentzen.Gentzen.Common;

namespace Gentzen.Gentzen.ND.NDRules
{
    public class NDRaa : NDRule
    {
        public static string _name = "raa";
        public override string Name => _name;

        public sealed override string Label { get; set; }
        public sealed override AST Predicate { get; set; }
        public sealed override List<string> Requirements { get; set; }

        public NDRaa(string label, AST predicate, List<string> requirements)
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
            
            var raaStartLabel = requirements[0].ToString();
            var raaEndLabel = requirements[1].ToString();

            if (Predicate.Token.TokenType != TokenType.Not && symbolTable.Statements[raaStartLabel].Token.TokenType != TokenType.Not)
            {
                throw new Exception($"Operand for raa or raa conclusion must be not");
            }

            if (symbolTable.Statements[raaEndLabel].Token.TokenType != TokenType.False)
            {
                throw new Exception($"Conclusion for raa must be 'false'");
            }

            if (symbolTable.Statements[raaStartLabel].Token.TokenType == TokenType.Not)
            {
                if (!(symbolTable.Statements[raaStartLabel].Children[1] == Predicate
                      || symbolTable.Statements[raaStartLabel].Children[1] == Predicate.Children[1]))
                {
                    throw new Exception($"Raa must conclude the negation of the disprove");
                }
            }
            else
            {
                if (symbolTable.Statements[raaStartLabel] != Predicate.Children[1])
                {
                    throw new Exception($"Raa must conclude the negation of the disprove");
                }
            }

            return true;
        }
    }
}