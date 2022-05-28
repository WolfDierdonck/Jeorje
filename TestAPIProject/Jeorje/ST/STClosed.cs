namespace Jeorje
{
    public class STClosed: ISTEntry
    {
        
        public string Requirements{ get; set; }

        public bool CheckRule(SymbolTable symbolTable)
        {
            return false;
        }
        
        public STEntryType entryType()
        {
            return STEntryType.Closed;
        }
    }
}