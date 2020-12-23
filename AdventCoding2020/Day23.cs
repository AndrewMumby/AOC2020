using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2020
{
    class Day23
    {
        static int maxCup;
        static Dictionary<int, Cup> cupLookup;


        public static string A(string input)
        {
            Cup firstCup = ProcessInput(input, 9);

            for (int i = 0; i < 100; i++)
            {
                firstCup = PlayRound(firstCup);
            }

            // find the 1
            Cup currentCup = cupLookup[1];

            StringBuilder returnString = new StringBuilder();
            do
            {
                currentCup = currentCup.NextCup;
                returnString.Append(currentCup.CupLabel);
            } while (currentCup.CupLabel != 1);

            returnString.Remove(returnString.Length - 1, 1);
            return returnString.ToString();
        }

        public static string B(string input)
        {
            Cup firstCup = ProcessInput(input, 1000000);
            for (int i = 0; i < 10000000; i++)
            {
                firstCup = PlayRound(firstCup);
            }

            // find the 1

            Cup currentCup = cupLookup[1];
            return ((long)currentCup.NextCup.CupLabel * (long)currentCup.NextCup.NextCup.CupLabel).ToString();

        }

        private static Cup ProcessInput(string input, int cupCount)
        {
            cupLookup = new Dictionary<int, Cup>();
            Cup firstCup = new Cup(input[0] - '0', null);
            cupLookup.Add(firstCup.CupLabel, firstCup);
            Cup newCup = firstCup;
            for (int i = 1; i < cupCount; i++)
            {
                if (i < input.Length)
                {
                    newCup = new Cup(input[i] - '0', newCup);
                    cupLookup.Add(newCup.CupLabel, newCup);
                    maxCup = Math.Max(maxCup, input[i] - '0');
                }
                else
                {
                    newCup = new Cup(i + 1, newCup);
                    cupLookup.Add(newCup.CupLabel, newCup);
                    maxCup = i+1;
                }
            }
            firstCup.PreviousCup = newCup;
            newCup.NextCup = firstCup;
            return firstCup;
        }

        private static Cup PlayRound(Cup firstCup)
        {
            Cup currentCup = firstCup;
            Cup startRemove = currentCup.NextCup;
            Cup endRemove = startRemove.NextCup.NextCup;
            int labelToFind = currentCup.CupLabel - 1;
            if (labelToFind < 1)
            {
                labelToFind = maxCup;
            }

            while (labelToFind == startRemove.CupLabel || labelToFind == startRemove.NextCup.CupLabel || labelToFind == endRemove.CupLabel)
            {
                labelToFind--;
                if (labelToFind <1)
                {
                    labelToFind = maxCup;
                }
            }

            Cup destinationCup = cupLookup[labelToFind];

            Cup newFirstCup = endRemove.NextCup;

            // fix the new loop, removing the removed cups
            newFirstCup.PreviousCup = firstCup;
            firstCup.NextCup = newFirstCup;

            // insert the removed Cups
            Cup postDestinationCup = destinationCup.NextCup;
            destinationCup.NextCup = startRemove;
            startRemove.PreviousCup = destinationCup;
            endRemove.NextCup = postDestinationCup;
            postDestinationCup.PreviousCup = endRemove;

            return newFirstCup;
        }
    }

    class Cup
    {
        Cup previousCup;
        Cup nextCup;
        int cupLabel;

        internal Cup NextCup
        {
            get
            {
                return nextCup;
            }

            set
            {
                nextCup = value;
            }
        }

        internal Cup PreviousCup
        {
            get
            {
                return previousCup;
            }

            set
            {
                previousCup = value;
            }
        }

        public int CupLabel
        {
            get
            {
                return cupLabel;
            }
        }

        public Cup(int label, Cup previous)
        {
            this.cupLabel = label;
            this.previousCup = previous;
            if (previous != null)
            {
                previous.NextCup = this;
            }
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            { 
                return false;
            }

            // TODO: write your implementation of Equals() here
            return this.cupLabel == ((Cup)obj).cupLabel;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            // TODO: write your implementation of GetHashCode() here
            return this.cupLabel.GetHashCode();
        }

        public override string ToString()
        {
            return cupLabel.ToString();
        }

    }
    
}
