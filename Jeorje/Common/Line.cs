using System.Collections.Generic;
using System.Linq;

namespace Jeorje
{
    public class Line
    {
        public List<Token> Tokens;

        public List<TokenType> TokenTypes => Tokens.Select(e => e.TokenType).ToList();
        
        public Line(List<Token> tokens)
        {
            Tokens = tokens;
        }
    }
}