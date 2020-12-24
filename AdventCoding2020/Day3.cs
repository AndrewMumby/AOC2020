using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2020
{
    enum TTree
    {
        TREE = '#',
        OPEN = '.'
    }
    class Day3
    {

        public static string A(string input)
        {
            int maxX, maxY;
            Dictionary<IntVector2, TTree> map = MakeMap(input);
            IntVector2 last = map.Keys.Last();
            maxX = last.X;
            maxY = last.Y;
            IntVector2 velocityVector = new IntVector2(3, 1);
            return Slide(map, velocityVector, maxX, maxY).ToString();
        }

        public static string B(string input)
        {
            int maxX, maxY;
            Dictionary<IntVector2, TTree> map = MakeMap(input);
            IntVector2 last = map.Keys.Last();
            maxX = last.X;
            maxY = last.Y;
            List<IntVector2> velocityVectors = new List<IntVector2>();
            velocityVectors.Add(new IntVector2(1, 1));            //Right 1, down 1.
            velocityVectors.Add(new IntVector2(3, 1));            //Right 3, down 1. 
            velocityVectors.Add(new IntVector2(5, 1));            //Right 5, down 1.
            velocityVectors.Add(new IntVector2(7, 1));            //Right 7, down 1.
            velocityVectors.Add(new IntVector2(1, 2));            //Right 1, down 2.

            long total = 1;
            foreach (IntVector2 velocityVector in velocityVectors)
            {
                total = total * Slide(map, velocityVector, maxX, maxY);
            }
            return total.ToString();
        }


        private static Dictionary<IntVector2, TTree> MakeMap(string input)
        {
            List<string> inputList = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();

            Dictionary<IntVector2, TTree> map = new Dictionary<IntVector2, TTree>();
            int y = 0;
            foreach (string line in inputList)
            {
                int x = 0;
                foreach (char tile in line)
                {
                    IntVector2 location = new IntVector2(x, y);
                    map.Add(location, (TTree)tile);
                    x++;
                }
                y++;
            }
            return map;
        }

        private static int Slide(Dictionary<IntVector2, TTree> map, IntVector2 velocityVector, int maxX, int maxY)
        {
            IntVector2 location = new IntVector2(0, 0);
            int count = 0;
            while (location.Y <= maxY)
            {
                if (map[location] == TTree.TREE)
                {
                    count++;
                }
                location = IntVector2.Add(location, velocityVector);
                location.X = location.X % (maxX+1);
                if (location.X > maxX)
                {
                    location.X -= (maxX + 1);
                }
            }
            return count;
        }
    }
}
