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
            int correct = 0;
            int chunkSize = solvedRules[42].First().Length;
            foreach (string line in inputLines)
            {
                // break it into chunks of chunkSize
                string workingLine = line;
                List<string> chunks = new List<string>();
                while (workingLine.Length >0)
                {
                    chunks.Add(workingLine.Substring(0, chunkSize));
                    workingLine = workingLine.Substring(chunkSize);
                }

                // Pattern is some 42s followed by some 31s
                // There needs to be at least one more 42 than 31s
                int count42 = 0;
                int count31 = 0;
                int i = 0;
                while (i < chunks.Count && solvedRules[42].Contains(chunks[i]))
                {
                    i++;
                    count42++;
                    Console.Write("1");
                }
                while (i < chunks.Count && solvedRules[31].Contains(chunks[i]))
                {
                    i++;
                    count31++;
                    Console.Write("2");
                }

                Console.Write(" ");
                if ((count42 > count31) && count31>0 && i == chunks.Count)
                {
                    Console.Write("PASS");
                    correct++;
                }
                else
                {
                    Console.Write("FAIL");
                }
                Console.WriteLine(": " + line.Length + " " + line);
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
