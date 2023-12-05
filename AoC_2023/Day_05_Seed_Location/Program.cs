using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace day_05
{
    class SeedLocation
    {
        static string filePath = "resources\\day_05_first-input.txt";
        static List<string> input = new List<string>();
        static List<Seed> seeds = new List<Seed>();
        static List<SeedGroup> seedGroups = new List<SeedGroup>();
        static List<Mapping> seedToSoilMappings = new List<Mapping>();
        static List<Mapping> soilToFertilizerMappings = new List<Mapping>();
        static List<Mapping> fertilizerToWaterMappings = new List<Mapping>();
        static List<Mapping> waterToLightMappings = new List<Mapping>();
        static List<Mapping> lightToTemperatureMappings = new List<Mapping>();
        static List<Mapping> temperatureToHumidityMappings = new List<Mapping>();
        static List<Mapping> humidityToLocationMappings = new List<Mapping>();


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
        }

        static void PopulateSeedGroupData()
        {
            for (int i = 0; i < input.Count; i++)
            {
                string line = input[i];
                if (Regex.IsMatch(line, @"seeds:"))
                {
                    MatchCollection seedDataList = Regex.Matches(line, @"\d+\s+\d+");
                    foreach (Match s in seedDataList)
                    {
                        string[] strings = Regex.Split(s.Value, @"\s+");
                        seedGroups.Add(new SeedGroup(long.Parse(strings[0]), long.Parse(strings[1])));
                    }
                }
            }
        }

        static void InterpretData(bool isRange)
        {
            seeds = new List<Seed>();

            for (int i = 0; i < input.Count; i++)
            {
                string line = input[i];
                if (Regex.IsMatch(line, @"seeds:"))
                {
                    if (!isRange)
                    {
                        foreach (string s in Regex.Split(line.Split(":")[1].Trim(), @"\s+"))
                        {
                            seeds.Add(new Seed(long.Parse(s)));
                        }
                    } else
                    {
                        MatchCollection seedDataList = Regex.Matches(line, @"\d+\s+\d+");
                        foreach (Match s in seedDataList)
                        {
                            string[] strings = Regex.Split(s.Value, @"\s+");
                            seedGroups.Add(new SeedGroup(long.Parse(strings[0]), long.Parse(strings[1])));
                        }
                    }
                }

                if (Regex.IsMatch(line, @"seed-to-soil map:"))
                {
                    seedToSoilMappings = new List<Mapping>();
                    int iterator = i + 1;
                    while (input[iterator].Length != 0)
                    {
                        string[] mapString = Regex.Split(input[iterator], @"\s+");
                        Mapping mapping = new Mapping(long.Parse(mapString[0]), long.Parse(mapString[1]), long.Parse(mapString[2]));
                        seedToSoilMappings.Add(mapping);
                        iterator++;
                    }
                }
                if (Regex.IsMatch(line, @"soil-to-fertilizer map:"))
                {
                    soilToFertilizerMappings = new List<Mapping>();
                    int iterator = i + 1;
                    while (input[iterator].Length != 0)
                    {
                        string[] mapString = Regex.Split(input[iterator], @"\s+");
                        Mapping mapping = new Mapping(long.Parse(mapString[0]), long.Parse(mapString[1]), long.Parse(mapString[2]));
                        soilToFertilizerMappings.Add(mapping);
                        iterator++;
                    }
                }
                if (Regex.IsMatch(line, @"fertilizer-to-water map:"))
                {
                    fertilizerToWaterMappings = new List<Mapping>();
                    int iterator = i + 1;
                    while (input[iterator].Length != 0)
                    {
                        string[] mapString = Regex.Split(input[iterator], @"\s+");
                        Mapping mapping = new Mapping(long.Parse(mapString[0]), long.Parse(mapString[1]), long.Parse(mapString[2]));
                        fertilizerToWaterMappings.Add(mapping);
                        iterator++;
                    }
                }
                if (Regex.IsMatch(line, @"water-to-light map:"))
                {
                    waterToLightMappings =  new List<Mapping>();
                    int iterator = i + 1;
                    while (input[iterator].Length != 0)
                    {
                        string[] mapString = Regex.Split(input[iterator], @"\s+");
                        Mapping mapping = new Mapping(long.Parse(mapString[0]), long.Parse(mapString[1]), long.Parse(mapString[2]));
                        waterToLightMappings.Add(mapping);
                        iterator++;
                    }
                }
                if (Regex.IsMatch(line, @"light-to-temperature map:"))
                {
                    lightToTemperatureMappings = new List<Mapping>();
                    int iterator = i + 1;
                    while (input[iterator].Length != 0)
                    {
                        string[] mapString = Regex.Split(input[iterator], @"\s+");
                        Mapping mapping = new Mapping(long.Parse(mapString[0]), long.Parse(mapString[1]), long.Parse(mapString[2]));
                        lightToTemperatureMappings.Add(mapping);
                        iterator++;
                    }
                }
                if (Regex.IsMatch(line, @"temperature-to-humidity map:"))
                {
                    temperatureToHumidityMappings = new List<Mapping>();
                    int iterator = i + 1;
                    while (input[iterator].Length != 0)
                    {
                        string[] mapString = Regex.Split(input[iterator], @"\s+");
                        Mapping mapping = new Mapping(long.Parse(mapString[0]), long.Parse(mapString[1]), long.Parse(mapString[2]));
                        temperatureToHumidityMappings.Add(mapping);
                        iterator++;
                    }
                }
                if (Regex.IsMatch(line, @"humidity-to-location map:"))
                {
                    humidityToLocationMappings = new List<Mapping>();
                    int iterator = i + 1;
                    while (iterator < input.Count)
                    {
                        if (input[iterator].Length != 0)
                        {
                            string[] mapString = Regex.Split(input[iterator], @"\s+");
                            Mapping mapping = new Mapping(long.Parse(mapString[0]), long.Parse(mapString[1]), long.Parse(mapString[2]));
                            humidityToLocationMappings.Add(mapping);
                            
                        }
                        iterator++;
                    }
                }
            }

            if (!isRange)
            {
                EnrichSeeds();
            }
            else
            {
                EnrichSeedGroups();
            }
        }

        static void EnrichSeeds()
        {
            foreach (Seed seed in seeds)
            {
                seed.soil = MapValue(seed.seed, seedToSoilMappings);
                seed.fertilizer = MapValue(seed.soil, soilToFertilizerMappings);
                seed.water = MapValue(seed.fertilizer, fertilizerToWaterMappings);
                seed.light = MapValue(seed.water, waterToLightMappings);
                seed.temperature = MapValue(seed.light, lightToTemperatureMappings);
                seed.humidity = MapValue(seed.temperature, temperatureToHumidityMappings);
                seed.location = MapValue(seed.humidity, humidityToLocationMappings);
            }
        }

        static void EnrichSeedGroups()
        {
            List<List<Mapping>> mappingListList = new List<List<Mapping>>();
            mappingListList.Add(seedToSoilMappings);
            mappingListList.Add(soilToFertilizerMappings);
            mappingListList.Add(fertilizerToWaterMappings);
            mappingListList.Add(waterToLightMappings);
            mappingListList.Add(lightToTemperatureMappings);
            mappingListList.Add(temperatureToHumidityMappings);
            mappingListList.Add(humidityToLocationMappings);
            foreach (List<Mapping> mappingList in mappingListList)
            {
                for (int i = 0; i < seedGroups.Count; i++)
                {
                    SeedGroup seedGroup = seedGroups[i];
                    foreach (Mapping mapping in mappingList)
                    {
                        long minSeed = seedGroup.seed + seedGroup.offset;
                        long maxSeed = seedGroup.seed + seedGroup.range - 1 + seedGroup.offset;
                        long minSource = mapping.source;
                        long maxSource = mapping.source + mapping.range -1;

                        // SeedGroup is entirely outside mapping.
                        if (maxSeed < minSource || minSeed > maxSource)
                        {
                            continue;
                        }

                        // SeedGroup is entirely within mapping.
                        if (minSeed >= minSource && maxSeed <= maxSource)
                        {
                            seedGroup.offset += mapping.offset;
                            break;
                        }

                        // SeedGroup encases mapping.
                        if(minSeed <= minSource && maxSeed >= maxSource) 
                        {
                            if(minSeed < minSource)
                            {
                                SeedGroup group = new SeedGroup(seedGroup.seed, minSource - minSeed);
                                group.offset = seedGroup.offset;
                                seedGroups.Add(group);
                            }
                            if(maxSeed > maxSource)
                            {
                                SeedGroup group = new SeedGroup(maxSource + 1 - seedGroup.offset, maxSeed - maxSource);
                                group.offset = seedGroup.offset;
                                seedGroups.Add(group);
                            }
                            seedGroup.seed = minSource - seedGroup.offset;
                            seedGroup.range = maxSource - minSource + 1;
                            seedGroup.offset += mapping.offset;

                            break;
                        }

                        // SeedGroup sticks out above mapping.
                        if(minSeed <= maxSource && minSeed >= minSource)
                        {
                            if (maxSeed > maxSource)
                            {
                                SeedGroup group = new SeedGroup(maxSource + 1 - seedGroup.offset, maxSeed - maxSource);
                                group.offset = seedGroup.offset;
                                seedGroups.Add(group);
                            }
                            seedGroup.range = maxSource - minSeed + 1;
                            seedGroup.offset += mapping.offset;

                            break;
                        }

                        // SeedGroup sticks out below mapping.
                        if(maxSeed >= minSource)
                        {
                            if (minSeed < minSource)
                            {
                                SeedGroup group = new SeedGroup(seedGroup.seed, minSource - minSeed);
                                group.offset = seedGroup.offset;
                                seedGroups.Add(group);
                            }
                            seedGroup.seed = minSource - seedGroup.offset;
                            seedGroup.range = maxSeed - minSource + 1;
                            seedGroup.offset += mapping.offset;

                            break;
                        }
                        
                    }
                }
            }
        }

        static void FirstTask()
        {
            InterpretData(false);

            long location = long.MaxValue;
            foreach(Seed seed in seeds)
            {
                location = Math.Min(location, seed.location);
            }

            Console.WriteLine("First task: " + location);
        }

        static void SecondTask()
        {
            InterpretData(true);

            long location = long.MaxValue;
            foreach (SeedGroup seedGroup in seedGroups)
            {
                location = Math.Min(location, seedGroup.seed + seedGroup.offset);
            }

            Console.WriteLine("Second task: " + location);
        }

        static long MapValue(long value, List<Mapping> mappings)
        {
            foreach (Mapping mapping in mappings)
            {
                if (value >= mapping.source && value < mapping.source + mapping.range)
                {
                    return value + mapping.offset;
                }
            }
            return value;
        }
    }

    class Mapping
    {
        public long source;
        public long destination;
        public long range;
        public long offset;

        public Mapping(long destination, long source, long range)
        {
            this.source = source;
            this.destination = destination;
            this.range = range;
            this.offset = this.destination - this.source;
        }
    }

    class Seed
    {
        public long seed;
        public long soil = 0;
        public long fertilizer = 0;
        public long water = 0;
        public long light = 0;
        public long temperature = 0;
        public long humidity = 0;
        public long location = 0;

        public Seed(long seed)
        {
            this.seed = seed;
        }
    }

    class SeedGroup
    {
        public long seed;
        public long range;
        public long offset = 0;

        public SeedGroup(long seed, long range)
        {
            this.seed = seed;
            this.range = range;
        }
    }
}