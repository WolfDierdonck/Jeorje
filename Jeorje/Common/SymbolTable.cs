using System.Collections.Generic;
using System.Linq;

namespace Jeorje
{
    public class SymbolTable
    {
        public Dictionary<string, AST> Rules = new Dictionary<string, AST>();
        public List<string> Identifiers = new List<string>();

        public void UpdateSymbols(string s, AST tree)
        {
            // Add (s, tree) to Rules;
            Rules.Add(s, tree);
            // Add identifiers in tree to Identifiers
           	/*Identifiers.AddRange(tree.Children
                .Where(child => child.Token.TokenType == TokenType.Identifier)
                .Select(child => child.Token.Lexeme)
            );*/ // TODO this is wrong, fix! (need recursion n shit)
        } 
    }
}