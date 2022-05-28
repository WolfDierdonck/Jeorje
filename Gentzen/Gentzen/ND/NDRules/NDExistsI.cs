using System.Collections.Generic;
using Gentzen.Gentzen.Common;

namespace Gentzen.Gentzen.ND.NDRules
{
    public class NDExistsI : NDRule
    {
        public static string _name = "exists_i";
        public override string Name => _name;

        public sealed override string Label { get; set; }
        public sealed override AST Predicate { get; set; }
        public sealed override List<string> Requirements { get; set; }

        public NDExistsI(string label, AST predicate, List<string> requirements)
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