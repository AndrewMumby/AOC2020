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

        public static string B(string input)
        {
            string[] inputLines = input.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            long total = 0;
            foreach (string line in inputLines)
            {
                total = total + DoBMaths(line);
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

        private static string ProcessAllAddition(string line)
        {
            while (true)
            {
                string newLine = ProcessAnAddition(line);
                if (newLine.Equals(line))
                {
                    return newLine;
                }
                line = newLine;
            }
        }

        private static string ProcessAnAddition(string line)
        {
            int leftStart = 0;
            int operatorPos = 0;
            int rightEnd = 0;
            // iterate looking for the top level +
            do
            {
                if (line[operatorPos] == '(')
                {
                    int bracketCount = 0;
                    leftStart = operatorPos;
                    // find the other end of the bracket
                    do
                    {
                        switch (line[operatorPos])
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
                        operatorPos++;
                    } while (bracketCount > 0);

                }
                else if (line[operatorPos] == '*')
                {
                    // move the left start here 
                    leftStart = operatorPos + 2;
                }
                else if (line[operatorPos] == '+')
                {
                    break;
                }
                operatorPos++;
            } while (operatorPos < line.Length);
            // Either we ran out of string, or found a +
            if (operatorPos >= line.Length)
            {
                // no top level plusses
                return line;
            }
            else
            {
                // We found a plus :(
                // find the right hand expression
                rightEnd = operatorPos + 1;
                do
                {
                    if (line[rightEnd] == '(')
                    {
                        // find the end of the bracket. Again.
                        int bracketCount = 0;
                        do
                        {
                            switch (line[rightEnd])
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
                            rightEnd++;
                        } while (bracketCount > 0);
                    }
                    else if (line[rightEnd] == '+' || line[rightEnd] == '*')
                    {
                        // found the end.
                        rightEnd = rightEnd - 1;
                        break;
                    }
                    rightEnd++;
                } while (rightEnd < line.Length);
                string leftString = line.Substring(leftStart, operatorPos - 1 - leftStart);
                string rightString;
                if (rightEnd >= line.Length)
                {
                    rightString = line.Substring(operatorPos + 2);

                }
                else
                {
                    rightString = line.Substring(operatorPos + 2, rightEnd - 2 - operatorPos);
                }
                if (leftString.StartsWith("("))
                {
                    leftString = leftString.Substring(1);
                    leftString = leftString.Remove(leftString.Length - 1);
                }
                if (rightString.StartsWith("("))
                {
                    rightString = rightString.Substring(1);
                    rightString = rightString.Remove(rightString.Length - 1);
                }

                long leftValue = DoBMaths(leftString);
                long rightValue = DoBMaths(rightString);
                long total = leftValue + rightValue;
                StringBuilder newString = new StringBuilder();
                newString.Append(line.Substring(0, leftStart));
                newString.Append(total.ToString());
                if (rightEnd < line.Length)
                {
                    newString.Append(line.Substring(rightEnd));
                }
                return newString.ToString();
            }
        }

        private static string ProcessAllMultiplication(string line)
        {
            while (true)
            {
                string newLine = ProcessAMultiplication(line);
                if (newLine.Equals(line))
                {
                    return newLine;
                }
                line = newLine;

            }
        }

        private static string ProcessAMultiplication(string line)
        {
            int leftStart = 0;
            int operatorPos = 0;
            int rightEnd = 0;
            // iterate looking for the top level +
            do
            {
                if (line[operatorPos] == '(')
                {
                    int bracketCount = 0;
                    leftStart = operatorPos;
                    // find the other end of the bracket
                    do
                    {
                        switch (line[operatorPos])
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
                        operatorPos++;
                    } while (bracketCount > 0);

                }
                else if (line[operatorPos] == '*')
                {
                    break;
                }
                else if (line[operatorPos] == '+')
                {
                    throw new Exception("found a plus");
                }
                operatorPos++;
            } while (operatorPos < line.Length);
            // Either we ran out of string, or found a *
            if (operatorPos >= line.Length)
            {
                // no top level stars
                return line;
            }
            else
            {
                // We found a star :(
                // find the right hand expression
                rightEnd = operatorPos + 1;
                do
                {
                    if (line[rightEnd] == '(')
                    {
                        // find the end of the bracket. Again.
                        int bracketCount = 0;
                        do
                        {
                            switch (line[rightEnd])
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
                            rightEnd++;
                        } while (bracketCount > 0);
                    }
                    else if (line[rightEnd] == '+' || line[rightEnd] == '*')
                    {
                        // found the end.
                        rightEnd = rightEnd - 1;
                        break;
                    }
                    rightEnd++;
                } while (rightEnd < line.Length);

                string leftString = line.Substring(leftStart, operatorPos - 1 - leftStart);
                string rightString;
                if (rightEnd >= line.Length)
                {
                    rightString = line.Substring(operatorPos + 2);

                }
                else
                {
                    rightString = line.Substring(operatorPos + 2, rightEnd - 2 - operatorPos);
                }
                if (leftString.StartsWith("("))
                {
                    leftString = leftString.Substring(1);
                    leftString = leftString.Remove(leftString.Length - 1);
                }
                if (rightString.StartsWith("("))
                {
                    rightString = rightString.Substring(1);
                    rightString = rightString.Remove(rightString.Length - 1);
                }

                long leftValue = DoBMaths(leftString);
                long rightValue = DoBMaths(rightString);
                long total = leftValue * rightValue;
                StringBuilder newString = new StringBuilder();
                newString.Append(line.Substring(0, leftStart));
                newString.Append(total.ToString());
                if (rightEnd < line.Length)
                {
                    newString.Append(line.Substring(rightEnd));
                }
                return newString.ToString();
            }
        }

        private static long DoBMaths(string line)
        {
            // Do all the additions
            line = ProcessAllAddition(line);
            // Do all the multiplications
            line = ProcessAllMultiplication(line);
            // What's left is the answer;
            return Convert.ToInt64(line);
        }

    }
}
