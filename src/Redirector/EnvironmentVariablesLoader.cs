using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Redirector;

public class EnvironmentVariablesLoader
{
    public static Dictionary<string, string> LoadEnvironmentVariablesMatchingPattern()
    {
        var pattern = @"^.*:.*$"; // Pattern to match "{something}:{something}"
        var matchingVariables = new Dictionary<string, string>();

        foreach (System.Collections.DictionaryEntry envVar in Environment.GetEnvironmentVariables())
        {
            var key = (string)envVar.Key;
            var value = (string?)envVar.Value;

            if (!string.IsNullOrWhiteSpace(value) && Regex.IsMatch(value, pattern))
            {
                matchingVariables.Add(key, value);
            }
        }

        return matchingVariables;
    }

    static void Main(string[] args)
    {
        var matchingEnvVars = LoadEnvironmentVariablesMatchingPattern();

        foreach (var envVar in matchingEnvVars)
        {
            Console.WriteLine($"{envVar.Key} = {envVar.Value}");
        }
    }
}
