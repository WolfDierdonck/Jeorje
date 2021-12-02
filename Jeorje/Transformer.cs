using System.Collections.Generic;
using System.ComponentModel;

namespace Jeorje
{
    public static class Transformer
    {
        public static (CheckType, List<Line>, Line, List<Line>) TransformLines(List<Line> lines)
        {
            var checkType = CheckType.ND;
            List<Line> predicates = null;
            Line goal = null;
            List<Line> proof = null;

            return (checkType, predicates, goal, proof);
        }
    }
}