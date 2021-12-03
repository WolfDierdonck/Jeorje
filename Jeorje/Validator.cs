using System.Collections.Generic;

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
            
            symbolTableStack.Push(currentSymbolTable);
            foreach (var rule in proof)
            {
                if (rule.CheckRule(symbolTableStack.Peek(), premises))
                {
                    if ( _enterScopingRules.Contains(rule.Name) )
                    {
                        symbolTableStack.Push(symbolTableStack.Peek());
                    }
                    else if ( _exitScopingRules.Contains(rule.Name) )
                    {
                        symbolTableStack.Pop();
                    }
                    symbolTableStack.Peek().UpdateSymbols(rule.Label, rule.Predicate);
                }
            }
            

            return "ND Proof is valid";
        }

        public static string ValidateST(List<AST> predicates, AST goal, List<AST> proof)
        {


            return "ST Proof is valid";
        }
    }
}