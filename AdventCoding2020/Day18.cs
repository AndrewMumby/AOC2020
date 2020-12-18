using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCoding2020
{
    class Day18
    {
        public static string A(string input)
        {
            string[] inputLines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            long total = 0;
            foreach (string line in inputLines)
            {
                total = total + ProcessExpressionAll(line);
            }
            return total.ToString();
        }

        private static long ProcessExpressionAll(string line)
        {
            long total = 0;
            int i = 0;
            long currentNumber = 0;
            char currentOperator = '+';
            do
            {
                switch (line[i])
                {
                    case '(':
                    case ')':
                        // find the closing ) and recurse
                        i++;
                        int bracketCount = 1;
                        StringBuilder innerString = new StringBuilder();
                        do
                        {
                            switch (line[i])
                            {
                                case '(':
                                    bracketCount++;
                                    break;
                                case ')':
                                    bracketCount--;
                                    break;
                                default:
                                    break;
                            }
                            innerString.Append(line[i]);
                            i++;
                        } while (bracketCount > 0);

                        innerString.Remove(innerString.Length - 1, 1);
                        currentNumber = ProcessExpressionAll(innerString.ToString());
                        break;

                    case '+':
                        if (currentOperator == '+')
                        {
                            total = total + currentNumber;
                        }
                        else
                        {
                            total = total * currentNumber;
                        }                    
                        currentOperator = '+';
                        currentNumber = 0;
                        break;

                    case '*':
                        if (currentOperator == '+')
                        {
                            total = total + currentNumber;
                        }
                        else
                        {
                            total = total * currentNumber;
                        }
                        currentOperator = '*';
                        currentNumber = 0;
                        break;

                    case ' ':
                        break;

                    default:
                        currentNumber = currentNumber * 10;
                        currentNumber = currentNumber + int.Parse(line[i].ToString());
                        break;
                }
                i++;
            } while (i < line.Length);

            if (currentOperator == '+')
            {
                total = total + currentNumber;
            }
            else
            {
                total = total * currentNumber;
            }

            return total;
        }
    }

}
