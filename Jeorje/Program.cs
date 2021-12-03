using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Jeorje
{
    class Program
    {
        static void Main(string[] args)
        {
            var jeorjeInput = "this is just a test\n" +
                              "#check ND\n" +
                              "\n" +
                              "a & b\n" +
                              "a => c\n" +
                              "|-\n" +
                              "c\n" +
                              "\n" +
                              "1abcd) a & b premise\n" +
                              "2) a => c premise\n" +
                              "3) a by and_e on 1\n" +
                              "4) c by imp_e on 2,3";
            
            string output;

            try
            {
                var tokens = Scanner.MaximalMunchScan(jeorjeInput);

                var proofFormat = Transformer.TransformTokens(tokens);

                switch (proofFormat.CheckType)
                {
                    case CheckType.ND:
                        var ndFormat = proofFormat as NDFormat;
                        List<AST> ndPredicates = Parser.ParseLines(ndFormat.Predicates);
                        AST ndGoal = Parser.ParseLine(ndFormat.Goal);
                        List<NDRule> ndProof = NDRulifier.RulifyLines(ndFormat.Proof);
                        
                        output = Validator.ValidateND(ndPredicates, ndGoal, ndProof);
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
