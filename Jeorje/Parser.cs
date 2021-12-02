using System.Collections.Generic;

namespace Jeorje
{
    public class Parser
    {
        private static List<TokenType> collapsibleOperators = new List<TokenType>
        {
            TokenType.And,
            TokenType.Or
        };

        public static AST collapseTree(BinaryAST input)
        {
            if (null == input)
            {
                return null;
            }
            
            if (input.Token.IsOperator)
            {
                // Operator, interesting stuff here
                if (collapsibleOperators.Contains(input.Token.TokenType))
                {
                    // Only consider the right node because the left node cannot be pulled in
                    return CollapseOperator(input);
                }
                else
                {
                    AST newNode = new AST(input.Token);
                    var left = collapseTree(input.left);
                    var right = collapseTree(input.right);
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
            var left = collapseTree(input.left);
            var right = input.right;
            AST newNode = new AST(input.Token);
            newNode.Children.Add(left);

            var rightTmp = right;
            while (rightTmp.Token.TokenType == input.Token.TokenType)
            {
                newNode.Children.Add(collapseTree(rightTmp.left));
                rightTmp = rightTmp.right;
            }

            newNode.Children.Add(collapseTree(rightTmp)); 
            return newNode;
        }

    }
}