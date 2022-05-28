using System;
using System.Collections.Generic;
using Gentzen.Gentzen.Common;

namespace Gentzen.Gentzen.ND.NDRules
{
    public class NDOrE : NDRule
    {
        public static string _name = "or_e";
        public override string Name => _name;

        public sealed override string Label { get; set; }
        public sealed override AST Predicate { get; set; }
        public sealed override List<string> Requirements { get; set; }

        public NDOrE(string label, AST predicate, List<string> requirements)
        {
            Label = label;
            Requirements = requirements;
            Predicate = predicate;
        }
        //0) !a premise
        //1) a | b | c premise
        //2) b | c by or_e on 0,1
        public override bool CheckRule(SymbolTable symbolTable, List<AST> premises)
        {
            
            if (Requirements == null || Requirements.Count != 2)
            {
                throw new Exception($"Error on line with label {Label}: {_name} must have 2 labels");
            }

            //the george implementation of or_e also sucks 
            var notSide = Requirements.Find(req => symbolTable.Statements[req].Token.TokenType == TokenType.Not);
            var orSide = Requirements.Find(req => req != notSide);

            if (notSide == null)
            {
                throw new Exception($"Error on line with label {Label}: could not find !Q");
            }

            if (orSide == null)
            {
                throw new Exception($"Error on line with label {Label}: could not find P | Q");
            }

            var toEliminate = symbolTable.Statements[notSide].Children[1];
            var eliminatedOrSide = symbolTable.Statements[orSide].Children.Find(x => x != toEliminate);

            if (Predicate != eliminatedOrSide)
            {
                throw new Exception($"Error on line with label {Label}: incorrect eliminate targets for or_e");
            }
            
            return true;
        }
    }
}