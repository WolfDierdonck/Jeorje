using System;
using System.Collections.Generic;
using System.Linq;

namespace Gentzen.Gentzen.Common
{
    public class TypeTable: ICloneable
    {
        public Dictionary<string, PredicateType> Table;

        public Dictionary<string, HashSet<string>> Bindings;

        public TypeTable()
        {
            Table = new Dictionary<string, PredicateType>();
            Bindings = new Dictionary<string, HashSet<string>>();
        }
        
        
        public TypeTable(Dictionary<string, PredicateType> table, Dictionary<string, HashSet<string>> bindings)
        {
            Table = table;
            Bindings = bindings;
        }

        public object Clone()
        {
            var table = Table.ToDictionary(entry => (string) entry.Key.Clone(),
                entry => (PredicateType) entry.Value?.Clone());
            var bindings = Bindings.ToDictionary(entry => (string) entry.Key.Clone(),
                entry => new HashSet<string>(entry.Value));
            return new TypeTable(table, bindings);
        }
    }
}