using System;
using System.Collections.Generic;
using System.Linq;

namespace Jeorje
{
    public class NDIffI : NDRule
    {
        public static string _name = "iff_i";
        public override string Name => _name;

        public sealed override string Label { get; set; }
        public sealed override AST Predicate { get; set; }
        public sealed override List<string> Requirements { get; set; }

        public NDIffI(string label, AST predicate, List<string> requirements)
        {
            Label = label;
            Requirements = requirements;
            Predicate = predicate;
        }
        //0) a  premise
        //1) a | (b | c) by or_i on 0
        public override bool CheckRule(SymbolTable symbolTable, List<AST> premises)
        {
            if (Requirements.Count !=2)
            {
                throw new Exception($"{_name} expecting 2 operands but received {Requirements.Count}");
            }

            var hyp1 = Predicate.Children[0]; // "P"
            var hyp2 = Predicate.Children[1];// "Q"
            
            /*
             * Could have 
             * P => Q, Q => P
             * or
             * Q => P, P => Q
             */

            var imp1 = symbolTable.Statements[Requirements[0]];
            var imp2 = symbolTable.Statements[Requirements[1]];

            if (imp1.Children[0] == hyp1 && imp1.Children[1] == hyp2 && imp2.Children[0] == hyp2 &&
                imp2.Children[1] == hyp1)
            {
                return true;
            }  else if (imp2.Children[0] == hyp1 && imp2.Children[1] == hyp2 && imp1.Children[0] == hyp2 &&
                        imp1.Children[1] == hyp1)
            {
                return true;
            }
            else
            {
                throw new Exception("Could not match any subrule of iff_i");
            }
            
            

        }
    }
}