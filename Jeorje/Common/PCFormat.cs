using System.Collections.Generic;

namespace Jeorje
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