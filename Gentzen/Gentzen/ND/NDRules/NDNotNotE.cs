using System;
using System.Collections.Generic;
using Gentzen.Gentzen.Common;

namespace Gentzen.Gentzen.ND.NDRules
{
    public class NDNotNotE : NDRule
    {
        public static string _name = "not_not_e";
        public override string Name => _name;

        public sealed override string Label { get; set; }
        public sealed override AST Predicate { get; set; }
        public sealed override List<string> Requirements { get; set; }

        public NDNotNotE(string label, AST predicate, List<string> requirements)
        {
            Label = label;
            Requirements = requirements;
            Predicate = predicate;
        }
        
        public override bool CheckRule(SymbolTable symbolTable, List<AST> premises)
        {
            if (Requirements.Count != 1)
            {
                throw new Exception($"{_name} must be performed on one line");
            }

            var firstReq = Requirements[0];

            var firstAST = symbolTable.Statements[firstReq];

            if (firstAST.Token.TokenType == TokenType.Not)
            {
                var negSide = firstAST.Children.Find(x => x.Token.TokenType == TokenType.Not);

                //counting the number of extra nots in the premise
                var i = 0;
                var duplicate = negSide;
                while (duplicate != null && duplicate.Children.Find(x => x.Token.TokenType == TokenType.Not) != null) 
                {
                    duplicate = duplicate.Children.Find(x => x.Token.TokenType == TokenType.Not);
                    i++;
                }

                var otherSide = firstAST.Children.Find(x => x.Token.TokenType != TokenType.Not);
                
                //again counting number of extra nots in the predicate
                var predicateDuplicate = Predicate;
                var testAST = Predicate;
                var j = 0;
                if (predicateDuplicate.Token.TokenType == TokenType.Not) 
                {
                    testAST = predicateDuplicate.Children.Find(x => (x.Token.TokenType != TokenType.Not) && (x.Token.TokenType != TokenType.DummyNotOperand));
                    j++;
                }
                while (predicateDuplicate != null && predicateDuplicate.Children.Find(x => x.Token.TokenType == TokenType.Not) != null) 
                {
                    predicateDuplicate = predicateDuplicate.Children.Find(x => x.Token.TokenType == TokenType.Not);
                    j++;
                }

                //conditional checks
                if (negSide == null || negSide.Token.TokenType != TokenType.Not) 
                {
                    throw new Exception($"Error on line with label {Label}: missing one not in not_not_e!");
                }
                if (i - j != 0) 
                {
                    throw new Exception($"Error on line with label {Label}: nots mismatched in not_not_e!");
                }
                if (otherSide == null || !(otherSide == testAST))
                {
                    throw new Exception($"Error on line with label {Label}: unable to match predicate in not_not_e!");
                }

            }
            else
            {
                throw new Exception($"Error on line with label {Label}: not_not_e not used properly!");
            }

            return true;
        }
    }
}