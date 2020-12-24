using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2020
{
    class Day13
    {
        public static string A(string input)
        {
            List<string> inputList = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();
            int now = Convert.ToInt32(inputList[0]);
            List<int> BusIds = new List<int>();
            foreach (string id in inputList[1].Split(new char[] { ',' }))
            {
                if (!id.Equals("x"))
                {
                    BusIds.Add(Convert.ToInt32(id));
                }
            }

            int best = 0;
            int bestWait = int.MaxValue;
            foreach (int id in BusIds)
            {
                int cycles = now / id + 1;
                int wait = cycles * id - now;
                if (wait < bestWait)
                {
                    bestWait = wait;
                    best = id;
                }
            }
            return (best * bestWait).ToString();
        }

        public static string B(string input)
        {
            List<string> inputList = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();
            int now = Convert.ToInt32(inputList[0]);
            List<int> BusIds = new List<int>();
            List<string> busStringList = inputList[1].Split(new char[] { ',' }).ToList();
            int count = 0;
            List<Tuple<int, int>> busses = new List<Tuple<int, int>>();

            for (int i = 0; i < busStringList.Count; i++)
            {
                if (!busStringList[i].Equals("x"))
                {
                    busses.Add(Tuple.Create(Convert.ToInt32(busStringList[i]), i));
                    count++;
                }
            }
            int busNo = 0;
            long interval = 1;
            long answer = 0;
            while (busNo < busses.Count)
            {
                Tuple<int, int> testBus = busses[busNo];
                while (!TestBus(answer, testBus))
                { 
                    answer += interval;
                }
                interval = interval * testBus.Item1;
                busNo++;
            }
            return answer.ToString();
        }

        private static bool TestBus(long time, Tuple<int, int> bus)
        {
            return ((time + bus.Item2) % bus.Item1 == 0);
        }

    }
}
