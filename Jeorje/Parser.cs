using System.Collections.Generic;
using System.Linq;

namespace Jeorje
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
            var ast = CollapseTree(ShuntingYard.ConvertInfixToAST(line));
            Typing.CheckPredicate(ast);
            Logger.RemoveError();
            return ast;
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