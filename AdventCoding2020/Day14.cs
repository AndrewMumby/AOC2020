using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2020
{
    class Day14
    {
        static int memorySize = 36;

        static Dictionary<long, bool[]> memory; 

        private static bool[] ConvertLongToBitArray(long value)
        {
            bool[] bits = new bool[memorySize];

            for (int i = memorySize-1; i >= 0; i--)
            {
                if (value % 2 == 1)
                {
                    bits[i] = true;
                }
                else
                {
                    bits[i] = false;
                }
                value = value / 2;
            }
            return bits;
        }

        private static long ConvertBitArrayToLong(bool[] bits)
        {
            long value = 0;
            for (int i = 0; i < memorySize; i++)
            {
                value *= 2;
                if (bits[i])
                {
                    value++;
                }
            }
            return value;
        }

        private static long ConvertBitListToLong(List<char> bits)
        {
            long value = 0;
            for (int i = 0; i < memorySize; i++)
            {
                value *= 2;
                if (bits[i] == '1')
                {
                    value++;
                }
            }
            return value;
        }
        private static List<char> ConvertLongToBitList(long value)
        {
            char[] bits = new char[memorySize];
            for (int i = memorySize - 1; i >= 0; i--)
            {
                if (value % 2 == 1)
                {
                    bits[i] = '1';
                }
                else
                {
                    bits[i] = '0';
                }
                value = value / 2;
            }
            return bits.ToList();
        }



        public static string A(string input)
        {
            memory = new Dictionary<long, bool[]>();
            string mask = "";
            List<string> inputList = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();
            foreach (string line in inputList)
            {
                if (line.StartsWith("mask"))
                {
                    mask = line.Split(new char[] { ' ' })[2];
                }
                else
                {
                    int start = line.IndexOf('[') + 1;
                    int end = line.IndexOf(']');
                    string substring = line.Substring(start, end - start);
                    long address = Convert.ToInt64(substring);
                    long value = Convert.ToInt64(line.Split(new char[] { ' ' })[2]);
                    bool[] bits = ConvertLongToBitArray(value);
                    if (!memory.ContainsKey(address))
                    {
                        memory.Add(address, new bool[memorySize]);
                    }
                    for (int i = 0; i < memorySize; i++)
                    {
                        switch (mask[i])
                        {
                            case 'X':
                                memory[address][i] = bits[i];
                                break;
                            case '1':
                                memory[address][i] = true;
                                break;
                            case '0':
                                memory[address][i] = false;
                                break;
                            default:
                                throw new Exception("Unknown mask value");

                        }

                    }
                }
            }
            long returnValue = 0;
            foreach (bool[] bits in memory.Values)
            {
                returnValue = returnValue + ConvertBitArrayToLong(bits);
            }
            return returnValue.ToString();
        }

        private static void WriteMemory(long address, bool[] value)
        {
            if (!memory.ContainsKey(address))
            {
                memory.Add(address, value);
            }
            else
            {
                memory[address] = value;
            }
        }

        private static void TryTwoOptions(bool[] value, List<char> start, List<char> end)
        {
            List<char> newAddress;
            newAddress = start.ToList();
            newAddress.Add('0');
            newAddress.AddRange(end);
            ProcessString(value, newAddress);
            newAddress = start.ToList();
            newAddress.Add('1');
            newAddress.AddRange(end);
            ProcessString(value, newAddress);
        }

        private static void ProcessString(bool[] value, List<char> address)
        {
            int nextX = address.IndexOf('X');
            if (nextX == -1)
            {
                WriteMemory(ConvertBitListToLong(address), value);
            }
            else
            {
                List<char> start = new List<char>();
                List<char> end = new List<char>();
                for (int i = 0; i < nextX; i++)
                {
                    start.Add(address[i]);
                }
                for (int i = nextX+1; i < memorySize; i++)
                {
                    end.Add(address[i]);
                }
                TryTwoOptions(value, start, end);
            }
        }


        public static string B(string input)
        {
            memory = new Dictionary<long, bool[]>();
            string mask = "";
            List<string> inputList = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();
            foreach (string line in inputList)
            {
                if (line.StartsWith("mask"))
                {
                    mask = line.Split(new char[] { ' ' })[2];
                }
                else
                {
                    int start = line.IndexOf('[') + 1;
                    int end = line.IndexOf(']');
                    string addressString = line.Substring(start, end - start);
                    long value = Convert.ToInt64(line.Split(new char[] { ' ' })[2]);

                    List<char> address = ConvertLongToBitList(Convert.ToInt64(addressString));
                    for (int i = 0; i < memorySize; i++)
                    {
                        switch (mask[i])
                        {
                            case 'X':
                                address[i] = 'X';
                                break;
                            case '1':
                                address[i] = '1';
                                break;
                            case '0':
                                break;
                            default:
                                throw new Exception("Unknown mask value");
                        }
                    }
                    ProcessString(ConvertLongToBitArray(value), address);
                }
            }
            long returnValue = 0;
            foreach (bool[] bits in memory.Values)
            {
                returnValue = returnValue + ConvertBitArrayToLong(bits);
            }
            return returnValue.ToString();
        }

    }
}
