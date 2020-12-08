using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2020
{
    internal enum HHOperation :int
    {
        ACC,
        JMP,
        NOP
    }

    internal class Handheld
    {
        int instruction;
        int accumulator;
        List<HHInstruction> instructions;

        public Handheld(string input)
        {
            List<string> inputList = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();
            instructions = new List<HHInstruction>();
            foreach(string line in inputList)
            {
                instructions.Add(new HHInstruction(line));
            }
            instruction = 0;
            accumulator = 0;
        }

        public Handheld(Handheld masterHH)
        {
            this.instruction = masterHH.Instruction;
            this.accumulator = masterHH.Accumulator;
            instructions = new List<HHInstruction>();
            foreach (HHInstruction masterInst in masterHH.instructions)
            {
                instructions.Add(new HHInstruction(masterInst));
            }
        }

        public int Accumulator
        {
            get
            {
                return accumulator;
            }
            internal set
            {
                accumulator = value;
            }
        }

        public int Instruction
        {
            get
            {
                return instruction;
            }

            set
            {
                instruction = value;
            }
        }

        internal List<HHInstruction> Instructions
        {
            get
            {
                return instructions;
            }

            set
            {
                instructions = value;
            }
        }

        internal bool Halts()
        {
            HashSet<int> executedLines = new HashSet<int>();
            do
            {
                executedLines.Add(instruction);
                switch (instructions[instruction].Operation)
                {
                    case HHOperation.ACC:
                        accumulator += instructions[instruction].Argument;
                        instruction++;
                        break;
                    case HHOperation.JMP:
                        instruction = instruction + instructions[instruction].Argument;
                        break;
                    case HHOperation.NOP:
                        instruction++;
                        break;
                    default:
                        throw new Exception("Unknown operation.");
                }
                if (executedLines.Contains(instruction))
                {
                    return false;
                }
                if (instruction == instructions.Count)
                {
                    return true;
                }
                if (instruction > instructions.Count || instruction < 0)
                {
                    throw new Exception("Didn't halt or exit cleanly");
                }
            } while (true);
        }
    }

    internal class HHInstruction
    {
        HHOperation operation;
        int argument;

        internal HHOperation Operation
            {
                get
                {
                    return operation;
                }

                set
                {
                    operation = value;
                }
            }

            public int Argument
            {
                get
                {
                    return argument;
                }

                set
                {
                    argument = value;
                }
            }

            public HHInstruction(string line)
        {
            string[] parts = line.Split(new char[] { ' ' });
            switch (parts[0])
            {
                case "acc":
                    operation = HHOperation.ACC;
                    break;
                case "jmp":
                    operation = HHOperation.JMP;
                    break;
                case "nop":
                    operation = HHOperation.NOP;
                    break;
                default:
                    throw new Exception("Unknown operation: " + parts[0]);
            }
            argument = Convert.ToInt32(parts[1]);
        }

        public HHInstruction(HHInstruction masterInst)
        {
            this.operation = masterInst.operation;
            this.argument = masterInst.argument;            
        }
    }

    class Day8
    {
        public static string A(string input)
        {
            Handheld hh = new Handheld(input);
            if (!hh.Halts())
            {
                return hh.Accumulator.ToString();
            }
            else
            {
                return "Program halts";
            }
        }

        public static string B(string input)
        {
            Handheld masterHH = new Handheld(input);
            for (int i = 0; i < masterHH.Instructions.Count; i++)
            {
                if (masterHH.Instructions[i].Operation != HHOperation.ACC)
                {
                    Handheld copyHH = new Handheld(masterHH);
                    switch (copyHH.Instructions[i].Operation)
                    {
                        case HHOperation.JMP:
                            copyHH.Instructions[i].Operation = HHOperation.NOP;
                            break;
                        case HHOperation.NOP:
                            copyHH.Instructions[i].Operation = HHOperation.JMP;
                            break;
                        default:
                            throw new Exception("Tried to change an ACC");
                    }
                    if (copyHH.Halts())
                    {
                        return copyHH.Accumulator.ToString();
                    }
                }
            }
            return "No halting change found";
        }
    }

}

