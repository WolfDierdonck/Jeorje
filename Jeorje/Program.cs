using System;
using System.Collections.Generic;

namespace Jeorje
{
    class Program
    {
        static void Main(string[] args)
        {
            var jeorjeInput = new string[]{"a & b | c", "forall x . x > 5 & P(x, f(y)) | !c"};
            var lines = Scanner.ScanInput(jeorjeInput);

            (CheckType checkType, List<Line> predicates, Line goal, List<Line> proof) =
                Transformer.TransformLines(lines);
        }
    }
}
