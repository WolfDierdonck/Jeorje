using System.Collections.Generic;

namespace Gentzen.Gentzen.ST
{
    public class STBranch: ISTEntry
    {
        public List<ISTEntry> children;

        public STEntryType entryType()
        {
            return STEntryType.Branch;
        }
        
    }
}