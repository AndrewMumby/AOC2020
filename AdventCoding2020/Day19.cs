using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2020
{
    class Day19
    {
        /*
        0: 4 1 5
        1: 2 3 | 3 2
        2: 4 4 | 5 5
        3: 4 5 | 5 4
        4: "a"
        5: "b"

        ababbb
        bababa
        abbbab
        aaabbb
        aaaabbb
        */
        static HashSet<string> validStrings;
        static Dictionary<int, HashSet<string>> solvedRules;
        static Dictionary<int, List<List<int>>> unsolvedRules;

        public static string A(string input)
        {
            string[] inputParts = input.Split(new string[] { Environment.NewLine + Environment.NewLine }, StringSplitOptions.None);

            // 0th part is the rules
            ParseInput(inputParts[0]);
            while (unsolvedRules.Count > 0)
            {
                ChewInput();
            }
            validStrings = new HashSet<string>(solvedRules[0]);

            // 1st part is the data
            string[] inputLines = inputParts[1].Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            int correct = 0;
            foreach (string line in inputLines)
            {
                if (ValidateString(line))
                {
                    correct++;
                }
            }
            return correct.ToString();
        }

        public static string B(string input)
        {
            string[] inputParts = input.Split(new string[] { Environment.NewLine + Environment.NewLine }, StringSplitOptions.None);

            // 0th part is the rules
            ParseInput(inputParts[0]);

            // Remove 8 and 11;
            unsolvedRules.Remove(8);
            unsolvedRules.Remove(11);

            // Process until only rule 0 remains
            while (unsolvedRules.Count > 1)
            {
                ChewInput();
            }

            // 1st part is the data
            string[] inputLines = inputParts[1].Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            // what's the longest string?
            int longest = inputLines.Max(l => l.Length);

            // Make loopy versions of 8 and 11 so that they're at least as long as longest

            // Make loopy versions of the middle till it's longer than longest

            // Gradually increase the size of first and last, pruning ones that don't match and ones that are too long till there's no new options

            HashSet<string> eight = new HashSet<string>(solvedRules[42]);
            HashSet<string> newSet = new HashSet<string>();

            do
            {
                eight = new HashSet<string>(eight.Union(newSet));
                newSet = new HashSet<string>();
                foreach (string first in eight)
                {
                    foreach (string second in solvedRules[42])
                    {
                        string newString = first + second;
                        if (newString.Length <= longest)
                        {
                            newSet.Add(first + second);
                        }
                    }
                }
            } while (newSet.Count > 0);


            int correct = 0;
            foreach (string line in inputLines)
            {
                if (validStrings.Contains(line))
                {

                }
            }
            return correct.ToString();
        }

        private static void GenerateValidStrings(string input)
        {
            ParseInput(input);
            while (unsolvedRules.Count > 0)
            {
                ChewInput();
            }
            validStrings = new HashSet<string>(solvedRules[0]);
        }

        private static void ChewInput()
        {
            HashSet<int> toRemove = new HashSet<int>();
            foreach (KeyValuePair<int, List<List<int>>> pair in unsolvedRules)
            {
                bool ready = true;
                foreach (List<int> list in pair.Value)
                {
                    foreach (int rule in list)
                    {
                        if (!solvedRules.ContainsKey(rule))
                        {
                            ready = false;
                        }
                    }
                }

                if (ready)
                {
                    HashSet<string> strings = new HashSet<string>();
                    foreach (List<int> list in pair.Value)
                    {
                        strings.UnionWith(RuleCombine(list));
                    }
                    solvedRules.Add(pair.Key, strings);
                    toRemove.Add(pair.Key);
                }
            }
            foreach (int item in toRemove)
            {
                unsolvedRules.Remove(item);
            }
        }

        private static void ParseInput(string input)
        {
            solvedRules = new Dictionary<int, HashSet<string>>();
            unsolvedRules = new Dictionary<int, List<List<int>>>();

            string[] inputLines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            foreach (string line in inputLines)
            {
                string[] lineParts = line.Split(new string[] { ": " }, StringSplitOptions.None);
                if (lineParts[1].StartsWith("\""))
                {
                    solvedRules.Add(Convert.ToInt32(lineParts[0]), new HashSet<string>(new string[] { lineParts[1][1].ToString() }));
                }
                else
                {
                    string[] rulelets = lineParts[1].Split(new string[] { " | " }, StringSplitOptions.None);
                    List<List<int>> bits = new List<List<int>>();
                    foreach (string rulelet in rulelets)
                    {
                        bits.Add(rulelet.Trim().Split(new char[] { ' ' }).Select(c => Convert.ToInt32(c)).ToList());
                    }
                    unsolvedRules.Add(Convert.ToInt32(lineParts[0]), bits);
                }
            }
        }

        private static HashSet<string> RuleCombine(List<int> rules)
        {
            List<int> newRules = rules.ToList();
            newRules.RemoveAt(0);
            return RuleCombineSub(rules.First(), newRules);
        }

        private static HashSet<string> RuleCombineSub(int rule, List<int> otherRules)
        {
            if (otherRules.Count == 0)
            {
                return solvedRules[rule];
            }
            else
            {
                HashSet<string> result = new HashSet<string>();
                List<int> newRules = otherRules.ToList();
                newRules.RemoveAt(0);
                HashSet<string> subRules = RuleCombineSub(otherRules.First(), newRules);
                foreach (string first in solvedRules[rule])
                {
                    foreach (string second in subRules)
                    {
                        result.Add(first + second);
                    }
                }
                return result;
            }
        }

        private static bool ValidateString(string line)
        {
            return validStrings.Contains(line);
        }
    }
}
