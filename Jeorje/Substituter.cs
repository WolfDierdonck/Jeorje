using System;
using System.Collections.Generic;

namespace Jeorje
{
    public static class Substituter
    {
        static void CheckFreeForVariables(AST root, SubstituterScope scope)
        {
            if (root.Token.TokenType == TokenType.Dot)
            {
                
                var newScope = scope.Clone();
                scope = (SubstituterScope)newScope;
                
                // if (root.Children.Count > 1 && root.Children[0].Token.TokenType == TokenType.Identifier)
                // {
                //     var newScope = scope.Clone();
                //     scope = (SubstituterScope)newScope;
                //     scope.removeBoundedVariable(root.Children[0].Token);
                // }
                // else
                // {
                //     throw new Exception("Internal Substituter error: Expecting identifier in CheckFreeForVariables but did not find it");
                // }
                
                // Comma hell
                
                
            }

            for (int i = 0; i < root.Children.Count; i++)
            {
                var child = root.Children[0];
                CheckFreeForVariables(child, scope);
            }
            
        }

        public static void CheckSubstituteAST(AST beforeSubstitution, AST afterSubstitution, AST toBeReplaced, AST replacement)
        {
            SubstituterScope scope = new SubstituterScope();
            CheckSubstituteASTHelper(beforeSubstitution, afterSubstitution, toBeReplaced, replacement, scope);
        }
        
        static void RemoveIdentifierFromScope(AST identifier, SubstituterScope scope)
        {
            // Identifier is not typed
            if (identifier.Token.TokenType == TokenType.Identifier) 
            {
                scope.removeBoundedVariable(identifier.Token);
            }
            else
            {
                throw new Exception("Internal Substituter error: Expecting identifier in RemoveIdentifierFromScope but did not find it");
            }
        }
        
        static void RemoveTypedIdentifierFromScope(AST colon, SubstituterScope scope)
        {
            // Identifier is typed
            if (colon.Token.TokenType == TokenType.Colon)
            {
                if (colon.Children.Count > 1 && colon.Children[0].Token.TokenType == TokenType.Identifier)
                {
                    scope.removeBoundedVariable(colon.Children[0].Token);    
                }
                else
                {
                    throw new Exception("Internal Substituter error: Expecting identifier in RemoveTypedIdentifierFromScope but did not find it");
                }
                
            }
            else
            {
                throw new Exception("Internal Substituter error: Expecting colon in Add but did not find it");

            }
            
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
        
        static void AddTypedIdentifier(AST colon, SubstituterScope scope)
        {
            // Identifier is typed
            if (colon.Token.TokenType == TokenType.Colon)
            {
                if (colon.Children.Count > 1 && colon.Children[0].Token.TokenType == TokenType.Identifier)
                {
                    scope.addBoundedVariable(colon.Children[0].Token);    
                }
                else
                {
                    throw new Exception("Internal Substituter error: Expecting identifier in AddTypedIdentifier but did not find it");
                }
                
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
            
            // Comma hell
            
            if (afterSubstitution.Children.Count != beforeSubstitution.Children.Count) 
                
                for (int i = 0; i < afterSubstitution.Children.Count; i++)
                {
                    var afterSubstitutionChild = afterSubstitution.Children[i];
                    var beforeSubstitutionChild = beforeSubstitution.Children[i];
                    
                    if (beforeSubstitutionChild == toBeReplaced)
                    {
                        CheckFreeForVariables(replacement, scope);
                    }
                    else
                    {
                        if (afterSubstitutionChild.Token != beforeSubstitutionChild.Token)
                        {
                            throw new Exception("Non substituted token does not match between after substitution token and before substitution token");
                        }
                        CheckSubstituteASTHelper(beforeSubstitution, afterSubstitution, toBeReplaced, replacement, scope);    
                    }
                
                }
        }
        
    }

    class SubstituterScope: ICloneable
    {
        
        public object Clone()
        {
            var clonedScope = new SubstituterScope();
            List<Token> newBoundedVariables = new List<Token>();
            boundedVariables.ForEach((item) =>
            {
                newBoundedVariables.Add((Token)item.Clone());
            });
            clonedScope.boundedVariables = newBoundedVariables;

            return clonedScope;
        }
        
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