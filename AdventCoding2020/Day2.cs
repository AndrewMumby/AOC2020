using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2020
{
    class Day2
    {
        public static string A(string input)
        {
            /*
                1-3 a: abcde
                1-3 b: cdefg
                2-9 c: ccccccccc
            */
            List<string> inputList = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();
            int answer = 0;
            foreach (string inputLine in inputList)
            {
                List<string> lineParts = inputLine.Split(new char[] { ' ' }).ToList();
                // First part is the quantity
                List<string> quantParts = lineParts[0].Split(new char[] { '-' }).ToList();
                int min = Convert.ToInt32(quantParts[0]);
                int max = Convert.ToInt32(quantParts[1]);

                // Second part is the character to look for
                char searchChar = lineParts[1][0];

                // Third part is the password
                string password = lineParts[2];

                // Test the password
                int count = 0;
                foreach (char passwordChar in password)
                {
                    if (passwordChar == searchChar)
                    {
                        count++;
                    }
                }
                if (min <= count && count <= max)
                {
                    answer++;
                }

            }
            return answer.ToString();
        }

        public static string B(string input)
        {

            /*
                1-3 a: abcde
                1-3 b: cdefg
                2-9 c: ccccccccc
            */
            List<string> inputList = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();
            int answer = 0;
            foreach (string inputLine in inputList)
            {
                List<string> lineParts = inputLine.Split(new char[] { ' ' }).ToList();

                // First part is the positions
                List<string> quantParts = lineParts[0].Split(new char[] { '-' }).ToList();
                int first = Convert.ToInt32(quantParts[0]);
                int second = Convert.ToInt32(quantParts[1]);

                // Second part is the character to look for
                char searchChar = lineParts[1][0];

                // Third part is the password
                string password = lineParts[2];

                // Test the password
                int count = 0;
                foreach (char passwordChar in password)
                {
                    if (passwordChar == searchChar)
                    {
                        count++;
                    }
                }
                if (password[first-1] == searchChar ^ password[second-1] == searchChar)
                {
                    answer++;
                }

            }
            return answer.ToString();
        }



    }
}
