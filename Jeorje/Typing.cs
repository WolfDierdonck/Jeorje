using System;
using System.Collections.Generic;
using System.Linq;

namespace Jeorje
{
    public static class Typing
    {
        public static void CheckPredicates(List<AST> predicates)
        {
            predicates.ForEach(CheckPredicate);
        }

        public static void CheckPredicate(AST predicate)
        {
            var typeTable = new TypeTable();
            var existingTypes = new HashSet<ValueType>();

            HashSet<PredicateType> CheckType(AST ast, HashSet<PredicateType> possibleTypes)
            {
                if (ast.Token.TokenType is TokenType.And or TokenType.Or or TokenType.Implies or TokenType.Iff)
                {
                    if (possibleTypes.Contains(new ValueType("bool"))) // AND/OR/<=>/=> must return a bool
                    {
                        ast.Children.ForEach(child => CheckType(child, CreateBool())); // every child must return a bool
                    
                        return CreateBool();
                    }
                }
                
                if (ast.Token.TokenType is TokenType.True or TokenType.False)
                {
                    if (possibleTypes.Contains(new ValueType("bool"))) // true/false must return a bool
                    {
                        return CreateBool();
                    }
                }

                if (ast.Token.TokenType is TokenType.Equal or TokenType.NotEqual)
                {
                    if (possibleTypes.Contains(new ValueType("bool")))
                    {
                        var typeOverlap = CheckType(ast.Children[0], typeTable.Table[ast.Children[0]])
                            .Union(CheckType(ast.Children[1], typeTable.Table[ast.Children[0]])).ToHashSet();
                    
                        if (typeOverlap.Any()) // left and right types must have overlap
                        {
                            typeTable.UpdateEntry(ast.Children[0], typeOverlap);
                            typeTable.UpdateEntry(ast.Children[1], typeOverlap);
                    
                            return CreateBool();
                        }
                    }
                }

                if (ast.Token.TokenType == TokenType.Dot)
                {
                    // TODO: overwrite typeTable with stuff on ast.children[0]
                    
                    return CheckType(ast.Children[1], possibleTypes);
                }

                if (ast.Token.TokenType == TokenType.MathOperator)
                {
                    if (possibleTypes.Contains(new ValueType("bool")))
                    {
                        if (CheckType(ast.Children[0], CreateNat()).Contains(new ValueType("nat")) 
                            && CheckType(ast.Children[1], CreateNat()).Contains(new ValueType("nat")))
                        {
                            return CreateBool();
                        }
                    }
                    
                }

                if (ast.Token.TokenType == TokenType.Comma)
                {
                    return ast.Children.Select(child => (PredicateType) new FunctionType(
                        CheckType(child, null), null)).ToHashSet();
                    // this is wrong, it must enumerate all possible combinations!
                }

                if (ast.Token.TokenType == TokenType.Not)
                {
                    if (possibleTypes.Contains(new ValueType("bool")))
                    {
                        return CheckType(ast.Children[1], CreateBool());
                    }
                }

                if (ast.Token.TokenType == TokenType.FuncSeparator)
                {
                    var function = ast.Children[0];
                    var argTypes = CheckType(ast.Children[1], null).Select(arg => 
                        arg is ValueType ? new FunctionType(new HashSet<PredicateType> {arg}, null) : arg)
                        .ToHashSet(); // do we need possibleTypes?
                    
                    if (!typeTable.Table.ContainsKey(function))
                    {
                        
                    }
                }

                if (ast.Token.TokenType == TokenType.Identifier)
                {
                    // TODO: :)
                }

                throw new Exception("Typing failed");
            }
            
            var topType = CheckType(predicate, CreateBool());
            
            if (!topType.SetEquals(CreateBool()))
            {
                throw new Exception("Top type is not a bool");
            }
        }


        private static HashSet<PredicateType> CreateBool()
        {
            return new HashSet<PredicateType> {new ValueType("bool")};
        }

        private static HashSet<PredicateType> CreateNat()
        {
            return new HashSet<PredicateType>() {new ValueType("nat")};
        }

        private static bool IsTypeBool(HashSet<PredicateType> types)
        {
            if (types.Contains(new ValueType("bool")))
            {
                return true;
            }

            return false;
        }

        private static bool IsTypeNat(HashSet<PredicateType> types)
        {
            if (types.Contains(new ValueType("nat")))
            {
                return true;
            }

            return false;
        }
    }
}