using System.Collections.Generic;

namespace Jeorje
{
    public class NDFormat : ProofFormat
    {
        public new CheckType CheckType = CheckType.ND;
        public List<Line> Predicates;
        public Line Goal;
        public List<Line> Proof;
        
        public NDFormat(List<Line> predicates, Line goal, List<Line> proof)
        {
            Predicates = predicates;
            Goal = goal;
            Proof = proof;
        }
    }
}