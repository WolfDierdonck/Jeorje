using System.Text;

namespace Jeorje
{
    public class Token
    {
        public TokenType TokenType;
        public string Lexeme;
        public bool IsOperator;
        
        public Token(string kind)
        {
            TokenType = ParseTokenType(kind);
            IsOperator = ParseIsOperator(TokenType); 
            Lexeme = kind;
        }

        public Token(string kind, string lexeme)
        {
            TokenType = ParseTokenType(kind, lexeme);
            Lexeme = lexeme;
            IsOperator = ParseIsOperator(TokenType);
        }

        public Token(TokenType tokenType, string lexeme)
        {
            TokenType = tokenType;
            Lexeme = lexeme;
            IsOperator = ParseIsOperator(TokenType);
        }
        
        private TokenType ParseTokenType(string kind, string lexeme = null)
        {
            switch (kind)
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
                case "|-":
                    return TokenType.Entails;
                case "//":
                    return TokenType.Comment;
                case "whiteSpace":
                    return TokenType.Whitespace;
                
                default:
                    if (lexeme != null) 
                    {
                        return ParseTokenType(lexeme); // this is so stupid :)
                    }

                    if (char.IsDigit(kind[0]))
                    {
                        if (int.TryParse(kind, out _))
                        {
                            return TokenType.Integer;
                        }
                        return TokenType.Label;
                    }

                    return TokenType.Identifier;
            }
        } 
        
        private bool ParseIsOperator(TokenType tokenType)
        {
            switch (tokenType)
            {
                case TokenType.Identifier:
                    return false;
                case TokenType.False:
                    return false;
                case TokenType.True:
                    return false;
                case TokenType.Forall:
                    return false;
                case TokenType.Exists:
                    return false;

                default:
                    return true;
            }
        } 
        
        public static bool operator ==(Token lhs, Token rhs)
        {
            if (lhs is null)
            {
                return rhs is null;
            }
            if (rhs is null)
            {
                return false;
            }
            return lhs.Lexeme == rhs.Lexeme && lhs.TokenType == rhs.TokenType;
        }
        public static bool operator !=(Token lhs, Token rhs) => !(lhs == rhs);            
        
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
        Comment, // lmao
        Whitespace, // \n or \r or \t or ' '
        Label, // must start with integer, then can be any alphanumeric character after
        Integer
    }
}