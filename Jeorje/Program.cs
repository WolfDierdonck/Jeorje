using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Jeorje
{
    class Program
    {
        static void Main(string[] args)
        {
            var jeorjeInput = new string[]{"a & b | c", "forall x . x > 5 & P(x, f(y)) | !c"};

            string output;

            try
            {
                var lines = Scanner.ScanInput(jeorjeInput);

                (CheckType checkType, List<Line> predicates, Line goal, List<Line> proof) =
                    Transformer.TransformLines(lines);

                List<AST> predicateASTs = Parser.ParseLines(predicates);
                AST goalAST = Parser.ParseLine(goal);
                List<AST> proofASTs = Parser.ParseLines(proof);

                switch (checkType)
                {
                    case CheckType.ND:
                        output = CheckND.Validate(predicateASTs, goalAST, proofASTs);
                        break;
                    
                    default:
                        throw new Exception($"check type {checkType.ToString()} not supported yet");
                }
            }
            catch (Exception e)
            {
                output = $"Exception thrown:\n{e.Message}";
            }
            
            Console.WriteLine(output);
            
        }
    }
}
