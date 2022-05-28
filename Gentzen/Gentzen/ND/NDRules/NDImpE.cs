using System;
using System.Collections.Generic;
using Gentzen.Gentzen.Common;

namespace Gentzen.Gentzen.ND.NDRules
{
    public class NDImpE : NDRule
    {
        public static string _name = "imp_e";
        public override string Name => _name;

        public sealed override string Label { get; set; }
        public sealed override AST Predicate { get; set; }
        public sealed override List<string> Requirements { get; set; }

        public NDImpE(string label, AST predicate, List<string> requirements)
        {
            Label = label;
            Requirements = requirements;
            Predicate = predicate;
        }
        
        public override bool CheckRule(SymbolTable symbolTable, List<AST> premises)
        {
            // There are 3 cases 
            // P => Q, P |- Q
            // P => Q, !Q |- !P
            // P => !Q, Q |- !P

            if (Requirements.Count != 2)
            {
                throw new Exception($"Expecting 2 premises, got {Requirements.Count}");
            }

            AST first = symbolTable.Statements[Requirements[0]];
            AST second = symbolTable.Statements[Requirements[1]];

            AST conditional; // terrible name but basically can be hypothesis, conclusion, negation of conclusion
            AST implication;
            
            if (first.Token.TokenType == TokenType.Implies && second.Token.TokenType != TokenType.Implies)
            {
                // first is the implication, second is the hypothesis
                conditional = second;
                implication = first;

            } else if (first.Token.TokenType != TokenType.Implies && second.Token.TokenType == TokenType.Implies)
            {
                // second is the implication, first is the hypothesis
                conditional = first;
                implication = second;

            } else if (first.Token.TokenType == TokenType.Implies && second.Token.TokenType == TokenType.Implies)
            {
                // there are three ways this can happen
                // either one matches hypothesis of other, one matches the conclusion of other, or one matches negation of conclusion of others

                if (second.Children[0] == first)
                {
                    // second is top level implication, first is hypothesis
                    conditional = first;
                    implication = second;
                } else if (first.Children[0] == second)
                {
                    // first is top level implication, second is hypothesis
                    conditional = second;
                    implication = first;
                }
                // else if (second.Children[1] == first)
                // {
                //     // second is top level implication, first is conclusion
                //     conditional = first;
                //     implication = second;
                // } else if (first.Children[1] == second)
                // {
                //     // first is top level implication, second is conclusion
                //     conditional = second;
                //     implication = first;
                // }
                else if (second.Children[1].Token.TokenType != TokenType.Not && first.Token.TokenType == TokenType.Not && first.Children[1] == second.Children[1])
                {
                    // second is top level implication, first is negation of nonegated conclusion
                    conditional = first;
                    implication = second;
                } else if (first.Children[1].Token.TokenType != TokenType.Not && second.Token.TokenType == TokenType.Not && second.Children[1] == first.Children[1])
                {
                    // first is top level implication, second is negation of nonegated conclusion
                    conditional = second;
                    implication = first;
                } else if (second.Children[1].Token.TokenType == TokenType.Not && second.Children[1].Children[1] == first)
                {
                    // second is top level implication, first is negation of negated conclusion
                    conditional = first;
                    implication = second;
                } else if (first.Children[1].Token.TokenType == TokenType.Not && first.Children[1].Children[1] == second)
                {
                    // first is top level implication, second is negation of negated conclusion
                    conditional = second;
                    implication = first;
                }
                else
                {
                    // oopsie
                    throw new Exception("Arguments do not match any subrule of imp_e");
                }
                // we need to determine which one is the correct hypothesis to use 
            }
            else
            {
                // neither is an implication
                throw new Exception("Neither operand to imp_e is an implication");
            }
            
            // Now need to check if the rule is actually correct LMAOOOOOOOOOOO
                if (implication.Children[0] == conditional)
                {
                    // conditional is hypothesis
                    if (Predicate != implication.Children[1])
                    {
                        throw new Exception("Arguments do not match any subrule of imp_e");
                    }
                } 
                // else if (implication.Children[1] == conditional)
                // {
                //     // conditional is conclusion
                //     return Predicate == implication.Children[0];
                //     return true;
                // }
                else if (implication.Children[1].Token.TokenType != TokenType.Not && conditional.Token.TokenType == TokenType.Not && conditional.Children[1] == implication.Children[1])
                {
                    // conditional is negation of nonnegated conclusion
                    if (!(Predicate.Token.TokenType == TokenType.Not && Predicate.Children[1] == implication.Children[0]))
                    {
                        throw new Exception("Arguments do not match any subrule of imp_e");
                    }
                }  
                else if (implication.Children[1].Token.TokenType == TokenType.Not && implication.Children[1].Children[1] == conditional)
                {
                    // conditional is negation of negated conclusion
                    if (!(Predicate.Token.TokenType == TokenType.Not && Predicate.Children[1] == implication.Children[0]))
                    {
                        throw new Exception("Arguments do not match any subrule of imp_e");
                    }
                }
                else
                {
                    // oopsie
                    throw new Exception("Arguments do not match any subrule of imp_e");
                }
                return true;
        }
    }
}