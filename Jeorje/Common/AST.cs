using System;
using System.Collections.Generic;
using System.Linq;

namespace Jeorje
{
    public class AST: IEquatable<AST>
    {
        public Token Token;

        public List<AST> Children;
        
        public AST(Token token)
        {
            Token = token;
            Children = new List<AST>();
        }

        public AST(Token token, List<AST> children)
        {
            Token = token;
            Children = children;
        }

        public void AddChild(Token token)
        {
            Children.Add(new AST(token));
        }

        public bool Equals(AST other)
        {
            if (other is null)
            {
                return false;
            }

            if (Token != other.Token)
            {
                return false;
            }

            return Children.SequenceEqual(other.Children);
        }
    }
}