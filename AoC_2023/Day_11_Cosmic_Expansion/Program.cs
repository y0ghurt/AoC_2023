using System.Text.RegularExpressions;

namespace day_11
{
    class CosmicExpansion
    {
        static string filePath = "resources\\day_11_first-input.txt";
        static List<string> space = new List<string>();
        static Dictionary<int, Galaxy> galaxies = new Dictionary<int, Galaxy>();

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
                space.Add(line);
            }

            for(int i = space.Count - 1; i >= 0; i--)
            {
                if (!space[i].Contains('#'))
                {
                    space[i] = space[i].Replace('.', 'M');
                    //space.Insert(i, space[i]);
                }
            }

            space = RotateRight(space);

            for (int i = space.Count - 1; i >= 0; i--)
            {
                if (!space[i].Contains('#'))
                {
                    space[i] = space[i].Replace('.', 'M');
                    //space.Insert(i, space[i]);
                }
            }
            
            space = RotateRight(space);
            space = RotateRight(space);
            space = RotateRight(space);


            int idCounter = 0;
            for(int v = 0; v < space.Count; v++)
            {
                for(int h = 0; h < space[v].Length; h++)
                {
                    if (space[v][h] == '#')
                    {
                        galaxies.Add(idCounter, new Galaxy(idCounter, new Position(v, h)));
                        idCounter++;
                    }
                }
            }
        }

        static List<string> RotateRight(List<string> space)
        {
            List<string> spaceFlipped = new List<string>();
            for (int row = 0; row < space.Count; row++)
            {
                for (int column = 0; column < space[row].Length; column++)
                {
                    if (row == 0)
                    {
                        spaceFlipped.Add(space[row].Substring(column, 1));
                    }
                    else
                    {
                        spaceFlipped[column] = space[row].Substring(column, 1) + spaceFlipped[column];
                    }
                }
            }
            return spaceFlipped;
        }

        static long CalculateDistance(long multiplier)
        {
            long l = 0;
            for (int i = 0; i < galaxies.Count; i++)
            {
                for (int j = 0; j < galaxies.Count; j++)
                {
                    if (i < j)
                    {
                        l += Math.Max(galaxies[i].position.v, galaxies[j].position.v) - Math.Min(galaxies[i].position.v, galaxies[j].position.v);
                        l += Math.Max(galaxies[i].position.h, galaxies[j].position.h) - Math.Min(galaxies[i].position.h, galaxies[j].position.h);
                        for (int v = Math.Max(galaxies[i].position.v, galaxies[j].position.v); v >= Math.Min(galaxies[i].position.v, galaxies[j].position.v); v--)
                        {
                            if (space[v][galaxies[i].position.h] == 'M')
                            {
                                l += multiplier - 1;
                            }
                        }
                        for (int h = Math.Max(galaxies[i].position.h, galaxies[j].position.h); h >= Math.Min(galaxies[i].position.h, galaxies[j].position.h); h--)
                        {
                            if (space[galaxies[i].position.v][h] == 'M')
                            {
                                l += multiplier - 1;
                            }
                        }
                    }
                }
            }
            return l;
        }

        static void FirstTask()
        {
            long l = CalculateDistance(2);

            Console.WriteLine("First task: " + l);
        }

        static void SecondTask()
        {
            long l = CalculateDistance(1000000);

            Console.WriteLine("Second task: " + l);
        }


    }

    class Galaxy
    {
        public int id;
        public Position position;

        public Galaxy(int id)
        {
            this.id = id;
        }

        public Galaxy(int id, Position position) : this(id)
        {
            this.position = position;
        }
    }

    class Position
    {
        public int v, h;

        public Position(int v, int h)
        {
            this.v = v;
            this.h = h;
        }

        public Position() { }

        public override int GetHashCode()
        {
            int hashCode = v * 1000000 + h;
            return hashCode;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Position);
        }

        public bool Equals(Position other)
        {
            return other.v == v && other.h == h;
        }
    }

}