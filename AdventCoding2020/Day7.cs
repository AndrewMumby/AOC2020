using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2020
{
    class Bag
    {
        private string colour;
        private Dictionary<Bag, int> contents;

        public Bag(string colour, Dictionary<Bag, int> contents)
        {
            this.Colour = colour;
            this.Contents = contents;
        }

        public bool Equals(Bag x, Bag y)
        {
            return x.Colour.Equals(y.Colour);
        }

        public int BagsInsideIncludingThisOne()
        {
            int bagCount = 1;
            foreach (KeyValuePair<Bag, int> pairs in Contents)
            {
                bagCount = bagCount + pairs.Value * pairs.Key.BagsInsideIncludingThisOne();
            }
            return bagCount;
        }


        public string Colour
        {
            get
            {
                return colour;
            }

            set
            {
                colour = value;
            }
        }

        internal Dictionary<Bag, int> Contents
        {
            get
            {
                return contents;
            }

            set
            {
                contents = value;
            }
        }
    }
    class Day7
    {
        static List<Bag> availableBags = new List<Bag>();        

        public static string A(string input)
        {
            // How many bag colours eventually contain one "shiny gold" bag?
            StuffBags(input);
            string targetBagColour = "shiny gold";
            Dictionary<Bag, bool> hasTarget = new Dictionary<Bag, bool>();
            bool changed = false;
            List<Bag> targetBags = new List<Bag>();
            targetBags.Add(FindBag(targetBagColour));
            do
            {
                changed = false;
                foreach (Bag bag in availableBags)
                {
                    if (!targetBags.Contains(bag) && bag.Contents.Keys.Intersect(targetBags).ToList().Count > 0)
                    {
                        targetBags.Add(bag);
                        changed = true;
                    }
                }
            }
            while (changed == true);

            return (targetBags.Count-1).ToString();
        }

        public static string B(string input)
        {
            StuffBags(input);
            string targetBagColour = "shiny gold";
            Bag startBag = FindBag(targetBagColour);
            return (startBag.BagsInsideIncludingThisOne()-1).ToString();
        }

        private static void StuffBags(string input)
        {
            List<string> inputList = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();
            foreach (string line in inputList)
            {
                // remove the trailing " bag."

                // split on " bags contain "
                string[] parts = line.Split(new string[] { " bags contain ", " bags, ", " bag, ", " bag.", " bags.", "no other bags." }, StringSplitOptions.RemoveEmptyEntries);
                Bag parentBag = FindBag(parts[0]);
                Dictionary<Bag, int> bagContents = new Dictionary<Bag, int>();
                for (int i = 1; i < parts.Length; i++)
                {
                    string baglet = parts[i];
                    int quantity = Convert.ToInt32(baglet.Split(' ')[0]);
                    string colour = baglet.Substring(baglet.IndexOf(' ') + 1);
                    bagContents.Add(FindBag(colour), quantity);
                }
                parentBag.Contents = bagContents;
            }
        }

        private static Bag FindBag(string bagColour)
        {
            foreach (Bag bag in availableBags)
            {
                if (bag.Colour == bagColour)
                {
                    return bag;
                }
            }

            Bag newBag = new Bag(bagColour, new Dictionary<Bag, int>());
            availableBags.Add(newBag);
            return newBag;
        }
    }
}
