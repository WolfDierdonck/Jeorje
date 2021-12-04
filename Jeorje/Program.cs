using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Jeorje
{
    class Program
    {
        static void Main(string[] args)
        {
            var jeorjeInput = @"
#check ND
a => b, a | c, c <=> b,!b |- c <=> a

1) a => b premise
2) a | c premise
3) c <=> b premise
4) !b premise

5) assume c {
    6) c => b by iff_e on 3
    7) b by imp_e on 6,5
    8) a by not_e on 4,7
}

10) assume a {
    11) b by imp_e on 1,10
    12) b => c by iff_e on 3
    13) c by imp_e on 11,12
}

9) c => a by imp_i on 5-8
14) a => c by imp_i on 10-13
15) c <=> a by iff_i on 9,14
                ";

            Console.WriteLine(AskJeorje(jeorjeInput));
        }

        public static string AskJeorje(string input)
        {
            try
            {
                var tokens = Scanner.MaximalMunchScan(input);
                Logger.AddStep("Successfully scanned input tokens");

                var proofFormat = Transformer.TransformTokens(tokens);
                Logger.AddStep($"Found proof format, performing {proofFormat.CheckType}");

                switch (proofFormat.CheckType)
                {
                    case CheckType.ND:
                        var ndFormat = proofFormat as NDFormat;
                        List<AST> ndPremises = Parser.ParseLines(ndFormat.Premises);
                        AST ndGoal = Parser.ParseLine(ndFormat.Goal);
                        Logger.AddStep("Parsed premises and goal");
                        
                        List<NDRule> ndProof = NDRulifier.RulifyLines(ndFormat.Proof);
                        Logger.AddStep("Rulify ND worked");
                        
                        Logger.AddStep(Validator.ValidateND(ndPremises, ndGoal, ndProof));
                        break;

                    default:
                        throw new Exception($"Check type {proofFormat.CheckType.ToString()} not supported yet");
                }
            }
            
            catch (Exception e)
            {
                Logger.AddError($"Exception thrown:\n{e.Message}");
            }
            
            return Logger.LogAll();
        }
    }
}
