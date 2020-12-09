using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2020
{
    class Day9
    {
        public static string A(string input)
        {
            List<string> inputList = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();
            List<long> longList = new List<long>();
            foreach (string line in inputList)
            {
                longList.Add(Convert.ToInt64(line));
            }
            return FindInvalidNumber(longList).ToString();
        }

        private static long FindInvalidNumber(List<long> longList)
        {
            int preamble = 25;
            if (longList.Count == 20)
            {
                preamble = 5;
            }
            for (int i = preamble; i < longList.Count; i++)
            {
                bool found = false;
                for (int x = i - preamble; x < i - 1; x++)
                {
                    for (int y = x + 1; y < i; y++)
                    {
                        if (longList[x] != longList[y] && longList[x] + longList[y] == longList[i])
                        {
                            found = true;
                            break;
                        }
                    }
                    if (found)
                    {
                        break;
                    }
                }
                if (!found)
                {
                    return longList[i];
                }
            }
            throw new Exception("Invalid number not found");
        }

        public static string B(string input)
        {
            List<string> inputList = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();
            List<long> longList = new List<long>();
            foreach (string line in inputList)
            {
                longList.Add(Convert.ToInt64(line));
            }
            long invalidNumber = FindInvalidNumber(longList);

            for (int start = 0; start < longList.Count-1; start++)
            {
                for (int end = start; end <longList.Count-1; end++)
                {
                    long total = 0;
                    for (int i = start; i <= end; i++)
                    {
                        total = total + longList[i];
                    }
                    if (total == invalidNumber)
                    {
                        //We've found the answer
                        long min = long.MaxValue;
                        long max = long.MinValue;
                        for (int i = start; i <= end; i++)
                        {
                            min = Math.Min(min, longList[i]);
                            max = Math.Max(max, longList[i]);
                        }
                        return (min + max).ToString();
                    }
                    else if (total > invalidNumber)
                    {
                        break;
                    }
                }
            }
            return "Not found";

        }

    }
}
