using System.Collections.Generic;

namespace Jeorje
{
    public class NDImpE : NDRule
    {
        public static string _name = "imp_e";
        public override string Name => _name;

        public sealed override string Label { get; set; }
        public sealed override AST Predicate { get; set; }
        public sealed override List<string> Requirements { get; set; }

        public NDImpE(string label, AST predicate, List<string> requirements)
        {
            Label = label;
            Requirements = requirements;
            Predicate = predicate;
        }
        
        public override bool CheckRule(SymbolTable symbolTable, List<AST> premises)
        {
            throw new System.NotImplementedException();
        }
    }
}