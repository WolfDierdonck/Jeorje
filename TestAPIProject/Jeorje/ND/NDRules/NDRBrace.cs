﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Jeorje
{
    public class NDRBrace : NDRule
    {
        public static string _name = "rbrace";
        public override string Name => _name;

        public sealed override string Label { get; set; }
        public sealed override AST Predicate { get; set; }
        public sealed override List<string> Requirements { get; set; }

        public NDRBrace(string label, AST predicate, List<string> requirements)
        {
            Label = label;
            Requirements = requirements;
            Predicate = predicate;
        }
        
        public override bool CheckRule(SymbolTable symbolTable, List<AST> premises)
        {
            return true;
        }
    }
}