using System;
using System.Collections.Generic;
using System.Linq;

namespace Gentzen.Gentzen.Common
{
    public class AST: IEquatable<AST>, ICloneable
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
        public override bool Equals(object obj) => this.Equals(obj as AST);

        public override int GetHashCode()
        {
            return (int) Token.TokenType;
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
        
        public static bool operator ==(AST lhs, AST rhs)
        {
            if (lhs is null && rhs is null)
            {
                return true;
            }

            if (lhs is null || rhs is null)
            {
                return false;
            }
            
            // Equals handles case of null on right side.
            return lhs.Equals(rhs);
        }

        public static bool operator !=(AST lhs, AST rhs) => !(lhs == rhs);
        
        public object Clone()
        {
            return new AST(Token, Children.Select(child => (AST) child.Clone()).ToList());
        }
        
    }
}