using System;
using System.Collections.Generic;
using Gentzen.Gentzen.Common;

namespace Gentzen.Gentzen.ND.NDRules
{
    public class NDPremise : NDRule
    {
        public static string _name = "premise";
        public override string Name => _name;

        public sealed override string Label { get; set; }
        public sealed override AST Predicate { get; set; }
        public sealed override List<string> Requirements { get; set; }

        public NDPremise(string label, AST predicate, List<string> requirements)
        {
            Label = label;
            Requirements = requirements;
            Predicate = predicate;
        }
        
        public override bool CheckRule(SymbolTable symbolTable, List<AST> premises)
        {
            var test = premises[0].Equals(Predicate);
            
            if (!premises.Contains(Predicate))
            {
                throw new Exception($"Error on line with label {Label}: Premise not found");
            }

            return true;
        }
    }
}