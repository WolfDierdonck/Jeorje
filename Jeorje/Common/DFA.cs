using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;

namespace Jeorje
{
    public static class DFA
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
            "mathOperator"
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
                if (char.IsLetterOrDigit(c))
                {
                    return "ID";
                }
            }

            return string.Empty;
        }
    }
}