using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Gentzen.Gentzen.Common;
using Gentzen.Gentzen.ND.NDRules;

namespace Gentzen.Gentzen.ND
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
            typeof(NDLBrace),
            typeof(NDRBrace),
            typeof(NDEnterImpI),
            typeof(NDEnterRaa),
            typeof(NDEnterForallI),
            typeof(NDEnterExistsE),
            typeof(NDCase),
            typeof(NDCases),
            typeof(NDImpI),
            typeof(NDIffE),
            typeof(NDIffMP),
            typeof(NDNotE),
            typeof(NDNotNotE),
            typeof(NDNotNotI),
            typeof(NDRaa),
            typeof(NDEqI),
            typeof(NDEqE),
            typeof(NDExistsE),
            typeof(NDExistsI),
            typeof(NDForallE),
            typeof(NDForallI)
        };
        
        public static List<NDRule> RulifyLines(List<Line> lines)
        {
            return lines.Select(RulifyLine).ToList();
        }

        public static NDRule RulifyLine(Line line)
        {
            Logger.AddError($"Latest: Rulifying line {line}");
            
            if (line.Tokens[0].TokenType == TokenType.LBrace)
            {
                Logger.RemoveError();
                return new NDLBrace("lbrace", null, null);
            }
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

            var endIndex = line.Tokens.FindIndex(token => token.Lexeme == "by" || token.Lexeme == "premise");
            var beginIndex = 2;
            var scopedRule = endIndex == -1;

            if (scopedRule)
            {
                beginIndex = 3;
                endIndex = line.Tokens.Count;
            }
            var predicate = Parser.ParseLine(new Line(line.Tokens.GetRange(beginIndex, endIndex - beginIndex)));
            if (scopedRule)
            {
                switch (line.Tokens[2].Lexeme)
                {
                    case "assume":
                        Logger.RemoveError();
                        return new NDEnterImpI(label, predicate, null);
                    case "disprove":
                        Logger.RemoveError();
                        return new NDEnterRaa(label, predicate, null);
                    case "case":
                        Logger.RemoveError();
                        return new NDCase(label, predicate, null);
                    case "for":
                        switch (line.Tokens[3].Lexeme)
                        {
                            case "every":
                                Logger.RemoveError();
                                return new NDEnterForallI(label, predicate, null);
                            case "some":
                                Logger.RemoveError();
                                return new NDEnterExistsE(label, predicate, null);
                            default:
                                throw new Exception($"Error on line with label {label}: invalid syntax");
                        }
                    default:
                        throw new Exception($"Error on line with label {label}: invalid syntax");
                };
            }
            if (line.Tokens[endIndex].Lexeme == "premise")
            {
                Logger.RemoveError();
                return new NDPremise(label, predicate, null);
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
                
                if (i < tokens.Count - 2 && tokens[i + 1].Lexeme == "-")
                {
                    if (tokens[i+2].TokenType != TokenType.Label)
                    {
                        throw new Exception($"Invalid label {tokens[i+2].Lexeme}");
                    }
                    requirements.Add(tokens[i].Lexeme + '-' + tokens[i+2].Lexeme);
                    i += 4;
                }
                else
                {
                    requirements.Add(tokens[i].Lexeme);
                    i += 2;
                }
            }

            return (tokens[1].Lexeme, requirements);
        }
    }
}