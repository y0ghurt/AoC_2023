using System.Security.AccessControl;
using System.Text.RegularExpressions;

namespace day_10
{
    class MirageMaintenance
    {
        static string filePath = "resources\\day_10_first-input.txt";
        static List<string> vertical = new List<string>();
        static Position startPosition;
        static Dictionary<Position, PipeSection> sections = new Dictionary<Position, PipeSection>();
        static Dictionary<Position, PipeSection> insiders = new Dictionary<Position, PipeSection>();
        static List<Position> positions = new List<Position>();
        static Position firstInsidePosition = null;

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
                vertical.Add(line);
            }
            for(int v = 0; v < vertical.Count; v++)
            {
                for(int h = 0; h < vertical[v].Length; h++)
                {
                    Position position = new Position(v, h);
                    sections.Add(position, new PipeSection(position, vertical[v][h]));
                    if (vertical[v][h] == 'S')
                    {
                        startPosition = position;
                    }
                }
            }
        }

        static void FirstTask()
        {
            FindPath();

            Console.WriteLine("First task: " + positions.Count / 2);
        }

        static void SecondTask()
        {

            Position lowestHorizontalPosition = new Position(0, int.MaxValue);
            foreach(Position position in positions)
            {
                if(position.h < lowestHorizontalPosition.h && !sections[position].position.Equals(startPosition))
                {
                    lowestHorizontalPosition = position;
                }
            }

            PipeSection section = sections[lowestHorizontalPosition];
            if(section.shape == 'L')
            {
                section.inside = Direction.TR;
            }
            else if(section.shape == '|')
            {
                section.inside = Direction.R;
            }
            else if(section.shape == 'F')
            {
                section.inside = Direction.BR;
            }

            PipeSection nextSection = sections[section.nextPosition];
            while(!nextSection.position.Equals(lowestHorizontalPosition))
            {
                nextSection.setInside(section);
                section = nextSection;
                nextSection = sections[section.nextPosition];
            }

            foreach (PipeSection p in sections.Values)
            {
                if (!positions.Contains(p.position))
                {
                    p.shape = '.';
                }
            }

            foreach (Position position in positions)
            {
                section = sections[position];
                if (section.shape == '|')
                {
                    if (section.inside == Direction.L)
                    {
                        Position pos = new Position(position.v, position.h - 1);
                        if (!positions.Contains(pos)) {
                            sections[pos].shape = 'I';
                            insiders[pos] = sections[pos];
                        }
                    }
                    else
                    {
                        Position pos = new Position(position.v, position.h + 1);
                        if (!positions.Contains(pos))
                        {
                            sections[pos].shape = 'I';
                            insiders[pos] = sections[pos];
                        }
                    }
                }
                else if (section.shape == '-')
                {
                    if (section.inside == Direction.T)
                    {
                        Position pos = new Position(position.v - 1, position.h);
                        if (!positions.Contains(pos))
                        {
                            sections[pos].shape = 'I';
                            insiders[pos] = sections[pos];
                        }
                    }
                    else
                    {
                        Position pos = new Position(position.v + 1, position.h);
                        if (!positions.Contains(pos))
                        {
                            sections[pos].shape = 'I';
                            insiders[pos] = sections[pos];
                        }
                    }
                }
                else if (section.shape == 'L')
                {
                    if (section.inside == Direction.BL)
                    {
                        Position pos = new Position(position.v + 1, position.h);
                        if (!positions.Contains(pos))
                        {
                            sections[pos].shape = 'I';
                            insiders[pos] = sections[pos];
                        }
                        pos = new Position(position.v, position.h - 1);
                        if (!positions.Contains(pos))
                        {
                            sections[pos].shape = 'I';
                            insiders[pos] = sections[pos];
                        }
                    }
                }
                else if (section.shape == 'F')
                {
                    if (section.inside == Direction.TL)
                    {
                        Position pos = new Position(position.v - 1, position.h);
                        if (!positions.Contains(pos))
                        {
                            sections[pos].shape = 'I';
                            insiders[pos] = sections[pos];
                        }
                        pos = new Position(position.v, position.h - 1);
                        if (!positions.Contains(pos))
                        {
                            sections[pos].shape = 'I';
                            insiders[pos] = sections[pos];
                        }
                    }
                }
                else if (section.shape == '7')
                {
                    if (section.inside == Direction.TR)
                    {
                        Position pos = new Position(position.v - 1, position.h);
                        if (!positions.Contains(pos))
                        {
                            sections[pos].shape = 'I';
                            insiders[pos] = sections[pos];
                        }
                        pos = new Position(position.v, position.h + 1);
                        if (!positions.Contains(pos))
                        {
                            sections[pos].shape = 'I';
                            insiders[pos] = sections[pos];
                        }
                    }
                }
                else if (section.shape == 'J')
                {
                    if (section.inside == Direction.BR)
                    {
                        Position pos = new Position(position.v, position.h + 1);
                        if (!positions.Contains(pos))
                        {
                            sections[pos].shape = 'I';
                            insiders[pos] = sections[pos];
                        }
                        pos = new Position(position.v + 1, position.h);
                        if (!positions.Contains(pos))
                        {
                            sections[pos].shape = 'I';
                            insiders[pos] = sections[pos];
                        }
                    }
                }
            }

            /* Debug prints.
            for (int v = 0; v < vertical.Count; v++)
            {
                Console.WriteLine("");
                for(int h = 0; h < vertical[0].Length; h++)
                {
                    Console.Write(sections[new Position(v, h)].shape);
                }
            }
            */

            int previousInsidersCounter = 0;
            while (previousInsidersCounter < insiders.Count) {
                previousInsidersCounter = insiders.Count;
                foreach (PipeSection ps in insiders.Values)
                {
                    if(Flow(ps))
                    break;
                }
            }
            
            /* Debug prints.
            Console.WriteLine("");
            Console.WriteLine("");
            for (int v = 0; v < vertical.Count; v++)
            {
                Console.WriteLine("");
                for (int h = 0; h < vertical[0].Length; h++)
                {
                    Console.Write(sections[new Position(v, h)].shape);
                }
            }
            Console.WriteLine("");
            Console.WriteLine("");
            */
            Console.WriteLine("Second task: " + insiders.Count);
        }

        static bool Flow(PipeSection ps)
        {
            bool flew = false;
            List<Position> p = new List<Position>();
            p.Add(new Position(ps.position.v, ps.position.h - 1));
            p.Add(new Position(ps.position.v, ps.position.h + 1));
            p.Add(new Position(ps.position.v - 1, ps.position.h));
            p.Add(new Position(ps.position.v + 1, ps.position.h));
            foreach (Position pp in p) {
                if (!(sections[pp].shape == 'I') && !positions.Contains(pp)) {
                    sections[pp].shape = 'I';
                    insiders.Add(pp, sections[pp]);
                    flew = true;
                }
            }
            return flew;
        }

        static void FindPath()
        {
            Position nextPosition = null;
            Position currentPosition = startPosition;
            positions.Add(startPosition);
            
            if (vertical[startPosition.v][startPosition.h + 1] is '-' or '7' or 'J')
            {
                nextPosition = new Position(startPosition.v, startPosition.h + 1);
            } else if (vertical[startPosition.v + 1][startPosition.h] is '|' or 'J' or 'L') {
                nextPosition = new Position(startPosition.v + 1, startPosition.h);
            }
            else if (vertical[startPosition.v][startPosition.h - 1] is '-' or 'F' or 'L')
            {
                nextPosition = new Position(startPosition.v, startPosition.h - 1);
            }
            sections[startPosition].nextPosition = nextPosition;

            while (sections[nextPosition].shape != 'S')
            {
                PipeSection nextSection = sections[nextPosition];
                nextPosition = nextSection.setPreviousSection(currentPosition);
                currentPosition = nextSection.position;
                positions.Add(currentPosition);
            }
            sections[nextPosition].previousPosition = currentPosition;
            if (sections[nextPosition].previousPosition.h == sections[nextPosition].position.h)
            {
                if(sections[nextPosition].previousPosition.v == sections[nextPosition].position.v + 1)
                {
                    if(sections[nextPosition].nextPosition.h == sections[nextPosition].position.h - 1)
                    {
                        sections[nextPosition].shape = '7';
                    }
                    else if(sections[nextPosition].nextPosition.v == sections[nextPosition].position.v - 1)
                    {
                        sections[nextPosition].shape = '|';
                    }
                    else if (sections[nextPosition].nextPosition.h == sections[nextPosition].position.h + 1)
                    {
                        sections[nextPosition].shape = 'F';
                    }
                }
                else if (sections[nextPosition].previousPosition.v == sections[nextPosition].position.v - 1)
                {
                    if (sections[nextPosition].nextPosition.h == sections[nextPosition].position.h - 1)
                    {
                        sections[nextPosition].shape = 'J';
                    }
                    else if (sections[nextPosition].nextPosition.v == sections[nextPosition].position.v + 1)
                    {
                        sections[nextPosition].shape = '|';
                    }
                    else if (sections[nextPosition].nextPosition.h == sections[nextPosition].position.h + 1)
                    {
                        sections[nextPosition].shape = 'L';
                    }
                }
            }
            else
            {
                if (sections[nextPosition].previousPosition.h == sections[nextPosition].position.h + 1)
                {
                    if (sections[nextPosition].nextPosition.v == sections[nextPosition].position.v + 1)
                    {
                        sections[nextPosition].shape = 'F';
                    }
                    else if (sections[nextPosition].nextPosition.h == sections[nextPosition].position.h - 1)
                    {
                        sections[nextPosition].shape = '-';
                    }
                    else if (sections[nextPosition].nextPosition.v == sections[nextPosition].position.v - 1)
                    {
                        sections[nextPosition].shape = 'L';
                    }
                }
                else if (sections[nextPosition].previousPosition.h == sections[nextPosition].position.h - 1)
                {
                    if (sections[nextPosition].nextPosition.v == sections[nextPosition].position.v - 1)
                    {
                        sections[nextPosition].shape = 'J';
                    }
                    else if (sections[nextPosition].nextPosition.h == sections[nextPosition].position.h + 1)
                    {
                        sections[nextPosition].shape = '-';
                    }
                    else if (sections[nextPosition].nextPosition.v == sections[nextPosition].position.v + 1)
                    {
                        sections[nextPosition].shape = 'J';
                    }
                }
            }
        }

    }

    class PipeSection
    {
        public Position position;
        public Position previousPosition;
        public Position nextPosition;
        public char shape;
        public Direction inside;

        public PipeSection(int v, int h, char shape)
        {
            position = new Position(v, h);
            this.shape = shape;
        }
        public PipeSection(Position position, char shape)
        {
            this.position = position;
            this.shape = shape;
        }

        public Position setPreviousSection(Position previousSection)
        {
            this.previousPosition = previousSection;

            if(previousSection.h == position.h - 1)
            {
                switch(shape)
                {
                    case 'J':
                        nextPosition = new Position(position.v - 1, position.h);
                        break;
                    case '-':
                        nextPosition = new Position(position.v, position.h + 1);
                        break;
                    case '7':
                        nextPosition = new Position(position.v + 1, position.h);
                        break;
                }
            } else if (previousSection.h == position.h + 1)
            {
                switch(shape)
                {
                    case 'L':
                        nextPosition = new Position(position.v - 1, position.h);
                        break;
                    case '-':
                        nextPosition = new Position(position.v, position.h - 1);
                        break;
                    case 'F':
                        nextPosition = new Position(position.v + 1, position.h);
                        break;
                }
            } else if (previousSection.v == position.v - 1)
            {
                switch (shape)
                {
                    case 'L':
                        nextPosition = new Position(position.v, position.h + 1);
                        break;
                    case '|':
                        nextPosition = new Position(position.v + 1, position.h);
                        break;
                    case 'J':
                        nextPosition = new Position(position.v, position.h - 1);
                        break;
                }
            }  else if (previousSection.v == position.v + 1)
            {
                switch (shape)
                {
                    case '7':
                        nextPosition = new Position(position.v, position.h - 1);
                        break;
                    case '|':
                        nextPosition = new Position(position.v - 1, position.h);
                        break;
                    case 'F':
                        nextPosition = new Position(position.v, position.h + 1);
                        break;
                }
            }

            return nextPosition;
        }

        public void setInside(PipeSection previousSection)
        {
            if (shape == '|')
            {
                if (previousSection.inside is Direction.L or Direction.TL or Direction.BL)
                {
                    inside = Direction.L;
                }
                else
                {
                    inside = Direction.R;
                }
            }
            else if (shape == '-')
            {
                if (previousSection.inside is Direction.T or Direction.TL or Direction.TR)
                {
                    inside = Direction.T;
                }
                else
                {
                    inside = Direction.B;
                }
            }
            else if (previousPosition.Equals(new Position(position.v, position.h + 1))
                || previousPosition.Equals(new Position(position.v, position.h - 1)))
            {
                if (shape == 'L')
                {
                    if (previousSection.inside is Direction.T or Direction.TR or Direction.TL)
                    {
                        inside = Direction.TR;
                    }
                    else
                    {
                        inside = Direction.BL;
                    }
                }
                else if (shape == 'F')
                {
                    if (previousSection.inside is Direction.T or Direction.TR or Direction.TL)
                    {
                        inside = Direction.TL;
                    }
                    else
                    {
                        inside = Direction.BR;
                    }
                }
                else if (shape == '7')
                {
                    if (previousSection.inside is Direction.T or Direction.TR or Direction.TL)
                    {
                        inside = Direction.TR;
                    }
                    else
                    {
                        inside = Direction.BL;
                    }
                }
                else if (shape == 'J')
                {
                    if (previousSection.inside is Direction.T or Direction.TR or Direction.TL)
                    {
                        inside = Direction.TL;
                    }
                    else
                    {
                        inside = Direction.BR;
                    }
                }
            }
            else
            {
                if (shape == 'L')
                {
                    if (previousSection.inside is Direction.R or Direction.TR or Direction.BR)
                    {
                        inside = Direction.TR;
                    }
                    else
                    {
                        inside = Direction.BL;
                    }
                }
                else if (shape == 'F')
                {
                    if (previousSection.inside is Direction.L or Direction.BL or Direction.TL)
                    {
                        inside = Direction.TL;
                    }
                    else
                    {
                        inside = Direction.BR;
                    }
                }
                else if (shape == '7')
                {
                    if (previousSection.inside is Direction.R or Direction.TR or Direction.BR)
                    {
                        inside = Direction.TR;
                    }
                    else
                    {
                        inside = Direction.BL;
                    }
                }
                else if (shape == 'J')
                {
                    if (previousSection.inside is Direction.L or Direction.TL or Direction.BL)
                    {
                        inside = Direction.TL;
                    }
                    else
                    {
                        inside = Direction.BR;
                    }
                }
            }
        }
    }

    enum Direction
    {
        T, B, L, R, TR, TL, BR, BL
    }

    class Filler
    {
        public char flowType;
        public Position position;
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