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
                              "a & b, a => c |- a | z\n" +
                              "\n" +
                              "1) a & b premise\n" +
                              "2two) a => c premise\n" +
                              "3) a by and_e on 1\n" +
                              "4) z | a | k by or_i on 3\n"
                              ;
            
            string output;

            try
            {
                var tokens = Scanner.MaximalMunchScan(jeorjeInput);

                var proofFormat = Transformer.TransformTokens(tokens);

                switch (proofFormat.CheckType)
                {
                    case CheckType.ND:
                        var ndFormat = proofFormat as NDFormat;
                        List<AST> ndPremises = Parser.ParseLines(ndFormat.Premises);
                        AST ndGoal = Parser.ParseLine(ndFormat.Goal);
                        List<NDRule> ndProof = NDRulifier.RulifyLines(ndFormat.Proof);
                        
                        output = Validator.ValidateND(ndPremises, ndGoal, ndProof);
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
