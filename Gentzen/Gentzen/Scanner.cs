using System;
using System.Collections.Generic;
using System.Linq;
using Gentzen.Gentzen.Common;
using Gentzen.Gentzen.ND;

namespace Gentzen.Gentzen
{
    public static class Scanner
    {
      private static List<T> Tail<T>(this List<T> list)
      {
        return list.GetRange(1, list.Count - 1);
      }
      
      public static List<Token> MaximalMunchScan(string input) {
        // def scanOne(input: List[Char], state: State, backtrack: (List[Char], State) ): (List[Char], State) = {
        //   if (!input.isEmpty && dfa.transition.isDefinedAt((state, input.head))) {
        //     val newState = dfa.transition(state, input.head)
        //     val newBack = if (dfa.accepting.contains(newState)) (input.tail,dfa.transition(state, input.head)) else backtrack
        //     return scanOne(input.tail, newState, newBack)
        //   }
        //   backtrack
        // }
        static (List<char>, string) ScanOne(List<char> input, string state, (List<char>, string) backtrack ) {
          if (input.Any() && !string.IsNullOrEmpty(NDDFA.Transition((state, input[0])))) {
            var newState = NDDFA.Transition((state, input[0]));
            var newBack = NDDFA.Accepting.Contains(newState)
              ? (input.Tail(), NDDFA.Transition((state, input[0])))
              : backtrack;
            return ScanOne(input.Tail(), newState, newBack);
          }

          return backtrack;
        }
        
        // def listDiff[A](list: List[A], rest: List[A]): List[A] =
        // if (list eq rest) Nil
        // else list.head :: listDiff(list.tail, rest)
        
        static List<char> ListDiff(List<char> list, List<char> rest)
        {
          if (list.Count == rest.Count)
          {
            bool listsEqual = true;
            for (int i = 0; i < list.Count; i++)
            {
              if (list[i] != rest[i])
              {
                listsEqual = false;
              }
            }

            if (listsEqual)
            {
              return new List<char>();
            }
          }

          var temp = ListDiff(list.Tail(), rest);
          temp.Insert(0, list[0]);
          return temp;
          
          // if (list eq rest) Nil
          // else list.head::listDiff(list.tail, rest)
        }
        
        
        // def recur(input: List[Char], accumulator: List[Token] = List.empty): List[Token] = {
        //   if (input.isEmpty) {
        //     return accumulator.reverse
        //   }
        //   val toAdd = scanOne(input, dfa.start,(input, dfa.start))
        //   if (toAdd._1 == input) {
        //     sys.error("Input is not in DFA at "+input)
        //   }
        //   val newAcc = accumulator :+ Token(toAdd._2,listDiff(input, toAdd._1).mkString)
        //   recur(toAdd._1, newAcc)
        // }
        
        static List<Token> Recur(List<char> input, List<Token> accumulator) {
          if (!input.Any())
          {
            return accumulator;
          }
          
          var toAdd = ScanOne(input, NDDFA.Start, (input, NDDFA.Start));
          
          if (toAdd.Item1 == input)
          {
            throw new Exception($"Scanning failed at: \n{new string(input.ToArray())}" );
          }
          
          accumulator.Add(new Token(toAdd.Item2,new string(ListDiff(input, toAdd.Item1).ToArray())));
          
          return Recur(toAdd.Item1, accumulator);
        }
        
        // return recur(input.toList)
        return Recur(input.ToCharArray().ToList(), new List<Token>());
      }
    }
}