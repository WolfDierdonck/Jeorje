using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Jeorje
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

            return true;
        }
    }
}