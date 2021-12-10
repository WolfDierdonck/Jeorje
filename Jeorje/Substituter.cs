using System;
using System.Collections.Generic;

namespace Jeorje
{
    public static class Substituter
    {

        static void CheckFreeForVariables(AST toReplaceWith, SubstituterScope scope)
        {
            
        }

        public static void CheckSubstituteAST(AST beforeSubstitution, AST afterSubstitution, AST toBeReplaced, AST replacement)
        {
            SubstituterScope scope = new SubstituterScope();
            CheckSubstituteASTHelper(beforeSubstitution, afterSubstitution, toBeReplaced, replacement, scope);
        }

        static void AddIdentifierToScope(AST identifier, SubstituterScope scope)
        {
            // Identifier is not typed
            if (identifier.Token.TokenType == TokenType.Identifier) 
            {
                scope.addBoundedVariable(identifier.Token);
            }
            else
            {
                throw new Exception("Internal Substituter error: Expecting identifier in AddIdentifierToScope but did not find it");

            }
        }
        
        static void AddTypedIdentifier(AST identifier, SubstituterScope scope)
        {
            // Identifier is typed
            if (identifier.Token.TokenType == TokenType.Colon)
            {
                
            }
            else
            {
                throw new Exception("Internal Substituter error: Expecting colon in Add but did not find it");

            }
            
        }
        
        static void CheckSubstituteASTHelper(AST beforeSubstitution, AST afterSubstitution, AST toBeReplaced, AST replacement, SubstituterScope scope)
        {

            if (beforeSubstitution == null && afterSubstitution == null)
            {
                return;
            } else if (beforeSubstitution == null || afterSubstitution == null)
            {
                throw new Exception("Substitution: One of the two trees unexpecctedly null. Both expected to be same or differing by substitution");
            }
            
            // Need to handle scoping here :)
            
            if (afterSubstitution.Children.Count != beforeSubstitution.Children.Count)
            
            for (int i = 0; i < afterSubstitution.Children.Count; i++)
            {
                // var afterSubstitutionChild = afterSubstitution.Children[i];
                // var beforeSubstitutio
                //
                // if (child == toBeReplaced)
                // {
                //     CheckFreeForVariables(toReplaceWith, scope);
                // }
                // else
                // {
                //     CheckSubstituteASTHelper(child, toBeReplaced, toReplaceWith, scope);    
                // }
                
            }
        }
        
    }

    class SubstituterScope
    {
        private List<Token> boundedVariables;

        public bool containsBoundedVariable(Token t)
        {
            return boundedVariables.Contains(t);
        }

        public void addBoundedVariable(Token t)
        {
            if (containsBoundedVariable(t))
            {
                throw new Exception($"Token {t.Lexeme} already exists in scope");
            }
            boundedVariables.Add(t);
        }
        
        public void removeBoundedVariable(Token t)
        {
            boundedVariables.Remove(t);
        }
        
    }
    
}