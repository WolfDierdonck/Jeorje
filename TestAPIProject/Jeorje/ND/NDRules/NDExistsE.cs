using System;
using System.Collections.Generic;

namespace Jeorje
{
    public class NDExistsE : NDRule
    {
        public static string _name = "exists_e";
        public override string Name => _name;

        public sealed override string Label { get; set; }
        public sealed override AST Predicate { get; set; }
        public sealed override List<string> Requirements { get; set; }

        public NDExistsE(string label, AST predicate, List<string> requirements)
        {
            Label = label;
            Requirements = requirements;
            Predicate = predicate;
        }

        public override bool CheckRule(SymbolTable symbolTable, List<AST> premises)
        {
            return true;
        }
    }
}