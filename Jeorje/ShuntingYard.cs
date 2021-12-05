using System;
using System.Collections.Generic;

namespace Jeorje
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
            {TokenType.Forall, 110},
            {TokenType.Exists, 100},
            {TokenType.MathOperator, 90},
            {TokenType.Not, 80}, // is this correct, shouldnt it be the highest? of the operators, it is
            {TokenType.And, 70},
            {TokenType.Or, 60},
            {TokenType.Implies, 50},
            {TokenType.Iff, 40},
            {TokenType.NotEqual, 30},
            {TokenType.Equal, 20},
            {TokenType.Comma, 10},
            {TokenType.Dot, 0},
        };

        private static Dictionary<TokenType, bool> _operatorRightAssociative = new Dictionary<TokenType, bool>
        {
            {TokenType.Exists, true},
            {TokenType.Forall, true},
            {TokenType.FuncSeparator, true},
            {TokenType.MathOperator, false},
            {TokenType.Not, true},
            {TokenType.And, true},
            {TokenType.Or, true},
            {TokenType.Implies, false},
            {TokenType.Iff, false},
            {TokenType.NotEqual, false},
            {TokenType.Equal, false},
            {TokenType.Dot, true},
            {TokenType.Comma, true},
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
                                if (!_operatorRightAssociative[o1.TokenType] || !greaterPrecedence(o1, o2))
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