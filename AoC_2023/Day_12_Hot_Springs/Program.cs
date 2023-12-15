using System.Reflection.Metadata;
using System.Text.RegularExpressions;

namespace day_12
{
    class HotSprings
    {
        static string filePath = "resources\\day_12_first-input.txt";
        static List<Record> records = new List<Record>();
        static List<Record> unfoldedRecords = new List<Record>();
        static Dictionary<string, long> recordCache = null;

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
                string[] splitLine = line.Split(' ');
                records.Add(new Record(splitLine[0], splitLine[1].Split(',')));
            }
        }

        static void FirstTask()
        {
            long l = 0;
            foreach(Record record in records)
            {
                record.permutations = AnalyzeRecord(record.data, record.faults, 0);
                l += record.permutations;
                Console.WriteLine(record.permutations);
            }

            Console.WriteLine("First task: " + l);
        }

        static void SecondTask()
        {

            foreach(Record record in records)
            {
                unfoldedRecords.Add(UnfoldRecord(record));
            }

            long l = 0;
            recordCache = new Dictionary<string, long>();
            foreach (Record record in unfoldedRecords)
            {
                record.permutations = AnalyzeRecord(record.data, record.faults, 0);
                //recordCache.Clear();
                l += record.permutations;
                Console.WriteLine(record.permutations);
            }

            Console.WriteLine("Second task: " + l);
        }

        static Record UnfoldRecord(Record record)
        {
            List<int> faults = new List<int>();
            string data = "";
            for (int i = 0; i < 5; i++)
            {
                foreach(int fault in record.faults)
                {
                    faults.Add(fault);
                }
                data += record.data;
                if(i < 4)
                {
                    data += "?";
                }
            }

            return new Record(data, faults);
        }

        static long AnalyzeRecord(string data, List<int> faults, int faultIndex)
        {
            long permutations = 0;

            string mainPattern = @"(^[\?\.]*?)([\#\?]{" + faults[faultIndex] + @"})([\.\?]|$)";
            string remainsPattern = @"^[^\#]*$";

            int minLength = 0;
            for(int i = faultIndex; i < faults.Count; i++)
            {
                minLength += faults[i];
            }
            minLength += faults.Count - 1 - faultIndex;

            // Single record caching!
            /*
            string cacheString = faultIndex + data;
            if (recordCache != null)
            {
                if (recordCache.ContainsKey(cacheString))
                {
                    return recordCache[cacheString];
                }
            }
            */

            // Multi record caching!
            string complexCacheString = "";
            if ( recordCache != null)
            {
                for(int i = faultIndex; i < faults.Count; i++)
                {
                    complexCacheString += faults[i] + ",";
                }
                complexCacheString += data;

                if(recordCache.ContainsKey(complexCacheString))
                {
                    return recordCache[complexCacheString];
                }
            }

            int iterator = 0;
            while(iterator <= data.Length - minLength)
            {

                if (faultIndex == 2)
                {
                    // Do nothing, I just want a breakpoint.
                }
                string startPattern = @"^[\?\.]{" + iterator + @"}";
                if (!Regex.IsMatch(data, startPattern))
                {
                    return permutations;
                }
                
                string oldString = Regex.Replace(data, startPattern, "");
                oldString = Regex.Replace(oldString, @"^\.*", "");

                if(iterator > 0 && oldString.Equals(data))
                {
                    return permutations;
                }
                Regex regex = new Regex(mainPattern);
                Match match = regex.Match(oldString);
                iterator = data.Length - oldString.Length;
                iterator += match.Groups[1].Length + 1;
                
                string newString = regex.Replace(oldString, "", 1);

                if (newString.Equals(oldString))
                {
                    return permutations;
                }
                if(faultIndex == faults.Count - 1)
                {
                    if(Regex.IsMatch(newString, remainsPattern))
                    {
                        permutations++;
                    }
                } else if (newString.Length > 0)
                {
                    permutations += AnalyzeRecord(newString, faults, faultIndex + 1);
                }
            }
            /*
            // Single record caching!
            if(recordCache != null)
            {
                recordCache.Add(cacheString, permutations);
            }
            */
            // Multiple record caching!
            if (recordCache != null)
            {
                recordCache.Add(complexCacheString, permutations);
            }


            return permutations;
        }
    }

    class Record
    {
        public string data;
        public List<int> faults;
        public long permutations = 0;

        public Record(string data, List<int> faults)
        {
            this.data = data;
            this.faults = faults;
        }

        public Record(string data, string[] faults)
        {
            this.data = data;
            this.faults = new List<int>();
            foreach(string s in faults)
            {
                this.faults.Add(int.Parse(s));
            }
        }
    }
}