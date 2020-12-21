using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2020
{
    class Day21
    {
        static HashSet<string> ingredients;
        static HashSet<string> allergens;
        static Dictionary<string, string> allergenToIngredient;

        public static string A(string input)
        {
            ingredients = new HashSet<string>();
            allergens = new HashSet<string>();
            allergenToIngredient = new Dictionary<string, string>();
            ProcessInput(input);
            HashSet<string> allergenFreeIngredients = new HashSet<string>(ingredients);
            allergenFreeIngredients.RemoveWhere(s => allergenToIngredient.Values.Contains(s));
            return CountEntries(input, allergenFreeIngredients).ToString();
        }

        public static string B(string input)
        {
            ingredients = new HashSet<string>();
            allergens = new HashSet<string>();
            allergenToIngredient = new Dictionary<string, string>();
            ProcessInput(input);
            SortedDictionary<string, string> sortedA2I = new SortedDictionary<string, string>(allergenToIngredient);
            StringBuilder answer = new StringBuilder();
            foreach (string ingredient in sortedA2I.Values)
            {
                answer.Append(ingredient);
                answer.Append(",");
            }
            return answer.ToString().TrimEnd(new char[] { ',' });
        }

        private static long CountEntries(string input, HashSet<string> allergenFreeIngredients)
        {
            long total = 0;
            string[] inputLines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (string line in inputLines)
            {
                HashSet<string> newIngredients = new HashSet<string>();
                HashSet<string> newAllergens = new HashSet<string>();
                ParseLine(line, out newIngredients, out newAllergens);
                total = total + (newIngredients.Intersect(allergenFreeIngredients)).Count();
            }
            return total;
        }

        internal static void ProcessInput(string input)
        {
            string[] inputLines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            // first get all the ingredients and allegens
            foreach (string line in inputLines)
            {
                HashSet<string> newIngredients = new HashSet<string>();
                HashSet<string> newAllergens= new HashSet<string>();
                ParseLine(line, out newIngredients, out newAllergens);
                ingredients.UnionWith(newIngredients);
                allergens.UnionWith(newAllergens);
            }

            // Set up the possible ingredients per allergen
            Dictionary<string, HashSet<string>> possibleIngredients = new Dictionary<string, HashSet<string>>();
            foreach (string allergen in allergens)
            {
                possibleIngredients.Add(allergen, new HashSet<string>(ingredients));
            }

            // Go through all the inputLines, filtering out ingredients that don't appear on lines with that allergen
            foreach (string line in inputLines)
            {
                HashSet<string> lineIngredients = new HashSet<string>();
                HashSet<string> lineAllergens = new HashSet<string>();
                ParseLine(line, out lineIngredients, out lineAllergens);
                foreach (string allergen in lineAllergens)
                {
                    possibleIngredients[allergen].IntersectWith(lineIngredients);
                }
            }

            // Go through all the possibleIngredients, looking for entires with only one ingredient
            while (possibleIngredients.Count > 0)
            {
                HashSet<string> allergensToRemove = new HashSet<string>();
                foreach (KeyValuePair<string, HashSet<string>> pair in possibleIngredients)
                {
                    if (pair.Value.Count == 1)
                    {
                        string allergen = pair.Key;
                        string ingredient = pair.Value.First();
                        allergenToIngredient.Add(allergen, ingredient);
                        foreach (HashSet<string> hs in possibleIngredients.Values)
                        {
                            hs.Remove(ingredient);
                        }
                        allergensToRemove.Add(allergen);
                    }
                }
                foreach (string allergen in allergensToRemove)
                {
                    possibleIngredients.Remove(allergen);
                }

            }
        }

        internal static void ParseLine(string line, out HashSet<string> ingredients, out HashSet<string> allergens)
        {
            // mxmxvkd kfcds sqjhc nhms (contains dairy, fish)

            string[] lineParts = line.Split(new string[] { " (contains " }, StringSplitOptions.None);
            ingredients = new HashSet<string>(lineParts[0].Split(new char[] { ' ' }));
            allergens = new HashSet<string>(lineParts[1].TrimEnd(new char[] { ')' }).Split(new string[] { ", " }, StringSplitOptions.None));
        }
    }
}
