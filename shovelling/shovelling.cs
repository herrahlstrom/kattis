namespace kattis
{
    public class Program
    {
        private const char Blocked = '#';
        private const char Snow = 'o';
        private const char Cleared = '.';

        private static void Main(string[] args)
        {
            while (ReadMap() is { } map)
            {
                Console.WriteLine("MAP: {" + map.Width + "," + map.Height + "}");
            }

            Console.WriteLine("0 0");
        }

        private static ShovellingMap? ReadMap()
        {
            string firstLine = Console.ReadLine() ?? throw new NotSupportedException();
            string[] dim = firstLine.Split();
            int n = int.Parse(dim[0]);
            int m = int.Parse(dim[1]);

            if (n == 0 && m == 0)
            {
                return null;
            }

            var map = new ShovellingMap(n, m);

            for (int y = 0; y < map.Height; y++)
            {
                string line = Console.ReadLine() ?? throw new NotSupportedException();
                for (int x = 0; x < map.Height; x++)
                {
                    map[x, y] = line[y];
                }
            }

            Console.ReadLine();

            return map;
        }
    }

    internal class ShovellingMap
    {
        private readonly char[] _map;

        public ShovellingMap(int width, int height)
        {
            Width = width;
            Height = height;

            _map = new char[width * height];
        }

        public int Height { get; }

        public char this[int index]
        {
            get => _map[index];
            set => _map[index] = value;
        }

        public char this[int x, int y]
        {
            get => _map[GetIndex(x, y)];
            set => _map[GetIndex(y, x)] = value;
        }

        public int Width { get; }

        public (int x, int y) GetHouseA() => GetPos(Array.IndexOf(_map, 'A'));
        public (int x, int y) GetHouseB() => GetPos(Array.IndexOf(_map, 'B'));
        public (int x, int y) GetHouseC() => GetPos(Array.IndexOf(_map, 'C'));
        public (int x, int y) GetHouseD() => GetPos(Array.IndexOf(_map, 'D'));

        private int GetIndex(int x, int y) => y * Width + x;
        private (int x, int y) GetPos(int index) => (index / Width, index % Width);

    }
}