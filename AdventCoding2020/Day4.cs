using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventCoding2020
{
    class Passport
    {
        string byr = ""; //(Birth Year)
        string iyr = ""; //(Issue Year)
        string eyr = ""; //(Expiration Year)
        string hgt = ""; //(Height)
        string hcl = ""; //(Hair Color)
        string ecl = ""; //(Eye Color)
        string pid = ""; //(Passport ID)
        string cid = ""; //(Country ID)

        public Passport(List<string> input)
        {
            foreach (string inputString in input)
            {
                List<string> inputParts = inputString.Split(new char[] { ' ' }).ToList();
                foreach (string part in inputParts)
                {
                    string[] partParts = part.Split(new char[] { ':' });
                    string key = partParts[0];
                    string value = partParts[1];
                    switch (key)
                    {
                        case "byr":
                            byr = value;
                            break;
                        case "iyr":
                            iyr = value;
                            break;
                        case "eyr":
                            eyr = value;
                            break;
                        case "hgt":
                            hgt = value;
                            break;
                        case "hcl":
                            hcl = value;
                            break;
                        case "ecl":
                            ecl = value;
                            break;
                        case "pid":
                            pid = value;
                            break;
                        case "cid":
                            cid = value;
                            break;
                        default:
                            throw new Exception("Unknown key " + key);
                    }
                }
            }
        }

        public bool IsPresent()
        {
            return (byr != "" && iyr != "" && eyr != "" && hgt != "" && hcl != "" && ecl != "" && pid != "");
        }

        internal bool IsValid()
        {
            int count = 0;
            //byr(Birth Year) - four digits; at least 1920 and at most 2002.
            if (byr.Length == 4)
            {
                int year;
                if (Int32.TryParse(byr, out year))
                {
                    if (1920 <= year && year <= 2002)
                    {
                        count++;
                    }
                }
            }

            //iyr(Issue Year) - four digits; at least 2010 and at most 2020.
            if (iyr.Length == 4)
            {
                int year;
                if (Int32.TryParse(iyr, out year))
                {
                    if (2010 <= year && year <= 2020)
                    {
                        count++;
                    }
                }
            }

            //eyr(Expiration Year) - four digits; at least 2020 and at most 2030.
            if (eyr.Length == 4)
            {
                int year;
                if (Int32.TryParse(eyr, out year))
                {
                    if (2020 <= year && year <= 2030)
                    {
                        count++;
                    }
                }
            }

            //hgt(Height) - a number followed by either cm or in:
            switch (hgt.Substring(hgt.Length - 2))
            {
                //If cm, the number must be at least 150 and at most 193.
                case "cm":
                    int height;
                    if (Int32.TryParse(hgt.Substring(0, hgt.Length - 2), out height))
                    {
                        if (150 <= height && height <= 193)
                        {
                            count++;
                        }
                    }
                    break;
                //If in, the number must be at least 59 and at most 76.
                case "in":
                    if (Int32.TryParse(hgt.Substring(0, hgt.Length - 2), out height))
                    {
                        if (59 <= height && height <= 76)
                        {
                            count++;
                        }
                    }
                    break;
                default:
                    break;
            }

            //hcl(Hair Color) - a # followed by exactly six characters 0-9 or a-f.
            Regex rgx = new Regex("^[#][0-9a-f]{6}$");
            if (rgx.IsMatch(hcl))
            {
                count++;
            }

            //ecl(Eye Color) - exactly one of: amb blu brn gry grn hzl oth.
            if (ecl == "amb" || ecl == "blu" || ecl == "brn" || ecl == "gry" || ecl == "grn" || ecl == "hzl" || ecl == "oth")
            {
                count++;
            }
            //pid(Passport ID) - a nine - digit number, including leading zeroes.
            rgx = new Regex("^[0-9]{9}$");
            if (rgx.IsMatch(pid))
            {
                count++;
            }

            return count == 7;
        }
    }

    class Day4
    {
        public static string A(string input)
        {
            List<string> inputList = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();
            List<string> passportStrings = new List<string>();
            List<Passport> passports = new List<Passport>();

            foreach (string inputLine in inputList)
            {
                if (inputLine.Length == 0)
                {
                    passports.Add(new Passport(passportStrings));
                    passportStrings = new List<string>();
                }
                else
                {
                    passportStrings.Add(inputLine);
                }
            }
            passports.Add(new Passport(passportStrings));

            int count = 0;
            foreach (Passport passport in passports)
            {
                if (passport.IsPresent())
                {
                    count++;
                }
            }
            return count.ToString();
        }

        public static string B(string input)
        {
            List<string> inputList = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None).ToList();
            List<string> passportStrings = new List<string>();
            List<Passport> passports = new List<Passport>();

            foreach (string inputLine in inputList)
            {
                if (inputLine.Length == 0)
                {
                    passports.Add(new Passport(passportStrings));
                    passportStrings = new List<string>();
                }
                else
                {
                    passportStrings.Add(inputLine);
                }
            }
            passports.Add(new Passport(passportStrings));

            int count = 0;
            foreach (Passport passport in passports)
            {
                if (passport.IsPresent() && passport.IsValid())
                {
                    count++;
                }
            }
            return count.ToString();
        }
    }

}
