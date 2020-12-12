using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2020
{
    class Day12
    {
        public static string A(string input)
        {
            List<string> inputList = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();
            IntVector2 position = new IntVector2(0, 0);
            IntVector2 facing = new IntVector2(0, 0).East();
            foreach (string line in inputList)
            {
                char direction = line[0];
                int magnitude = Convert.ToInt32(line.Substring(1));
                switch (direction)
                {
                    case 'N':
                        position = position.North(magnitude);
                        break;
                    case 'E':
                        position = position.East(magnitude);
                        break;
                    case 'S':
                        position = position.South(magnitude);
                        break;
                    case 'W':
                        position = position.West(magnitude);
                        break;
                    case 'F':
                        position = position.Add(facing.Multiply(magnitude));
                        break;
                    case 'L':
                        while (magnitude > 0)
                        {
                            facing = facing.Left();
                            magnitude -= 90;
                        }
                        break;
                    case 'R':
                        while (magnitude > 0)
                        {
                            facing = facing.Right();
                            magnitude -= 90;
                        }
                        break;
                    default:
                        throw new Exception("Invalid direction: " + direction);

                }
                //Console.WriteLine("Position: " + position + ", Direction: " + facing);
            }
            return position.Distance(new IntVector2(0, 0)).ToString(); ;
        }

        public static string B(string input)
        {
            List<string> inputList = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();
            IntVector2 position = new IntVector2(0, 0);
            IntVector2 waypoint = new IntVector2(10, -1);
            foreach (string line in inputList)
            {
                char direction = line[0];
                int magnitude = Convert.ToInt32(line.Substring(1));
                switch (direction)
                {
                    case 'N':
                        waypoint = waypoint.North(magnitude);
                        break;
                    case 'E':
                        waypoint = waypoint.East(magnitude);
                        break;
                    case 'S':
                        waypoint = waypoint.South(magnitude);
                        break;
                    case 'W':
                        waypoint = waypoint.West(magnitude);
                        break;
                    case 'F':
                        position = position.Add(waypoint.Multiply(magnitude));
                        break;
                    case 'L':
                        while (magnitude > 0)
                        {
                            waypoint = waypoint.Left();
                            magnitude -= 90;
                        }
                        break;
                    case 'R':
                        while (magnitude > 0)
                        {
                            waypoint = waypoint.Right();
                            magnitude -= 90;
                        }
                        break;
                    default:
                        throw new Exception("Invalid direction: " + direction);

                }
                //Console.WriteLine("Position: " + position + ", Direction: " + facing);
            }
            return position.Distance(new IntVector2(0, 0)).ToString(); ;
        }
    }
}
