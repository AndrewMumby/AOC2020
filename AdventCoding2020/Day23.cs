using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2020
{
    class Day23
    {
        static int maxCup;
        public static string A(string input)
        {
            List<int> cups = ParseCups(input);
            maxCup = cups.Max();
            for (int i = 0; i < 100; i++)
            {
                cups = PlayRound(cups);
            }
            return GetLabels(cups).ToString();

        }

        public static string B(string input)
        {
            List<int> cups = ParseCups(input);
            for (int i = cups.Max()+1; i <= 1000000; i++)
            {
                cups.Add(i);
            }
            maxCup = cups.Max();
            for (int i = 0; i < 10000000; i++)
            {
                cups = PlayRound(cups);
                if (i %10 == 0)
                {
                    Console.WriteLine(i);
                }
            }

            int pos1 = cups.IndexOf(1);
            return (cups[pos1 + 1] * cups[pos1 + 2]).ToString();
        }

        private static long GetLabels(List<int> cups)
        {
            long answer = 0;
            // find the 1
            int onePos = cups.IndexOf(1);
            int i = onePos + 1;
            while (i < cups.Count)
            {
                answer *= 10;
                answer += cups[i];
                i++;
            }
            i = 0;
            while (i < onePos)
            {
                answer *= 10;
                answer += cups[i];
                i++;
            }
            return answer;
        }

        private static List<int> ParseCups(string input)
        {
            return input.Select(c => c - '0').ToList();
        }

        private static List<int> PlayRound(List<int> cups)
        {
            // get current cup
            int currentCup = cups[0];

            //get removed cups
            List<int> removedCups = cups.GetRange(1, 3);

            // select destination cup
            int destinationCup = (currentCup - 1);
            while (destinationCup <= 0 || removedCups.Contains(destinationCup))
            {
                destinationCup--;
                if (destinationCup <= 0)
                {
                    destinationCup = maxCup;
                }
            }

            // new order : <4 - destinationCupPos-1> <destinationCup> <removedCups> <destinationCupPos+1 - end> <currentCup>
            int destinationCupPos = cups.IndexOf(destinationCup);
            List<int> newCups = new List<int>(10000000);
            newCups.AddRange(cups.GetRange(4, destinationCupPos - 4));
            newCups.Add(destinationCup);
            newCups.AddRange(removedCups);
            newCups.AddRange(cups.GetRange(destinationCupPos + 1, cups.Count - (destinationCupPos + 1)));
            newCups.Add(currentCup);
            return newCups;
        }
    }
}
