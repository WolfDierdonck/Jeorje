using System.Collections.Generic;
using System.Linq;

namespace Jeorje
{
    public class SymbolTable
    {
        public Dictionary<string, AST> Statements = new Dictionary<string, AST>();
        public HashSet<string> Identifiers = new HashSet<string>();

        public void UpdateSymbols(string s, AST tree)
        {
            // Add (s, tree) to Rules;
            Statements.Add(s, tree);
            // Add identifiers in tree to Identifiers
            void AddLeaves(AST node)
            {
                node.Children.ForEach(AddLeaves);
                if (node.Token.TokenType == TokenType.Identifier)
                {
                    Identifiers.Add(node.Token.Lexeme);
                }
            }

            AddLeaves(tree);
        } 
    }
}