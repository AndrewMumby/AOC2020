using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2020
{
    class Day17
    {
        internal static HashSet<IntVector3> grid3;
        internal static HashSet<IntVector4> grid4;

        public static string A(string input)
        {
            /*
             .#.
             ..#
             ### 
             */
            grid3 = new HashSet<IntVector3>();
            string[] inputLines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            for (int y = 0; y < inputLines.Length; y++)
            {
                for (int x = 0; x < inputLines[0].Length; x++)
                {
                    if (inputLines[y][x] == '#')
                    {
                        grid3.Add(new IntVector3(x, y, 0));
                    }
                }
            }

            RunStep3(6);
            return grid3.Count.ToString();
        }

        public static string B(string input)
        {
            /*
             .#.
             ..#
             ### 
             */
            grid4 = new HashSet<IntVector4>();
            string[] inputLines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            for (int y = 0; y < inputLines.Length; y++)
            {
                for (int x = 0; x < inputLines[0].Length; x++)
                {
                    if (inputLines[y][x] == '#')
                    {
                        grid4.Add(new IntVector4(x, y, 0, 0));
                    }
                }
            }

            RunStep4(6);
            return grid4.Count.ToString();
        }

        internal static void RunStep3(int count)
        {
            for (int i = 0; i < count; i++)
            {
                RunStep3();
            }
        }
        internal static void RunStep3()
        {
            Dictionary<IntVector3, int> adjCount = new Dictionary<IntVector3, int>();
            foreach (IntVector3 location in grid3)
            {
                foreach (IntVector3 direction in IntVector3.CardinalDirectionsIncludingDiagonals)
                {
                    IntVector3 position = IntVector3.Add(location, direction);
                    if (!adjCount.ContainsKey(position))
                    {
                        adjCount.Add(position, 0);
                    }
                    adjCount[position]++;
                }
            }

            HashSet<IntVector3> newGrid = new HashSet<IntVector3>();

            foreach (KeyValuePair<IntVector3, int> pair in adjCount)
            {
                //If a cube is active and exactly 2 or 3 of its neighbors are also active, the cube remains active. Otherwise, the cube becomes inactive.
                if (grid3.Contains(pair.Key) && (pair.Value == 2 || pair.Value == 3))
                {
                    newGrid.Add(pair.Key);
                }
                //If a cube is inactive but exactly 3 of its neighbors are active, the cube becomes active. Otherwise, the cube remains inactive.
                if ((!grid3.Contains(pair.Key)) && pair.Value == 3)
                {
                    newGrid.Add(pair.Key);
                }
            }

            grid3 = newGrid;
        }


        internal static void RunStep4(int count)
        {
            for (int i = 0; i < count; i++)
            {
                RunStep4();
            }
        }

        internal static void RunStep4()
        {
            Dictionary<IntVector4, int> adjCount = new Dictionary<IntVector4, int>();
            foreach (IntVector4 location in grid4)
            {
                foreach (IntVector4 direction in IntVector4.CardinalDirectionsIncludingDiagonals)
                {
                    IntVector4 position = IntVector4.Add(location, direction);
                    if (!adjCount.ContainsKey(position))
                    {
                        adjCount.Add(position, 0);
                    }
                    adjCount[position]++;
                }
            }

            HashSet<IntVector4> newGrid = new HashSet<IntVector4>();

            foreach (KeyValuePair<IntVector4, int> pair in adjCount)
            {
                //If a cube is active and exactly 2 or 3 of its neighbors are also active, the cube remains active. Otherwise, the cube becomes inactive.
                if (grid4.Contains(pair.Key) && (pair.Value == 2 || pair.Value == 3))
                {
                    newGrid.Add(pair.Key);
                }
                //If a cube is inactive but exactly 3 of its neighbors are active, the cube becomes active. Otherwise, the cube remains inactive.
                if ((!grid4.Contains(pair.Key)) && pair.Value == 3)
                {
                    newGrid.Add(pair.Key);
                }
            }

            grid4 = newGrid;
        }
    }
}
