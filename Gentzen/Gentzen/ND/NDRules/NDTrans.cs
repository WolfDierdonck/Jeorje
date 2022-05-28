using System;
using System.Collections.Generic;
using Gentzen.Gentzen.Common;

namespace Gentzen.Gentzen.ND.NDRules
{
    public class NDTrans : NDRule
    {
        public static string _name = "trans";
        public override string Name => _name;

        public sealed override string Label { get; set; }
        public sealed override AST Predicate { get; set; }
        public sealed override List<string> Requirements { get; set; }

        public NDTrans(string label, AST predicate, List<string> requirements)
        {
            Label = label;
            Requirements = requirements;
            Predicate = predicate;
        }
        
        public override bool CheckRule(SymbolTable symbolTable, List<AST> premises)
        {

            if (Requirements.Count != 2)
            {
                throw new Exception($"Expecting 2 premises for {_name}, got {Requirements.Count}");
            }
            
            var firstIff = symbolTable.Statements[Requirements[0]];
            var secondIff =  symbolTable.Statements[Requirements[1]];

            var firstHyp = Predicate.Children[0]; // a
            var secondHyp = Predicate.Children[1]; // c

            if (firstIff.Children[0] == firstHyp && firstIff.Children[1] == secondIff.Children[0] && secondIff.Children[1] == secondHyp)
            {
                return true;
            }
            else if (firstIff.Children[0] == secondIff.Children[1] && firstIff.Children[1] == secondHyp && secondIff.Children[0] == firstHyp)
            {
                return true;
            }
            else if (firstIff.Children[0] == firstHyp && firstIff.Children[1] == secondIff.Children[1] && secondIff.Children[0] == secondHyp)
            {
                return true;
            }
            else if (firstIff.Children[0] == secondHyp && firstIff.Children[1] == secondIff.Children[1] && secondIff.Children[0] == firstHyp)
            {
                return true;
            }
            else if (firstIff.Children[0] == secondIff.Children[0] && firstIff.Children[1] == firstHyp && secondIff.Children[1] == secondHyp)
            {
                return true;
            }
            else if (firstIff.Children[0] == secondIff.Children[0] && firstIff.Children[1] == secondHyp && secondIff.Children[1] == firstHyp)
            {
                return true;
            }
            else if (firstIff.Children[0] == secondIff.Children[1] && firstIff.Children[1] == firstHyp && secondIff.Children[0] == secondHyp)
            {
                return true;
            }
            else if (firstIff.Children[0] == secondHyp && firstIff.Children[1] == secondIff.Children[0] && secondIff.Children[1] == firstHyp)
            {
                return true;
            }
            else
            {
                throw new Exception($"Expression does not match any subrule of {_name}");
            }
            
            /* 
             * Must work in all ways
             * If we have a <=> c as predicate we could have any of the following as premises
             * a <=> b, b <=> c
             * b <=> c, a <=> b 
             * a <=> b, c <=> b 
             * c <=> b, a <=> b
             * b <=> a, b <=> c
             * b <=> c, b <=> a
             * b <=> a, c <=> b
             * c <=> b, b <=> a 
             */
            
        }
    }
}