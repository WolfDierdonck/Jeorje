using System.Collections.Generic;

namespace Jeorje
{
    public class AST
    {
        public Token Token;

        public List<AST> Children;
        
        public AST(Token token)
        {
            Token = token;
            Children = null;
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
    }
}