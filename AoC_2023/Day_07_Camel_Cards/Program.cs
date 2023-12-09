using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace day_07
{
    class CamelCards
    {
        static string filePath = "resources\\day_07_first-input.txt";
        static List<CamelCardHand> simpleHands = new List<CamelCardHand>();
        static List<CamelCardHand> advancedHands = new List<CamelCardHand>();

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
                string[] splitString = Regex.Split(line, @"\s+");
                simpleHands.Add(new CamelCardHand(splitString[0], int.Parse(splitString[1]), false));
                advancedHands.Add(new CamelCardHand(splitString[0], int.Parse(splitString[1]), true));
            }
        }


        static void FirstTask()
        {
            sortCards(simpleHands);

            long winnings = 0;
            for(int i = 0; i < simpleHands.Count; i++)
            {
                winnings += simpleHands[i].bet * (simpleHands.Count - i);
            }

            Console.WriteLine("First task: " + winnings);
        }

        static void SecondTask()
        {
            sortCards(advancedHands);

            long winnings = 0;
            for (int i = 0; i < advancedHands.Count; i++)
            {
                winnings += advancedHands[i].bet * (advancedHands.Count - i);
            }

            Console.WriteLine("Second task: " + winnings);
        }
        static void sortCards(List<CamelCardHand> hands)
        {
            if(hands.Count <= 1) { return ; }

            bool changed = true;

            while (changed) {
                changed = false;
                for (int i = 0; i < hands.Count - 1; i++)
                {
                    CamelCardHand currentHand = hands[i];
                    CamelCardHand nextHand = hands[i + 1];
                    bool change = false;
                    if (currentHand.category < nextHand.category)
                    {
                        change = true;
                    } else if (currentHand.category == nextHand.category)
                    {
                        for (int k = 0; k < 5; k++)
                        {
                            if (currentHand.handArray[k] > nextHand.handArray[k])
                            {
                                break;
                            }
                            if (currentHand.handArray[k] < nextHand.handArray[k])
                            {
                                change = true;
                                break;
                            }
                        }
                    }

                    if (change)
                    {
                        hands[i] = nextHand;
                        hands[i+1] = currentHand;
                        changed = true;
                    }
                }
            }
        }
    }

    class CamelCardHand
    {
        public bool advancedRules;
        public string hand;
        public int bet;
        public int category = 0;
        public int[] handArray = new int[5];

        public CamelCardHand(string hand, int bet, bool advancedRules)
        {
            this.hand = hand;
            this.bet = bet;
            this.advancedRules = advancedRules;

            for(int i = 0; i < 5; i++)
            {
                string card = hand.Substring(i, 1);
                if(Regex.IsMatch(card, @"\d"))
                {
                    handArray[i] = int.Parse(card);
                } else
                {
                    switch(card)
                    {
                        case "T":
                            handArray[i] = 10;
                            break;
                        case "J":
                            if (!advancedRules)
                                handArray[i] = 11;
                            else
                                handArray[i] = 1;
                            break;
                        case "Q":
                            handArray[i] = 12;
                            break;
                        case "K":
                            handArray[i] = 13;
                            break;
                        case "A":
                            handArray[i] = 14;
                            break;
                    }
                }
            }
            Dictionary<int, int> cardCount = new Dictionary<int, int>();
            for(int i = 1; i <= 14; i++)
            {
                cardCount.Add(i, 0);
            }

            foreach(int card in handArray)
            {
                for(int i = 1; i <= 14; i++)
                {
                    if(i == card)
                    {
                        cardCount[i]++;
                    }
                }
            }

            int maxCount = 0;
            for(int i = 2; i <= 14; i++)
            {
                maxCount = Math.Max(maxCount, cardCount[i] + cardCount[1]);
            }

            if(maxCount == 5)
            {
                category = 6;
            } else if (maxCount == 4)
            {
                category = 5;
            } else if(maxCount == 3)
            {
                if(cardCount.Values.Contains(2) && cardCount.Values.Contains(3))
                {
                    category = 4;
                } else
                {
                    int pairCounter = 0;
                    for(int i = 2; i <= 14; i++)
                    {
                        if (cardCount[i] == 2)
                        {
                            pairCounter++;
                        }
                    }
                    if (pairCounter == 2 && cardCount[1] == 1)
                    {
                        category = 4;
                    }
                    else
                    {
                        category = 3;
                    }
                }
            } else if (maxCount == 2) 
            {
                int pairCounter = 0;
                for (int i = 2; i <= 14; i++)
                {
                    if (cardCount[i] == 2)
                    {
                        pairCounter++;
                    }
                }
                if(pairCounter == 2)
                {
                    category = 2;
                } else
                {
                    category = 1;
                }
            }
        }
    }
}