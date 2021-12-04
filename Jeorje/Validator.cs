using System;
using System.Collections.Generic;
using System.Linq;

namespace Jeorje
{
    public static class Validator
    {
        private static readonly List<string> _exitScopingRules = new List<string>()
        {
            "raa",
            "cases",
            "imp_i",
            "forall_i",
            "exists_e",
        };

        private static readonly List<string> _enterScopingRules = new List<string>()
        {
            "enter_raa",
            "enter_cases",
            "enter_imp_i",
            "enter_forall_i",
            "enter_exists_e",
        };
        
        public static string ValidateND(List<AST> premises, AST goal, List<NDRule> proof)
        {
            var symbolTableStack = new Stack<SymbolTable>();
            var currentSymbolTable = new SymbolTable();
            var scopesStack = new Stack<string>();
            var shouldEnterScope = false;
            var shouldExitScope = false;
            
            symbolTableStack.Push(currentSymbolTable);
            foreach (var rule in proof)
            {
                Logger.AddError($"Latest: Checking line with label ${rule.Label}");
                if (rule.CheckRule(symbolTableStack.Peek(), premises))
                {
                    if (shouldEnterScope && rule.Name != "lbrace")
                    {
                        throw new Exception($"Error on line {rule.Label}: Missing '{{'");
                    }
                    else if ( _enterScopingRules.Contains(rule.Name) )
                    {
                        scopesStack.Push(rule.Name);
                        shouldEnterScope = true;
                    }
                    else if ( _exitScopingRules.Contains(rule.Name) )
                    {
                        if (!shouldExitScope)
                        {
                            throw new Exception($"Error on line {rule.Label}: Missing '}}'");
                        }
                        symbolTableStack.Pop();
                        var ruleName = scopesStack.Pop();
                        shouldExitScope = false;
                    }
                    if (rule.Name == "lbrace")
                    {
                        if (!shouldEnterScope)
                        {
                            throw new Exception("Invalid use of '{'");
                        }
                        symbolTableStack.Push(symbolTableStack.Peek());
                        shouldEnterScope = false;
                    }
                    else if (rule.Name == "rbrace")
                    {
                        if (shouldExitScope || scopesStack.Count == 0)
                        {
                            throw new Exception("Invalid use of '}'");
                        }
                        shouldExitScope = true;
                    }
                    else
                    {
                        symbolTableStack.Peek().UpdateSymbols(rule.Label, rule.Predicate);
                    }
                    Logger.RemoveError();
                }
            }

            if (goal != proof.Last().Predicate)
            {
                throw new Exception("Last line of ND proof does not match goal! ");
            } 
            
            return "ND Proof is valid";
        }

        public static string ValidateST(List<AST> predicates, AST goal, List<AST> proof)
        {


            return "ST Proof is valid";
        }
    }
}