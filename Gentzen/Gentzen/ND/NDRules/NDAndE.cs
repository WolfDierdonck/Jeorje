using System;
using System.Collections.Generic;
using Gentzen.Gentzen.Common;

namespace Gentzen.Gentzen.ND.NDRules
{
    public class NDAndE : NDRule
    {
        public static string _name = "and_e";
        public override string Name => _name;

        public sealed override string Label { get; set; }
        public sealed override AST Predicate { get; set; }
        public sealed override List<string> Requirements { get; set; }

        public NDAndE(string label, AST predicate, List<string> requirements)
        {
            Label = label;
            Requirements = requirements;
            Predicate = predicate;
        }
        //0) a  premise
        //1) a | (b | c) by or_i on 0
        public override bool CheckRule(SymbolTable symbolTable, List<AST> premises)
        {
            if (Requirements.Count != 1)
            {
                throw new Exception($"{_name} must be performed on one line");
            }
            
            var requirement = Requirements[0];
            var requirementAST = symbolTable.Statements[requirement];

            if (!requirementAST.Children.Contains(Predicate))
            {
                throw new Exception($"Error on line with label {Label}: and_e used incorrectly");
            }

            return true;
        }
    }
}