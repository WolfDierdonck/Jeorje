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
isRow(x), clue(x) = 12, 
isBlue(x, 1), isBlue(x, 4), isBlue(x, 5), isPink(x, 6),
forall r . isRow(r) & (clue(r) = 12) => exists i, j . (i > 0) & (i < 4) & (j > i+1) & (j < 6)
    & isBlue(r, i) & isBlue(r, j) & isBlue(r, j+1) 
        & (forall s . (s > 0) & (s < i) => isPink(r, s))
        & (forall s . (s > i) & (s < j) => isPink(r, s))
        & (forall s . (s > j+1) & (s < 7) => isPink(r, s)),
forall a, b . isBlue(a, b) => !isPink(a, b)
|-
isPink(x, 2) & isPink(x, 3)

1) isRow(x) premise
2) clue(x) = 12 premise
3) isBlue(x, 1) premise
4) isBlue(x, 4) premise
5) isBlue(x, 5) premise
6) isPink(x, 6) premise

7) forall r . isRow(r) & (clue(r) = 12) => exists i, j . (i > 0) & (i < 4) & (j > i+1) & (j < 6)
    & isBlue(r, i) & isBlue(r, j) & isBlue(r, j+1) 
        & (forall s . (s > 0) & (s < i) => isPink(r, s))
        & (forall s . (s > i) & (s < j) => isPink(r, s))
        & (forall s . (s > j+1) & (s < 7) => isPink(r, s)) premise

1h) forall a, b . isBlue(a, b) => !isPink(a, b) premise

8) isRow(x) & (clue(x) = 12) => exists i, j . (i > 0) & (i < 4) & (j > i+1) & (j < 6)
    & isBlue(x, i) & isBlue(x, j) & isBlue(x, j+1) 
        & (forall s . (s > 0) & (s < i) => isPink(x, s))
        & (forall s . (s > i) & (s < j) => isPink(x, s))
        & (forall s . (s > j+1) & (s < 7) => isPink(x, s)) by forall_e on 7
        
9) isRow(x) & (clue(x) = 12) by and_i on 1, 2
10) exists i, j . (i > 0) & (i < 4) & (j > i+1) & (j < 6)
    & isBlue(x, i) & isBlue(x, j) & isBlue(x, j+1) 
        & (forall s . (s > 0) & (s < i) => isPink(x, s))
        & (forall s . (s > i) & (s < j) => isPink(x, s))
        & (forall s . (s > j+1) & (s < 7) => isPink(x, s)) by imp_e on 8, 9

