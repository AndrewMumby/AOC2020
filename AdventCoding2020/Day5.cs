using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2020
{
    class Day5
    {
        public static string A(string input)
        {
            List<string> inputList = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();
            int highestValue = 0;
            foreach (string inputLine in inputList)
            {
                IntVector2 coordinate = SeatCoordinate(inputLine);
                int value = coordinate.X + coordinate.Y * 8;
                highestValue = Math.Max(highestValue, value);
            }
            return highestValue.ToString();
        }

        public static string B(string input)
        {
            List<string> inputList = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();
            bool[] seats = new bool[1024];
            foreach (string inputLine in inputList)
            {
                IntVector2 coordinate = SeatCoordinate(inputLine);
                seats[coordinate.X + coordinate.Y * 8] = true;
            }
            for (int i = 1; i < 1023; i++)
            {
                if (!seats[i] && seats[i - 1] && seats[i + 1])
                {
                    return i.ToString();
                }
            }
            return "Error";
        }

        private static IntVector2 SeatCoordinate(string inputLine)
        {
            // FBFBBFFRLR
            string rowString = inputLine.Substring(0, 7);
            string columnString = inputLine.Substring(7);
            int row = 0;
            int column = 0;

            foreach (char c in rowString)
            {
                row *= 2;
                if (c == 'B')
                {
                    row++;
                }
            }

            foreach (char c in columnString)
            {
                column *= 2;
                if (c == 'R')
                {
                    column++;
                }
            }
            return new IntVector2(column, row);
        }
    }
}
