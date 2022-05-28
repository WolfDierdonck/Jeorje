using System.Collections.Generic;

namespace Gentzen.Gentzen.Common
{
    public class PCFormat: ProofFormat
    {
        public new CheckType CheckType = CheckType.PC;
        public List<Line> Program;
        
        public PCFormat(List<Line> program)
        {
            Program = program;
        }
    }
}