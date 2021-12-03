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
            IsOperator = ParseIsOperator(TokenType); 
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
                case "&":
                    return TokenType.And;
                case "|":
                    return TokenType.Or;
                case "=>":
                    return TokenType.Implies;
                case "<=>":
                    return TokenType.Iff;
                case "false":
                    return TokenType.False;
                case "true":
                    return TokenType.True;
                case "forall":
                    return TokenType.Forall;
                case "exists":
                    return TokenType.Exists;
                case "=":
                    return TokenType.Equal;
                case "!=":
                    return TokenType.NotEqual;
                case ".":
                    return TokenType.Dot;
                case ":":
                    return TokenType.Colon;
                case "{":
                    return TokenType.LBrace;
                case "}":
                    return TokenType.RBrace;
                case "(":
                    return TokenType.LParen;
                case ")":
                    return TokenType.RParen;
                case "+":
                    return TokenType.MathOperator;
                case "-":
                    return TokenType.MathOperator;
                case "*":
                    return TokenType.MathOperator;
                case "/":
                    return TokenType.MathOperator;
                case ">":
                    return TokenType.MathOperator;
                case "<":
                    return TokenType.MathOperator;
                case ">=":
                    return TokenType.MathOperator;
                case "<=":
                    return TokenType.MathOperator;
                case ",":
                    return TokenType.Comma;
                case "#":
                    return TokenType.Hashtag;
                
                default:
                    return TokenType.Identifier;
            }
        } 
        
        private bool ParseIsOperator(TokenType tokenType)
        {
            switch (tokenType)
            {
                case TokenType.Identifier:
                    return false;
                default:
                    return true;
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
        Hashtag, // #
        Entails, // |-
        DummyNotOperand, // $
        LessEquals, // <=
        GreaterEquals, // >=
        LessThan, // < 
        GreaterThan, // > 
        Comment // lmao
    }
}