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
            string output = "";
            for (int i = 0; i < busStringList.Count; i++)
            {
                if (!busStringList[i].Equals("x"))
                {
                    //Console.WriteLine("Bus ID " + busStringList[i] + " needs to arrive " + i + " minutes after t");
                    output = output  + "t + " + i + " mod " + busStringList[i] + " = 0, ";
                        count++;
                }
            }
            return output.Substring(0, output.Length - 2);
        }

    }
}
