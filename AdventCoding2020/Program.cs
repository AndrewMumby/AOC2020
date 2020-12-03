using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace AdventCoding2020
{
    class Program
    {
        [STAThreadAttribute]
        static void Main(string[] args)
        {
            int day = 25;
            string part = "B";
            for (day = 25; day > 0; day--)
            {
                Type searchDayClass = Type.GetType("AdventCoding2020.Day" + day);
                if (searchDayClass != null)
                {
                    MethodInfo searchPartMethod = searchDayClass.GetMethod("B");
                    if (searchPartMethod != null)
                    {
                        part = "B";
                        break;
                    }
                    else
                    {
                        searchPartMethod = searchDayClass.GetMethod("A");
                        if (searchPartMethod != null)
                        {
                            part = "A";
                            break;
                        }
                    }
                }
            }

            Console.WriteLine("Enter the day to run, or none for the latest (Day" + day + part + "):");
            string input = Console.ReadLine();
            if (input != "")
            {

                day = Convert.ToInt32(input.Substring(0, input.Length - 1));
                part = input.Substring(input.Length-1).ToUpper();
            }
            Console.WriteLine("Running code for " + day + part);
            Type dayClass = Type.GetType("AdventCoding2020.Day" + day);
            if (dayClass != null)
            {
                MethodInfo partMethod = dayClass.GetMethod(part);
                if (partMethod != null)
                {
                    // Got the method we're running
                    // First, run the tests
                    string[] testFileNames = Directory.GetFiles("Day" + day + "\\" + part);
                    bool passed = true;
                    foreach(string testFileName in testFileNames)
                    {
                        string[] testText = File.ReadAllLines(testFileName);
                        string correctAnswer = testText[0];
                        string question = "";
                        for (int i = 1; i < testText.Length; i++)
                        {
                            question += testText[i] + Environment.NewLine;
                        }
                        question = question.Trim();
                        string actualAnswer = partMethod.Invoke(dayClass, new object[] { question }).ToString();
                        Console.Write("Test " + testFileName + ": ");
                        if (actualAnswer.Equals(correctAnswer))
                        {
                            Console.WriteLine("PASSED");
                        }
                        else
                        {
                            Console.WriteLine("FAILED! Expected: " + correctAnswer + ", Actual: " + actualAnswer);
                            passed = false;
                        }
                    }
                    // If all the tests pass, run the problem text
                    if (passed)
                    {
                        string problem = File.ReadAllText("Day" + day + "\\input.txt");
                        Stopwatch sw = new Stopwatch();
                        sw.Start();
                        string answer = partMethod.Invoke(dayClass, new object[] { problem }).ToString();
                        sw.Stop();
                        Console.WriteLine("Answer: " + answer);
                        Console.WriteLine("In: " + Convert.ToDouble(sw.ElapsedMilliseconds) / 1000 + "s");
                        Clipboard.SetText(answer);
                    }
                    else
                    {
                        Console.WriteLine("Tests failed. Aborting");
                    }
                }
            }
            Console.ReadLine();
        }
    }
}
