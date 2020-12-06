using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2020
{
    class Day6
    {
        public static string A(string input)
        {
            List<string> inputList = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();
            List<string> groupList = new List<string>();
            int total = 0;
            foreach (string inputLine in inputList)
            {
                if (inputLine.Length == 0)
                {
                    total += AnyYes(TestCount(groupList));
                    groupList = new List<string>();
                }
                else
                {
                    groupList.Add(inputLine);
                }
            }
            total += AnyYes(TestCount(groupList));
            return total.ToString();
        }

        public static string B(string input)
        {
            List<string> inputList = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();
            List<string> groupList = new List<string>();
            int total = 0;
            foreach (string inputLine in inputList)
            {
                if (inputLine.Length == 0)
                {
                    total += AllYes(TestCount(groupList));
                    groupList = new List<string>();
                }
                else
                {
                    groupList.Add(inputLine);
                }
            }
            total += AllYes(TestCount(groupList));
            return total.ToString();
        }

        private static Dictionary<char, int> TestCount(List<string> groupList)
        {
            Dictionary<char, int> scores = new Dictionary<char, int>();
            scores.Add('0', 0);
            foreach (string person in groupList)
            {
                foreach (char c in person)
                {
                    if (!scores.ContainsKey(c))
                    {
                        scores.Add(c, 0);
                    }
                    scores[c]++;
                }
                scores['0']++;
            }
            return scores;
        }

        private static int AnyYes (Dictionary<char, int> scores)
        {
            return scores.Count-1;
        }

        private static int AllYes(Dictionary<char,int> scores)
        {
            int count = 0;
            foreach (char c in scores.Keys)
            {
                if (c != '0')
                {
                    if (scores[c] == scores['0'])
                    {
                        count++;
                    }
                }
            }
            return count;
        }
    }
}
