using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Jeorje
{
    public static class NDRulifier
    {
        private static List<Type> _ruleTypes = new()
        {
            typeof(NDAndI),
            typeof(NDPremise),
            typeof(NDAndE),
            typeof(NDImpE),
            typeof(NDOrI),
            typeof(NDLem),
            typeof(NDOrE),
            typeof(NDTrue),
            typeof(NDMagic),
            typeof(NDArith),
            typeof(NDSet),
            typeof(NDTrans),
            typeof(NDIffI),
            typeof(NDRBrace),
            typeof(NDEnterImpI)
        };
        
        public static List<NDRule> RulifyLines(List<Line> lines)
        {
            return lines.Select(RulifyLine).ToList();
        }

        public static NDRule RulifyLine(Line line)
        {
            Logger.AddError($"Latest: Rulifying line {line}");
            
            if (line.Tokens[0].TokenType == TokenType.RBrace)
            {
                Logger.RemoveError();
                return new NDRBrace("rbrace", null, null);
            }
            if (line.Tokens[0].TokenType != TokenType.Label)
            {
                throw new Exception("Beginning of line was not a label");
            }
            
            var label = line.Tokens[0].Lexeme;

            var endIndex = line.Tokens.FindIndex(token => token.Lexeme == "by" || token.Lexeme == "premise" || token.TokenType == TokenType.LBrace);

            var beginIndex = line.Tokens[endIndex].TokenType == TokenType.LBrace ? 3 : 2;

            var predicate = Parser.ParseLine(new Line(line.Tokens.GetRange(beginIndex, endIndex - beginIndex)));

            if (line.Tokens[endIndex].Lexeme == "premise")
            {
                Logger.RemoveError();
                return new NDPremise(label, predicate, null);
            }
            if (line.Tokens[endIndex].TokenType == TokenType.LBrace)
            {
                switch (line.Tokens[2].Lexeme)
                {
                    case "assume":
                        Logger.RemoveError();
                        return new NDEnterImpI(label, predicate, null);
                    case "disprove":
                        Logger.RemoveError();
                        return null;
                    case "case":
                        Logger.RemoveError();
                        return null;
                    default:
                        throw new Exception($"Error on line with label {label}: invalid syntax");
                };
            }

            (string ruleName, List<string> requirements) = GetRequirements(line.Tokens.GetRange(endIndex, line.Tokens.Count - endIndex), label);
            
            //magic, dont try to understand
            foreach (var ruleType in _ruleTypes)
            {
                string currentRuleName = (string) 
                    ruleType.GetField("_name", BindingFlags.Public | BindingFlags.Static).GetValue(null);

                if (currentRuleName == ruleName)
                {
                    Logger.RemoveError();
                    return (NDRule) Activator.CreateInstance(ruleType, label, predicate, requirements);
                }
            }

            throw new Exception($"Error on line with label {label}: Rule name {ruleName} not found");
        }

        private static (string, List<string>) GetRequirements(List<Token> tokens, string label)
        {
            if (tokens.Count == 1)
            {
                throw new Exception($"Error on line with label {label}: You need to specify a rule");
            }
            
            if (tokens.Count == 2) // need at least 3 tokens to have 'by X on Y'
            {
                return (tokens[1].Lexeme, null);
            }

            if (tokens.Count == 3)
            {
                throw new Exception($"Error on line with label {label}: You forgot labels after 'on'");
            }

            if (tokens[2].Lexeme != "on")
            {
                throw new Exception($"Error on line with label {label}: Couldn't find 'on'");
            }
            
            var i = 3;
            var requirements = new List<string>();

            while (i < tokens.Count)
            {
                if (tokens[i].TokenType != TokenType.Label)
                {
                    throw new Exception($"Invalid label {tokens[i].Lexeme}");
                }

                if (i < tokens.Count - 1 && tokens[i + 1].Lexeme != "," && tokens[i + 1].Lexeme != "-")
                {
                    throw new Exception("Commas are required between line labels");
                }
                
                requirements.Add(tokens[i].Lexeme);
                i += 2;
            }

            return (tokens[1].Lexeme, requirements);
        }
    }
}