namespace Jeorje
{
    public class Token
    {
        public TokenType TokenType;
        public string Lexeme;
        public bool IsOperator;
        
        public Token(string token)
        {
            TokenType = ParseTokenType(token);
            IsOperator = ParseIsOperator(token); 
            Lexeme = token;
        }

        public Token(TokenType tokenType, string lexeme)
        {
            TokenType = tokenType;
            Lexeme = lexeme;
            IsOperator = false;
        }
        
        private TokenType ParseTokenType(string token)
        {
            switch (token)
            {
                case "!":
                    return TokenType.Not;
                    break;
                
                default:
                    return TokenType.Identifier;
            }
        } 
        
        private bool ParseIsOperator(string token)
        {
            switch (token)
            {
                case "!":
                    return true;
                    break;
                
                default:
                    return false;
            }
        } 
        
    }

    public enum TokenType
    {
        Not, // !
        And, // &
        Or, // |
        Implies, // =>
        Iff, // <=>
        False, // false
        True, // true
        Forall, // forall
        Exists, // exists
        Equal, // =
        NotEqual, // !=
        Dot, // .
        Colon, // :
        LBrace, // {
        RBrace, // }
        LParen, // (
        RParen, // )
        Identifier, // ???
        MathOperator, // +, - , *
        Comma, // ,
        FuncSeparator, // "@"
    }
}