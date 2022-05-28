using System.Collections.Generic;
using System.Linq;
using Gentzen.Gentzen.Common;

namespace Gentzen.Gentzen
{
    public class Parser
    {
        private static readonly List<TokenType> _collapsibleOperators = new List<TokenType>
        {
            TokenType.And,
            TokenType.Or,
            TokenType.Comma
        };

        public static List<AST> ParseLines(List<Line> lines)
        {
            return lines.Select(ParseLine).ToList();
        }

        public static AST ParseLine(Line line)
        {
            Logger.AddError($"Latest: Parsing line {line}");
            line = QuantifierScoping(line);
            var ast = CollapseTree(ShuntingYard.ConvertInfixToAST(line));
            Typing.CheckPredicate(ast);
            Logger.RemoveError();
            return ast;
        }

        public static Line QuantifierScoping(Line line)
        {
            List<Token> finalTokens = new List<Token>();
            
            Stack<int> stack = new Stack<int>();
            int currentCount = 0;

            for (int i = 0; i < line.Tokens.Count; i++)
            {
                Token currentToken = line.Tokens[i];
                if (currentToken.TokenType == TokenType.DummyQuantifierOperand)
                {
                    currentCount++;
                    finalTokens.Add(new Token(TokenType.LParen, "("));
                    
                } else if (currentToken.TokenType == TokenType.LParen)
                { 
                    stack.Push(currentCount);
                    currentCount++;
                } else if (currentToken.TokenType == TokenType.RParen)
                {
                    int numLParensWhenEnteringScope = stack.Pop();
                    int numExcessParensToAdd = currentCount - numLParensWhenEnteringScope - 1;
                    for (int j = 0; j < numExcessParensToAdd; j++)
                    {
                        finalTokens.Add(new Token(TokenType.RParen, ")"));
                    }
                    currentCount -= numExcessParensToAdd;
                    currentCount -= 1; // for the prexisting rparen
                }
                finalTokens.Add(currentToken);
            }

            // Add excess parens at the end
            for (int i = 0; i < currentCount; i++)
            {
                finalTokens.Add(new Token(TokenType.RParen, ")"));
            }
           
            return new Line(finalTokens);
        }

        private static AST CollapseTree(BinaryAST input)
        {
            if (null == input)
            {
                return null;
            }

            if (input.Token.IsOperator)
            {
                // Operator, interesting stuff here
                if (_collapsibleOperators.Contains(input.Token.TokenType))
                {
                    // Only consider the right node because the left node cannot be pulled in
                    return CollapseOperator(input);
                }
                else
                {
                    AST newNode = new AST(input.Token);
                    var left = CollapseTree(input.left);
                    var right = CollapseTree(input.right);
                    newNode.Children.Add(left);
                    newNode.Children.Add(right);
                    return newNode;
                }
            }
            else
            {
                return new AST(input.Token);
            }
        }

        private static AST CollapseOperator(BinaryAST input)
        {   
            var left = CollapseTree(input.left);
            var right = input.right;
            AST newNode = new AST(input.Token);
            newNode.Children.Add(left);

            var rightTmp = right;
            while (rightTmp.Token.TokenType == input.Token.TokenType)
            {
                newNode.Children.Add(CollapseTree(rightTmp.left));
                rightTmp = rightTmp.right;
            }

            newNode.Children.Add(CollapseTree(rightTmp)); 
            return newNode;
        }

    }
}