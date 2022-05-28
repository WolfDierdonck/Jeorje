using System;
using System.Collections.Generic;
using Gentzen.Gentzen.Common;

namespace Gentzen.Gentzen
{
    public class ShuntingYard
    {
        private static void addNode(Stack<BinaryAST> stack, Token op) {
            var rightASTNode = stack.Pop();
            var leftASTNode = stack.Pop();
            var newNode = new BinaryAST(op);
            newNode.left = leftASTNode;
            newNode.right = rightASTNode;
            stack.Push(newNode);
        }
        
        private static Dictionary<TokenType, int> _operatorPrecedence = new Dictionary<TokenType, int>
        {
            {TokenType.FuncSeparator, 120},
            {TokenType.Forall, 11},
            {TokenType.Exists, 10},
            {TokenType.MathOperator, 90},
            {TokenType.CompareOperator, 89},
            {TokenType.Not, 80},
            {TokenType.And, 70},
            {TokenType.Or, 60},
            {TokenType.Implies, 50},
            {TokenType.Iff, 40},
            {TokenType.NotEqual, 30},
            {TokenType.Equal, 89},
            {TokenType.Comma, 20},
            {TokenType.Dot, 0},
            {TokenType.Colon, 122}
        };

        private static Dictionary<TokenType, bool> _operatorRightAssociative = new Dictionary<TokenType, bool>
        {
            {TokenType.Exists, true},
            {TokenType.Forall, true},
            {TokenType.FuncSeparator, true},
            {TokenType.MathOperator, false},
            {TokenType.CompareOperator, false},
            {TokenType.Not, true},
            {TokenType.And, true},
            {TokenType.Or, true},
            {TokenType.Implies, true},
            {TokenType.Iff, true},
            {TokenType.NotEqual, true},
            {TokenType.Equal, true},
            {TokenType.Dot, false},
            {TokenType.Comma, true},
            {TokenType.Colon, true}
        };
        public static BinaryAST ConvertInfixToAST(Line input)
        {
            Stack<Token> operatorStack = new Stack<Token>();
            Stack<BinaryAST> operandStack = new Stack<BinaryAST>();

            foreach (var t in input.Tokens)
            {
                Token popped;
                switch (t.TokenType)
                {
                    case TokenType.LParen:
                        operatorStack.Push(t);
                        break;
                    case TokenType.RParen:
                        var endOuterLoop = false;
                        while (operatorStack.Count != 0 && !endOuterLoop)
                        {
                            popped = operatorStack.Pop();
                            if (popped.TokenType == TokenType.LParen)
                            {
                                endOuterLoop = true; 
                            }
                            else
                            {
                                addNode(operandStack, popped);
                            }
                        }

                        /*if (operatorStack.Count > 0 && operatorStack.Peek().IsOperator)
                        {
                            addNode(operandStack, operatorStack.Pop());
                        }*/

                        if (!endOuterLoop)
                            
                        {
                            throw new Exception("There are no open left braces and we are trying to add a right brace");
                        }

                        break;
                    default:
                        if (t.IsOperator)
                        {
                            var o1 = t;
                            Token o2;
                            while (operatorStack.Count != 0 && null != (o2 = operatorStack.Peek()) && o2.TokenType != TokenType.LParen)
                            {
                                
                                if (_operatorPrecedence[o1.TokenType] == _operatorPrecedence[o2.TokenType] && !_operatorRightAssociative[o1.TokenType] || greaterPrecedence(o2, o1))
                                {
                                    operatorStack.Pop();
                                    addNode(operandStack, o2);
                                }
                                else
                                {
                                    break;
                                }
                            }
                            operatorStack.Push(t);
                        }
                        else
                        {
                            var nonOperatorNode = new BinaryAST(t);
                            operandStack.Push(nonOperatorNode);
                        }
                        break;
                }
            }

            while (operatorStack.Count != 0)
            {
                addNode(operandStack, operatorStack.Pop());
            }

            return operandStack.Pop();
        }

        private static bool greaterPrecedence(Token op1, Token op2) {
            if (_operatorPrecedence[op1.TokenType] > _operatorPrecedence[op2.TokenType]) { 
                return true;
            }
            return false;
        }

    }
    
}