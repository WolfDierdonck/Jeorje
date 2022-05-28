using System.Collections.Generic;
using Gentzen.Gentzen.Common;

namespace Gentzen.Gentzen.ST
{
    public abstract class STRule: ISTEntry
    {
        private static string _name;

        public abstract string Name { get; }
        
        public abstract string Requirement{ get; set; }

        public abstract List<ISTEntry> predicartes { get; set; }
        
        public abstract bool CheckRule(SymbolTable symbolTable);
        
        public STEntryType entryType()
        {
            return STEntryType.Rule;
        }

    }
}