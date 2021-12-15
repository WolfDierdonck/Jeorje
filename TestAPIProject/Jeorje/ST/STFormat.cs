using System.Collections.Generic;

namespace Jeorje
{
    public class STFormat: ProofFormat
    {
        public new CheckType CheckType = CheckType.ST;
        public List<Line> Premises;
        public Line Goal;
        public List<Line> Proof;
        
        public STFormat(List<Line> premises, Line goal, List<Line> proof)
        {
            Premises = premises;
            Goal = goal;
            Proof = proof;
        }
    }
}