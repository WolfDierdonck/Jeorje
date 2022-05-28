using Gentzen.Gentzen.Common;

namespace Gentzen.Gentzen.ST
{
    public class STLine: ISTEntry
    {
        public string Label { get; set; }
        public AST Predicate { get; set; }
        
        public STEntryType entryType()
        {
            return STEntryType.Line;
        }

        
    }
}