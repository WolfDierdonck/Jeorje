using System.Collections.Generic;

namespace Jeorje
{
    public interface NDRule
    {
        public string Label { get; set; }
        public string Name { get; set; }
        public AST Predicate { get; set; }
        public List<string> Requirements { get; set; }
        public bool CheckRule(SymbolTable symbolTable);
    }
}