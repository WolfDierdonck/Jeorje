using System;
using System.Collections.Generic;
using System.Linq;

namespace Jeorje
{
    public class NDSet : NDRule
    {
        public static string _name = "set";
        public override string Name => _name;

        public sealed override string Label { get; set; }
        public sealed override AST Predicate { get; set; }
        public sealed override List<string> Requirements { get; set; }

        public NDSet(string label, AST predicate, List<string> requirements)
        {
            Label = label;
            Requirements = requirements;
            Predicate = predicate;
        }
        //0) a&b|c=>d=>e&!z by set
        public override bool CheckRule(SymbolTable symbolTable, List<AST> premises)
        {
            return true;
        }
    }
}