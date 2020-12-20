using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2020
{
    enum Direction : int
    {
        North = 0,
        West = 1,
        East = 2,
        South = 3
    }

    class Day20
    {
        static List<SatSector> freeSectors;
        static Dictionary<IntVector2, SatSector> lockedSectors;
        static HashSet<IntVector2> seaMonster = new HashSet<IntVector2>();

        public static string A(string input)
        {
            ProcessInput(input);
            LockSectors();
            DrawSectors();
            for (int y = -5; y <= 5; y++)
            {
                for (int x = -5; x <= 5; x++)
                {
                    if (lockedSectors.ContainsKey(new IntVector2(x, y)))
                    {
                        Console.Write("X");
                    }
                    else
                    {
                        Console.Write(".");
                    }
                }
                Console.WriteLine();
            }

            // find the corners
            // first, find the north west corner
            long answer = 1;
            IntVector2 currentPos = new IntVector2(0, 0);
            while (lockedSectors.ContainsKey(currentPos))
            {
                currentPos = currentPos.North();
            }
            currentPos = currentPos.South();
            while (lockedSectors.ContainsKey(currentPos))
            {
                currentPos = currentPos.West();
            }
            currentPos = currentPos.East();
            answer = answer * lockedSectors[currentPos].Id;

            // move to the north east corner
            while (lockedSectors.ContainsKey(currentPos))
            {
                currentPos = currentPos.East();
            }
            currentPos = currentPos.West();
            answer = answer * lockedSectors[currentPos].Id;

            // move to the south east corner
            while (lockedSectors.ContainsKey(currentPos))
            {
                currentPos = currentPos.South();
            }
            currentPos = currentPos.North();
            answer = answer * lockedSectors[currentPos].Id;

            // move to the south west corner
            while (lockedSectors.ContainsKey(currentPos))
            {
                currentPos = currentPos.West();
            }
            currentPos = currentPos.East();
            answer = answer * lockedSectors[currentPos].Id;

            return answer.ToString();

        }

        public static string B(string input)
        {
            seaMonster.Add(new IntVector2(18, 0));
            seaMonster.Add(new IntVector2(0, 1));
            seaMonster.Add(new IntVector2(5, 1));
            seaMonster.Add(new IntVector2(6, 1));
            seaMonster.Add(new IntVector2(11, 1));
            seaMonster.Add(new IntVector2(12, 1));
            seaMonster.Add(new IntVector2(17, 1));
            seaMonster.Add(new IntVector2(18, 1));
            seaMonster.Add(new IntVector2(19, 1));
            seaMonster.Add(new IntVector2(1, 2));
            seaMonster.Add(new IntVector2(4, 2));
            seaMonster.Add(new IntVector2(7, 2));
            seaMonster.Add(new IntVector2(10, 2));
            seaMonster.Add(new IntVector2(13, 2));
            seaMonster.Add(new IntVector2(16, 2));

            ProcessInput(input);
            LockSectors();

            // find the corners
            // first, find the north west corner
            IntVector2 currentPos = new IntVector2(0, 0);
            while (lockedSectors.ContainsKey(currentPos))
            {
                currentPos = currentPos.North();
            }
            currentPos = currentPos.South();
            while (lockedSectors.ContainsKey(currentPos))
            {
                currentPos = currentPos.West();
            }
            currentPos = currentPos.East();

            int minX = currentPos.X;
            int minY = currentPos.Y;
            // Find the south east corner;
            while (lockedSectors.ContainsKey(currentPos))
            {
                currentPos = currentPos.South();
            }
            currentPos = currentPos.North();
            while (lockedSectors.ContainsKey(currentPos))
            {
                currentPos = currentPos.East();
            }
            currentPos = currentPos.West();
            int maxX = currentPos.X;
            int maxY = currentPos.Y;

            // Make the map
            char[,] map = new char[(maxX - minX + 1) * 8, (maxY - minY + 1) * 8];
            for (int outerX = minX; outerX <= maxX; outerX++)
            {
                for (int outerY = minY; outerY <= maxY; outerY++)
                {
                    char[,] contents = lockedSectors[new IntVector2(outerX, outerY)].Contents;
                    for (int x = 0; x < 8; x++)
                    {
                        for (int y = 0; y < 8; y++)
                        {
                            map[(outerX - minX) * 8 + x, (outerY - minY) * 8 + y] = contents[x + 1, y + 1];
                        }
                    }
                }
            }

            // Map made. Now we need to find sea monsters
            int size = map.GetLength(0);
            int seaMonsterCount = 0;
            for (int i = 0; i < 8; i++)
            {
                seaMonsterCount = CountSeaMonsters(map);
                if (seaMonsterCount> 0)
                {
                    break;
                }
                // (x,y) -> (height-y, x)
                char[,] newMap = new char[size, size];
                for (int x = 0; x < size; x++)
                {
                    for (int y = 0; y < size; y++)
                    {
                        newMap[x, y] = map[size - y - 1, x];
                    }
                }
                map = newMap;
                if (i == 4)
                {
                    newMap = new char[size, size];
                    for (int x = 0; x < size; x++)
                    {
                        for (int y = 0; y < size; y++)
                        {
                            newMap[x, y] = map[y, x];
                        }
                    }
                    map = newMap;
                }
            }

            // Sea monsters are 15 squares

            // count the number of waves
            int waveCount = 0;
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    if (map[x,y] == '#')
                    {
                        waveCount++;
                    }
                }
            }

            return (waveCount - 15 * seaMonsterCount).ToString();
        }

        private static int CountSeaMonsters(char[,] map)
        {
            int count = 0;
            for (int x = 0; x < map.GetLength(0); x++)
            {
                for (int y = 0; y < map.GetLength(1); y++)
                {
                    if (IsSeaMonster(map, x, y))
                    {
                        count++;                    }
                }
            }
            return count;
        }

        private static bool IsSeaMonster(char[,] map, int x, int y)
        {
            // Need to check there's space for a sea monster first
            if (map.GetLength(0) < x + 20 || map.GetLength(1) < y + 3)
            {
                return false;
            }
            foreach (IntVector2 monsterBit in seaMonster)
            {
                if (map[x+monsterBit.X, y+monsterBit.Y] != '#')
                {
                    return false;
                }
            }
            return true;
        }


        private static void LockSectors()
        {
            // Arbitrarily set the first freeSector to the centre
            lockedSectors = new Dictionary<IntVector2, SatSector>();
            SatSector first = freeSectors.First();
            IntVector2 origin = new IntVector2(0, 0);
            lockedSectors.Add(origin, first);
            freeSectors.Remove(first);
            Queue<IntVector2> toDoQueue = new Queue<IntVector2>();
            foreach (IntVector2 direction in IntVector2.CardinalDirections)
            {
                toDoQueue.Enqueue(origin.Add(direction));
            }

            while (toDoQueue.Count > 0)
            {
                IntVector2 toDo = toDoQueue.Dequeue();
                // Find a tile it's next to
                string edgeToMatch;
                IntVector2 matchingFrom = null;
                Direction matchingFromDirection = Direction.South;
                Direction toDoDirection = Direction.North;
                for (int i = 0; i < 4; i++)
                {
                    IntVector2 linkedLocation = toDo.Add(IntVector2.CardinalDirections[i]);
                    if (lockedSectors.ContainsKey(linkedLocation))
                    {
                        matchingFrom = linkedLocation;
                        toDoDirection = (Direction)i;
                        switch (toDoDirection)
                        {
                            case Direction.North:
                                matchingFromDirection = Direction.South;
                                break;
                            case Direction.East:
                                matchingFromDirection = Direction.West;
                                break;
                            case Direction.South:
                                matchingFromDirection = Direction.North;
                                break;
                            case Direction.West:
                                matchingFromDirection = Direction.East;
                                break;
                        }
                    }
                }
                edgeToMatch = lockedSectors[matchingFrom].GetEdge(matchingFromDirection);
                SatSector matchingSector = null;
                foreach (SatSector sector in freeSectors)
                {
                    if (sector.HasEdge(edgeToMatch))
                    {
                        matchingSector = sector;
                        break;
                    }
                }
                // No match. Break out to the next queued location
                if (matchingSector != null)
                {
                    // We have a match. Rotate the matchingSector till it lines up with the edgeToMatch
                    matchingSector.RotateTillMatch(edgeToMatch, toDoDirection);

                    // Now fix it in place, and add the surrounding locations to the queue
                    lockedSectors.Add(toDo, matchingSector);
                    freeSectors.Remove(matchingSector);
                    foreach (IntVector2 direction in IntVector2.CardinalDirections)
                    {
                        IntVector2 newLocation = toDo.Add(direction);
                        if (!lockedSectors.ContainsKey(newLocation) && !toDoQueue.Contains(newLocation))
                        {
                            toDoQueue.Enqueue(newLocation);
                        }
                    }
                }
            }
        }

        internal static void DrawSectors()
        {
            int minX = lockedSectors.Keys.Min(i => i.X);
            int maxX = lockedSectors.Keys.Max(i => i.X);
            int minY = lockedSectors.Keys.Min(i => i.Y);
            int maxY = lockedSectors.Keys.Max(i => i.Y);
            for (int y = minY; y <= maxY; y++)
            {
                StringBuilder[] lines = new StringBuilder[10];
                for (int i = 0; i < 10; i++)
                {
                    lines[i] = new StringBuilder();
                }
                for (int x = minX; x <= maxX; x++)
                {
                    if (lockedSectors.ContainsKey(new IntVector2(x, y)))
                    {
                        char[,] contents = lockedSectors[new IntVector2(x, y)].Contents;
                        for (int innerY = 0; innerY < 10; innerY++)
                        {
                            for (int innerX = 0; innerX < 10; innerX++)
                            {
                                lines[innerY].Append(contents[innerX, innerY]);
                            }
                        }
                    }
                    else
                    {
                        foreach (StringBuilder line in lines)

                        {
                            line.Append("..........");
                        }
                    }
                    foreach (StringBuilder line in lines)

                    {
                        line.Append("|");
                    }

                }
                foreach (StringBuilder line in lines)
                {
                    Console.WriteLine(line.ToString());
                }
                Console.WriteLine(new string('-', (maxX - minX+1) * 11));
            }
            Console.WriteLine();

        }

        private static void ProcessInput(string input)
        {
            freeSectors = new List<SatSector>();
            string[] inputParts = input.Split(new string[] { Environment.NewLine + Environment.NewLine }, StringSplitOptions.None);
            foreach (string inputPart in inputParts)
            {
                freeSectors.Add(new SatSector(inputPart));
            }
        }
    }

    internal class SatSector
    {
        long id;
        HashSet<string> edges;
        char[,] contents;

        public long Id
        {
            get
            {
                return id;
            }
        }

        public char[,] Contents
        {
            get
            {
                return contents;
            }
        }

        public SatSector(string input)
        {
            string[] inputParts = input.Split(new string[] { ":" + Environment.NewLine }, StringSplitOptions.None);
            id = Convert.ToInt64(inputParts[0].Split(new char[] { ' ' })[1]);
            inputParts = inputParts[1].Split(new string[] { Environment.NewLine}, StringSplitOptions.None);

            int width = inputParts[0].Length;
            int height = inputParts.Length;
            contents = new char[width, height];
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    contents[x, y] = inputParts[y][x];
                }
            }
            edges = new HashSet<string>();
            StringBuilder topString = new StringBuilder();
            StringBuilder bottomString = new StringBuilder();
            for (int x = 0; x < width; x++)
            {
                topString.Append(contents[x, 0]);
                bottomString.Append(contents[x, height - 1]);
            }
            edges.Add(topString.ToString());
            edges.Add(bottomString.ToString());

            topString = new StringBuilder();
            bottomString = new StringBuilder();
            for (int y = 0; y < height; y++)
            {
                topString.Append(contents[0, y]);
                bottomString.Append(contents[width - 1, y]);
            }
            edges.Add(topString.ToString());
            edges.Add(bottomString.ToString());

            HashSet<string> reversedEdges = new HashSet<string>();
            foreach (string edge in edges)
            {
                string newEdge = new string(edge.ToCharArray().Reverse().ToArray());
                reversedEdges.Add(newEdge);
            }
            edges.UnionWith(reversedEdges);
        }

        public override bool Equals(object obj)
        {

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return this.id == ((SatSector)obj).id;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return id.GetHashCode();
        }

        public bool HasEdge(string edge)
        {
            return edges.Contains(edge);
        }

        public void Clockwise()
        {
            // (x,y) -> (height-y, x)
            int width = contents.GetLength(0);
            int height = contents.GetLength(1);
            char[,] newContents = new char[height, width];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    newContents[x, y] = contents[height-y-1, x];
                }
            }
            contents = newContents;
        }
        public void AntiClockwise()
        {
            // (x,y) -> (height-y, x)
            int width = contents.GetLength(0);
            int height = contents.GetLength(1);
            char[,] newContents = new char[height, width];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    newContents[x, y] = contents[y, width - x-1];
                }
            }
            contents = newContents;

        }
        public void Flip()
        {
            // (x,y) -> (height-y, x)
            int width = contents.GetLength(0);
            int height = contents.GetLength(1);
            char[,] newContents = new char[height, width];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    newContents[x, y] = contents[y, x];
                }
            }
            contents = newContents;
        }

        public string GetEdge(Direction direction)
        {
            StringBuilder edge = new StringBuilder();
            int size = contents.GetLength(0);
            for (int i = 0; i < size; i++)
            {

                switch (direction)
                {
                    case Direction.North:
                        edge.Append(contents[i, 0]);
                        break;
                    case Direction.East:
                        edge.Append(contents[size-1, i]);
                        break;
                    case Direction.South:
                        edge.Append(contents[size-i -1, size-1]);
                        break;
                    case Direction.West:
                        edge.Append(contents[0, size-i-1]);
                        break;
                }
            }
            return edge.ToString();
        }

        internal void RotateTillMatch(string edge, Direction direction)
        {
            // flip edge as we're looking to match the mirror of it
            string flippedEdge = new string(edge.ToArray().Reverse().ToArray());
            for (int i = 0; i < 9; i++)
            {
                if (this.GetEdge(direction) == flippedEdge)
                {
                    break;
                }
                this.Clockwise();
                if (i == 4)
                {
                    this.Flip();
                }
            }
        }
    }
}