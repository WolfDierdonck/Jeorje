using System;
using System.Collections.Generic;
using System.Linq;
using Gentzen.Gentzen.Common;
using Gentzen.Gentzen.ND;

namespace Gentzen.Gentzen
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
            "case",
            "enter_imp_i",
            "enter_forall_i",
            "enter_exists_e",
        };

        private static readonly Dictionary<string, string> _enterToExit = new Dictionary<string, string>()
        {
            {"enter_raa", "raa"}, {"case", "cases"}, {"enter_imp_i", "imp_i"}, {"enter_forall_i", "forall_i"}, {"enter_exists_e", "exists_e"}
        };

        public static string ValidateND(List<AST> premises, AST goal, List<NDRule> proof)
        {
            var symbolTableStack = new Stack<SymbolTable>();
            var closedSymbolTables = new Dictionary<string, Tuple<SymbolTable, string>>();
            var scopesStack = new Stack<Tuple<string, string>>(); // First: rule name; second: rule label (where scope starts)
            var shouldEnterScope = false;
            var lastLabel = "";
            
            symbolTableStack.Push(new SymbolTable());
            foreach (var rule in proof)
            {
                Logger.AddError($"Latest: Checking line with label ${rule.Label}");
                var currentSymbolTable = symbolTableStack.Peek();
                if ( _exitScopingRules.Contains(rule.Name) )
                {
                    currentSymbolTable = new SymbolTable(new Dictionary<string, AST>(symbolTableStack.Peek().Statements), new HashSet<string>(symbolTableStack.Peek().Identifiers));
                    foreach (var requirement in rule.Requirements)
                    {
                        if (requirement.Split('-').Length == 2)
                        {
                            if (!closedSymbolTables.ContainsKey(requirement))
                            {
                                throw new Exception(
                                    $"Error on line {rule.Label}: Step depends on proof lines {requirement} which are not active lines at this point in the proof");
                            }
                            var symbolTableInfo = closedSymbolTables[requirement];
                            if (symbolTableInfo.Item2 != rule.Name)
                            {
                                throw new Exception(
                                    $"Error on line {rule.Label}: Invalid {rule.Name} usage");
                            }
                            symbolTableInfo.Item1.Statements.ToList().ForEach(x => currentSymbolTable.Statements[x.Key] = x.Value);
                            currentSymbolTable.Identifiers.UnionWith(symbolTableInfo.Item1.Identifiers);
                        }
                    }
                }
                if (rule.CheckRule(currentSymbolTable, premises))
                {
                    if (shouldEnterScope && rule.Name != "lbrace")
                    {
                        throw new Exception($"Error on line {rule.Label}: Missing '{{'");
                    }
                    else if ( _enterScopingRules.Contains(rule.Name) )
                    {
                        symbolTableStack.Push(new SymbolTable(new Dictionary<string, AST>(currentSymbolTable.Statements), new HashSet<string>(currentSymbolTable.Identifiers)));
                        scopesStack.Push(new Tuple<string, string>(_enterToExit[rule.Name], rule.Label));
                        shouldEnterScope = true;
                    }
                    if (rule.Name == "lbrace")
                    {
                        if (!shouldEnterScope)
                        {
                            throw new Exception("Invalid use of '{'");
                        }
                        shouldEnterScope = false;
                    }
                    else if (rule.Name == "rbrace")
                    {
                        if (scopesStack.Count == 0)
                        {
                            throw new Exception("No matching '{' found for '}'");
                        }
                        var scopeInfo = scopesStack.Pop();
                        closedSymbolTables.Add($"{scopeInfo.Item2}-{lastLabel}", new Tuple<SymbolTable, string>(symbolTableStack.Pop(), scopeInfo.Item1));
                    }
                    else
                    {
                        symbolTableStack.Peek().UpdateSymbols(rule.Label, rule.Predicate);
                        lastLabel = rule.Label;
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


    }
}