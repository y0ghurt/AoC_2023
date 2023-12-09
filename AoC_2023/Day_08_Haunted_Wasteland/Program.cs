using System.Text.RegularExpressions;

namespace day_08
{
    class HauntedWasteland
    {
        static string filePath = "resources\\day_08_first-input.txt";
        static Dictionary<string, Tuple<string, string>> nodeDict = new Dictionary<string, Tuple<string, string>>();
        static string directions = "";

        static void Main(string[] args)
        {
            ReadInputData();

            FirstTask();

            SecondTask();
        }
        static void ReadInputData()
        {
            int i = 0;
            foreach (string line in File.ReadLines(filePath))
            {
                if(i == 0)
                {
                    directions = line;
                } else if(i > 1)
                {
                    MatchCollection matches = Regex.Matches(line, @"[0-9a-zA-Z]{3}");
                    nodeDict.Add(matches[0].ToString(), new Tuple<string, string>(matches[1].ToString(), matches[2].ToString())); 
                }
                i++;
            }
        }


        static void FirstTask()
        {
            int directionCounter = 0;
            bool stillMoving = true;
            Tuple<string, string> node = nodeDict["AAA"];
            long stepCounter = 0;
            while(stillMoving)
            {
                string direction = "";
                switch(directions[directionCounter])
                {
                    case 'L':
                        direction = node.Item1;
                        break;
                    case 'R':
                        direction = node.Item2;
                        break;
                }
                node = nodeDict[direction];
                directionCounter++;
                if(directionCounter == directions.Length)
                {
                    directionCounter = 0;
                }
                stepCounter++;
                if (direction.Equals("ZZZ"))
                {
                    stillMoving = false;
                }
            }
            Console.WriteLine("First task: " + stepCounter);
        }

        static void SecondTask()
        {
            int directionCounter = 0;
            bool stillMoving = true;
            List<string> tempNodes = new List<string>();
            foreach(string k in nodeDict.Keys) 
            {
                if (k.EndsWith("A"))
                {
                    tempNodes.Add(k);
                }
            }
            string[] nodes = tempNodes.ToArray();
            long[] lapSteps = new long[nodes.Length];
            Tuple<string, string> node;
            long stepCounter = 0;
            while (stillMoving)
            {
                stillMoving = false;
                char direction = directions[directionCounter];
                for(int i = 0; i < nodes.Length; i++)
                {
                    if (nodes[i].Equals("   "))
                    {
                        continue;
                    }
                    node = nodeDict[nodes[i]];
                    switch (direction)
                    {
                        case 'L':
                            nodes[i] = node.Item1;
                            break;
                        case 'R':
                            nodes[i] = node.Item2;
                            break;
                    }
                }

                directionCounter++;
                if (directionCounter == directions.Length)
                {
                    directionCounter = 0;
                }
                stepCounter++;
                for (int i = 0; i < nodes.Length; i++)
                {
                    if (nodes[i].EndsWith("Z"))
                    {
                        nodes[i] = "   ";
                        lapSteps[i] = stepCounter;
                    }
                    if (!nodes[i].Equals("   "))
                    {
                        stillMoving = true;
                    }
                }
            }

            long lcm = FindLcm(lapSteps);
            
            Console.WriteLine("Second task: " + lcm);
        }

        static long FindLcm(long[] input)
        {
            long low = long.MaxValue;
            foreach (long l in input)
            {
                low = Math.Min(low, l);
            }
            long maxValue = 1;
            foreach(long l in input)
            {
                maxValue *= l;
            }

            long check;
            bool matches = true;
            for(long l = 1; l < maxValue; l++)
            {
                matches = true;
                check = low * l;
                foreach(long value in input)
                {
                    if(check % value != 0)
                    {
                        matches = false;
                    }
                }
                if(matches)
                {
                    return check;
                }
            }
            return maxValue;
        }
    }
}