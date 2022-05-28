using System;
using System.Collections.Generic;

namespace Gentzen.Gentzen.ND
{
    public static class NDDFA
    {
        public static readonly string Start = "start";
        public static readonly HashSet<string> Accepting = new HashSet<string>()
        {
            "!",
            "&",
            "|",
            "=>",
            "<=>",
            "=",
            "!=",
            ".",
            ":",
            "{",
            "}",
            "(",
            ")",
            ",",
            "#",
            "|-",
            "<=",
            ">=",
            "<",
            ">",
            "ID",
            "mathOperator",
            "whiteSpace",
        };

        public static string Transition((string, char) t)
        {
            (string s, char c) = t;

            if (s == "start")
            {
                switch (c)
                {
                    case '!':
                        return "!";
                    
                    case '&':
                        return "&";
                    
                    case '|':
                        return "|";
                    
                    case '=':
                        return "=";
                    
                    case '.':
                        return ".";
                    
                    case ':':
                        return ":";
                    
                    case '<':
                        return "<";
                    
                    case '>':
                        return ">";
                    
                    case '{':
                        return "{";
                    
                    case '}':
                        return "}";
                    
                    case '(':
                        return "(";
                    
                    case ')':
                        return ")";
                    
                    case '+':
                        return "mathOperator";
                    
                    case '-':
                        return "mathOperator";
                    
                    case '*':
                        return "mathOperator";
                    
                    case ',':
                        return ",";
                    
                    case '#':
                        return "#";
                    
                    case '/':
                        return "/";
                    
                    case ' ':
                        return "whiteSpace";
                    
                    case '\n':
                        return "whiteSpace";
                    
                    case '\t':
                        return "whiteSpace";
                    
                    case '\r':
                        return "whiteSpace";
                    
                    default:
                        if (char.IsLetterOrDigit(c) || c == '_')
                        {
                            return "ID";
                        }

                        return string.Empty;
                }
            }
            else if (s == "=")
            {
                switch (c)
                {
                    case '>':
                        return "=>";
                }
            }
            else if (s == "<")
            {
                switch (c)
                {
                    case '=':
                        return "<=";
                }
            }
            else if (s == ">")
            {
                switch (c)
                {
                    case '=':
                        return "=";
                }
            }
            else if (s == "/")
            {
                switch (c)
                {
                    case '/':
                        return "//";
                }
            }
            else if (s == "!")
            {
                switch (c)
                {
                    case '=':
                        return "!=";
                }
            }
            else if (s == "|")
            {
                switch (c)
                {
                    case '-':
                        return "|-";
                }
            }
            else if (s == "<=")
            {
                switch (c)
                {
                    case '>':
                        return "<=>";
                }
            }
            else if (s == "ID")
            {
                if (char.IsLetterOrDigit(c) || c == '_')
                {
                    return "ID";
                }
            }
            else if (s == "//")
            {
                switch (c)
                {
                    case '\n':
                        return String.Empty;
                    
                    default:
                        return "//";
                }
            }

            return string.Empty;
        }
    }
}