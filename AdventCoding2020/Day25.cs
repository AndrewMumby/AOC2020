using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2020
{
    class Day25
    {
        public static string A(string input)
        {
            long[] inputLines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).Select(s => Convert.ToInt64(s)).ToArray();
            long cardPublicKey = inputLines[0];
            long doorPublicKey = inputLines[1];
            int cardLoops = TransformTillValue(7, cardPublicKey);
            int doorLoops = TransformTillValue(7, doorPublicKey);

            long answer = Transform(cardPublicKey, doorLoops);
            if (answer == Transform(doorPublicKey, cardLoops))
            {
                return answer.ToString();
            }
            return "";
        }

        public static string B(string input)
        {
            return "";
        }

        private static long Transform(long subjectNumber, int loops)
        {
            long value = 1;
            for (int i = 0; i < loops; i++)
            {
                value = TransformSingle(subjectNumber, value);
            }
            return value;
        }

        private static long TransformSingle(long subjectNumber, long value)
        {
            return (value * subjectNumber) % 20201227;
        }

        private static int TransformTillValue(long subjectNumber, long target)
        {
            int loops = 0;
            long value = 1;
            while ( value != target)
            {
                value = TransformSingle(subjectNumber, value);
                loops++;
                //Console.WriteLine(loops + " " + value);
            }
            return loops;
        }
    }
}