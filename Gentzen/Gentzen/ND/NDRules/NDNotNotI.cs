using System;
using System.Collections.Generic;
using Gentzen.Gentzen.Common;

namespace Gentzen.Gentzen.ND.NDRules
{
    public class NDNotNotI : NDRule
    {
        public static string _name = "not_not_i";
        public override string Name => _name;

        public sealed override string Label { get; set; }
        public sealed override AST Predicate { get; set; }
        public sealed override List<string> Requirements { get; set; }

        public NDNotNotI(string label, AST predicate, List<string> requirements)
        {
            Label = label;
            Requirements = requirements;
            Predicate = predicate;
        }
        
        public override bool CheckRule(SymbolTable symbolTable, List<AST> premises)
        {
            var firstReq = Requirements[0];

            var firstAST = symbolTable.Statements[firstReq];

            if (Predicate.Token.TokenType == TokenType.Not && Predicate.Children[1].Token.TokenType == TokenType.Not)
            {
                if (Predicate.Children[1].Children[1].Equals(firstAST))
                {
                    return true;
                }
            }
            
            throw new Exception($"Error on line with label {Label}: not_not_i not used properly!");
            
            // if (Predicate.Token.TokenType == TokenType.Not)
            // {
            //     var negSide = Predicate.Children.Find(x => x.Token.TokenType == TokenType.Not);
            //
            //     //counting the number of extra nots in the predicate
            //     var i = 0;
            //     var duplicate = negSide;
            //     while (duplicate != null && duplicate.Children.Find(x => x.Token.TokenType == TokenType.Not) != null) 
            //     {
            //         duplicate = duplicate.Children.Find(x => x.Token.TokenType == TokenType.Not);
            //         i++;
            //     }
            //
            //     var otherSide = Predicate.Children.Find(x => x.Token.TokenType != TokenType.Not);
            //     
            //     //again counting number of extra nots in the premise
            //     var premiseDuplicate = firstAST;
            //     var testAST = firstAST;
            //     var j = 0;
            //     if (premiseDuplicate.Token.TokenType == TokenType.Not) 
            //     {
            //         testAST = premiseDuplicate.Children.Find(x => (x.Token.TokenType != TokenType.Not) && (x.Token.TokenType != TokenType.DummyNotOperand));
            //         j++;
            //     }
            //     while (premiseDuplicate != null && premiseDuplicate.Children.Find(x => x.Token.TokenType == TokenType.Not) != null) 
            //     {
            //         premiseDuplicate = premiseDuplicate.Children.Find(x => x.Token.TokenType == TokenType.Not);
            //         j++;
            //     }
            //
            //     //conditional checks
            //     if (negSide == null || negSide.Token.TokenType != TokenType.Not) 
            //     {
            //         throw new Exception($"Error on line with label {Label}: missing one not in not_not_i!");
            //     }
            //     if (i - j != 0) 
            //     {
            //         throw new Exception($"Error on line with label {Label}: nots mismatched in not_not_i!");
            //     }
            //     if (otherSide == null || !(otherSide == testAST))
            //     {
            //         throw new Exception($"Error on line with label {Label}: unable to match predicate in not_not_i!");
            //     }
            //
            // }
            // else
            // {
            //     throw new Exception($"Error on line with label {Label}: not_not_e not used properly!");
            // }

            return true;
        }
    }
}