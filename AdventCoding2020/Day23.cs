using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2020
{
    class Day23
    {
        public static string A(string input)
        {
            List<char> cups = ParseCups(input);
            for (int i = 0; i < 100; i++)
            {
                cups = PlayRound(cups);
                // Console.WriteLine(GetLabels(cups));
                //Console.WriteLine(new string(cups.ToArray()));
            }
            return GetLabels(cups);

        }

        private static string GetLabels(List<char> cups)
        {
            List<char> answer = new List<char>();
            // find the 1
            int onePos = cups.IndexOf('1');
            int i = onePos + 1;
            while (i < cups.Count)
            {
                answer.Add(cups[i]);
                i++;
            }
            i = 0;
            while (i < onePos)
            {
                answer.Add(cups[i]);
                i++;
            }
            return new string(answer.ToArray());
        }

        private static List<char> ParseCups(string input)
        {
            List<char> cups = input.ToList();
            return cups;
        }

        private static List<char> PlayRound(List<char> cups)
        {
            // get current cup
            char currentCup = cups[0];
            //Console.WriteLine(currentCup);

            //get removed cups
            List<char> removedCups = cups.GetRange(1, 3);
            //Console.WriteLine(new string(removedCups.ToArray()));

            // select destination cup
            char destinationCup = (char)(currentCup - 1);
            while (destinationCup <= '0' || removedCups.Contains(destinationCup))
            {
                destinationCup--;
                if (destinationCup <= '0')
                {
                    destinationCup = '9';
                }
            }

            // new order : <pack> <destinationCup> <removedCups> <pack> <currentCup>
            int i = 4;
            List<char> newCups = new List<char>();
            while (i < 9)
            {
                if (cups[i] == destinationCup)
                {
                    newCups.Add(destinationCup);
                    newCups.AddRange(removedCups);
                }
                else
                {
                    newCups.Add(cups[i]);
                }
                i++;
            }
            newCups.Add(currentCup);
            return newCups;
        }
    }
}
