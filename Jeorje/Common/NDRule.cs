namespace Jeorje
{
    public abstract class NDRule
    {
        public string Label;
        public string Name;
        public AST Predicate;
        private List<string> Requirements;
        public bool CheckRule(SymbolTable);
    }
}