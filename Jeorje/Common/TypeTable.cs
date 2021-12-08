using System;
using System.Collections.Generic;
using System.Linq;

namespace Jeorje
{
    public class TypeTable
    {
        public Dictionary<AST, HashSet<PredicateType>> Table;

        public TypeTable()
        {
            Table = new Dictionary<AST, HashSet<PredicateType>>();
        }

        public void UpdateEntry(AST ast, HashSet<PredicateType> possibleTypes)
        {
            if (!Table.ContainsKey(ast))
            {
                Table[ast] = possibleTypes;
            }
            else
            {
                var tableTypes = Table[ast];
                var union = tableTypes.Union(possibleTypes);

                if (union == null)
                {
                    throw new Exception("typing failed");
                }

                Table[ast] = union.ToHashSet();
            }
        }
    }
}