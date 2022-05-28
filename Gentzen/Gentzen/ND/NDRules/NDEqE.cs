using System;
using System.Collections.Generic;
using Gentzen.Gentzen.Common;

namespace Gentzen.Gentzen.ND.NDRules
{
    public class NDEqE : NDRule
    {
        public static string _name = "eq_e";
        public override string Name => _name;

        public sealed override string Label { get; set; }
        public sealed override AST Predicate { get; set; }
        public sealed override List<string> Requirements { get; set; }

        public NDEqE(string label, AST predicate, List<string> requirements)
        {
            Label = label;
            Requirements = requirements;
            Predicate = predicate;
        }

        public override bool CheckRule(SymbolTable symbolTable, List<AST> premises)
        {
            if (Requirements == null || Requirements.Count != 2)
            {
                throw new Exception($"Error on line with label {Label}: eq_e must take two arguments");
            }


            //the george implementation of or_e also sucks 
            var eqSide = Requirements.Find(req => symbolTable.Statements[req].Token.TokenType == TokenType.Equal);
            var otherSide = Requirements.Find(req => req != eqSide);

            if (eqSide == null)
            {
                throw new Exception($"Error on line with label {Label}: could not find t1 = t2");
            }

            if (otherSide == null)
            {
                throw new Exception($"Error on line with label {Label}: could not find P");
            }
            
            Logger.AddError($"Latest: Checking eq_e on line with label {Label}");

            try
            {
                Substituter.CheckSubstituteAST(symbolTable.Statements[otherSide], Predicate,
                    symbolTable.Statements[eqSide].Children[0], symbolTable.Statements[eqSide].Children[1]);
            }
            catch (Exception e)
            {
                Logger.AddError(e.Message);
                Substituter.CheckSubstituteAST(symbolTable.Statements[otherSide], Predicate,
                    symbolTable.Statements[eqSide].Children[1], symbolTable.Statements[eqSide].Children[0]);
                Logger.RemoveError();
            }
            
            Logger.RemoveError();

            return true;
        }
    }
}