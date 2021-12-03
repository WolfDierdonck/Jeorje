using System.Collections.Generic;
using System.Linq;

namespace Jeorje
{
    public class SymbolTable
    {
        public Dictionary<string, AST> Rules;
        public List<string> Identifiers;

        public void UpdateSymbols(string s, AST tree)
        {
            // Add (s, tree) to Rules;
            Rules.Add(s, tree);
            // Add identifiers in tree to Identifiers
           	Identifiers.AddRange(tree.Children
                .Where(child => child.Token.TokenType == TokenType.Identifier)
                .Select(child => child.Token.Lexeme)
            );
        } 
    }
}