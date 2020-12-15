using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2020
{
    class Day15
    {
        public static string A(string input)
        {
            return ElfMemoryGame(input, 2020);
        }

        public static string B(string input)
        {
            return ElfMemoryGame(input, 30000000);
        }

        private static string ElfMemoryGame(string input, long end)
        {
            List<long> inputList = input.Split(new char[] { ',' }).ToList().Select((numberString) => Convert.ToInt64(numberString)).ToList();
            Dictionary<long, long> lastSaidTime = new Dictionary<long, long>();
            long now = 1;
            long numberToSay = inputList[0];
            long numberLastSaid = 0;
            while (now < inputList.Count)
            {
                numberLastSaid = numberToSay;
                numberToSay = inputList[(int)now];
                lastSaidTime[numberLastSaid] = now - 1;
                now++;
                //Console.WriteLine(now + " " + numberToSay);
            }

            do
            {
                numberLastSaid = numberToSay;
                // work out the number to say
                if (lastSaidTime.ContainsKey(numberToSay))
                {
                    numberToSay = now - 1 - lastSaidTime[numberLastSaid];
                }
                else
                {
                    numberToSay = 0;
                }
                lastSaidTime[numberLastSaid] = now - 1;

                // Add the last number said to the list
                now++;
                //Console.WriteLine(now + " " + numberToSay);

            } while (now < end);
            return numberToSay.ToString();
        }
    }
}
