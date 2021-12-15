static IEnumerable<string> EnumerateLines()
{
    string line;
    while ((line = Console.ReadLine()) != null)
    {
        line = line.Trim();
        if (line != "")
            yield return line;
    }
}

static var Lines = EnumerateLines().ToArray();
static int MapHeight = Lines.Length;
static int MapWidth = Lines.Select(l => l.Length).Max();
static int[,] Map = new int[MapWidth, MapHeight];

for (int y = 0; y < MapHeight; y++)
for (int x = 0; x < MapWidth; x++)
    Map[x, y] = (int)(Lines[y][x] - '0');

public static int GetMap(int x, int y)
{
    int nx = x % MapWidth, dx = x / MapWidth;
    int ny = y % MapHeight, dy = y / MapHeight;
    return (Map[nx, ny] + dx + dy - 1) % 9 + 1;
}

static int Width = MapWidth * 5;
static int Height = MapHeight * 5;
static int GoalX = Width - 1;
static int GoalY = Height - 1;

public readonly record struct Point(int X, int Y);

struct State
{
    public int X, Y;
    public int Distance;

    public int Heuristic()
    {
        return Math.Max(Math.Abs(GoalX - X), Math.Abs(GoalY - Y));
    }

    public IEnumerable<State> Neighbors()
    {
        int d = Distance;
        if (X > 0) yield return new State { X=X-1, Y=Y, Distance=d + GetMap(X-1, Y) };
        if (Y > 0) yield return new State { X=X, Y=Y-1, Distance=d + GetMap(X, Y-1) };
        if (X + 1 < Width) yield return new State { X=X+1, Y=Y, Distance=d + GetMap(X+1, Y) };
        if (Y + 1 < Height) yield return new State { X=X, Y=Y+1, Distance=d + GetMap(X, Y+1) };
    }
}

int FindPath()
{
    HashSet<Point> closed = new();
    PriorityQueue<State, int> work = new();
    work.Enqueue(new State { X=0, Y=0, Distance=0 }, 0);

    while (work.TryDequeue(out State s, out _))
    {
        closed.Add(new Point(s.X, s.Y));
        if (s.X == GoalX && s.Y == GoalY)
            return s.Distance;

        foreach (State nb in s.Neighbors())
        {
            if (closed.Contains(new Point(nb.X, nb.Y))) continue;
            int score = nb.Distance + nb.Heuristic();
            work.Enqueue(nb, score);
        }
    }

    throw new Exception("Could not find path");
}

int distance = FindPath();
Console.WriteLine(distance);
