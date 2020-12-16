using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2020
{
    class Day16
    {
        static List<TrainTicketRule> rules;
        static TrainTicket myTicket;
        static List<TrainTicket> otherTickets;

        public static string A(string input)
        {
            ParseInput(input);

            List<int> allFields = new List<int>();
            foreach (TrainTicket ticket in otherTickets)
            {
                allFields.AddRange(ticket.Fields);
            }

            int total = 0;

            foreach (int field in allFields)
            {
                bool valid = false;
                foreach (TrainTicketRule rule in rules)
                {
                    if (rule.Validate(field))
                    {
                        valid = true;
                        break;
                    }
                }
                if (!valid)
                { 
                    total += field;
                }
            }
            return total.ToString();
        }

        public static string B(string input)
        {
            ParseInput(input);

            //Prune the shitty tickets        
            for (int i = otherTickets.Count - 1; i >= 0; i--)
            {
                if (!otherTickets[i].IsValid(rules))
                {
                    otherTickets.RemoveAt(i);
                }
            }

            // Work out the fields

            // Bundle each field up
            List<List<int>> fieldsValues = new List<List<int>>();
            for (int i = 0; i < otherTickets[0].Fields.Count(); i++)
            {
                fieldsValues.Add(new List<int>());
            }
            foreach (TrainTicket ticket in otherTickets)
            {
                for (int i = 0; i < ticket.Fields.Count; i++)
                {
                    fieldsValues[i].Add(ticket.Fields[i]);
                }
            }

            Dictionary<int, TrainTicketRule> foundFields = new Dictionary<int, TrainTicketRule>();

            while (foundFields.Count < rules.Count)
            {
                // If a field matches only one rule, we know what that field is
                for (int i = 0; i < fieldsValues.Count; i++)
                {
                    if (!foundFields.ContainsKey(i))
                    {
                        TrainTicketRule matchingRule = null;
                        int matchingRuleCount = 0;
                        foreach (TrainTicketRule rule in rules)
                        {
                            if (!foundFields.ContainsValue(rule))
                            {
                                if (rule.Validate(fieldsValues[i]))
                                {
                                    //Console.WriteLine("Field " + i + " matches rule " + rule.Name);
                                    matchingRule = rule;
                                    matchingRuleCount++;
                                }
                            }
                        }

                        if (matchingRuleCount == 1)
                        {
                            //Console.WriteLine("Field " + i + " only matches rule " + matchingRule.Name + " so it must be that");
                            foundFields.Add(i, matchingRule);
                        }
                    }
                }
            }

            long total = 1;
            foreach (KeyValuePair<int, TrainTicketRule> pair in foundFields)
            {
                //Console.WriteLine(pair.Value.Name + " " + myTicket.Fields[pair.Key]);
                if (pair.Value.Name.Contains("departure"))
                {
                    total = total * myTicket.Fields[pair.Key];
                }

            }
            return total.ToString();
        }

        private static void ParseInput(string input)
        {
            rules = new List<TrainTicketRule>();
            List<string> inputParts = input.Split(new string[] { Environment.NewLine + "your ticket:" + Environment.NewLine, Environment.NewLine + "nearby tickets:" + Environment.NewLine }, StringSplitOptions.None).ToList();
            //List<string> inputList = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();
            // part 0 is the rules
            List<string> ruleLines = inputParts[0].Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToList();
            foreach (string ruleLine in ruleLines)
            {
                rules.Add(new TrainTicketRule(ruleLine));
            }

            // part 1 is my ticket (Don't care)
            myTicket = new TrainTicket(inputParts[1].TrimEnd(new char[] { '\n', '\r' }));


            // part 2 is the other tickets
            otherTickets = new List<TrainTicket>();
            foreach (string ticketString in inputParts[2].Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
            {
                otherTickets.Add(new TrainTicket(ticketString));
            }
        }
    }

    internal class TrainTicket
    {
        List<int> fields;

        public TrainTicket(string ticketString)
        {
            // 7,3,47
            fields = ticketString.Split(new char[] { ',' }).Select(v => Convert.ToInt32(v)).ToList();
        }

        public List<int> Fields
        {
            get
            {
                return fields.ToList();
            }
        }

        internal bool IsValid(List<TrainTicketRule> rules)
        {
            // for it to be valid, all fields must match at least one rule
            foreach (int field in fields)
            {
                bool fieldValid = false;
                foreach (TrainTicketRule rule in rules)
                {
                    if (rule.Validate(field))
                    {
                        fieldValid = true;
                        break;
                    }
                }
                if (!fieldValid)
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsValid(TrainTicketRule rule)
        {
            foreach (int value in fields)
            {
                if (!rule.Validate(value))
                {
                    return false;
                }
            }
            return true;
        }
    }

    internal class TrainTicketRule
    {
        string name;
        List<TrainTicketRuleMinMax> boundaries;

        public string Name
        {
            get
            {
                return name;
            }
        }

        public TrainTicketRule(string ruleLine)
        {
            // departure date: 26-290 or 306-962
            boundaries = new List<TrainTicketRuleMinMax>();
            string[] ruleParts = ruleLine.Split(new string[] { ": " }, StringSplitOptions.None);
            this.name = ruleParts[0];
            foreach (string rangeString in ruleParts[1].Split(new string[] { " or " }, StringSplitOptions.None))
            {
                boundaries.Add(new TrainTicketRuleMinMax(rangeString));
            }
        }

        public override bool Equals(object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                TrainTicketRule p = (TrainTicketRule)obj;
                return name.Equals(p.name);
            }
        }

        public override int GetHashCode()
        {
            return name.GetHashCode();
        }

        public override string ToString()
        {
            StringBuilder output = new StringBuilder();
            output.Append(name);
            output.Append(": ");
            foreach (TrainTicketRuleMinMax minMax in boundaries)
            {
                output.Append(minMax.ToString());
                output.Append(" or ");
            }
            output.Remove(output.Length - 4, 4);
            return output.ToString();
        }

        internal bool Validate(int value)
        {
            foreach (TrainTicketRuleMinMax boundary in boundaries)
            {
                if (boundary.IsInside(value))
                {
                    return true;
                }
            }
            return false;
        }

        internal bool Validate(List<int> values)
        {
            foreach (int value in values)
            {
                if (!this.Validate(value))
                {
                    return false;
                }
            }
            return true;
        }
    }

    internal class TrainTicketRuleMinMax
    {
        int min;
        int max;

        public TrainTicketRuleMinMax(string rangeString)
        {
            string[] parts = rangeString.Split(new char[] { '-' });
            min = Convert.ToInt32(parts[0]);
            max = Convert.ToInt32(parts[1]);
        }

        public override string ToString()
        {
            return min.ToString() + "-" + max.ToString();
        }

        internal bool IsInside(int value)
        {
            return min <= value && value <= max;
        }
    }
}
