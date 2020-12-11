using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2020
{
    class Day11
    {
        internal enum SeatState
        {
            Floor = '.',
            Seat = 'L',
            Person = '#'
        }

        internal class SeatingPlan
        {
            Dictionary<IntVector2, SeatState> seats = new Dictionary<IntVector2, SeatState>();
            int width;
            int height;

            public SeatingPlan(string input)
            {
                List<string> inputList = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();
                height = inputList.Count;
                width = inputList[0].Length;
                for (int y = 0; y < inputList.Count; y++)
                {
                    for (int x = 0; x < inputList[y].Length; x++)
                    {
                        seats.Add(new IntVector2(x, y), (SeatState)inputList[y][x]);
                    }
                }
            }

            internal void IterateTillStatic()
            {
                bool changed = false;
                do
                {
                    changed = false;
                    Dictionary<IntVector2, SeatState> newSeats = new Dictionary<IntVector2, SeatState>();
                    foreach (KeyValuePair<IntVector2, SeatState> pair in seats)
                    {
                        if (pair.Value != SeatState.Floor)
                        {
                            int peopleAround = PeopleAround(pair.Key);
                            if (peopleAround >= 4)
                            {
                                newSeats.Add(pair.Key, SeatState.Seat);
                                if (pair.Value != SeatState.Seat)
                                {
                                    changed = true;
                                }
                            }
                            else if (peopleAround == 0)
                            {
                                newSeats.Add(pair.Key, SeatState.Person);
                                if (pair.Value != SeatState.Person)
                                {
                                    changed = true;
                                }
                                
                            }
                            else
                            {
                                newSeats.Add(pair.Key, pair.Value);
                            }
                        }
                    }
                    seats = newSeats;
                    //DebugPrint();
                } while (changed);
            }

            internal void LoSIterateTillStatic()
            {
                bool changed = false;
                do
                {
                    changed = false;
                    Dictionary<IntVector2, SeatState> newSeats = new Dictionary<IntVector2, SeatState>();
                    foreach (KeyValuePair<IntVector2, SeatState> pair in seats)
                    {
                        if (pair.Value != SeatState.Floor)
                        {
                            int peopleAround = LoSPeopleAround(pair.Key);
                            if (peopleAround >= 5)
                            {
                                newSeats.Add(pair.Key, SeatState.Seat);
                                if (pair.Value != SeatState.Seat)
                                {
                                    changed = true;
                                }
                            }
                            else if (peopleAround == 0)
                            {
                                newSeats.Add(pair.Key, SeatState.Person);
                                if (pair.Value != SeatState.Person)
                                {
                                    changed = true;
                                }

                            }
                            else
                            {
                                newSeats.Add(pair.Key, pair.Value);
                            }
                        }
                    }
                    seats = newSeats;
                    //DebugPrint();
                } while (changed);
            }

            private int LoSPeopleAround(IntVector2 position)
            {
                int count = 0;
                foreach (IntVector2 direction in IntVector2.CardinalDirectionsIncludingDiagonals)
                {
                    IntVector2 testPos = position;
                    bool found;
                    do
                    {
                        found = false;
                        testPos = IntVector2.Add(testPos, direction);
                        if (testPos.X < 0 || testPos.X >= width || testPos.Y < 0 || testPos.Y >= height)
                        {
                            break;
                        }
                        if (seats.ContainsKey(testPos))
                        {
                            found = true;
                        }
                        if (IsPerson(testPos))
                        {
                            count++;
                        }
                    }
                    while (!found);

                }
                return count;
            }

            private int PeopleAround(IntVector2 position)
            {
                int count = 0;
                foreach (IntVector2 direction in IntVector2.CardinalDirectionsIncludingDiagonals)
                {
                    IntVector2 testPos = IntVector2.Add(position, direction);
                    if (IsPerson(testPos))
                    {
                        count++;
                    }

                }
                return count;
            }

            private bool IsPerson(IntVector2 position)
            {
                return seats.ContainsKey(position) && seats[position] == SeatState.Person;
            }

            internal int PersonCount()
            {
                int count = 0;
                foreach (SeatState seat in seats.Values)
                {
                    if (seat == SeatState.Person)
                    {
                        count++;
                    }
                }
                return count;
            }

            internal void DebugPrint()
            {
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        IntVector2 location = new IntVector2(x, y);
                        if (seats.ContainsKey(location))
                        {
                            if (IsPerson(location))
                            {
                                Console.Write("#");
                            }
                            else
                            {
                                Console.Write("L");
                            }
                        }
                        else
                        {
                            Console.Write(".");
                        }
                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }

        }
        public static string A(string input)
        {
            SeatingPlan seats = new SeatingPlan(input);
            seats.IterateTillStatic();
            return seats.PersonCount().ToString();
        }

        public static string B(string input)
        {
            SeatingPlan seats = new SeatingPlan(input);
            seats.LoSIterateTillStatic();
            return seats.PersonCount().ToString();
        }
    }
}
