using System;
using System.Collections.Generic;
using Gentzen.Gentzen.Common;

namespace Gentzen.Gentzen.ND.NDRules
{
    public class NDMagic : NDRule
    {
        public static string _name = "magic";
        public override string Name => _name;

        public sealed override string Label { get; set; }
        public sealed override AST Predicate { get; set; }
        public sealed override List<string> Requirements { get; set; }

        public NDMagic(string label, AST predicate, List<string> requirements)
        {
            Label = label;
            Requirements = requirements;
            Predicate = predicate;
        }
        //0) a&b|c=>d=>e&!z by magic
        public override bool CheckRule(SymbolTable symbolTable, List<AST> premises)
        {
            if (Requirements != null)
            {
                throw new Exception($"{_name} must be performed on 0 lines");
            }
            
            Logger.AddWarning($"Rule magic used on line with label {Label}");
            
            return true;
        }
    }
}