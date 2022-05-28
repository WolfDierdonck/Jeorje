using System;
using System.Collections.Generic;
using System.Linq;
using Gentzen.Gentzen.Common;
using ValueType = Gentzen.Gentzen.Common.ValueType;

namespace Gentzen.Gentzen
{
    public static class Typing
    {
        public static void CheckPredicates(List<AST> predicates)
        {
            predicates.ForEach(CheckPredicate);
        }

        public static void CheckPredicate(AST predicate)
        {
            var typeTables = new Stack<TypeTable>();
            typeTables.Push(new TypeTable());

            PredicateType CheckType(AST ast, PredicateType expectedType) // expected type is the type the parent expects this child to have
            {
                var typeTable = typeTables.Peek();
                
                if (ast.Token.TokenType is TokenType.And or TokenType.Or or TokenType.Implies or TokenType.Iff)
                {
                    if (!IsTypeBool(expectedType)) // checks if expectedType is not a bool (null is counted as bool)
                    {
                        throw new Exception(
                            $"Error with {ast.Token.Lexeme}: Returns bool but is expected to return {expectedType}");
                    }
                    
                    ast.Children.ForEach(child => CheckType(child, CreateBool()));
                    return CreateBool();
                }
                
                if (ast.Token.TokenType is TokenType.True or TokenType.False)
                {
                    if (!IsTypeBool(expectedType)) // checks if expectedType is not a bool (null is counted as bool)
                    {
                        throw new Exception(
                            $"Error with {ast.Token.Lexeme}: Returns bool but is expected to return {expectedType}");
                    }
                    
                    return CreateBool();
                }

                if (ast.Token.TokenType is TokenType.Equal or TokenType.NotEqual)
                {
                    if (!IsTypeBool(expectedType)) // checks if expectedType is not a bool (null is counted as bool)
                    {
                        throw new Exception(
                            $"Error with {ast.Token.Lexeme}: Returns bool but is expected to return {expectedType}");
                    }

                    var leftType = CheckType(ast.Children[0], null);
                    var rightType = CheckType(ast.Children[1], leftType);
                    
                    if (leftType is null && rightType is not null)
                    {
                        AddBindingHelper(typeTable, ast.Children[0], ast.Children[1], rightType);
                    }
                    else if (leftType is not null && rightType is null)
                    {
                        AddBindingHelper(typeTable, ast.Children[1], ast.Children[0], leftType);
                    }
                    else if (leftType is null && rightType is null)
                    {
                        AddBindingHelper(typeTable, ast.Children[0], ast.Children[1], rightType);
                        AddBindingHelper(typeTable, ast.Children[0], ast.Children[1], leftType);
                    }
                    
                    else
                    {
                        if (leftType is ValueType leftValue && rightType is ValueType rightValue)
                        {
                            if (leftValue != rightValue)
                            {
                                throw new Exception($"Error with {ast.Token.Lexeme}: both sides must be of equal types");
                            }
                        }
                        
                        else if (leftType is FunctionType leftFunction && rightType is FunctionType rightFunction)
                        {
                            if (leftFunction != rightFunction)
                            {
                                throw new Exception($"Error with {ast.Token.Lexeme}: both sides must be of equal types");
                            }
                        }
                        else
                        {
                            throw new Exception($"Error with {ast.Token.Lexeme}: both sides must be of equal types");
                        }
                    }
                    return CreateBool();
                }

                if (ast.Token.TokenType == TokenType.Dot)
                {
                    var quantifier = ast.Children[0];
                    var newTypeTable = (TypeTable) typeTable.Clone();
                    typeTables.Push(newTypeTable);
                    typeTable = typeTables.Peek();

                    if (quantifier.Children[1].Token.TokenType != TokenType.Comma)
                    {
                        CheckType(quantifier.Children[1], null);
                    }
                    else // right side is a comma
                    {
                        var commaAst = quantifier.Children[1];
                        commaAst.Children.ForEach(child => CheckType(child, null));
                    }
                    
                    CheckType(ast.Children[1], CreateBool());
                    
                    CheckBindingsHelper(typeTable);
                    
                    typeTables.Pop();
                    return CreateBool();
                }

                if (ast.Token.TokenType == TokenType.MathOperator)
                {
                    if (!IsTypeNat(expectedType)) // checks if expectedType is not a bool (null is counted as bool)
                    {
                        throw new Exception(
                            $"Error with {ast.Token.Lexeme}: Returns bool but is expected to return {expectedType}");
                    }

                    CheckType(ast.Children[0], CreateNat());
                    CheckType(ast.Children[1], CreateNat());
                    return CreateNat();
                }

                if (ast.Token.TokenType == TokenType.CompareOperator)
                {
                    if (!IsTypeBool(expectedType)) // checks if expectedType is not a bool (null is counted as bool)
                    {
                        throw new Exception(
                            $"Error with {ast.Token.Lexeme}: Returns bool but is expected to return {expectedType}");
                    }

                    CheckType(ast.Children[0], CreateNat());
                    CheckType(ast.Children[1], CreateNat());
                    return CreateBool();
                }

                if (ast.Token.TokenType == TokenType.Not)
                {
                    if (!IsTypeBool(expectedType)) // checks if expectedType is not a bool (null is counted as bool)
                    {
                        throw new Exception(
                            $"Error with {ast.Token.Lexeme}: Returns bool but is expected to return {expectedType}");
                    }

                    CheckType(ast.Children[1], CreateBool());
                    return CreateBool();
                }

                if (ast.Token.TokenType == TokenType.FuncSeparator)
                {
                    var functionIdentifier = ast.Children[0].Token.Lexeme;

                    if (typeTable.Table.ContainsKey(functionIdentifier))
                    {
                        var returnType = ((FunctionType) typeTable.Table[functionIdentifier]).ReturnType;

                        if (returnType == null)
                        {
                            ((FunctionType) typeTable.Table[functionIdentifier]).ReturnType = expectedType;
                        }
                        
                        else if (expectedType == null) // REQUIRED, DO NOT REMOVE, DONT ASK QUESTIONS (not a joke)
                        {
                            
                        }
                        
                        else if (expectedType is ValueType expectedValue && returnType is ValueType returnValue)
                        {
                            if (returnValue != expectedValue)
                            {
                                throw new Exception($"Error with {functionIdentifier}: " +
                                                    $"Returns {returnType} but is expected to return {expectedType}");
                            }
                        }
                        
                        else if (expectedType is FunctionType expectedFunction && returnType is FunctionType returnFunction)
                        {
                            if (returnFunction != expectedFunction)
                            {
                                throw new Exception($"Error with {functionIdentifier}: " +
                                                    $"Returns {returnType} but is expected to return {expectedType}");
                            }
                        }
                        else
                        {
                            throw new Exception($"Error with {functionIdentifier}: " +
                                                $"Returns {returnType} but is expected to return {expectedType}");
                        }
                    }
                    else
                    {
                        typeTable.Table[functionIdentifier] = new FunctionType(null, expectedType);
                    }

                    var functionType = (FunctionType) typeTable.Table[functionIdentifier];

                    if (ast.Children[1].Token.TokenType != TokenType.Comma)
                    {
                        if (functionType.ParamTypes != null && functionType.ParamTypes.Count != 1)
                        {
                            throw new Exception("function must have same number of parameters everywhere");
                        }

                        var paramType = CheckType(ast.Children[1], functionType.ParamTypes?[0]);
                        functionType.ParamTypes = new List<PredicateType> {paramType};
                    }
                    else
                    {
                        var commaAst = ast.Children[1];
                        
                        if (functionType.ParamTypes == null)
                        {
                           functionType.ParamTypes = commaAst.Children.Select(
                               child => CheckType(child, null)).ToList();
                        }
                        else
                        {
                            var paramTypes = new List<PredicateType>();

                            if (functionType.ParamTypes.Count != commaAst.Children.Count)
                            {
                                throw new Exception("function must have same number of parameters everywhere");
                            }
                            
                            for (int i = 0; i < functionType.ParamTypes.Count; i++)
                            {
                                paramTypes.Add(CheckType(commaAst.Children[i], functionType.ParamTypes[i]));
                            }

                            functionType.ParamTypes = paramTypes;
                        }
                    }

                    return functionType.ReturnType;
                }

                if (ast.Token.TokenType == TokenType.Identifier)
                {
                    if (!typeTable.Table.ContainsKey(ast.Token.Lexeme))
                    {
                        typeTable.Table[ast.Token.Lexeme] = expectedType;
                        return expectedType;
                    }
                    
                    var identifierType = typeTable.Table[ast.Token.Lexeme];

                    if (expectedType == null)
                    {
                        return identifierType;
                    }
                    
                    if (identifierType == null)
                    {
                        typeTable.Table[ast.Token.Lexeme] = expectedType;
                        return expectedType;
                    }

                    if (expectedType is ValueType expectedValue && identifierType is ValueType returnValue)
                    {
                        if (returnValue != expectedValue)
                        {
                            throw new Exception($"Error with {ast.Token.Lexeme}: " +
                                                $"Returns {identifierType} but is expected to return {expectedType}");
                        }
                    }
                        
                    else if (expectedType is FunctionType expectedFunction && identifierType is FunctionType returnFunction)
                    {
                        if (returnFunction != expectedFunction)
                        {
                            throw new Exception($"Error with {ast.Token.Lexeme}: " +
                                                $"Returns {identifierType} but is expected to return {expectedType}");
                        }
                    }
                    else
                    {
                        throw new Exception($"Error with {ast.Token.Lexeme}: " +
                                            $"Returns {identifierType} but is expected to return {expectedType}");
                    }

                    return expectedType;
                }

                if (ast.Token.TokenType == TokenType.Colon)
                {
                    var rightType = new ValueType(ast.Children[1].Token.Lexeme);
                    return CheckType(ast.Children[0], rightType);
                }

                if (ast.Token.TokenType == TokenType.Label)
                {
                    if (int.TryParse(ast.Token.Lexeme, out _))
                    {
                        if (!IsTypeNat(expectedType)) // checks if expectedType is not a nat (null is counted as nat)
                        {
                            throw new Exception(
                                $"Error with {ast.Token.Lexeme}: Returns nat but is expected to return {expectedType}");
                        }

                        return CreateNat();
                    }
                }

                throw new Exception($"unsupported token {ast.Token.TokenType} while typing");
            }

            if (!IsTypeBool(CheckType(predicate, CreateBool())))
            {
                throw new Exception("outermost statement must be of type bool");
            }
            
            CheckBindingsHelper(typeTables.Peek());
            
        }

