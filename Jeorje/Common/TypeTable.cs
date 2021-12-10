using System;
using System.Collections.Generic;
using System.Linq;

namespace Jeorje
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

        public object Clone()
        {
            return new TypeTable
            {
                Table = Table.ToDictionary(entry => (string) entry.Key.Clone(),
                    entry => (PredicateType) entry.Value.Clone()),
                Bindings = Bindings.ToDictionary(entry => (string) entry.Key.Clone(),
                    entry => new HashSet<string>(entry.Value))
            };
        }
    }
}