using System.Text.RegularExpressions;
using System.Windows.Markup;

namespace day_01
{
    class DecodeCoordinates
    {
        static string filePath = "resources\\day_01_first-input.txt";

        static void Main(string[] args)
        {
            FirstTask();

            SecondTask();
        }
        static void FirstTask()
        {
            List<int> input = new List<int>();
            foreach (string line in File.ReadLines(filePath))
            {
                List<int> values = new List<int>();

                for (int i = 0; i < line.Length; i++)
                {
                    char sign = line.Substring(i, 1).ToCharArray()[0];
                    if (char.IsDigit(sign))
                    {
                        values.Add(sign - '0');
                    }
                }

                if (values.Count() == 0)
                {
                    input.Add(0);
                }

                else
                {
                    input.Add(values[0] * 10 + values.Last());
                }
            }

            int result = 0;
            foreach (int value in input)
            {
                result += value;
            }

            Console.WriteLine("First task: " + result);
        }

        static void SecondTask()
        {
            string pattern = @"(one)|(two)|(three)|(four)|(five)|(six)|(seven)|(eight)|(nine)|\d";

            List<int> values = new List<int>();

            foreach (string line in File.ReadLines(filePath))
            {

                string first = Regex.Match(line, pattern).ToString();
                string last = Regex.Match(line, pattern, RegexOptions.RightToLeft).ToString();

                int firstInt;
                int lastInt;

                if(first.Length == 0)
                {
                    //Console.WriteLine(0);

                    values.Add(0);
                }
                else
                {
                    if (first.Length == 1)
                    {
                        firstInt = int.Parse(first);
                    }
                    else
                    {
                        firstInt = Intify(first);
                    }

                    if (last.Length == 1)
                    {
                        lastInt = int.Parse(last);
                    }
                    else
                    {
                        lastInt = Intify(last);
                    }

                    int value = firstInt * 10 + lastInt;

                    //Console.WriteLine(value);

                    values.Add(value);
                }

            }

            int result = 0;
            foreach (int value in values)
            {
                result += value;
            }

            Console.WriteLine("Second task: " + result);

        }
        static int Intify(String inputString)
        {
            switch(inputString)
            {
                case "one":
                    return 1;
                case "two":
                    return 2;
                case "three":
                    return 3;
                case "four":
                    return 4;
                case "five":
                    return 5;
                case "six":
                    return 6;
                case "seven":
                    return 7;
                case "eight":
                    return 8;
                case "nine":
                    return 9;
            }

            Console.WriteLine("[ERROR] Unexpected input: " + inputString);
            return -1;
        }
    }
}