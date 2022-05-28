using System.Collections.Generic;
using Gentzen.Gentzen.Common;

namespace Gentzen.Gentzen.ND.NDRules
{
    public class NDArith : NDRule
    {
        public static string _name = "arith";
        public override string Name => _name;

        public sealed override string Label { get; set; }
        public sealed override AST Predicate { get; set; }
        public sealed override List<string> Requirements { get; set; }

        public NDArith(string label, AST predicate, List<string> requirements)
        {
            Label = label;
            Requirements = requirements;
            Predicate = predicate;
        }
        //0) a&b|c=>d=>e&!z by magic
        public override bool CheckRule(SymbolTable symbolTable, List<AST> premises)
        {
            Logger.AddWarning($"Rule arith used on line with label {Label}");
            return true;
        }
    }
}