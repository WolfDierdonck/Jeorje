using System;
using System.Collections.Generic;

namespace Gentzen.Gentzen.Common
{
    public class SymbolTable
    {
        public Dictionary<string, AST> Statements;
        public HashSet<string> Identifiers;

        public SymbolTable()
        {
            Statements = new Dictionary<string, AST>();
            Identifiers = new HashSet<string>();
        }
        public SymbolTable(Dictionary<string, AST> statements, HashSet<string> identifiers)
        {
            Statements = statements;
            Identifiers = identifiers;
        }
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
        public AST GetStatement(string s)
        {
            if (!Statements.ContainsKey(s))
            {
                throw new Exception($"Error on line: Step depends on proof lines {s} which are not active lines at this point in the proof");
            }
            return Statements[s];
        }
    }
}