using System;
using System.Collections.Generic;

namespace Jeorje
{
    public static class Substituter
    {

        static bool FreeForVariables(AST toReplaceWith, SubstituterScope scope)
        {
            return true;
        }

        public static void CheckSubstituteAST(AST predicate, AST toBeReplaced, AST toReplaceWith)
        {
            SubstituterScope scope = new SubstituterScope();
            CheckSubstituteASTHelper(predicate, toBeReplaced, toReplaceWith, scope);
        }

        static void CheckSubstituteASTHelper(AST predicate, AST toBeReplaced, AST toReplaceWith, SubstituterScope scope)
        {

            if (predicate == null)
            {
                return;
            }
            
            if (predicate.Token.TokenType == TokenType.Dot)
            {
                // SubstituterScope newScope =  ;
                
                if (predicate.Children.Count > 0 && predicate.Children[0].Token.TokenType == TokenType.Forall)
                {
                    // either typed or untyped here
                    if (predicate.Children.Count > 1 && predicate.Children[1].Token.TokenType == TokenType.Colon)
                    {
                        AST colon = predicate.Children[1];
                        AST id = colon.Children[0];
                        AST type = colon.Children[1];

                    } else if (predicate.Children.Count > 1 && predicate.Children[1].Token.TokenType == TokenType.Identifier)
                    {
                        AST id = predicate.Children[1];
                    }
                    else
                    {
                        throw new Exception("Invalid token type found: expecting colon or identifier");
                    }
                    
                }
                else
                {
                    throw new Exception("Forall not found next to dot");
                }
            }

            for (int i = 0; i < predicate.Children.Count; i++)
            {
                var child = predicate.Children[i];
                if (child == toBeReplaced && FreeForVariables(toReplaceWith, scope))
                {
                    // predicate.Children[i] = toReplaceWith;
                }
                else
                {
                    CheckSubstituteASTHelper(child, toBeReplaced, toReplaceWith, scope);    
                }
                
            }
        }
        
    }

    class SubstituterScope
    {
        private List<Token> boundedVariables;
    }
    
}