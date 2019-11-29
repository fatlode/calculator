using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class Calculator
    {
        private string equation;

        public Calculator(string s)
        {
            equation = s;
        }

        //Solves the equation
        public double calculate()
        {
            double[] numbers;
            double answer;
            char[] operators;

            numbers = numberSplit(equation);
            operators = operatorSplit(equation, ref numbers);

            answer = orderSolve(numbers, operators);
            return answer;
        }


        //Determines the order in which the operation is solved by cycling through the operator array
        private double orderSolve(double[] numbers, char[] operators)
        {
            double opsLeft = 0;
            double total = 0;
            int count = 0;

            for (int i = 0; i < operators.Length; i++)
            {
                while (operators[i] == '^')
                {
                    opsLeft = operators.Length - i;
                    numbers[i] = solve(numbers[i], operators[i], numbers[i + 1]);
                    for (int n = i; n < operators.Length - 1; n++)
                    {
                        operators[n] = operators[n + 1];
                    }
                    operators[operators.Length - 1] = ' ';
                    for (int n = i + 1; n < numbers.Length - 1; n++)
                    {
                        numbers[n] = numbers[n + 1];
                    }
                }
            }

            for (int i = 0; i < operators.Length; i++)
            {
                while (operators[i] == 'x' || operators[i] == '/' || operators[i] == '%')
                {
                    opsLeft = operators.Length - i;
                    numbers[i] = solve(numbers[i], operators[i], numbers[i + 1]);
                    for (int n = i; n < operators.Length - 1; n++)
                    {
                        operators[n] = operators[n + 1];
                    }
                    operators[operators.Length - 1] = ' ';
                    for (int n = i + 1; n < numbers.Length - 1; n++)
                    {
                        numbers[n] = numbers[n + 1];
                    }
                }
            }
            count = 0;
            total = numbers[0];
            while ((count < operators.Length) && (operators[count] == '+' || operators[count] == '-'))
            {
                total = solve(total, operators[count], numbers[count + 1]);
                count++;
            }
            return total;
        }


        private double order(double[] numbers, char[] operators)
        {
            double answer = 0;
            return answer;
        }


        //splitting the equation string into an array of doubles
        private double[] numberSplit(String s)
        {
            double[] numbers = new double[s.Length / 2 + 1];
            string number = string.Empty;
            int count = 0;

            for (int i = 0; i < s.Length; i++)
            {
                if (Char.IsDigit(s[i]) || s[i] == '.')
                {
                    number += s[i];
                }
                else if (number.Length > 0)
                {
                    if (!Double.TryParse(number, out numbers[count]))
                    {
                        throw new ArgumentException("Invalid Double");
                    }
                    else
                    {
                        number = string.Empty;
                        count++;
                    }
                }
            }

            if (!Double.TryParse(number, out numbers[count]))
                throw new ArgumentException("Invalid Double");

            if (s[0] == '-')
            {
                numbers[0] = -numbers[0];
            }

            return numbers;
        }


        //Parses through string to identify operators and evluates validity of consecutive operators, then splits the equation string into an array of operators
        private char[] operatorSplit(String s, ref double[] numbers)
        {
            char[] operators;
            int count = 0;

            foreach (char o in equation)
            {
                if (o == '+' || o == '-' || o == 'x' || o == '/' || o == '%' || o == '^')
                {
                    count++;
                }
            }

            operators = new char[count];
            count = 0;
            for (int i = 0; i < equation.Length; i++)
            {
                if (equation[i] == '+' || equation[i] == '-' || equation[i] == 'x' || equation[i] == '/' || equation[i] == '%' || equation[i] == '^')
                {
                    if (equation[i] == '+' && equation[i + 1] == '+')
                    {
                        operators[count] = '+';
                        count++;
                        i++;
                    }
                    else if (equation[i] == '-' && equation[i + 1] == '-')
                    {
                        operators[count] = '+';
                        count++;
                        i++;
                    }
                    else if (equation[i] == '-' && equation[i + 1] == '+')
                    {
                        operators[count] = '-';
                        count++;
                        i++;
                    }
                    else if (equation[i] == '+' && equation[i + 1] == '-')
                    {
                        operators[count] = '-';
                        count++;
                        i++;
                    }
                    else if ((equation[i] == 'x' || equation[i] == '/' || equation[i] == '%' || equation[i] == '^') && equation[i + 1] == '-')
                    {
                        operators[count] = equation[i];
                        numbers[count + 1] = -numbers[count + 1];
                        count++;
                        i++;
                    }
                    else if ((equation[i] == 'x' || equation[i] == '/' || equation[i] == '%' || equation[i] == '^') && equation[i + 1] == '+')
                    {
                        operators[count] = equation[i];
                        count++;
                        i++;
                    }
                    else if (equation[i + 1] != '+' || equation[i + 1] != '-' || equation[i + 1] != 'x' || equation[i + 1] != '/' || equation[i + 1] != '%' || equation[i + 1] != '^')
                    {
                        operators[count] = equation[i];
                        count++;
                    }
                    else
                    {
                        throw new ArgumentException("Invalid Operators");
                    }
                }
            }
            return operators;
        }



        //Gets two numbers and solves them depending on the operator that is found between them
        private double solve(double left, char operation, double right)
        {
            double answer;
            switch (operation)
            {
                case '+':
                    answer = left + right;
                    return answer;
                case '-':
                    answer = left - right;
                    return answer;
                case 'x':
                    answer = left * right;
                    return answer;
                case '/':
                    if (right == 0)
                        throw new ArgumentException("∞");
                    answer = left / right;
                    return answer;
                case '%':
                    answer = left % right;
                    return answer;
                case '^':
                    answer = Math.Pow(left, right);
                    return answer;
                default: throw new ArgumentException("Invalid Operation");
            }
        }


        //Method to determine if the equation ends with an operator and if there are any invalid characters within the equation by cycling through each character
        public bool isValid(String s)
        {
            if (String.IsNullOrWhiteSpace(s) || !Char.IsNumber(s[s.Length - 1]))
            {
                return false;
            }

            for (int i = 0; i < s.Length; i++)
            {
                if (!Char.IsNumber(s[i]))
                {
                    if (s[i] != 'x' && s[i] != '/' && s[i] != '+' && s[i] != '-' && s[i] != '%' && s[i] != '^' && s[i] != '.')
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}