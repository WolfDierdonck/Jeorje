using System.Collections.Generic;
using System.Linq;

namespace Gentzen.Gentzen.Common
{
    public class Line
    {
        public List<Token> Tokens;

        public List<TokenType> TokenTypes => Tokens.Select(e => e.TokenType).ToList();
        
        public Line(List<Token> tokens)
        {
            Tokens = tokens;
        }

        public override string ToString()
        {
            return string.Join(" ", Tokens);
        }
    }
}