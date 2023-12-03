using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Markup;

namespace day_02
{
    class CubeConundrum
    {
        static string filePath = "resources\\day_02_first-input.txt";
        static List<Game> games = new List<Game>();

        static void Main(string[] args)
        {
            ParseGames();

            FirstTask();

            SecondTask();
        }
        static void ParseGames()
        {
            foreach (string line in File.ReadLines(filePath))
            {
                string[] splitline = line.Split(':');
                string pattern = @"\d+";
                int id = int.Parse(Regex.Match(splitline[0], pattern).Value);
                Game game = new Game(id);

                string[] handsStrings = splitline[1].Split(';');
                foreach (string handsString in handsStrings)
                {
                    Hand hand = new Hand();
                    string[] colorsString = handsString.Split(",");
                    foreach (string color in colorsString)
                    {
                        string red = "(red)";
                        string green = "(green)";
                        string blue = "(blue)";
                        if (Regex.Match(color, red).Value != string.Empty)
                        {
                            hand.red = int.Parse(Regex.Match(color, pattern).Value);
                        } else if (Regex.Match(color, green).Value != string.Empty)
                        {
                            hand.green = int.Parse(Regex.Match(color, pattern).Value);
                        }
                        else if (Regex.Match(color, blue).Value != string.Empty)
                        {
                            hand.blue = int.Parse(Regex.Match(color, pattern).Value);
                        }

                    }
                    game.showHand(hand);
                }
                games.Add(game);
            }
        }

        static void FirstTask()
        {
            Game limit = new Game(0, 12, 13, 14);
            int idSum = 0;
            foreach(Game game in games)
            {
                if (game.red <= limit.red && game.green <= limit.green && game.blue <= limit.blue)
                {
                    idSum += game.id;
                }
            }
            Console.WriteLine("First task: " + idSum);
        }

        static void SecondTask()
        {
            int combinedPower = 0;
            foreach (Game game in games)
            {
                combinedPower += game.red * game.green * game.blue;
            }
            Console.WriteLine("Second task: " + combinedPower);
        }
    }

    class Game
    {
        public Game(int id, int red, int green, int blue)
        {
            this.id = id;
            this.red = red;
            this.green = green;
            this.blue = blue;
        }

        public Game(int id) : this(id, 0, 0, 0) { }

        public Game() : this(0) { }

        public void showHand(Hand hand)
        {
            red = Math.Max(red, hand.red);
            green = Math.Max(green, hand.green);
            blue = Math.Max (blue, hand.blue);
        }

        public int id;
        public int red;
        public int green;
        public int blue;
    }

    class Hand
    {
        public int red = 0;
        public int green = 0;
        public int blue = 0;
    }
}