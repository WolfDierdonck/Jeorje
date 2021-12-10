using System.Collections.Generic;

namespace Jeorje
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