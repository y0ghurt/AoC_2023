using System.Text.RegularExpressions;

namespace day_04
{
    class Scratchcards
    {
        static string filePath = "resources\\day_04_first-input.txt";
        static List<string> input = new List<string>();
        static Dictionary<int, Scratchcard> scratchcards = new Dictionary<int, Scratchcard>();


        static void Main(string[] args)
        {
            ParsePartNumbers();

            FirstTask();

            SecondTask();
        }
        static void ParsePartNumbers()
        {
            foreach (string line in File.ReadLines(filePath))
            {
                input.Add(line);
            }

            foreach(string line in input)
            {
                Scratchcard scratchcard = new Scratchcard();
                scratchcard.id = int.Parse(Regex.Split(line.Split(":")[0], @"\s+")[1]);
                foreach(string winner in Regex.Split(line.Trim().Split(":")[1].Trim().Split("|")[0].Trim(), @"\s+"))
                {
                    scratchcard.winningNumbers.Add(int.Parse(winner));
                }
                foreach (string number in Regex.Split(line.Trim().Split(":")[1].Trim().Split("|")[1].Trim(), @"\s+"))
                {
                    scratchcard.numbers.Add(int.Parse(number));
                }
                scratchcards.Add(scratchcard.id, scratchcard);
            }

        }

        static void FirstTask()
        {
            int score = 0;
            foreach (Scratchcard scratchcard in scratchcards.Values)
            {
                score += scratchcard.evaluateScore();
            }

            Console.WriteLine("First task: " + score);
        }

        static void SecondTask()
        {
            int cards = 0;
            foreach(Scratchcard scratchcard in scratchcards.Values)
            {
                scratchcard.evaluateScore();
                for(int i = 0; i < scratchcard.matches; i++)
                {
                    scratchcards[scratchcard.id + 1 + i].numberOfCards += scratchcard.numberOfCards;
                }
                cards += scratchcard.numberOfCards;
            }

            Console.WriteLine("Second task: " + cards);
        }
    }

    class Scratchcard
    {
        public int id = 0;
        public List<int> winningNumbers = new List<int>();
        public List<int> numbers = new List<int>();
        public int numberOfCards = 1;
        public int matches = 0;
        public Scratchcard() { }

        public int evaluateScore()
        {
            matches = 0;
            foreach(int number in numbers)
            {
                if(winningNumbers.Contains(number))
                {
                    matches++;
                }
            }
            if (matches == 0)
                return matches;

            int score = (int)Math.Pow(2, matches - 1);
            return score;
        }
    }
}