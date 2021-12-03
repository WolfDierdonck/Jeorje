using System.Collections.Generic;

namespace Jeorje
{
    public class NDAndI : NDRule
    {
        public string Label { get; set; }
        public string Name { get; set; }
        public AST Predicate { get; set; }
        public List<string> Requirements { get; set; }

        public NDAndI(string label, AST predicate, List<string> requirements)
        {
            Label = label;
            Requirements = requirements;
            Name = "and_i";
            Predicate = predicate;
        }
        
        public bool CheckRule(SymbolTable symbolTable)
        {
            throw new System.NotImplementedException();
        }
    }
}