using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2020
{
    class Day1
    {
        private static List<int> ParseInput(string input)
        {
            string[] inputList = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            List<int> numberList = new List<int>();
            foreach (string inputItem in inputList)
            {
                numberList.Add(Int32.Parse(inputItem));
            }
            return numberList;
        }
        public static string A(string input)
        {
            List<int> numbers = ParseInput(input);
            for (int i = 0; i < numbers.Count; i++)
            {
                for (int j = i+1; j < numbers.Count; j++)
                {
                    if (numbers[i] + numbers[j] == 2020)
                    {
                        return (numbers[i] * numbers[j]).ToString();
                    }
                }
            }
            return "Error";
        }
        
        public static string B(string input)
        {
            List<int> numbers = ParseInput(input);
            for (int i = 0; i < numbers.Count; i++)
            {
                for (int j = i + 1; j < numbers.Count; j++)
                {
                    for (int k = j+1; k < numbers.Count; k++)
                    if (numbers[i] + numbers[j] + numbers[k] == 2020)
                    {
                        return (numbers[i] * numbers[j] * numbers[k]).ToString();
                    }
                }
            }
            return "Error";
        }

    }
}
