using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace day_06
{
    class WaitForIt
    {
        static string filePath = "resources\\day_06_first-input.txt";
        static List<string> input = new List<string>();
        static List<Race> raceList = new List<Race>();
        static Race secondRace;


        static void Main(string[] args)
        {
            ReadInputData();

            FirstTask();

            SecondTask();
        }
        static void ReadInputData()
        {
            foreach (string line in File.ReadLines(filePath))
            {
                input.Add(line);
            }

            input[0] = input[0].Split(":")[1].Trim();
            input[1] = input[1].Split(":")[1].Trim();

            List<string> timeList = new List<string>();
            List<string> distanceList = new List<string>();

            foreach (string time in Regex.Split(input[0], @"\s+"))
            {
                timeList.Add(time);
            }

            foreach (string distance in Regex.Split(input[1], @"\s+"))
            {
                distanceList.Add(distance);
            }

            for(int i = 0; i < timeList.Count; i++)
            {
                raceList.Add(new Race(long.Parse(timeList[i]), long.Parse(distanceList[i])));
            }

            secondRace = new Race(long.Parse(Regex.Replace(input[0], @"\s+", "")), long.Parse(Regex.Replace(input[1], @"\s+", "")));
        }


        static void FirstTask()
        {
            long result = 1;
            foreach(Race race in raceList)
            {
                result = result * race.number;
            }
            Console.WriteLine("First task: " + result);
        }

        static void SecondTask()
        {
            Console.WriteLine("Second task: " + secondRace.number);
        }
    }

    class Race
    {
        public long number = 1;

        public Race(long time, long distance)
        {
            long bonus = time % 2;
            number += bonus;
            long startingTime = (time + bonus) / 2;
            long startingDistance = time - startingTime;
            for(long i = 1; (startingTime + i) * (startingDistance - i) > distance; i++)
            {
                number += 2;
            }
        }
    }
}