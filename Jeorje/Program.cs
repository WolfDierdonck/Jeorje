﻿using System;
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
            forall x, y . x | a, a = (forall y . y) |- forall x, y . x | (forall y . y) 

            1) forall x, y . x | a premise 
            2) a = (y) premise
            3) forall x, y . x | (y)  by eq_e on 1, 2
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
                    case CheckType.ST:
                        var stFormat = proofFormat as STFormat;
                        List<AST> stPremises = Parser.ParseLines(stFormat.Premises);
                        AST stGoal = Parser.ParseLine(stFormat.Goal);
                        Logger.AddError("Parsed premises and goal");

                        STBranch stProof = STRulifier.RulifyLines(stFormat.Proof);
                        Logger.AddStep("Rulify ST worked");
                        Logger.AddError(Validator.ValidateST(stPremises, stGoal, stProof));
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