        private static void AddBindingHelper(TypeTable typeTable, AST notDefinedAST, AST definedAST, PredicateType rightType)
        {
            var leftIdentifier = notDefinedAST.Token.TokenType == TokenType.Identifier
                ? notDefinedAST.Token.Lexeme
                : notDefinedAST.Children[0].Token.Lexeme;
                        
            var rightIdentifier = definedAST.Token.TokenType is TokenType.Identifier or TokenType.Label
                ? definedAST.Token.Lexeme
                : definedAST.Children[0].Token.Lexeme;
                        
            if (definedAST.Token.TokenType is TokenType.Identifier or TokenType.FuncSeparator)
            {
                if (typeTable.Bindings.ContainsKey(notDefinedAST.Token.Lexeme))
                {
                    typeTable.Bindings[leftIdentifier].Add(rightIdentifier);
                }
                else
                {
                    typeTable.Bindings[leftIdentifier] = new HashSet<string> {rightIdentifier};
                }
            }
            else
            {
                if (typeTable.Bindings.ContainsKey(notDefinedAST.Token.Lexeme))
                {
                    typeTable.Bindings[leftIdentifier].Add(rightType.ToString());
                }
                else
                {
                    typeTable.Bindings[leftIdentifier] = new HashSet<string> {rightType.ToString()};
                }
            }
        }

