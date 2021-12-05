﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Jeorje
{
    public class NDCases : NDRule
    {
        public static string _name = "cases";
        public override string Name => _name;

        public sealed override string Label { get; set; }
        public sealed override AST Predicate { get; set; }
        public sealed override List<string> Requirements { get; set; }

        public NDCases(string label, AST predicate, List<string> requirements)
        {
            Label = label;
            Requirements = requirements;
            Predicate = predicate;
        }
        
        public override bool CheckRule(SymbolTable symbolTable, List<AST> premises)
        {
            if (Requirements.Count < 3)
            {
                throw new Exception($"Expecting 3 or more premises, got {Requirements.Count}");
            }

            if (symbolTable.Statements[Requirements[0]].Token.TokenType != TokenType.Or)
            {
                throw new Exception($"First premise of cases must be or rule");
            }

            HashSet<AST> orStatementChildren = new HashSet<AST>();
            HashSet<AST> casesChildren = new HashSet<AST>();

            Queue<AST> childrenToTraverse = new Queue<AST>();
            
            foreach (var requirement in Requirements.GetRange(1, Requirements.Count - 1))
            {
                var casePredicate = symbolTable.Statements[requirement.Split('-')[0]];
                var caseConclusion = symbolTable.Statements[requirement.Split('-')[1]];
                childrenToTraverse.Enqueue(casePredicate);
                if (caseConclusion != Predicate)
                {
                    throw new Exception("Every case must conclude the case conclusion");
                }
            }
            while (childrenToTraverse.Count > 0)
            {
                var node = childrenToTraverse.Dequeue();
                if (node.Token.TokenType == TokenType.Or)
                {
                    childrenToTraverse.Enqueue(node.Children[0]);
                    childrenToTraverse.Enqueue(node.Children[1]);
                }
                else
                {
                    if (casesChildren.Contains(node))
                    {
                        throw new Exception("Cases must exactly cover all statements in or statement");
                    }
                    casesChildren.Add(node);
                }
            }
            
            childrenToTraverse.Enqueue(symbolTable.Statements[Requirements[0]]);
            while (childrenToTraverse.Count > 0)
            {
                var node = childrenToTraverse.Dequeue();
                if (node.Token.TokenType == TokenType.Or)
                {
                    childrenToTraverse.Enqueue(node.Children[0]);
                    childrenToTraverse.Enqueue(node.Children[1]);
                }
                else
                {
                    orStatementChildren.Add(node);
                }
            }
            
            if (!orStatementChildren.SetEquals(casesChildren))
            {
                throw new Exception("Cases must exactly cover all statements in or statement");
            }

            return true;
        }
    }
}