11) for some iu assume exists j . (iu > 0) & (iu < 4) & (j > iu+1) & (j < 6)
    & isBlue(x, iu) & isBlue(x, j) & isBlue(x, j+1) 
        & (forall s . (s > 0) & (s < iu) => isPink(x, s))
        & (forall s . (s > iu) & (s < j) => isPink(x, s))
        & (forall s . (s > j+1) & (s < 7) => isPink(x, s)) {
        
    200) for some ju assume (iu > 0) & (iu < 4) & (ju > iu+1) & (ju < 6)
    & isBlue(x, iu) & isBlue(x, ju) & isBlue(x, ju+1) 
        & (forall s . (s > 0) & (s < iu) => isPink(x, s))
        & (forall s . (s > iu) & (s < ju) => isPink(x, s))
        & (forall s . (s > ju+1) & (s < 7) => isPink(x, s)) {
        
        201) forall b . isBlue(x, b) => !isPink(x, b) by forall_e on 1h
        202) isBlue(x, 1) => !isPink(x, 1) by forall_e on 201
        203) !isPink(x, 1) by imp_e on 3, 202
        
        204) forall s . (s > 0) & (s < iu) => isPink(x, s) by and_e on 200
        205) (1 > 0) & (1 < iu) => isPink(x, 1) by forall_e on 204
        206) !((1 > 0) & (1 < iu)) by imp_e on 203, 205
        207) disprove !(!(1 > 0) | !(1 < iu)) {
            208) disprove !(1 > 0) {
                209) !(1 > 0) | !(1 < iu) by or_i on 208
                210) false by not_e on 207, 209
            }
            211) (1 > 0) by raa on 208-210
            212) disprove !(1 < iu) {
                213) !(1 > 0) | !(1 < iu) by or_i on 212
                214) false by not_e on 207, 213
            }
            215) (1 < iu) by raa on 212-214
            216) (1 > 0) & (1 < iu) by and_i on 211, 215
            217) false by not_e on 206, 216
        }
        218) !(1 > 0) | !(1 < iu) by raa on 207-217
        219) (1 > 0) by arith
        2180) !!(1 > 0) by not_not_i on 219
        220) !(1 < iu) by or_e on 218, 2180
        221) 1 >= iu by arith on 220
        
        222) iu > 0 by and_e on 200
        223) iu = 1 by arith on 221, 222
        
        224) ju > iu+1 by and_e on 200
        225) ju > 1+1 by eq_e on 223, 224
        226) ju > 2 by arith on 225
        227) ju < 6 by and_e on 200
        228) (ju = 3) | (ju = 4) | (ju = 5) by arith on 226, 227
        
        229) case ju = 3 {
            230) forall s . (s > ju+1) & (s < 7) => isPink(x, s) by and_e on 200
            231) (5 > ju+1) & (5 < 7) => isPink(x, 5) by forall_e on 230
            232) (5 > 3+1) & (5 < 7) => isPink(x, 5) by eq_e on 229, 231
            233) 5 > 3 + 1 by arith
            234) 5 < 7 by arith
            235) (5 > 3+1) & (5 < 7) by and_i on 233, 234
            236) isPink(x, 5) by imp_e on 232, 235
            
            237) isBlue(x, 5) => !isPink(x, 5) by forall_e on 201
            238) !isPink(x, 5) by imp_e on 5, 237
            
            239) isPink(x, 2) & isPink(x, 3) by not_e on 236, 238
        }
        250) case ju = 4 {
            251) forall s . (s > iu) & (s < ju) => isPink(x, s) by and_e on 200
            252) (2 > iu) & (2 < ju) => isPink(x, 2) by forall_e on 251
            253) (3 > iu) & (3 < ju) => isPink(x, 3) by forall_e on 251
            
            254) 2 > 1 by arith
            255) 3 > 1 by arith
            256) 2 < 4 by arith
            257) 3 < 4 by arith
            
            260) 2 > ju by eq_e on 223, 254
            261) 3 > iu by eq_e on 223, 255
            262) 2 < ju by eq_e on 250, 256
            263) 3 < ju by eq_e on 250, 257
            
            270) (2 > iu) & (2 < ju) by and_i on 260, 262
            271) isPink(x, 2) by imp_e on 252, 270
            272) (3 > iu) & (3 < ju) by and_i on 261, 263
            273) isPink(x, 3) by imp_e on 253, 272
            
            300) isPink(x, 2) & isPink(x, 3) by and_i on 271, 273
        }
        310) case ju = 5 {
            311) forall s . (s > iu) & (s < ju) => isPink(x, s) by and_e on 200
            312) (4 > iu) & (4 < ju) => isPink(x, 4) by forall_e on 311
            313) (4 > 1) & (4 < ju) => isPink(x, 4) by eq_e on 223, 312
            314) (4 > 1) & (4 < 5) => isPink(x, 4) by eq_e on 310, 313
            315) 4 > 1 by arith
            316) 4 < 5 by arith
            317) (4 > 1) & (4 < 5) by and_i on 315, 316
            318) isPink(x, 4) by imp_e on 314, 317
            
            320) isBlue(x, 4) => !isPink(x, 4) by forall_e on 201
            321) !isPink(x, 4) by imp_e on 4, 320
        
            400) isPink(x, 2) & isPink(x, 3) by not_e on 318, 321
        }

        498) isPink(x, 2) & isPink(x, 3) by cases on 228, 229-239, 250-300, 310-400
    }
    499) isPink(x, 2) & isPink(x, 3) by exists_e on 11, 200-498
}

500) isPink(x, 2) & isPink(x, 3) by exists_e on 10, 11-499
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
