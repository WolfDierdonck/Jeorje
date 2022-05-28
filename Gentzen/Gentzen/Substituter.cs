using System;
using System.Collections.Generic;
using Gentzen.Gentzen.Common;

namespace Gentzen.Gentzen
{
    public static class Substituter
    {
        static void CheckFreeForVariables(AST root, SubstituterScope scope)
        {
            if (root.Token.TokenType == TokenType.Dot)
            {
                
                var newScope = scope.Clone();
                scope = (SubstituterScope)newScope;
                
                // Comma hell
                // dot --> forall --> comma or id
                if (root.Children.Count > 0 && root.Children[0].Children.Count > 1)
                {
                    var extractedIdentifiersFromForall = extractIdentifiersFromList(root.Children[0].Children[1]);
                    foreach (var identifier in extractedIdentifiersFromForall)
                    {
                        scope.removeBoundedVariable(identifier);
                    }
                }
                else
                {
                    throw new Exception("Invalid children count for Check free variable dots");
                }
            }

            if (root.Token.TokenType == TokenType.Identifier)
            {
                if (scope.containsBoundedVariable(root.Token))
                {
                    throw new Exception("Variable capture");
                }
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
        //
        // static void RemoveIdentifierFromScope(AST identifier, SubstituterScope scope)
        // {
        //     // Identifier is not typed
        //     if (identifier.Token.TokenType == TokenType.Identifier) 
        //     {
        //         scope.removeBoundedVariable(identifier.Token);
        //     }
        //     else
        //     {
        //         throw new Exception("Internal Substituter error: Expecting identifier in RemoveIdentifierFromScope but did not find it");
        //     }
        // }
        //
        // static void RemoveTypedIdentifierFromScope(AST colon, SubstituterScope scope)
        // {
        //     // Identifier is typed
        //     if (colon.Token.TokenType == TokenType.Colon)
        //     {
        //         if (colon.Children.Count > 1 && colon.Children[0].Token.TokenType == TokenType.Identifier)
        //         {
        //             scope.removeBoundedVariable(colon.Children[0].Token);    
        //         }
        //         else
        //         {
        //             throw new Exception("Internal Substituter error: Expecting identifier in RemoveTypedIdentifierFromScope but did not find it");
        //         }
        //         
        //     }
        //     else
        //     {
        //         throw new Exception("Internal Substituter error: Expecting colon in Add but did not find it");
        //
        //     }
        //     
        // }
        //
        // static void AddIdentifierToScope(AST identifier, SubstituterScope scope)
        // {
        //     // Identifier is not typed
        //     if (identifier.Token.TokenType == TokenType.Identifier) 
        //     {
        //         scope.addBoundedVariable(identifier.Token);
        //     }
        //     else
        //     {
        //         throw new Exception("Internal Substituter error: Expecting identifier in AddIdentifierToScope but did not find it");
        //     }
        // }
        //
        // static void AddTypedIdentifier(AST colon, SubstituterScope scope)
        // {
        //     // Identifier is typed
        //     if (colon.Token.TokenType == TokenType.Colon)
        //     {
        //         if (colon.Children.Count > 1 && colon.Children[0].Token.TokenType == TokenType.Identifier)
        //         {
        //             scope.addBoundedVariable(colon.Children[0].Token);    
        //         }
        //         else
        //         {
        //             throw new Exception("Internal Substituter error: Expecting identifier in AddTypedIdentifier but did not find it");
        //         }
        //         
        //     }
        //     else
        //     {
        //         throw new Exception("Internal Substituter error: Expecting colon in Add but did not find it");
        //
        //     }
        //     
        // }

        static List<Token> extractIdentifiersFromList(AST root)
        {
            List<Token> extractedIdentifiers = new List<Token>();
            // Root could be comma, colon, or identifier
            
            switch (root.Token.TokenType)
            {
                case TokenType.Colon:
                    if (root.Children.Count > 0 && root.Children[0].Token.TokenType == TokenType.Identifier)
                    {
                        extractedIdentifiers.Add(root.Children[0].Token);
                    }
                    else
                    {
                        throw new Exception("Colon with only one child (or no identifier)! Invalid! Why would you do this?!");
                    }
                    break;
                case TokenType.Identifier:
                    extractedIdentifiers.Add(root.Token);
                    break;
                case TokenType.Comma:
                    foreach (var child in root.Children)
                    {
                        extractedIdentifiers.AddRange(extractIdentifiersFromList(child)); 
                    } 
                    break;
                default:
                    throw new Exception("expecting colon, identifier or comma while doing comma hell");
            }

            return extractedIdentifiers;
        }
        
        static void CheckSubstituteASTHelper(AST beforeSubstitution, AST afterSubstitution, AST toBeReplaced, AST replacement, SubstituterScope scope)
        {

            if (beforeSubstitution == null && afterSubstitution == null)
            {
                return;
            } else if (beforeSubstitution == null || afterSubstitution == null)
            {
                throw new Exception("Substitution: One of the two trees unexpectedly null. Both expected to be same or differing by substitution");
            }
            
            // Need to handle scoping here :)
            if (beforeSubstitution.Token.TokenType == TokenType.Dot)
            {
                
                var newScope = scope.Clone();
                scope = (SubstituterScope)newScope;
                
                // Comma hell
                // dot --> forall --> comma or id
                if (beforeSubstitution.Children.Count > 0 && beforeSubstitution.Children[0].Children.Count > 1)
                {
                    var extractedIdentifiersFromForall = extractIdentifiersFromList(beforeSubstitution.Children[0].Children[1]);
                    foreach (var identifier in extractedIdentifiersFromForall)
                    {
                        scope.addBoundedVariable(identifier);
                    }
                }
                else
                {
                    throw new Exception("Invalid children count for Check free variable dots");
                }
            }


            if (afterSubstitution.Children.Count != beforeSubstitution.Children.Count)
            {
                throw new Exception("mismatched children count in substitution");
            }
                
                for (int i = 0; i < afterSubstitution.Children.Count; i++)
                {
                    var afterSubstitutionChild = afterSubstitution.Children[i];
                    var beforeSubstitutionChild = beforeSubstitution.Children[i];
                    
                    if (beforeSubstitutionChild == toBeReplaced)
                    {
                        CheckFreeForVariables(replacement, scope);
                        if (afterSubstitutionChild != replacement)
                        {
                            throw new Exception("subsituted wrong value");
                        }
                    } else if (beforeSubstitutionChild.Token.TokenType == TokenType.Forall)
                    {
                        if (beforeSubstitutionChild.Token == afterSubstitutionChild.Token)
                        return;
                    }
                    else
                    {
                        if (afterSubstitutionChild.Token != beforeSubstitutionChild.Token)
                        {
                            throw new Exception("Non substituted token does not match between after substitution token and before substitution token");
                        }
                        CheckSubstituteASTHelper(beforeSubstitutionChild, afterSubstitutionChild, toBeReplaced, replacement, scope);    
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

        public SubstituterScope()
        {
            boundedVariables = new List<Token>();
        }

        public bool containsBoundedVariable(Token t)
        {
            return boundedVariables.Contains(t);
        }

        public void addBoundedVariable(Token t)
        {
            if (!containsBoundedVariable(t))
            {
                boundedVariables.Add(t);
            }
        }
        
        public void removeBoundedVariable(Token t)
        {
            boundedVariables.Remove(t);
        }
        
    }
    
}