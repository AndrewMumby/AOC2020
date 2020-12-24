using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2020
{

    class Day24
    {
        public static string A(string input)
        {
            HashSet<IntVector2> floor = ParseInput(input);
            return floor.Count.ToString();
        }

        public static string B(string input)
        {
            HashSet<IntVector2> floor = ParseInput(input);
            IntVector2[] hexagonalDirections = new IntVector2[6]
            {
                new IntVector2(1,0),
                new IntVector2(0,1),
                new IntVector2(-1, 1),
                new IntVector2(-1, 0),
                new IntVector2(0, -1),
                new IntVector2(1, -1)
            };
            for (int i = 0; i < 100; i++)
            {
                Dictionary<IntVector2, int> floorCounter = new Dictionary<IntVector2, int>();
                foreach (IntVector2 tile in floor)
                {
                    foreach (IntVector2 direction in hexagonalDirections)
                    {
                        IntVector2 newLocation = IntVector2.Add(tile, direction);
                        if (!floorCounter.ContainsKey(newLocation))
                        {
                            floorCounter.Add(newLocation, 0);
                        }
                        floorCounter[newLocation]++;
                    }
                }
                HashSet<IntVector2> newFloor = new HashSet<IntVector2>(floor);

                HashSet<IntVector2> tilesToDo = new HashSet<IntVector2>(floor.Union(floorCounter.Keys));
                foreach (IntVector2 location in tilesToDo)
                {
                    int count = 0;
                    floorCounter.TryGetValue(location, out count);
                    if (floor.Contains(location) && (count == 0 || count  > 2))
                    {
                        newFloor.Remove(location);
                    }
                    else if (!floor.Contains(location) && count == 2)
                    {
                        newFloor.Add(location);
                    }
                }
                floor = newFloor;
            }
            return floor.Count.ToString();
        }

        private static void DrawFloor (HashSet<IntVector2> floor)
        {
            int minX = floor.Min(v => v.X);
            int maxX = floor.Max(v => v.X);
            int minY = floor.Min(v => v.Y);
            int maxY = floor.Max(v => v.Y);
            for (int y = minY; y <= maxY; y++)
            {
                Console.Write(new string(' ', y - minY));
                for (int x = minX; x <= maxX; x++)
                {
                    if (x == 0 && y == 0)
                    {
                        if (floor.Contains(new IntVector2(x, y)))
                        {
                            Console.Write("0 ");
                        }
                        else
                        {
                            Console.Write("_ ");
                        }

                    }
                    else
                    {
                        if (floor.Contains(new IntVector2(x, y)))
                        {
                            Console.Write("X ");
                        }
                        else
                        {
                            Console.Write("  ");
                        }
                    }
                }
                Console.WriteLine();
            }
        }

        private static HashSet<IntVector2> ParseInput(string input)
        {
            HashSet<IntVector2> floor = new HashSet<IntVector2>();
            string[] inputLines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (string line in inputLines)
            {
                int i = 0;
                IntVector2 location = new IntVector2();
                while (i < line.Length)
                {
                    string directionString;
                    if (line[i] == 'n' || line[i] == 's')
                    {
                        directionString = line.Substring(i, 2);
                        i++;
                    }
                    else
                    {
                        directionString = line[i].ToString();
                    }
                    i++;

                    // 0 1 2 3 4 5 6 7 8 9
                    //  0 1 2 3 4 5 6 7 8 9 
                    //   0 1 2 3 4 5 6 7 8 9
                    //    0 1 2 3 4 5 6 7 8 9

                    switch (directionString)
                    {
                        case "e":
                            location.X++;
                            break;
                        case "se":
                            location.Y++;
                            break;
                        case "sw":
                            location.Y++;
                            location.X--;
                            break;
                        case "w":
                            location.X--;
                            break;
                        case "nw":
                            location.Y--;
                            break;
                        case "ne":
                            location.Y--;
                            location.X++;
                            break;
                        default:
                            throw new Exception("Unknown direction");
                    }

                }
                if (floor.Contains(location))
                {
                    floor.Remove(location);
                }
                else
                {
                    floor.Add(location);
                }
            }
            return floor;
        }
    }
}
