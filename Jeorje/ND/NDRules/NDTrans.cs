using System;
using System.Collections.Generic;
using System.Linq;

namespace Jeorje
{
    public class NDTrans : NDRule
    {
        public static string _name = "trans";
        public override string Name => _name;

        public sealed override string Label { get; set; }
        public sealed override AST Predicate { get; set; }
        public sealed override List<string> Requirements { get; set; }

        public NDTrans(string label, AST predicate, List<string> requirements)
        {
            Label = label;
            Requirements = requirements;
            Predicate = predicate;
        }
        
        public override bool CheckRule(SymbolTable symbolTable, List<AST> premises)
        {
            
            var firstIff = Requirements[0];
            
            /* 
             * Must work in all ways
             * 1) a <=> b, b <=> c --> a <=> c
             * 2) a <=> b, a <=> c --> b <=> c
             * 3) b <=> c, 
             */
            
            return true;
        }
    }
}