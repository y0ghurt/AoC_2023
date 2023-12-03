using System.Text.RegularExpressions;

namespace day_03
{
    class GearRatios
    {
        static string filePath = "resources\\day_03_first-input.txt";
        static List<string> input = new List<string>();
        static List<Part> parts = new List<Part>();
        public static Dictionary<Tuple<int, int>, int> potentialGears = new Dictionary<Tuple<int, int>, int>();

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

            ProcessingState processingState = ProcessingState.NaN;
            string digit = @"\d";
            string partNumberString = "";
            Part part = new Part();
            
            for (int row = 0; row < input.Count; row++)
            {
                for (int column = 0; column < input[row].Length; column++)
                {
                    string character = input[row].Substring(column, 1);
                    if (Regex.IsMatch(character, digit)) {
                        if (processingState == ProcessingState.NaN)
                        {
                            part = new Part();
                            part.startPosition = new Position(row, column);
                            partNumberString = character;
                            processingState = ProcessingState.Number;
                            part.isAdjacentToSymbol |= SymbolChecker.checkAll(input, new Position(row, column), part);
                        } else if (processingState == ProcessingState.Number)
                        {
                            partNumberString += character;
                            part.isAdjacentToSymbol |= SymbolChecker.checkForward(input, new Position(row, column), part);
                        }

                        bool isEndOfNumber = false;
                        if (column + 1 >= input[row].Length)
                            isEndOfNumber = true;
                        else if (!Regex.IsMatch(input[row].Substring(column + 1, 1), digit))
                            isEndOfNumber = true;
                        if (isEndOfNumber)
                        {
                            part.partNumber = int.Parse(partNumberString);
                            parts.Add(part);
                            processingState = ProcessingState.NaN;
                        }

                    }
                }
            }
        }

        static void FirstTask()
        {

            int sum = 0;
            foreach (Part part in parts)
            {
                if (part.isAdjacentToSymbol)
                {
                    sum += part.partNumber;
                }
            }

            Console.WriteLine("First task: " + sum);
        }

        static void SecondTask()
        {
            int power = 0;
            foreach(KeyValuePair<Tuple<int, int>, int> potentialGear in potentialGears)
            {
                if(potentialGear.Value == 2)
                {
                    List<int> gearPowers = new List<int>();
                    foreach(Part part in parts) 
                    {
                        foreach (Position position in part.potentialGearPositions)
                        { 
                            if (potentialGear.Key.Equals(new Tuple<int, int>(position.row, position.column)))
                            {
                                gearPowers.Add(part.partNumber);
                            }
                        }
                        if (gearPowers.Count == 2)
                        {
                            power += gearPowers[0] * gearPowers[1];
                            break;
                        }
                    }
                }
            }
            Console.WriteLine("Second task: " + power);
        }
    }

    static class SymbolChecker
    {
        static string symbol = @"[^\d\.]";
        static string gearSymbol = @"\*";

        /*public static bool checkAll(List<string> input, int row, int column, Part part)
        {
            List<string> checkableArea = new List<string>();
            bool adjacentSymbol = false;
            int startpoint = Math.Max(column - 1, 0);
            int length = Math.Min(input[row].Length - startpoint - 1, 3 - (startpoint - (column - 1)));
            if (row != 0)
            {
                checkableArea.Add(input[row - 1].Substring(startpoint, length));
            }
            checkableArea.Add(input[row].Substring(startpoint, length));
            if (row + 1 < input.Count)
            {
                checkableArea.Add(input[row + 1].Substring(startpoint, length));
            }

            foreach(string checkableRow in checkableArea)
            {
                if(Regex.IsMatch(checkableRow, symbol))
                {
                    adjacentSymbol = true;
                }
            }
            return adjacentSymbol;
        }*/

        public static bool checkAll(List<string> input, Position position, Part part)
        {
            List<string> checkableArea = new List<string>();
            bool adjacentSymbol = false;
            string character = "";
            Position tempPosition = new Position(position.row, position.column);
            int baseColumn = 0;
            if (tempPosition.row != 0)
            {
                tempPosition.row--;
            }
            if (tempPosition.column != 0)
            {
                tempPosition.column--;
                baseColumn = tempPosition.column;
            }

            while(true)
            {
                if (tempPosition.row > position.row + 1 || tempPosition.row == input.Count)
                {
                    break;
                }
                while (true)
                {
                    if (tempPosition.column > position.column + 1 || tempPosition.column == input[tempPosition.row].Length)
                    {
                        tempPosition.column = baseColumn;
                        break;
                    }

                    character = input[tempPosition.row].Substring(tempPosition.column, 1);
                    if (Regex.IsMatch(character, symbol))
                    {
                        adjacentSymbol = true;
                        if(Regex.IsMatch(character, gearSymbol))
                        {
                            part.potentialGearPositions.Add(new Position(tempPosition.row, tempPosition.column));
                            countGear(new Position(tempPosition.row, tempPosition.column));
                        }
                    }

                    tempPosition.column++;
                }
                tempPosition.row++;
            }
            return adjacentSymbol;
        }
        public static bool checkForward(List<string> input, Position position, Part part)
        {
            List<string> checkableArea = new List<string>();
            bool adjacentSymbol = false;
            string character = "";
            Position tempPosition = new Position(position.row, position.column);
            int baseColumn = 0;
            if (tempPosition.row != 0)
            {
                tempPosition.row--;
            }
            tempPosition.column++;
            baseColumn = tempPosition.column;

            while (true)
            {
                if (tempPosition.row > position.row + 1 || tempPosition.row == input.Count)
                {
                    break;
                }
                while (true)
                {
                    if (tempPosition.column > position.column + 1 || tempPosition.column == input[tempPosition.row].Length)
                    {
                        tempPosition.column = baseColumn;
                        break;
                    }

                    character = input[tempPosition.row].Substring(tempPosition.column, 1);
                    if (Regex.IsMatch(character, symbol))
                    {
                        adjacentSymbol = true;
                        if (Regex.IsMatch(character, gearSymbol))
                        {
                            part.potentialGearPositions.Add(new Position(tempPosition.row, tempPosition.column));
                            countGear(new Position(tempPosition.row, tempPosition.column));
                        }
                    }

                    tempPosition.column++;
                }
                tempPosition.row++;
            }
            return adjacentSymbol;
        }

        static void countGear(Position position)
        {
            if (GearRatios.potentialGears.ContainsKey(new Tuple<int, int>(position.row, position.column)))
            {
                GearRatios.potentialGears[new Tuple<int, int>(position.row, position.column)]++;
            } else
            {
                GearRatios.potentialGears.Add(new Tuple<int, int>(position.row, position.column), 1);
            }
        }
    }

    class Part
    {
        public int partNumber;
        public Position startPosition;
        public List<Position> potentialGearPositions = new List<Position>();
        public bool isAdjacentToSymbol;

        public Part(int partNumber, Position startPosition, bool isAdjacentToSymbol)
        {
            this.partNumber = partNumber;
            this.startPosition = startPosition;
            this.isAdjacentToSymbol = isAdjacentToSymbol;
        }

        public Part()
        {
            partNumber = 0;
            startPosition = new Position(0, 0);
            isAdjacentToSymbol = false;
        }
    }

    class Position
    {
        public int row;
        public int column;

        public Position(int row, int column)
        {
            this.row = row;
            this.column = column;
        }
    }

    enum ProcessingState
    {
        NaN,
        Number
    }
}