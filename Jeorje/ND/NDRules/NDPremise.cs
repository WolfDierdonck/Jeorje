using System.Collections.Generic;

namespace Jeorje
{
    public class NDPremise : NDRule
    {
        public string Label { get; set; }
        public string Name { get; set; }
        public AST Predicate { get; set; }
        public List<string> Requirements { get; set; }

        public NDPremise(string label, AST predicate, List<string> requirements)
        {
            Label = label;
            Requirements = requirements;
            Name = "premise";
            Predicate = predicate;
        }

        public bool CheckRule(SymbolTable symbolTable)
        {
            throw new System.NotImplementedException();
        }
    }
}