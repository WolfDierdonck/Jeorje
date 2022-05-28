using System.Collections.Generic;

namespace Gentzen.Gentzen.Common
{
    public static class Logger
    {
        private static List<string> steps = new List<string>();
        private static List<string> warnings = new List<string>();
        private static List<string> errors = new List<string>();

        public static void AddStep(string step)
        {
            steps.Add(step);
        }
        
        public static void AddWarning(string warning)
        {
            warnings.Add(warning);
        }

        public static void AddError(string error)
        {
            errors.Add(error);
        }
        
        public static void RemoveError()
        {
            errors.RemoveAt(errors.Count-1);
        }

        public static string LogSteps() => 
            steps.Count != 0 ? "-------------\nSteps:\n" + string.Join("\n", steps) : null;

        public static string LogWarnings() =>
            warnings.Count != 0 ? "-------------\nWarnings:\n" + string.Join("\n", warnings) : null;

        public static string LogErrors() =>
            errors.Count != 0 ? "-------------\nErrors:\n" + string.Join("\n", errors) : null;

        public static string LogAll() => LogSteps() + "\n" + LogWarnings() + "\n" + LogErrors();

        public static void LogClear() {
            steps = new List<string>();
            warnings = new List<string>();
            errors = new List<string>();
        }
    }
}