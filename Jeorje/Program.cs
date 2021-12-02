using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Jeorje
{
    class Program
    {
        static void Main(string[] args)
        {
            var jeorjeInput = "#check ND\n" +
                              "\n" +
                              "a & b\n" +
                              "a => c\n" +
                              "|-\n" +
                              "c\n" +
                              "\n" +
                              "1) a & b premise\n" +
                              "2) a => c premise\n" +
                              "3) a by and_e on 1\n" +
                              "4) c by imp_e on 2,3";
            
            string output;

            try
            {
                var lines = Scanner.ScanInput(jeorjeInput.Split("\n"));

                var proofFormat = Transformer.TransformLines(lines);

                switch (proofFormat.CheckType)
                {
                    case CheckType.ND:
                        var ndFormat = proofFormat as NDFormat;
                        List<AST> ndPredicates = Parser.ParseLines(ndFormat.Predicates);
                        AST ndGoal = Parser.ParseLine(ndFormat.Goal);
                        List<AST> ndProof = Parser.ParseLines(ndFormat.Proof);
                        
                        output = Validator.ValidateND(ndPredicates, ndGoal, ndProof);
                        break;
                    
                    case CheckType.ST:
                        var stFormat = proofFormat as NDFormat;
                        List<AST> stPredicates = Parser.ParseLines(stFormat.Predicates);
                        AST stGoal = Parser.ParseLine(stFormat.Goal);
                        List<AST> stProof = Parser.ParseLines(stFormat.Proof);
                        
                        output = Validator.ValidateST(stPredicates, stGoal, stProof);
                        break;
                    
                    default:
                        throw new Exception($"check type {proofFormat.CheckType.ToString()} not supported yet");
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
