using System.Collections.Generic;

namespace Jeorje
{
    public class STFormat: ProofFormat
    {
        public new CheckType CheckType = CheckType.ST;
        public List<Line> Predicates;
        public Line Goal;
        public List<Line> Proof;
        
        public STFormat(List<Line> predicates, Line goal, List<Line> proof)
        {
            Predicates = predicates;
            Goal = goal;
            Proof = proof;
        }
    }
}