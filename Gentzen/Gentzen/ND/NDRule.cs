using System.Collections.Generic;
using Gentzen.Gentzen.Common;

namespace Gentzen.Gentzen.ND
{
    public abstract class NDRule
    {
        private static string _name;

        public abstract string Name { get; }
        public abstract string Label { get; set; }
        public abstract AST Predicate { get; set; }
        public abstract List<string> Requirements{ get; set; }
        public abstract bool CheckRule(SymbolTable symbolTable, List<AST> premises);
    }
}