using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2020
{
    class Day10
    {
        public static string A(string input)
        {
            List<string> inputList = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();
            List<int> intList = new List<int>();
            foreach (string line in inputList)
            {
                intList.Add(Convert.ToInt32(line));
            }
            intList.Sort();


            Dictionary<int, int> counts = new Dictionary<int, int>();
            int current = 0;
            for (int i = 0; i < intList.Count; i++)
            {
                int difference = intList[i] - current;
                if (!counts.ContainsKey(difference))
                {
                    counts.Add(difference, 0);
                }
                counts[difference]++;
                current = intList[i];

            }
            counts[3]++;


            return (counts[3] * counts[1]).ToString();
        }

        public static string B(string input)
        {
            List<string> inputList = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();
            List<int> intList = new List<int>();
            foreach (string line in inputList)
            {
                intList.Add(Convert.ToInt32(line));
            }
            intList.Sort();

            return PathsFromValue(0, intList, new Dictionary<int, long>()).ToString(); ;

        }

        internal static long PathsFromValue(int currentJolt, List<int> intList, Dictionary<int, long> answers)
        {
            List<int> dupList = new List<int>(intList);
            long total = 0;
            while (currentJolt + 3 >= dupList.First())
            {
                int adaptorValue = dupList.First();
                dupList.RemoveAt(0);
                if (dupList.Count == 0)
                {
                    total++;
                    break;
                }
                else
                {
                    if (!answers.ContainsKey(adaptorValue))
                    {
                        long paths = PathsFromValue(adaptorValue, dupList, answers);
                        answers.Add(adaptorValue, paths);
                    }
                    total = total + answers[adaptorValue];
                }
            }
            return total;
        }
    }
}