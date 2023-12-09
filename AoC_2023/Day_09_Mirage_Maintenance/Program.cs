using System.Text.RegularExpressions;

namespace day_09
{
    class MirageMaintenance
    {
        static string filePath = "resources\\day_09_first-input.txt";
        static List<Dataset> datasets = new List<Dataset>();

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
                string[] strings = line.Split(" ");
                List<int> ints = new List<int>();

                for(int i = 0; i < strings.Length;i++)
                {
                    ints.Add(int.Parse(strings[i]));
                }

                datasets.Add(new Dataset(ints));
            }
        }


        static void FirstTask()
        {
            long value = 0;
            foreach(Dataset dataset in datasets)
            {
                value += dataset.nextValue;
            }
            Console.WriteLine("First task: " + value);
        }

        static void SecondTask()
        {
            long value = 0;
            foreach (Dataset dataset in datasets)
            {
                value += dataset.previousValue;
            }
            Console.WriteLine("Second task: " + value);
        }

        
    }
    class Dataset
    {
        public int nextValue = 0;
        public int previousValue = 0;
        List<List<int>> values = new List<List<int>>();

        public Dataset(List<int> input) 
        {
            values.Add(input);
            bool allZeroes = false;
            while(!allZeroes)
            {
                allZeroes = true;
                List<int> currentList = values[values.Count - 1];
                List<int> nextList = new List<int>();

                for(int i = 0; i < currentList.Count - 1; i++)
                {
                    nextList.Add(currentList[i + 1] - currentList[i]);
                }

                foreach(int i in nextList)
                {
                    if(i != 0)
                    {
                        allZeroes = false;
                    }
                }
                values.Add(nextList);
            }

            for(int i = values.Count - 2; i >= 0; i--)
            {
                values[i].Add(values[i + 1].Last() + values[i].Last());
                values[i].Insert(0, values[i].First() - values[i + 1].First());
            }

            nextValue = values[0].Last();
            previousValue = values[0].First();
        }
    }
}