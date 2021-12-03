using System.Collections.Generic;

namespace Jeorje
{
    public abstract class NDRule
    {
        public string Label;
        public string Name;
        public AST Predicate;
        private List<string> _requirements;
        public abstract bool CheckRule(SymbolTable symbolTable);
    }
}