        private static void CheckBindingsHelper(TypeTable typeTable)
        {
            foreach (var binding in typeTable.Bindings)
            {
                if (binding.Key != null)
                {
                    foreach (var type in binding.Value)
                    {
                        if (type != null)
                        {
                            var leftType = typeTable.Table[binding.Key];
                            if (!(leftType is null || leftType is FunctionType functionType && functionType.IsNull()))
                            {
                                if (typeTable.Table.ContainsKey(type))
                                {
                                    var rightType = typeTable.Table[type];
                                    if (leftType is ValueType leftValue && rightType is ValueType rightValue)
                                    {
                                        if (leftValue != rightValue)
                                        {
                                            throw new Exception($"Equal types do not match");
                                        }
                                    }

                                    else if (leftType is FunctionType leftFunction && rightType is FunctionType rightFunction)
                                    {
                                        if (leftFunction != rightFunction)
                                        {
                                            throw new Exception($"Equal types do not match");
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception($"Equal types do not match");
                                    }
                                }
                                else
                                {
                                if (leftType is ValueType)
                                {
                                    if (leftType.ToString() != type)
                                    {
                                        throw new Exception($"Equal types do not match");
                                    }
                                }
                                else
                                {
                                    if (((FunctionType) leftType).ReturnType.ToString() != type)
                                    {
                                        throw new Exception($"Equal types do not match");
                                    }
                                }
                            }
                            }
                        }
                    }
                }
            }
        }

        private static PredicateType CreateBool()
        {
            return new ValueType("bool");
        }

        private static PredicateType CreateNat()
        {
            return new ValueType("nat");
        }

        private static bool IsTypeBool(PredicateType type)
        {
            return type is null || type is ValueType && (ValueType) type == (ValueType) CreateBool();
        }
        
        private static bool IsTypeNat(PredicateType type)
        {
            return type is null || type is ValueType && (ValueType) type == (ValueType) CreateNat();
        }
    }
}