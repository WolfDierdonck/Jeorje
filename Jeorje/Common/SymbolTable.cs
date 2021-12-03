namespace Jeorje
{
    public class SymbolTable
    {
        public Dictionary<string, AST> Rules;
        public List<string> Identifiers;

        public void UpdateSymbols(string s, AST tree)
        {
            // add (s, tree) to Rules; Add identifiers in tree to Identifiers
        } 
    }
}